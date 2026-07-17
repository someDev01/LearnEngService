using Application.Profile.Dtos;

namespace Application.Interfaces.ProfileQuery;

public interface IProfileQueryService
{
    Task<ProfileDto> GetAsync(Guid userId, CancellationToken cancellationToken);
}