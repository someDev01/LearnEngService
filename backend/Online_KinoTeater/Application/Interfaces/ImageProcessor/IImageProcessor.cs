using Domain.Model.Common;

namespace Application.Interfaces.ImageProcessor;

public interface IImageProcessor
{
    Task<Result<Stream>> ProcessAsync(Stream stream, CancellationToken cancellationToken);
}