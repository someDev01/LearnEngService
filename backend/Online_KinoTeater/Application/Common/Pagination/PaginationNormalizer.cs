namespace Application.Common.Pagination;

public static class PaginationNormalizer
{
    private static readonly int _minPageSize = 16;
    private static readonly int _maxPageSize = 51;

    public static (int page, int pageSize) Normalize(
        int? page,
        int? pageSize)
    {
        int currentPage = page ?? 1;
        if (currentPage < 1) currentPage = 1;

        int size = pageSize ?? _minPageSize;
        if (size < 1) size = _minPageSize;
        if(size > _maxPageSize) size = _maxPageSize;

        return (currentPage, size);
    }
}
