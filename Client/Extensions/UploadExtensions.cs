using DOOH.Server.Models.DOOHDB;

namespace DOOH.Client.Extensions
{
    public static class UploadExtensions
    {
        
        public static string GetUrl(this string key) => $"/api/s3/object/{key}";
    }
}
