using Application.Interfaces.Token;
using Infrastructure.Settings.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Jwt;

public class JwtService(IOptions<JwtSettings> options) : ITokenService
{
    public string GenerationToken(Domain.Model.Common.Entity.User user)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(options.Value.SecretKey!);
        var secretKey = new SymmetricSecurityKey(keyBytes);

        var credentials = new SigningCredentials(
            secretKey,
            SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString()!)
        ];

        var now = DateTime.UtcNow;
        var expires = user.Role == Role.User ? 
            now.AddDays(options.Value.TokenExpireUserDays) : 
            now.AddHours(options.Value.TokenExpireAdminHours);

        var tokenDescriptr = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = now,
            Expires = expires,
            Audience = options.Value.Audience,
            Issuer = options.Value.Issuer,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.CreateToken(tokenDescriptr);

        string result = handler.WriteToken(jwt);
        return result;
    }
}
