using Application.Auth.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId): IRequest<Result<UserByIdDto>>;
