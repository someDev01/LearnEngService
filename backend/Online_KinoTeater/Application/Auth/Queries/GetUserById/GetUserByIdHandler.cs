using Application.Auth.Dtos;
using Application.Interfaces.Context;
using Domain.Model.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Queries.GetUserById;

public class GetUserByIdHandler(IDataContext context) : IRequestHandler<GetUserByIdQuery, Result<UserByIdDto>>
{
    public async Task<Result<UserByIdDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userDto = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == request.UserId)
            .Select(u => new UserByIdDto(u.Email!.Value))
            .FirstOrDefaultAsync(cancellationToken);

        if (userDto is null)
            return Result<UserByIdDto>.Failure("Пользователь не найден");

        return Result<UserByIdDto>.Success(userDto);
    }
}
