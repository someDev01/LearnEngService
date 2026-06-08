using Application.Interfaces.NoteRead;
using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Queries.GetPagedNotes;

public class GetPagedNotesHandler(
    INoteReadService noteReadService) : IRequestHandler<GetPagedNotesQuery, Result<PagedResult<NoteDto>>>
{
    public async Task<Result<PagedResult<NoteDto>>> Handle(GetPagedNotesQuery request, CancellationToken cancellationToken)
    {
        var pagedNotes = await noteReadService.GetPagedAsync(
            request.UserId,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result<PagedResult<NoteDto>>.Success(pagedNotes);
    }
}
