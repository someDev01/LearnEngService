using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.Vocabulary;
using Application.Note.Dtos;
using Application.Requests.Vocabulary;
using Application.SharedDtos;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Note;
using FluentValidation;
using MediatR;

namespace Application.Note.Commands.CreateNote;

public class CreateNoteWithContextCommandHandler(
    INoteRepository noteRepository,
    IVocabularyService vocabularyService,
    INotePolicyService notePolicyService,
    INoteCacheService noteCacheService,
    IValidator<CreateNoteWithContextCommand> validator,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateNoteWithContextCommand, Result<NoteDto>>
{
    public async Task<Result<NoteDto>> Handle(CreateNoteWithContextCommand request, CancellationToken cancellationToken)
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
        string editedWord = request.Word
            .ToLower()
            .Trim();
        string context = request.Context;

        var examples = new List<Example>();

        var resultFromGroq = await vocabularyService.GenerationTranslateAsync(
            new VocabularyRequestDto(
                editedWord,
                context), 
            cancellationToken);
        if (!resultFromGroq.IsSuccess) return Result<NoteDto>.Failure(resultFromGroq.Error!);

        var data = resultFromGroq.Value;

        var groqTranslations = data?.Translations;
        var groqTranscription = data?.Transcription;
        var groqExamples = data?.Examples;
        var groqLvl = data?.Level;

        int repetitionScore = 0;
        #endregion

        #region VALUEOBJECTS/ONE -> DOMAIN
        var transcriptionResult = Transcription.Create(groqTranscription!);
        if (!transcriptionResult.IsSuccess)
            return Result<NoteDto>.Failure(transcriptionResult.Error!);

        var lvlResult = Lvl.Create(groqLvl!);
        if (!lvlResult.IsSuccess)
            return Result<NoteDto>.Failure(lvlResult.Error!);

        var sourceResult = Source.Create(
            request.YoutubeVideoId,
            request.YoutubeId,
            request.YoutubeVideoTitle,
            request.Context,
            request.Duration.Hours,
            request.Duration.Minutes,
            request.Duration.Seconds);
        if (!sourceResult.IsSuccess)
            return Result<NoteDto>.Failure(sourceResult.Error!);
        #endregion

        #region VALUEOBJECTS/MANY -> DOMAIN
        foreach(var ex in groqExamples!)
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
            editedWord,
            groqTranslations,
            transcriptionResult.Value!,
            examples,
            repetitionScore,
            lvlResult.Value,
            sourceResult.Value);

        if (!noteResult.IsSuccess)
            return Result<NoteDto>.Failure(noteResult.Error!);

        var note = noteResult.Value!;

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
            new SourceDto(
                note.Source!.YoutubeVideoId,
                note.Source!.YoutubeId,
                note.Source.YoutubeVideoTitle,
                note.Source.Context,
                new DurationContextDto(
                    note.Source.Hours,
                    note.Source.Minutes,
                    note.Source.Seconds)),
            note.LastTrainedAt,
            note.CreatedAt);

        await noteRepository.AddAsync(note, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
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
