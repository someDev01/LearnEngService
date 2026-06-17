using Application.Interfaces.Context;
using Application.User.Dtos;
using Domain.Model.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.User.Queries.GetAllUsers;

public class GetUsersHandler(IDataContext context) : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
{
    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await context.Users
            .AsNoTracking()
            .Select(u => new UserDto(
                u.Email!.Value,
                u.Role.ToString()!,
                u.CreatedAt))
            .ToListAsync(cancellationToken);
        if (users is null || users.Count == 0)
            return Result<List<UserDto>>.Failure("Пользователей не найдено");

        return Result<List<UserDto>>.Success(users);
    }
}