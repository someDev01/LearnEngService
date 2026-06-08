using Application.Auth.Dtos;
using Application.Common.Cache;
using Application.Common.Hash;
using Application.Interfaces.CacheService;
using Application.Interfaces.VerificationCode;
using Application.Settings.Code;
using Microsoft.Extensions.Options;

namespace Application.Services.VerificationCode;

public class VerificationCodeService(
    ICacheService cacheService,
    IOptions<CodeSettings> config) : IVerificationCodeService
{
    private readonly CodeSettings _codeSettings = config.Value;

    public async Task SaveCodeAsync(string email, string code)
    {
        var codeKey = CacheKeyBuilder.BuildCodeKey(email);
        var codeKeyTtl = TimeSpan.FromMinutes(_codeSettings.ExpireSecondsCode);

        await cacheService.DeleteByKeyAsync(codeKey);

        var codeWithSalt = $"{code}{email}";
        var hash = HashHelper.ComputeSha256(codeWithSalt);

        var sendCodeDto = new UserSendCodeDto(email, hash);

        await cacheService.SetAsync(
            codeKey,
            sendCodeDto,
            codeKeyTtl);
    }

    public async Task<UserSendCodeDto?> VerifyCodeAsync(string email, string inputCode)
    {
        var codeKey = CacheKeyBuilder.BuildCodeKey(email);

        var codeDto = await cacheService.GetByKeyAsync<UserSendCodeDto>(codeKey);
        if (codeDto is null) return null;

        var codeWithSalt = $"{inputCode}{email}";
        var verifyCode = HashHelper.VerifySha256(codeWithSalt, codeDto.Code);
        if (!verifyCode)
            return null;

        return codeDto;
    }

    public async Task DeleteCodeAsync(string email)
    {
        var codeKey = CacheKeyBuilder.BuildCodeKey(email);
        await cacheService.DeleteByKeyAsync(codeKey);
    }
}
