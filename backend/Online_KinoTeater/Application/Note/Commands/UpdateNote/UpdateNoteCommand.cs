using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.UpdateNote;

public record UpdateNoteCommand(
    Guid UserId,
    Guid NoteId,
    string? Word,
    List<string>? Translations,
    List<ExampleDto>? Examples) : IRequest<Result<NoteDto>>;
