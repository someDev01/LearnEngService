using Application.SharedDtos;
using Domain.Model.Common;

namespace Application.Interfaces.NoteRead;

public interface INoteReadService
{
    Task<PagedResult<NoteDto>> GetPagedAsync(
        Guid userId,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken);

    Task<Result<PagedResult<NoteDto>>> SearchPagedAsync(
        Guid userId,
        string query,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
