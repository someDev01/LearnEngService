namespace Api.Requests;

public record DeleteNoteSourceRequest(
    Guid NoteId,
    Guid YoutubeVideoId,
    int Hours,
    int Minutes,
    int Seconds);