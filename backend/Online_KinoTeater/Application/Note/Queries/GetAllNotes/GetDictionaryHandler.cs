using Application.Common.Cache;
using Application.Interfaces.CacheService;
using Application.Interfaces.Context;
using Application.Note.Dtos;
using Application.Settings.Cache;
using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Note.Queries.GetAllNotes;

public class GetDictionaryHandler(
    IDataContext context,
    ICacheService cacheService,
    IOptions<CacheTtlSettings> cacheTtlConfig) : IRequestHandler<GetDictionaryQuery, Result<List<NoteDto>>>
{
    private readonly CacheTtlSettings _cacheTtlSettings = cacheTtlConfig.Value;

    public async Task<Result<List<NoteDto>>> Handle(GetDictionaryQuery request, CancellationToken cancellationToken)
    {
        string keyNotes = CacheKeyBuilder.BuildNotesListKey(request.UserId.ToString());
        TimeSpan ttlKeyNotes = TimeSpan.FromSeconds(_cacheTtlSettings.NotesTtlSeconds);

        #region CHACING
        var chachedNotes = await cacheService.GetByKeyAsync<List<NoteDto>>(keyNotes);
        if (chachedNotes is not null)
            return Result<List<NoteDto>>.Success(chachedNotes);
        #endregion

        #region GETTING
        var notesDto = await context.Notes
            .AsNoTracking()
            .Where(n => n.UserId == request.UserId)
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
                n.CreatedAt))
            .ToListAsync(cancellationToken);
        #endregion

        #region SET KEY
        var setKeyResult = await cacheService.SetAsync(
            keyNotes,
            notesDto,
            ttlKeyNotes);
        if (!setKeyResult)
            return Result<List<NoteDto>>.Failure("Ошибка при установки ключа notes в redis");
        #endregion

        return Result<List<NoteDto>>.Success(notesDto);
    }
}
