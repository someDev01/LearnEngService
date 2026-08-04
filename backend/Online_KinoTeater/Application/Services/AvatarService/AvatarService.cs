using Application.Interfaces.AvatarService;
using Application.Interfaces.ImageProcessor;
using Application.Interfaces.Storage;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;

namespace Application.Services.AvatarService;

public class AvatarService(
    IFileStorageService fileStorageService,
    IImageProcessor imageProcessor,
    IUnitOfWork unitOfWork): IAvatarService
{
    public async Task<Result<string>> SetAsync(
        Domain.Model.Entyties.User user, 
        Stream stream, 
        CancellationToken cancellationToken)
    {
        var processResult = await imageProcessor.ProcessAsync(stream, cancellationToken);
        if(!processResult.IsSuccess)
            return Result<string>.Failure(processResult.Error!);
        var processedImageStream = processResult.Value;
        
        var key = BuildAvatarKey(user.Id);
        
        if (!string.IsNullOrWhiteSpace(user.AvatarPath))
            await fileStorageService.DeleteAsync(user.AvatarPath, cancellationToken);

        var uploadedResult = await fileStorageService.UploadAsync(
            processedImageStream!, 
            key, 
            "image/jpeg", 
            cancellationToken);
        if (!uploadedResult.IsSuccess)
            return Result<string>.Failure($"{uploadedResult.Error}");

        user.UploadAvatar(key);
        await unitOfWork.CommitAsync(cancellationToken);

        var url = fileStorageService.GetPublicUrl(key);
        return Result<string>.Success(url.Value!);
    }
    
    private static string BuildAvatarKey(Guid userId) => $"avatars/{userId}/avatar.jpeg";
}