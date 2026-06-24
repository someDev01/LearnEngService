using Application.Common.Cache;
using Application.Common.Pagination;
using Application.Interfaces.CacheService;
using Application.Interfaces.Context;
using Application.Interfaces.VideoRead;
using Application.Settings.Cache;
using Application.SharedDtos;
using Application.YoutubeVideo.Dtos;
using Domain.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Services.VideoRead;

public class VideoReadService(
    IDataContext context,
    ICacheService cacheService,
    IOptions<CacheTtlSettings> cacheTtlConfig) : IVideoReadService
{
    private readonly CacheTtlSettings _cacheTtlSettings = cacheTtlConfig.Value;

    public async Task<PagedResult<YoutubeVideosPreviewDto>> GetPagedAsync(
        int? page, int? pageSize, CancellationToken cancellationToken)
    {
        var (pageNormalized, pageSizeNormalized) = PaginationNormalizer.Normalize(
            page,
            pageSize);

        var pagedKey = CacheKeyBuilder.BuildGetAllVideosKey(
            pageNormalized,
            pageSizeNormalized);
        TimeSpan pagedKeyTtl = TimeSpan.FromMinutes(_cacheTtlSettings.PagedVideosTtlMinutes);

        var cached = await cacheService.GetByKeyAsync<PagedResult<YoutubeVideosPreviewDto>>(pagedKey);
        if (cached is not null)
            return cached;

        var query = context.YoutubeVideos
            .AsNoTracking()
            .Where(yv => !yv.IsBlocked)
            .OrderByDescending(yv => yv.CreatedAt)
            .Select(yv => new YoutubeVideosPreviewDto(
                yv.Id,
                yv.YoutubeId,
                yv.Title,
                new DurationContextDto(
                    yv.Duration.Hour,
                    yv.Duration.Minutes,
                    yv.Duration.Seconds),
                yv.LexicalComplexity.Value!));

        var result = await query.ToPagedResultAsync(
            pageNormalized,
            pageSizeNormalized,
            cancellationToken);

        await cacheService.SetAsync(
            pagedKey,
            result,
            pagedKeyTtl);

        return result;
    }

    public async Task<Result<PagedResult<YoutubeVideosPreviewDto>>> SearchPagedAsync(
        string query, int page, int pageSize, CancellationToken cancellationToken)
    {
        var (pageNormalized, pageSizeNormalized) = PaginationNormalizer.Normalize(
            page,
            pageSize);

        var like = $"%{query}%";

        var searchedVideosDto = context.YoutubeVideos
            .AsNoTracking()
            .Where(yv => EF.Functions.ILike(yv.Title, like))
            .OrderBy(yv => yv.Title)
            .ThenByDescending(yv => yv.CreatedAt)
            .Select(yv => new YoutubeVideosPreviewDto(
                yv.Id,
                yv.YoutubeId,
                yv.Title,
                new DurationContextDto(
                    yv.Duration.Hour,
                    yv.Duration.Minutes,
                    yv.Duration.Seconds),
                yv.LexicalComplexity.Value!));

        var result = await searchedVideosDto.ToPagedResultAsync(
            pageNormalized,
            pageSizeNormalized,
            cancellationToken);

        return Result<PagedResult<YoutubeVideosPreviewDto>>.Success(result);
    }
}
