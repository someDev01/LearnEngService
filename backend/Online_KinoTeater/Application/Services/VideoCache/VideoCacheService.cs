using Application.Common.Cache;
using Application.Interfaces.CacheService;
using Application.Interfaces.VideoCache;

namespace Application.Services.VideoCache;

public class VideoCacheService(ICacheService cacheService) : IVideoCacheService
{
    public async Task InvalidatePagedVideoAsync(CancellationToken cancellationToken)
    {
        var pagedVideosKey = CacheKeyBuilder.BuildResetPagedVideoKey();
        await cacheService.DeleteByPatternAsync(pagedVideosKey, cancellationToken);
    }
}
