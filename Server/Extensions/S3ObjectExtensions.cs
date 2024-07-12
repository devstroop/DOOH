using Amazon.S3.Model;

namespace DOOH.Server.Extensions;

public static class S3ObjectExtensions
{
    public static string GetUrl(this S3Object s3Object)
    {
        return $"/api/s3/object/{s3Object.Key}";
    }
    public static string GetThumbnailUrl(this S3Object s3Object)
    {
        var extension = s3Object.Key.Split('.').Last().ToLower();

        return extension switch
        {
            "jpg" or "jpeg" or "png" or "gif" => $"/api/s3/object/{s3Object.Key}",
            "mp4" or "webm" or "ogg" => $"/api/s3/object/{s3Object.Key}.thumbnail.png",
            _ => "/icons/unknown-file.png"
        };
    }
}