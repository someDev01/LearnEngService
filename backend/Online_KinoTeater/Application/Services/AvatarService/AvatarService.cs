using Application.Interfaces.AvatarService;
using Application.Interfaces.Storage;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;

namespace Application.Services.AvatarService;

public class AvatarService(
    IFileStorageService fileStorageService,
    IUnitOfWork unitOfWork): IAvatarService
{
    public async Task<Result> SetAsync(
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
            return Result.Failure($"{uploadedResult.Error}");

        user.UploadAvatar(key);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
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