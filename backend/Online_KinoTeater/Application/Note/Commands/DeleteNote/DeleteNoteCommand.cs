using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.DeleteNote;

public record DeleteNoteCommand(
    Guid UserId,
    Guid NoteId): IRequest<Result<Guid>>;
