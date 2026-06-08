using Application.Interfaces.Context;
using Application.Interfaces.UnitOfWork;

namespace Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork(IDataContext context): IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken) => 
        await context.SaveChangesAsync(cancellationToken);
}
