namespace Domain.Repositories.Subtitle;

public interface ISubtitleRepository
{
    Task<Model.Entyties.Subtitle?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(Model.Entyties.Subtitle subtitle, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        Guid contentId,
        string language,
        CancellationToken cancellationToken);

    Task DeleteAsync(Model.Entyties.Subtitle subtitle, CancellationToken cancellationToken);
}
