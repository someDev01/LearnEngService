using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;
using Domain.Repositories.Note;
using MediatR;

namespace Application.Note.Commands.DeleteNote;

public class DeleteNoteCommandHandler(
    INoteRepository noteRepository,
    INotePolicyService notePolicyService,
    INoteCacheService noteCacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteNoteCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        #region CHECK RATE LIMIT
        var noteRateLimitResult = await notePolicyService.CheckRateLimitAsync(request.UserId);
        if (!noteRateLimitResult.IsSuccess)
            return Result<Guid>.Failure(noteRateLimitResult.Error!);
        #endregion

        #region EXISTING
        var existingNote = await noteRepository.GetUserNoteByIdAsync(
            request.UserId,
            request.NoteId,
            cancellationToken);
        if (existingNote is null)
            return Result<Guid>.Failure("Такой заметки у пользователя нет");
        #endregion

        #region DELETE
        await noteRepository.DeleteAsync(existingNote, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        #endregion

        #region SET RATE LIMIT
        await notePolicyService.SetRateLimitAsync(request.UserId);
        #endregion

        #region DELETE KEYS
        await noteCacheService.InvalidateNotesAsync(request.UserId);
        await noteCacheService.InvalidatePagedNotesAsync(request.UserId, cancellationToken);    
        #endregion

        return Result<Guid>.Success(existingNote.Id);
    }
}
