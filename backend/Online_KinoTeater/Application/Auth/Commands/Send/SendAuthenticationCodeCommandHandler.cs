using Application.Interfaces.VerificationCodeSender;
using Application.Services.VerificationCodeSender;
using Domain.Model.Common;
using FluentValidation;
using MediatR;

namespace Application.Auth.Commands.Send;

public class SendAuthenticationCodeCommandHandler(
    IVerificationCodeSenderService verificationCodeSenderService,
    IValidator<SendAuthenticationCodeCommand> validator) : IRequestHandler<SendAuthenticationCodeCommand, Result<TimeSpan>>
{
    public async Task<Result<TimeSpan>> Handle(SendAuthenticationCodeCommand request, CancellationToken cancellationToken)
    {
        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<TimeSpan>.Failure(errors);
        }
        #endregion
        
        var result = await verificationCodeSenderService.SendCodeAsync(
            request.Email, 
            VerificationCodePurpose.Authentication,
            cancellationToken);
        return result;  
    }
}
