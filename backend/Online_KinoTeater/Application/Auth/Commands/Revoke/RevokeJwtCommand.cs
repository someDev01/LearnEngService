using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Commands.Revoke;

public record RevokeJwtCommand(Guid? Jti, TimeSpan Ttl): IRequest<Result>;
