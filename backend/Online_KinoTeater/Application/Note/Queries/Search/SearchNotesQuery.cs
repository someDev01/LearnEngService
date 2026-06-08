using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Queries.Search;

public record SearchNotesQuery(
    Guid UserId,
    string Query,
    int Page,
    int PageSize): IRequest<Result<PagedResult<NoteDto>>>;
