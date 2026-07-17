using Application.Services.VerificationCodeSender;
using Domain.Model.Common;

namespace Application.Interfaces.VerificationCodeSender;

public interface IVerificationCodeSenderService
{
    Task<Result<TimeSpan>> SendCodeAsync(string email, VerificationCodePurpose purpose, CancellationToken cancellationToken);
}