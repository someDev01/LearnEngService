using Application.Common.Constants;
using Application.User.Commands.UploadAvatar;
using FluentValidation;

namespace Application.Validators.UploadAvatar;

public class UploadAvatarCommandValidator: AbstractValidator<UploadAvatarCommand>
{
    public UploadAvatarCommandValidator()
    {
        RuleFor(r => r.OriginalFileName)
            .Must(n =>
            {
                var ext = Path.GetExtension(n).ToLowerInvariant();
                return ImageConstants.AllowedImagesTypes.ContainsKey(ext);
            })
            .WithMessage("Недопустимое расширение файла");

        RuleFor(r => r)
            .Must(cmd =>
            {
                var ext = Path.GetExtension(cmd.OriginalFileName).ToLowerInvariant();
                return
                    ImageConstants.AllowedImagesTypes.TryGetValue(ext, out var expectedContentType) &&
                    cmd.ContentType.Equals(expectedContentType, StringComparison.OrdinalIgnoreCase);
            })
            .WithMessage("Расширение файла не соответствует заявленному типу содержимого");
        
        RuleFor(r => r.FileLength)
            .GreaterThan(0)
            .LessThanOrEqualTo(ImageConstants.MAX_IMAGE_SIZE)
            .WithMessage($"Размер файла должен быть больше 0 но не превышать {ImageConstants.MAX_IMAGE_SIZE}MB");
    }   
}