using Application.Interfaces.AvatarService;
using Application.Interfaces.Storage;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;

namespace Application.Services.AvatarService;

public class AvatarService(
    IFileStorageService fileStorageService,
    IUnitOfWork unitOfWork): IAvatarService
{
    public async Task<Result<string>> SetAsync(
        Domain.Model.Entyties.User user, 
        Stream file, 
        string contentType, 
        string originalFileName, 
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(user.AvatarPath))
            await fileStorageService.DeleteAsync(user.AvatarPath, cancellationToken);

        var key = BuildAvatarKey(user.Id, originalFileName);

        var uploadedResult = await fileStorageService.UploadAsync(file, key, contentType, cancellationToken);
        if (!uploadedResult.IsSuccess)
            return Result<string>.Failure($"{uploadedResult.Error}");

        user.UploadAvatar(key);
        await unitOfWork.CommitAsync(cancellationToken);

        var url = fileStorageService.GetPublicUrl(key);
        return Result<string>.Success(url.Value!);
    }
    
    private static string BuildAvatarKey(Guid userId, string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension =  ".jpg";

        var key = $"avatars/{userId}/avatar{extension}";
        return key;
    }
}