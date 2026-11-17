using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.UnitOfWork;
using Application.SharedDtos;
using Domain.Model.Common;
using Domain.Repositories.Note;
using MediatR;

namespace Application.Note.Commands.DeleteNoteSource;

public class DeleteNoteSourceCommandHandler(
    INoteRepository noteRepository,
    INotePolicyService notePolicyService,
    INoteCacheService noteCacheService,
    IUnitOfWork unitOfWork): IRequestHandler<DeleteNoteSourceCommand, Result>
{
    public async Task<Result> Handle(DeleteNoteSourceCommand request, CancellationToken cancellationToken)
    {
        #region CHECK RATE LIMIT
        var noteRateLimitResult = await notePolicyService.CheckRateLimitAsync(request.UserId);
        if (!noteRateLimitResult.IsSuccess)
            return Result<NoteDto>.Failure(noteRateLimitResult.Error!);
        #endregion
        
        var existingNote = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken);
        if (existingNote is null)
            return Result.Failure("Заметка с таким id отсутствует");
        
        var deletingResult = existingNote.RemoveSource(
            request.YoutubeVideoId,
            request.Hours,
            request.Minutes,
            request.Seconds);
        if (!deletingResult.IsSuccess)
            return Result.Failure(deletingResult.Error!);
        
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