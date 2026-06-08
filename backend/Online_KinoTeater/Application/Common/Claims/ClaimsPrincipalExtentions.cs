using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Application.Common.Claims;

public static class ClaimsPrincipalExtentions
{
    public static Guid GetUserId(this ClaimsPrincipal claims)
    {
        string userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("Id пользователя в токене отсутствует");
        if (!Guid.TryParse(userId, out var result))
            throw new UnauthorizedAccessException("Id пользователя имеет неверный формат для токена");

        return result;
    }

    public static string GetNameUser(this ClaimsPrincipal claims)
    {
        string name = claims?.FindFirst(ClaimTypes.Name)?.Value!;
        return name;
    }

    public static string GetEmailUser(this ClaimsPrincipal claims)
    {
        string email = claims?.FindFirst(ClaimTypes.Email)?.Value!;
        return email;
    }

    public static string GetRole(this ClaimsPrincipal claims)
    {
        string role = claims?.FindFirst(ClaimTypes.Role)!.Value!;
        return role;
    }

    public static Guid GetJti(this ClaimsPrincipal claims)
    {
        var jti = claims.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrWhiteSpace(jti))
            throw new UnauthorizedAccessException("Jti отсутствует в токене");
        if (!Guid.TryParse(jti, out var result))
            throw new UnauthorizedAccessException("Некорректный Jti формат в токене");

        return result;
    }

    public static DateTime GetTokenExpireTime(this ClaimsPrincipal claims)
    {
        var expire = claims.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        if (string.IsNullOrWhiteSpace(expire))
            throw new UnauthorizedAccessException("Время жизни токена отсутствует");

        if (!long.TryParse(expire, out var expireUnix))
            throw new UnauthorizedAccessException("Некорректный время жизни токена");

        var result = DateTimeOffset.FromUnixTimeSeconds(expireUnix).UtcDateTime;

        return result;
    }
}
