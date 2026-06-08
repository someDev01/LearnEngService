using Application.SharedDtos;
using Application.YoutubeVideo.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.YoutubeVideo.Queries.GetAllVideos;

public record GetYoutubeVideosQuery(int Page, int PageSize): 
    IRequest<Result<PagedResult<YoutubeVideosPreviewDto>>>;
