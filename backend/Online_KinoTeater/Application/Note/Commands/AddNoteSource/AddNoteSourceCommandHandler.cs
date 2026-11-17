using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.UnitOfWork;
using Application.SharedDtos;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Note;
using MediatR;

namespace Application.Note.Commands.AddNoteSource;

public class AddNoteSourceCommandHandler(INoteRepository noteRepository,
    INotePolicyService notePolicyService,
    INoteCacheService noteCacheService,
    IUnitOfWork unitOfWork): IRequestHandler<AddNoteSourceCommand, Result>
{
    public async Task<Result> Handle(AddNoteSourceCommand request, CancellationToken cancellationToken)
    {
        #region CHECK RATE LIMIT
        var noteRateLimitResult = await notePolicyService.CheckRateLimitAsync(request.UserId);
        if (!noteRateLimitResult.IsSuccess)
            return Result<NoteDto>.Failure(noteRateLimitResult.Error!);
        #endregion
        
        var existingNote = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken);
        if(existingNote is null)
            return Result.Failure("Заметка с таким id отсутствует");
        
        var sourceResult = Source.Create(
            request.YoutubeVideoId,
            request.YoutubeId,
            request.YoutubeVideoTitle,
            request.Context,
            request.Hours,
            request.Minutes,
            request.Seconds);
        if (!sourceResult.IsSuccess)
            return Result.Failure(sourceResult.Error!);
        var source = sourceResult.Value;
        
        var addingResult = existingNote.AddSource(source!);
        if (!addingResult.IsSuccess)
            return Result.Failure(addingResult.Error!);
        
        await unitOfWork.CommitAsync(cancellationToken);

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
        
        return Result.Success();    
    }
}