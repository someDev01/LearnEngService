using Application.Interfaces.ImageProcessor;
using Domain.Model.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services.ImageProcessor;

public class ImageProcessor: IImageProcessor
{
    public async Task<Result<Stream>> ProcessAsync(Stream stream, CancellationToken cancellationToken)
    {
        try
        {
            using Image image = await Image.LoadAsync(stream, cancellationToken);

            if (image.Width > 512 || image.Height > 512)
            {
                image.Mutate(x => x.Resize(
                    new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(512, 512)
                    }));
            }
            
            var jpegStream = new MemoryStream();
            await image.SaveAsJpegAsync(jpegStream, new JpegEncoder
            {
                Quality = 90
            }, cancellationToken);

            jpegStream.Position = 0;
            
            return Result<Stream>.Success(jpegStream);
        }
        catch (UnknownImageFormatException)
        {
            return Result<Stream>.Failure("Файл не является изображением");
        }
        catch (InvalidImageContentException)
        {
            return Result<Stream>.Failure("Файл изображения поврежден");
        }
    }
}