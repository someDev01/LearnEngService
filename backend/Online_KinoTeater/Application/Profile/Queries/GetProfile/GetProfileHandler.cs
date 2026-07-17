using Application.Interfaces.ProfileQuery;
using Application.Profile.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Profile.Queries.GetProfile;

public class GetProfileHandler(
    IProfileQueryService profileQueryService): IRequestHandler<GetProfileQuery, Result<ProfileDto>>
{
    public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await profileQueryService.GetAsync(request.UserId, cancellationToken);

        return Result<ProfileDto>.Success(result);
    }
}