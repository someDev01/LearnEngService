using Application.Interfaces.AvatarService;
using Domain.Model.Common;
using Domain.Repositories.User;
using FluentValidation;
using MediatR;

namespace Application.User.Commands.UploadAvatar;

public class UploadAvatarHandler(
    IAvatarService avatarService,
    IUserRepository userRepository,
    IValidator<UploadAvatarCommand> validator): IRequestHandler<UploadAvatarCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<string>.Failure(errors);
        }
        
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<string>.Failure("Пользователь не найден");

        var result = await avatarService.SetAsync(
            user,
            request.FileStream,
            cancellationToken);
        if(!result.IsSuccess)
            return Result<string>.Failure(result.Error!);

        return Result<string>.Success(result.Value!);
    }
}