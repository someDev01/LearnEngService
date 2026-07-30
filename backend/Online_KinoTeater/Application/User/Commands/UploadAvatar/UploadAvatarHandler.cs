using Application.Interfaces.AvatarService;
using Domain.Model.Common;
using Domain.Repositories.User;
using MediatR;

namespace Application.User.Commands.UploadAvatar;

public class UploadAvatarHandler(
    IAvatarService avatarService,
    IUserRepository userRepository): IRequestHandler<UploadAvatarCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<string>.Failure("Пользователь не найден");

        var result = await avatarService.SetAsync(
            user,
            request.FileStream,
            request.ContentType,
            request.OriginalFileName,
            cancellationToken);

        return Result<string>.Success(result.Value!);
    }
}