using Domain.Model.Common;

namespace Application.Interfaces.Storage;

public interface IFileStorageService
{
    Task<Result<string>> UploadAsync(Stream fileStream, string key, string contentType, CancellationToken cancellationToken);

    Result<string> GetPublicUrl(string key);

    Task<string> GetFileContentAsync(string key, CancellationToken cancellationToken);

    Task<Result> DeleteAsync(string key, CancellationToken cancellationToken);
}
