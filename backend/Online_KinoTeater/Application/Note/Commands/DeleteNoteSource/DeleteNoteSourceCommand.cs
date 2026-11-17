using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.DeleteNoteSource;

public record DeleteNoteSourceCommand(
    Guid NoteId,
    Guid UserId,
    Guid YoutubeVideoId,
    int Hours,
    int Minutes,
    int Seconds
    ): IRequest<Result>;