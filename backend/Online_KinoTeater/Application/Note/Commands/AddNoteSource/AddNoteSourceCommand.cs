using Domain.Model.Common;
using MediatR;

namespace Application.Note.Commands.AddNoteSource;

public record AddNoteSourceCommand(
    Guid NoteId,
    Guid UserId,
    Guid YoutubeVideoId,
    string YoutubeId,
    string YoutubeVideoTitle,
    string Context,
    int Hours,
    int Minutes,
    int Seconds): IRequest<Result>;