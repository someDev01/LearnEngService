using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.UpdateRepetition;

public record UpdateRepetitionScoreCommand(
    Guid UserId,
    Guid NoteId,
    bool IsCorrect): IRequest<Result>;
