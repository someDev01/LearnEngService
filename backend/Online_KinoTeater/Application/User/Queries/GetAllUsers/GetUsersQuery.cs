using Application.User.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.User.Queries.GetAllUsers;

public record GetUsersQuery(): IRequest<Result<List<UserDto>>>;