using Application.Common.Cache;
using Application.Interfaces.CacheService;
using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Commands.Revoke;

public class RevokeJwtCommandHandler(ICacheService cacheService): IRequestHandler<RevokeJwtCommand, Result>
{
    public async Task<Result> Handle(RevokeJwtCommand request, CancellationToken cancellationToken)
    {
        if (request.Jti == Guid.Empty || !request.Jti.HasValue)
            return Result.Failure("Jti не указан для отзыва Jwt");

        var result = await cacheService.SetAsync(
            CacheKeyBuilder.BuildRevokeKey(request.Jti.ToString()!),
            request.Jti,
            request.Ttl);
        if (!result)
            return Result.Failure("Не удалось положить ключ revoke в redis");

        return Result.Success();
    }
}
