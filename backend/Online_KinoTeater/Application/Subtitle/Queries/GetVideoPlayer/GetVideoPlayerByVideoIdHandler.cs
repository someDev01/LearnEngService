using Application.Common.Cache;
using Application.Interfaces.CacheService;
using Application.Interfaces.Context;
using Application.Interfaces.Storage;
using Application.Settings.Cache;
using Application.Subtitle.Dtos;
using Application.Subtitle.Queries.GetAllSubtitles;
using Domain.Model.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Subtitle.Queries.GetVideoPlayer;

public class GetVideoPlayerByVideoIdHandler(
    IDataContext context,
    ICacheService cacheService,
    IFileStorageService fileStorageService,
    IOptions<CacheTtlSettings> cacheTtlConfig)
    : IRequestHandler<GetVideoPlayerByVideoIdQuery, Result<VideoPlayerResponseDto>>
{
    private readonly CacheTtlSettings _cacheTtlSettings = cacheTtlConfig.Value;

    public async Task<Result<VideoPlayerResponseDto>> Handle(GetVideoPlayerByVideoIdQuery request, CancellationToken cancellationToken)
    {
        var subtitlesKey = CacheKeyBuilder.BuildSubtitlesKey(request.VideoId.ToString());
        TimeSpan subtitlesKeyTtl = TimeSpan.FromSeconds(_cacheTtlSettings.SubtitlesTtlSeconds);

        #region CACHING
        var subtitlesCached = await cacheService.GetByKeyAsync<VideoPlayerResponseDto>(subtitlesKey);
        if (subtitlesCached is not null)
        {
            return Result<VideoPlayerResponseDto>.Success(subtitlesCached);
        }
        #endregion

        #region GET FROM DB
        var youtubeVideoData = await context.YoutubeVideos
            .AsNoTracking()
            .Where(yv => yv.Id == request.VideoId && !yv.IsBlocked)
            .Select(yv => new
            {
                yv.YoutubeId,
                yv.Title
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (youtubeVideoData is null)
            return Result<VideoPlayerResponseDto>.Failure("Такого youtube video нет");

        var subtitlesWithKey = await context.Subtitles
            .AsNoTracking()
            .Where(sb => sb.VideoId == request.VideoId)
            .Select(sb => new SubtitleDto(
                sb.Id,
                sb.Language!.Code,
                sb.FileKey,
                sb.Format.ToString()))
            .ToListAsync(cancellationToken);
        if (subtitlesWithKey is null)
            return Result<VideoPlayerResponseDto>.Failure("Субтитры видео не найдены");

        var subtitlesWithContent = (await Task.WhenAll(
            subtitlesWithKey
                .Select(
                    async sb => new SubtitleDto(
                        sb.Id,
                        sb.Language,
                        await fileStorageService.GetFileContentAsync(sb.Value, cancellationToken),
                        sb.Format))))
                .ToList();
        #endregion

        #region RESULT
        var result = new VideoPlayerResponseDto(
            youtubeVideoData.Title,
            youtubeVideoData.YoutubeId,
            subtitlesWithContent);
        #endregion

        #region SET KEY
        var setResult = await cacheService.SetAsync(
            subtitlesKey,
            result,
            subtitlesKeyTtl);
        if (!setResult)
            return Result<VideoPlayerResponseDto>.Failure("Ошибка при установке ключа subtitles в redis");
        #endregion

        return Result<VideoPlayerResponseDto>.Success(result);
    }
}
