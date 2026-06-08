using Application.InternalDtos.Youtube;

namespace Application.Interfaces.Clients.Youtube;

public interface IYoutubeClient
{
    Task<YoutubeVideoApiResponse?> GetVideoAsync(string youtubeVideoId);
}
