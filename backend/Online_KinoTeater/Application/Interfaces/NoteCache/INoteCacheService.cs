namespace Application.Interfaces.NoteCache;

public interface INoteCacheService
{
    Task InvalidatePagedNotesAsync(Guid userId, CancellationToken cancellationToken);

    Task InvalidateNotesAsync(Guid userId);
}
