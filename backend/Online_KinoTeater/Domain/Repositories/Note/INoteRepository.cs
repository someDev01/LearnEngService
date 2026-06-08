namespace Domain.Repositories.Note;

public interface INoteRepository
{
    Task<Model.Entyties.Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Model.Entyties.Note?> GetUserNoteByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken);

    Task AddAsync(Model.Entyties.Note note, CancellationToken cancellationToken);

    Task DeleteAsync(Model.Entyties.Note note, CancellationToken cancellationToken);
}
