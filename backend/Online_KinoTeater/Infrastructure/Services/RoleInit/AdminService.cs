using Application.Interfaces.Context;
using Application.Interfaces.RoleAdmin;
using Domain.Model.Entyties;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RoleInit;

public class AdminService(IDataContext context) : IAdminService
{
    public async Task<bool> ExistsRoleAdmin(CancellationToken cancellationToken) =>
        await context.Users.AnyAsync(u => u.Role == Role.Admin, cancellationToken);
}
