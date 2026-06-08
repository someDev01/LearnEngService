using Application.Interfaces.Context;
using Domain.Repositories.Note;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Note;

public class NoteRepository(IDataContext context) : INoteRepository
{
    public async Task<Domain.Model.Entyties.Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => 
        await context.Notes
            .AsSplitQuery()
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task AddAsync(Domain.Model.Entyties.Note note, CancellationToken cancellationToken) => 
        await context.Notes.AddAsync(note, cancellationToken);

    public Task DeleteAsync(Domain.Model.Entyties.Note note, CancellationToken cancellationToken)
    {
        context.Notes.Remove(note);
        return Task.CompletedTask;
    }

    public async Task<Domain.Model.Entyties.Note?> GetUserNoteByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken) =>
        await context.Notes
            .FirstOrDefaultAsync(n => n.UserId == userId && n.Id == noteId, cancellationToken);
}
