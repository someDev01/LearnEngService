using Application.SharedDtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Pagination;

public static class QueryableExtension
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        int totalCount = await query.CountAsync(cancellationToken);
        int skip = (page - 1) * pageSize;
        int take = pageSize;

        int totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

        var items = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Data = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPage
        };
    }
}
