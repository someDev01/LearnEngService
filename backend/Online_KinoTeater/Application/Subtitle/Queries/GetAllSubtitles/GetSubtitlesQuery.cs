using Application.Subtitle.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Subtitle.Queries.GetAllSubtitles;

public record GetSubtitlesQuery: IRequest<Result<List<SubtitleForAdmin>>>;