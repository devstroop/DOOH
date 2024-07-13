using System.Text.Json.Serialization;

namespace DOOH.Server.Models;
using System;
using System.Text.Json.Serialization;

public class CustomS3ObjectModel
{
    [JsonPropertyName("checksumAlgorithm")]
    public List<string> ChecksumAlgorithm { get; set; }

    [JsonPropertyName("eTag")]
    public string ETag { get; set; }

    [JsonPropertyName("bucketName")]
    public string BucketName { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime LastModified { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("restoreStatus")]
    public string RestoreStatus { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("storageClass")]
    public StorageClass StorageClass { get; set; }
}

public class StorageClass
{
    [JsonPropertyName("value")]
    public string Value { get; set; }
}



public static class CustomS3ObjectModelExtensions
{
    public static string GetUrl(this CustomS3ObjectModel s3Object)
    {
        return s3Object.Key == null ? null : $"/api/s3/object/{s3Object.Key}";
    }
    public static string GetThumbnailUrl(this CustomS3ObjectModel s3Object)
    {
        if (s3Object.Key == null)
        {
            return null;
        }
        
        var extension = s3Object.Key.Split('.').Last().ToLower();

        return extension switch
        {
            "jpg" or "jpeg" or "png" or "gif" => $"/api/s3/object/{s3Object.Key}",
            "mp4" or "webm" or "ogg" => $"/api/s3/object/{s3Object.Key}.thumbnail.png",
            _ => "/icons/unknown-file.png"
        };
    }
}
