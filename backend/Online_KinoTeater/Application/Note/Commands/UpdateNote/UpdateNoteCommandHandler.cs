using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.UnitOfWork;
using Application.SharedDtos;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Note;
using FluentValidation;
using MediatR;

namespace Application.Note.Commands.UpdateNote;

public class UpdateNoteCommandHandler(
    INoteRepository noteRepository,
    INotePolicyService notePolicyService,
    INoteCacheService noteCacheService,
    IValidator<UpdateNoteCommand> validator,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateNoteCommand, Result<NoteDto>>
{
    public async Task<Result<NoteDto>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
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

        #region EXISTING
        var existingNote = await noteRepository.GetUserNoteByIdAsync(
            request.UserId,
            request.NoteId,
            cancellationToken);
        if (existingNote is null)
            return Result<NoteDto>.Failure("Такой заметки у пользователя нет");
        #endregion

        #region UPDATE
        if(!string.IsNullOrWhiteSpace(request.Word))
        {
            var result = existingNote.UpdateWord(request.Word);
            if (!result.IsSuccess)
                return Result<NoteDto>.Failure(result.Error!);
        }    

        if(request.Translations is not null)
        {
            var result = existingNote.UpdateTranslations(request.Translations);
            if(!result.IsSuccess)
                return Result<NoteDto>.Failure(result.Error!);
        }

        if(request.Examples is not null)
        {
            var examples = new List<Example>();
            
            foreach(var ex in request.Examples)
            {
                var result = Example.Create(ex.Text, ex.Translate);
                if (!result.IsSuccess)
                    return Result<NoteDto>.Failure(result.Error!);

                examples.Add(result.Value!);
            }

            var updatedExamples = existingNote.UpdateExamples(examples);  
            if(!updatedExamples.IsSuccess)
                return Result<NoteDto>.Failure(updatedExamples.Error!);
        }

        await unitOfWork.CommitAsync(cancellationToken);
        #endregion

        #region RESULT
        var noteDto = new NoteDto(
            existingNote.Id,
            existingNote.Word,
            existingNote.Translations,
            existingNote.Transcription.Value!,
            existingNote.Lvl!.Value,
            existingNote.Examples
                .Select(ex => new ExampleDto(ex.Text!, ex.Translate!))
                .ToList(),
            existingNote.RepetitionScore,
            null,
            existingNote.LastTrainedAt,
            existingNote.CreatedAt);
        #endregion

        #region SET RATE LIMIT
        await notePolicyService.SetRateLimitAsync(request.UserId);
        #endregion

        #region DELETE KEYS
        await noteCacheService.InvalidateNotesAsync(request.UserId);
        await noteCacheService.InvalidatePagedNotesAsync(request.UserId, cancellationToken);    
        #endregion

        return Result<NoteDto>.Success(noteDto);   
    }
}
