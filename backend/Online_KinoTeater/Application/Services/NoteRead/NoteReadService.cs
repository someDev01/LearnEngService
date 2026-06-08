using Application.Common.Cache;
using Application.Common.Pagination;
using Application.Interfaces.CacheService;
using Application.Interfaces.Context;
using Application.Interfaces.NoteRead;
using Application.Note.Dtos;
using Application.Settings.Cache;
using Application.SharedDtos;
using Domain.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Services.NoteRead;

public class NoteReadService(
    IDataContext context,
    ICacheService cacheService,
    IOptions<CacheTtlSettings> cacheTtlConfig) : INoteReadService
{
    private readonly CacheTtlSettings _cacheTtlSettings = cacheTtlConfig.Value;

    public async Task<PagedResult<NoteDto>> GetPagedAsync(
        Guid userId, int? page, int? pageSize, CancellationToken cancellationToken)
    {
        var (pageNormalized, pageSizeNormalized) = PaginationNormalizer.Normalize(
            page,
            pageSize);

        var pagedKey = CacheKeyBuilder.BuildPagedNotesKey(
            userId.ToString(),
            pageNormalized,
            pageSizeNormalized);
        TimeSpan pagedKeyTtl = TimeSpan.FromMinutes(_cacheTtlSettings.PagedNotesTtlMinutes);

        var cached = await cacheService.GetByKeyAsync<PagedResult<NoteDto>>(pagedKey);
        if (cached is not null)
            return cached;

        var notesDto = context.Notes
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NoteDto(
                n.Id,
                n.Word,
                n.Translations,
                n.Transcription.Value,
                n.Lvl!.Value,
                n.Examples
                    .Select(ex => new ExampleDto(ex.Text!, ex.Translate!))
                    .ToList(),
                n.RepetitionScore,
                n.Source == null ? null : new SourceDto(
                    n.Source.YoutubeVideoId,
                    n.Source.YoutubeId,
                    n.Source.YoutubeVideoTitle,
                    n.Source.Context,
                    new DurationContextDto(
                        n.Source.Hours,
                        n.Source.Minutes,
                        n.Source.Seconds)),
                n.LastTrainedAt,
                n.CreatedAt));

        var result = await notesDto.ToPagedResultAsync(
            pageNormalized,
            pageSizeNormalized,
            cancellationToken);

        await cacheService.SetAsync(
            pagedKey,
            result,
            pagedKeyTtl);

        return result;
    }

    public async Task<Result<PagedResult<NoteDto>>> SearchPagedAsync(
        Guid userId, string query, int page, int pageSize, CancellationToken cancellationToken)
    {
        var (pageNormalized, pageSizeNormalized) = PaginationNormalizer.Normalize(
            page,
            pageSize);

        var like = $"{query}%";

        var searchedNotesDto = context.Notes
            .AsNoTracking()
            .Where(n => n.UserId == userId && (
                    EF.Functions.ILike(n.Word, like) ||
                    n.Translations.Any(t => EF.Functions.ILike(t, like))
            ))
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NoteDto(
                n.Id,
                n.Word,
                n.Translations,
                n.Transcription.Value,
                n.Lvl!.Value,
                n.Examples
                    .Select(ex => new ExampleDto(ex.Text!, ex.Translate!))
                    .ToList(),
                n.RepetitionScore,
                n.Source == null ? null : new SourceDto(
                    n.Source.YoutubeVideoId,
                    n.Source.YoutubeId,
                    n.Source.YoutubeVideoTitle,
                    n.Source.Context,
                    new DurationContextDto(
                        n.Source.Hours,
                        n.Source.Minutes,
                        n.Source.Seconds)),
                n.LastTrainedAt,
                n.CreatedAt));

        var result = await searchedNotesDto.ToPagedResultAsync(
            pageNormalized,
            pageSizeNormalized,
            cancellationToken);

        return Result<PagedResult<NoteDto>>.Success(result);  
    }
}
