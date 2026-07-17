using Application.Profile.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Profile.Queries.GetProfile;

public record GetProfileQuery(Guid UserId): IRequest<Result<ProfileDto>>;