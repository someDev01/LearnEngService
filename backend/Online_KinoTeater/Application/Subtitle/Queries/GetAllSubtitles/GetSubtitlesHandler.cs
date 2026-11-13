using Application.Interfaces.Context;
using Application.Subtitle.Dtos;
using Domain.Model.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Subtitle.Queries.GetAllSubtitles;

public class GetSubtitlesHandler(
    IDataContext context): IRequestHandler<GetSubtitlesQuery, Result<List<SubtitleForAdmin>>>
{
    public async Task<Result<List<SubtitleForAdmin>>> Handle(GetSubtitlesQuery request, CancellationToken cancellationToken)
    {
        var subtitlesDto = await context.Subtitles
            .AsNoTracking()
            .Join(
                context.YoutubeVideos,
                s => s.VideoId,
                v => v.Id,
                (s, v) => new SubtitleForAdmin(
                    s.Id,
                    s.VideoId,
                    v.Title,
                    s.LanguageCode.ToString(),
                    s.FileKey,
                    s.Format.ToString()))
            .ToListAsync(cancellationToken);
        
        return Result<List<SubtitleForAdmin>>.Success(subtitlesDto);
    }
}