namespace DOOH.Server.Models;
using System;
using System.Collections.Generic;

public class MediaMetadata(
    string aspectRatio,
    string bitRate,
    string codec,
    string contentType,
    string createdAt,
    string duration,
    string filename,
    string frameRate,
    int? height,
    string key,
    string owner,
    string size,
    string thumbnail,
    int? width)
{
    public string AspectRatio { get; set; } = aspectRatio;
    public string BitRate { get; set; } = bitRate;
    public string Codec { get; set; } = codec;
    public string ContentType { get; set; } = contentType;
    public string CreatedAt { get; set; } = createdAt;
    public string Duration { get; set; } = duration;
    public string Filename { get; set; } = filename;
    public string FrameRate { get; set; } = frameRate;
    public int? Height { get; set; } = height;
    public string Key { get; set; } = key;
    public string Owner { get; set; } = owner;
    public string Size { get; set; } = size;
    public string Thumbnail { get; set; } = thumbnail;
    public int? Width { get; set; } = width;

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { "aspect_ratio", AspectRatio },
            { "bit_rate", BitRate },
            { "codec", Codec },
            { "content_type", ContentType },
            { "created_at", CreatedAt },
            { "duration", Duration },
            { "filename", Filename },
            { "frame_rate", FrameRate },
            { "height", Height?.ToString() },
            { "key", Key },
            { "owner", Owner },
            { "size", Size },
            { "thumbnail", Thumbnail },
            { "width", Width?.ToString() }
        };
    }

    public static MediaMetadata FromDictionary(Dictionary<string, string> dictionary)
    {
        return new MediaMetadata(
            dictionary.GetValueOrDefault("aspect_ratio"),
            dictionary.GetValueOrDefault("bit_rate"),
            dictionary.GetValueOrDefault("codec"),
            dictionary.GetValueOrDefault("content_type"),
            dictionary.GetValueOrDefault("created_at"),
            dictionary.GetValueOrDefault("duration"),
            dictionary.GetValueOrDefault("filename"),
            dictionary.GetValueOrDefault("frame_rate"),
            int.TryParse(dictionary.GetValueOrDefault("height"), out var height) ? height : (int?)null,
            dictionary.GetValueOrDefault("key"),
            dictionary.GetValueOrDefault("owner"),
            dictionary.GetValueOrDefault("size"),
            dictionary.GetValueOrDefault("thumbnail"),
            int.TryParse(dictionary.GetValueOrDefault("width"), out var width) ? width : (int?)null
        );
    }
}
