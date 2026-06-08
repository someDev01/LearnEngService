using Application.Common.Seacrh;
using Application.Interfaces.NoteRead;
using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Queries.Search;

public class SearchNotesHandler(
    INoteReadService noteReadService) : IRequestHandler<SearchNotesQuery, Result<PagedResult<NoteDto>>>
{
    public async Task<Result<PagedResult<NoteDto>>> Handle(SearchNotesQuery request, CancellationToken cancellationToken)
    {
        var queryNormalized = SearchNormaliser.Normalize(request.Query);
        if (string.IsNullOrEmpty(queryNormalized) || queryNormalized.Length < 2)
            return Result<PagedResult<NoteDto>>.Success(new PagedResult<NoteDto> { Data = [] });

        if (queryNormalized.Length >= 25)
            return Result<PagedResult<NoteDto>>.Failure("Поиск должен быть небольше 25 символов");

        var searched = await noteReadService.SearchPagedAsync(
            request.UserId,
            queryNormalized,
            request.Page,
            request.PageSize,
            cancellationToken);
        if (!searched.IsSuccess)
            return Result<PagedResult<NoteDto>>.Failure(searched.Error!);

        return Result<PagedResult<NoteDto>>.Success(searched.Value!);
    }
}
