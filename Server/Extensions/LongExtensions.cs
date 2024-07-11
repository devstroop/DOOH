namespace DOOH.Server.Extensions;

public static class LongExtensions
{
    public static string ToFileSize(this long size)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size = size / 1024;
        }

        return $"{size:0.##} {sizes[order]}";
    }
    
}