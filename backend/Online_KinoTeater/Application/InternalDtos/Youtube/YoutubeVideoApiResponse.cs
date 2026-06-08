namespace Application.InternalDtos.Youtube;

public record YoutubeVideoApiResponse(
    string Title,
    int Hours,
    int Minutes,
    int Seconds);
