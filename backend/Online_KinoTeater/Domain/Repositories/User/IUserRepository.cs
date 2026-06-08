namespace Domain.Repositories.User;

public interface IUserRepository
{
    Task<Model.Entyties.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Model.Entyties.User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task AddAsync(Model.Entyties.User user, CancellationToken cancellationToken);

    Task DeleteAsync(Model.Entyties.User user, CancellationToken cancellationToken);
}
