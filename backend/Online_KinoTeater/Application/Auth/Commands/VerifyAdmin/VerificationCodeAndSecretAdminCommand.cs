using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Commands.VerifyAdmin;

public record VerificationCodeAndSecretAdminCommand(
    string Email,
    string Code, 
    string Secret) : IRequest<Result<string>>;
