using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.CreateNote;

public record CreateNoteWithContextCommand(
    Guid UserId,
    Guid YoutubeVideoId,
    string YoutubeId,
    string YoutubeVideoTitle,
    DurationContextDto Duration,
    string Word,
    string Context): IRequest<Result<NoteDto>>;