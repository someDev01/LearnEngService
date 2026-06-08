using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.Storage;
using Domain.Model.Common;
using Infrastructure.Settings.Storage;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Storage;

public class S3StorageService(
    IAmazonS3 s3, 
    IOptions<S3Settings> optionsS3) : IFileStorageService
{
    private readonly S3Settings _settings = optionsS3.Value;

    public Result<string> GetPublicUrl(string key)
    {
        try
        {
            var url = $"{_settings.PublicUrl}/{_settings.BucketName}/{key}";

            return Result<string>.Success(url);
        }
        catch (AmazonS3Exception ex)
        {
            return Result<string>.Failure($"Ошибка S3: {ex.Message}");
        }
    }

    public async Task<Result<string>> UploadAsync(
        Stream fileStream, string key, string contentType, CancellationToken cancellationToken)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType
            };

            await s3.PutObjectAsync(request, cancellationToken);
            return Result<string>.Success(key);
        }
        catch(AmazonS3Exception ex)
        {
            return Result<string>.Failure($"Ошибка S3: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key,
            };

            await s3.DeleteObjectAsync(request, cancellationToken);
            return Result.Success();
        }
        catch(AmazonS3Exception ex)
        {
            return Result.Failure($"Ошибка S3: {ex.Message}");
        }
    }

    public async Task<string> GetFileContentAsync(string key, CancellationToken cancellationToken)
    {
        var response = await s3.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
        }, cancellationToken);

        var stream = response.ResponseStream;

        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync(cancellationToken);

        return content;
    }
}
