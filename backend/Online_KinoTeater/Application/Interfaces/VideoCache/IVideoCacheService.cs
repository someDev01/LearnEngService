namespace Application.Interfaces.VideoCache;

public interface IVideoCacheService
{
    Task InvalidatePagedVideoAsync(CancellationToken cancellationToken);
}
