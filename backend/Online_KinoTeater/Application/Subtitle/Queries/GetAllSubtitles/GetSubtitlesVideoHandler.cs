using Application.Interfaces.Context;
using Application.Subtitle.Dtos;
using Domain.Model.Common;
using Domain.Repositories.Video;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Subtitle.Queries.GetAllSubtitles;

public class GetSubtitlesVideoHandler(
    IVideoRepository videoRepository,
    IDataContext context) : IRequestHandler<GetSubtitlesVideoQuery, Result<List<SubtitleDto>>>
{
    public async Task<Result<List<SubtitleDto>>> Handle(GetSubtitlesVideoQuery request, CancellationToken cancellationToken)
    {
        var youtubeVideo = await videoRepository.GetByIdAsync(request.VideoId, cancellationToken);
        if (youtubeVideo is null)
            return Result<List<SubtitleDto>>.Failure("Такое видео не найдено");  

        var subtitles = await context.Subtitles
            .AsNoTracking()
            .Where(sb => sb.VideoId == request.VideoId)
            .Select(sb => new SubtitleDto(
                sb.Id,
                sb.Language.Code,
                sb.FileKey,
                sb.Format.ToString()))
            .ToListAsync(cancellationToken);

        return Result<List<SubtitleDto>>.Success(subtitles);
    }
}
