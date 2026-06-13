using Application.Common.Cache;
using Application.Interfaces.AuthVerifucationPolicy;
using Application.Interfaces.CacheService;
using Application.Settings.Code;
using Microsoft.Extensions.Options;

namespace Application.Services.AuthVerificationPolicy;

public class AuthVerificationPolicyService(
    ICacheService cacheService,
    IOptions<CodeSettings> codeConfig) : IAuthVerificationPolicyService
{
    private readonly CodeSettings _codeSettings = codeConfig.Value;

    public async Task<bool> CanSendCodeAsync(string email)
    {
        var resendKey = CacheKeyBuilder.BuildResendKey(email);
        var existsResend = await cacheService.KeyExistsAsync(resendKey);
        if (existsResend)
            return false;

        return true;
    }

    public async Task<TimeSpan> LockCodeSendingAsync(string email)
    {
        var resendKey = CacheKeyBuilder.BuildResendKey(email);
        var resendKeyTtl = TimeSpan.FromSeconds(_codeSettings.ResendIntervalSeconds);

        await cacheService.SetAsync(
            resendKey,
            "lock",
            resendKeyTtl);

        return resendKeyTtl;
    }

    public async Task<bool> IsVerificationAttemptsBlockedAsync(
        string email, long maxAttempts)
    {
        var attemptsKey = CacheKeyBuilder.BuildAttemptsKey(email);

        var currentAttempts = await cacheService.GetByKeyAsync<long>(attemptsKey);
        if (currentAttempts >= maxAttempts) return false;

        return true;
    }

    public async Task IncrementVerificationAttemptsAsync(string email)
    {
        var attemptsKey = CacheKeyBuilder.BuildAttemptsKey(email);
        var attemptsKeyTtl = TimeSpan.FromSeconds(_codeSettings.AttemptsExpireHours);

        var incrKeyAttempts = await cacheService.IncrementAsync(attemptsKey);
        if (incrKeyAttempts == 1)
            await cacheService.SetExpireAsync(
                attemptsKey,
                attemptsKeyTtl);
    }


    public async Task ResetVerificationAttemptsAsync(string email)
    {
        var attemptsKey = CacheKeyBuilder.BuildAttemptsKey(email);
        await cacheService.DeleteByKeyAsync(attemptsKey);
    }
}
