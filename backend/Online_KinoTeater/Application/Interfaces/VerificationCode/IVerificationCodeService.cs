using Application.Auth.Dtos;

namespace Application.Interfaces.VerificationCode;

public interface IVerificationCodeService
{
    Task SaveCodeAsync(string email, string code);

    Task<UserSendCodeDto?> VerifyCodeAsync(string email, string inputCode);

    Task DeleteCodeAsync(string email);
}
