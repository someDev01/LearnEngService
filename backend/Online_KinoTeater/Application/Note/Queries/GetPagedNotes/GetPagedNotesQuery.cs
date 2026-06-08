using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Queries.GetPagedNotes;

public record GetPagedNotesQuery(
    Guid UserId,
    int? Page,
    int? PageSize): IRequest<Result<PagedResult<NoteDto>>>;
