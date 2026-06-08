using Domain.Model.Entyties;

namespace Domain.Repositories.Video;

public interface IVideoRepository
{
    Task<YoutubeVideo?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(YoutubeVideo video, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        string youtubeId,
        CancellationToken cancellationToken);

    Task DeleteAsync(YoutubeVideo video, CancellationToken cancellationToken);
}
