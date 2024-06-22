using DOOH.Server.Models.DOOHDB;

namespace DOOH.Client.Extensions
{
    public static class UploadExtensions
    {
        public static string GetThumbnail(this Upload upload)
        {
            try
            {
                if (upload == null)
                {
                    return string.Empty;
                }
                if (upload.ContentType.Contains("video"))
                {
                    return $"/api/cdn/object/{upload.Thumbnail ?? upload.Key}";
                }
                return $"/api/cdn/object/{upload.Key}";
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }
        
        public static string GetUrl(this Upload upload)
        {
            try
            {
                if (upload == null)
                {
                    return string.Empty;
                }
                return $"/api/cdn/object/{upload.Key}";
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }
    }
}
