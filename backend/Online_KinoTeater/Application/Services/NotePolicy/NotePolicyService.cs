using Application.Common.Cache;
using Application.Common.Ttl;
using Application.Interfaces.CacheService;
using Application.Interfaces.NotePolicy;
using Application.Settings.Cache;
using Application.Settings.RateLimit;
using Domain.Model.Common;
using Microsoft.Extensions.Options;

namespace Application.Services.NotePolicy;

public class NotePolicyService(
    ICacheService cacheService,
    IOptions<RateLimitSettings> rateLimitConfig,
    IOptions<CacheTtlSettings> configCacheTtl) : INotePolicyService
{
    private readonly RateLimitSettings _rateLimitSettings = rateLimitConfig.Value;
    private readonly CacheTtlSettings _cacheTtlSettings = configCacheTtl.Value;

    public async Task<Result> CheckCreateDailyAsync(Guid userId)
    {
        string userLimitKey = CacheKeyBuilder.BuildLlmUserLimitsKey(userId.ToString());

        int maxUserLimit = _rateLimitSettings.UserDaily;
        int currentUserLimit = await cacheService.GetByKeyAsync<int>(userLimitKey);

        if (currentUserLimit >= maxUserLimit)
            return Result.Failure("Лимит создание заметок превышен. Попробуйте завтра");

        return Result.Success();
    }

    public async Task IncrementCreateDailyLimitAsync(Guid userId)
    {
        string userLimitKey = CacheKeyBuilder.BuildLlmUserLimitsKey(userId.ToString());
        TimeSpan userLimitKeyTtl = TtlHelper.UntilNextUtcTime();

        var incrResult = await cacheService.IncrementAsync(userLimitKey);
        if (incrResult == 1)
            await cacheService.SetExpireAsync(
                userLimitKey,
                userLimitKeyTtl);
    }

    public async Task<Result> CheckRateLimitAsync(Guid userId)
    {
        string noteRateLimitKey = CacheKeyBuilder.BuildNoteRateLimit(userId.ToString());

        bool existsRateLimit = await cacheService.KeyExistsAsync(noteRateLimitKey);
        if (existsRateLimit)
            return Result.Failure("Подождите...");

        return Result.Success();
    }

    public async Task SetRateLimitAsync(Guid userId)
    {
        string noteRateLimitKey = CacheKeyBuilder.BuildNoteRateLimit(userId.ToString());
        TimeSpan rateLimitTtl = TimeSpan.FromSeconds(_cacheTtlSettings.RateLimitNoteTtlSeconds);

        await cacheService.SetAsync(
            noteRateLimitKey,
            "",
            rateLimitTtl);
    }
}
