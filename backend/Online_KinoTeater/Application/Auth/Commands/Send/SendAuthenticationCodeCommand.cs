using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Commands.Send;

public record SendAuthenticationCodeCommand(string Email) : IRequest<Result<TimeSpan>>;
