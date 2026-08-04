using Domain.Model.Common;

namespace Application.Interfaces.AvatarService;

public interface IAvatarService
{
    Task<Result<string>> SetAsync(
        Domain.Model.Entyties.User user,
        Stream stream,
        CancellationToken cancellationToken);
}