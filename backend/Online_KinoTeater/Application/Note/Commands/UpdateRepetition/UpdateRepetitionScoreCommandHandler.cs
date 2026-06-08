using Application.Interfaces.NoteCache;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;
using Domain.Repositories.Note;
using MediatR;

namespace Application.Note.Commands.UpdateRepetition;

public class UpdateRepetitionScoreCommandHandler(
    INoteRepository noteRepository,
    INoteCacheService noteCacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateRepetitionScoreCommand, Result>
{
    public async Task<Result> Handle(UpdateRepetitionScoreCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken);
        if (note is null)
            return Result.Failure("Такой заметки нет");

        #region RESULT
        var updateScoreResult = note.UpdateRepetitionScore(request.IsCorrect);
        if (!updateScoreResult.IsSuccess)
            return Result.Failure(updateScoreResult.Error!);

        await unitOfWork.CommitAsync(cancellationToken);
        #endregion

        #region DELETE KEYS
        await noteCacheService.InvalidateNotesAsync(request.UserId);
        await noteCacheService.InvalidatePagedNotesAsync(request.UserId, cancellationToken);
        #endregion

        return Result.Success();
    }
}
