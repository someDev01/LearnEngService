using Application.Common.Cache;
using Application.Interfaces.CacheService;
using Application.Interfaces.NoteCache;

namespace Application.Services.NoteCache;

public class NoteCacheService(ICacheService cacheService) : INoteCacheService
{
    public async Task InvalidateNotesAsync(Guid userId)
    {
        var notesListKey = CacheKeyBuilder.BuildNotesListKey(userId.ToString());

        await cacheService.DeleteByKeyAsync(notesListKey);
    }

    public async Task InvalidatePagedNotesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var pagedNotesKey = CacheKeyBuilder.BuildResetPagedNotesKey(userId.ToString());

        await cacheService.DeleteByPatternAsync(pagedNotesKey, cancellationToken);
    }
}
