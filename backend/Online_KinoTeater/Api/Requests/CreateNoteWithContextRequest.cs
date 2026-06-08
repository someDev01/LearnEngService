namespace Api.Requests;

public record CreateNoteWithContextRequest(
    Guid YoutubeVideoId,
    string YoutubeId,
    string YoutubeVideoTitle,
    int Hours,
    int Minutes,
    int Seconds,
    string Word,
    string Context);
