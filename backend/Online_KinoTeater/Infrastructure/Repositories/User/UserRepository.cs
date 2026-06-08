using Application.Interfaces.Context;
using Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.User;

public class UserRepository(IDataContext context): IUserRepository
{
    public async Task<Domain.Model.Entyties.User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await context.Users
            .FirstOrDefaultAsync(u => u.Email!.Value == email, cancellationToken);

    public async Task<Domain.Model.Entyties.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await context.Users
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task AddAsync(Domain.Model.Entyties.User user, CancellationToken cancellationToken) => 
        await context.Users.AddAsync(user, cancellationToken);
    
    public Task DeleteAsync(Domain.Model.Entyties.User user, CancellationToken cancellationToken)
    {
        context.Users.Remove(user);
        return Task.CompletedTask;
    }
}
