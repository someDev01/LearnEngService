namespace Api.Requests;

public record AddNoteSourceRequest(
    Guid NoteId,
    Guid YoutubeVideoId,
    string YoutubeId,
    string YoutubeVideoTitle,
    string Context,
    int Hours,
    int Minutes,
    int Seconds);