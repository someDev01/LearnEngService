using Domain.Model.Common;

namespace Application.Interfaces.AvatarService;

public interface IAvatarService
{
    Task<Result> SetAsync(
        Domain.Model.Entyties.User user,
        Stream file,
        string contentType,
        string originalFileName,
        CancellationToken cancellationToken);
}