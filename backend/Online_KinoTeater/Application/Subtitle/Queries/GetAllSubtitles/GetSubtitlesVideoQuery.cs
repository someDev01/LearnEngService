using Application.Subtitle.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Subtitle.Queries.GetAllSubtitles;

public record GetSubtitlesVideoQuery(Guid VideoId): IRequest<Result<List<SubtitleDto>>>;
