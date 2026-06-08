using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.Vocabulary;
using Application.Requests.Vocabulary;
using Application.SharedDtos;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Note;
using FluentValidation;
using MediatR;

namespace Application.Note.Commands.CreateNote;

public class CreateNoteCommandHandler(
    INoteRepository noteRepository,
    IVocabularyService vocabularyService,
    INotePolicyService notePolicyService,
    INoteCacheService noteCacheService,
    IValidator<CreateNoteCommand> validator,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateNoteCommand, Result<NoteDto>>
{
    public async Task<Result<NoteDto>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        #region CHECK RATE LIMIT
        var noteRateLimitResult = await notePolicyService.CheckRateLimitAsync(request.UserId);
        if (!noteRateLimitResult.IsSuccess)
            return Result<NoteDto>.Failure(noteRateLimitResult.Error!);
        #endregion

        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<NoteDto>.Failure(errors);
        }

        #endregion

        #region CHECK LIMIT
        var noteDailyLimitResult = await notePolicyService.CheckCreateDailyAsync(request.UserId);
        if (!noteDailyLimitResult.IsSuccess)
            return Result<NoteDto>.Failure(noteDailyLimitResult.Error!);
        #endregion

        #region PROPERTIES
        string editedWord = request.EnglishName
            .ToLower()
            .Trim();

        var editedTranslations = (request.Translations ?? [])
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.ToLowerInvariant().Trim())
            .ToList();
        var editedExamples = (request.Examples ?? [])
            .Where(ex => !string.IsNullOrWhiteSpace(ex.Text) && !string.IsNullOrWhiteSpace(ex.Translate))
            .Select(ex => new ExampleDto(
                ex.Text.ToLowerInvariant().Trim(),
                ex.Translate.ToLowerInvariant().Trim()))
            .ToList();
        var examplesDto = new List<ExampleDto>();
        examplesDto.AddRange(editedExamples);

        var examples = new List<Example>();

        var translations = editedTranslations;
        var resultFromGroq = await vocabularyService.GenerationTranslateAsync(
            new VocabularyRequestDto(
                editedWord,
                Context: null,
                IsIncludedTranslations: editedTranslations.Count == 0, 
                IsIncludedExamples: editedExamples.Count == 0), 
            cancellationToken);
        if (!resultFromGroq.IsSuccess) return Result<NoteDto>.Failure(resultFromGroq.Error!);

        var data = resultFromGroq.Value!;

        var groqWord = data?.Word;
        var groqTranslations = data?.Translations;
        var groqTranscription = data?.Transcription;
        var groqExamples = data?.Examples;
        var groqLvl = data?.Level;

        int repetitionScore = 0;

        if(groqTranslations is not null)
            translations.AddRange(groqTranslations);
        if(groqExamples is not null)
            examplesDto.AddRange(groqExamples);
        #endregion

        #region VALUEOBJECTS/ONE -> DOMAIN
        var transcriptionResult = Transcription.Create(groqTranscription!);
        if (!transcriptionResult.IsSuccess)
            return Result<NoteDto>.Failure(transcriptionResult.Error!);

        var lvlResult = Lvl.Create(groqLvl!);
        if (!lvlResult.IsSuccess)
            return Result<NoteDto>.Failure(lvlResult.Error!);
        #endregion

        #region VALUEOBJECTS/MANY -> DOMAIN
        foreach (var ex in examplesDto)
        {
            var result = Example.Create(ex.Text, ex.Translate);
            if (!result.IsSuccess)
                return Result<NoteDto>.Failure(result.Error!);

            examples.Add(result.Value!);
        }
        #endregion

        #region CREATE
        var noteResult = Domain.Model.Entyties.Note.Create(
            request.UserId,
            groqWord!,
            translations,
            transcriptionResult.Value!,
            examples!,
            repetitionScore,
            lvlResult.Value);

        if (!noteResult.IsSuccess)
            return Result<NoteDto>.Failure(noteResult.Error!);

        var note = noteResult.Value!;

        await noteRepository.AddAsync(note, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var noteDto = new NoteDto(
            note.Id,
            note.Word,
            note.Translations,
            note.Transcription.Value,
            note.Lvl!.Value,
            note.Examples
                .Select(ex => new ExampleDto(ex.Text!, ex.Translate!))
                .ToList(),
            note.RepetitionScore,
            null,
            note.LastTrainedAt,
            note.CreatedAt);
        #endregion

        #region SET RATE LIMIT
        await notePolicyService.SetRateLimitAsync(request.UserId);
        #endregion

        #region SET DAILY LIMIT
        await notePolicyService.IncrementCreateDailyLimitAsync(request.UserId);
        #endregion

        #region DELETE KEYS
        await noteCacheService.InvalidateNotesAsync(request.UserId);
        await noteCacheService.InvalidatePagedNotesAsync(request.UserId, cancellationToken);
        #endregion

        return Result<NoteDto>.Success(noteDto);
    }
}
