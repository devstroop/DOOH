using DOOH.Server.Models.DOOHDB;

namespace DOOH.Client.Extensions
{
    public static class AttachmentExtensions
    {
        public static string GetThumbnail(this Attachment attachment)
        {
            try
            {
                if (attachment.ContentType.Contains("video"))
                {
                    return $"/api/cdn/object/{attachment.Thumbnail ?? attachment.AttachmentKey}";
                }
                return $"/api/cdn/object/{attachment.AttachmentKey}";
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
