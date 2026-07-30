using Domain.Model.Common;
using MediatR;

namespace Application.User.Commands.UploadAvatar;

public record UploadAvatarCommand(
    Guid UserId,
    Stream FileStream,
    string OriginalFileName,
    string ContentType): IRequest<Result<string>>;