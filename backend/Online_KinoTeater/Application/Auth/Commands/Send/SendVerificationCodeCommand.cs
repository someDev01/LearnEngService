using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Commands.Send;

public record SendVerificationCodeCommand(string Email) : IRequest<Result<TimeSpan>>;
