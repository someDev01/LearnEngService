using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.CreateNote;

public record CreateNoteCommand(
    Guid UserId,
    string EnglishName,
    List<string>? Translations,
    List<ExampleDto>? Examples): IRequest<Result<NoteDto>>;
