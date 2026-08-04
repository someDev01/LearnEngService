namespace Application.Common.Constants;

public static class ImageConstants
{
    public const long MAX_IMAGE_SIZE = 5 * 1024 * 1024; //5MB

    public static readonly Dictionary<string, string> AllowedImagesTypes = new()
    {
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".png", "image/png"},
        {".webp", "image/webp"}
    };
}