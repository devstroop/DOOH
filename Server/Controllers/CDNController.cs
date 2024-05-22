using DOOH.Server.Data;
using DOOH.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DOOH.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CDNController : ControllerBase
    {
        private readonly DOOHDBService DOOHDBService;
        private readonly Services.CDNService CDNService;
        private readonly ApplicationIdentityDbContext IdentityContext;
        private readonly Services.FFMPEGService FFMPEGService;

        public CDNController(Services.CDNService CDNService, DOOHDBService DOOHDBService, ApplicationIdentityDbContext identityContext, Services.FFMPEGService FFMPEGService)
        {
            this.CDNService = CDNService ?? throw new ArgumentNullException(nameof(CDNService));
            this.DOOHDBService = DOOHDBService ?? throw new ArgumentNullException(nameof(DOOHDBService));
            this.IdentityContext = identityContext ?? throw new ArgumentNullException(nameof(IdentityContext));
            this.FFMPEGService = FFMPEGService ?? throw new ArgumentNullException(nameof(FFMPEGService));
        }

        //[Authorize]
        [HttpGet("objects")] // List objects in S3.
        public async Task<IActionResult> ListObjectsAsync()
        {
            try
            {
                var objects = await CDNService.ListObjectsAsync();
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("objects/presigned")] // List presigned object URLs in S3.
        public async Task<IActionResult> ListPresignedObjectUrlsAsync()
        {
            try
            {
                var objects = await CDNService.ListPresignedObjectUrlsAsync(TimeSpan.FromMinutes(15));
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("object/{**key}")] // Get an object from S3.
        public async Task<IActionResult> GetObjectAsync(string key)
        {
            try
            {
                var stream = await CDNService.GetObjectAsync(key);
                return Ok(stream);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("probe/{**key}")] // Probe an object in S3.
        public async Task<IActionResult> GetProbeAsync(string key)
        {
            try
            {
                var stream = await CDNService.GetObjectAsync(key);
                var probe = await FFMPEGService.Probe(stream);
                return Ok(probe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("object/presigned/{**key}")] // Get a presigned URL for an object in S3.
        public IActionResult GetPresignedObjectUrlAsync(string key)
        {
            try
            {
                var presignedUrl = CDNService.GetPresignedObjectUrl(key, TimeSpan.FromMinutes(15));
                return Ok(presignedUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("single")] // Upload an object to S3.
        public async Task<Server.Models.DOOHDB.Attachment> UploadObjectAsync(IFormFile file)
        {
            var userId = IdentityContext.Users.Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault()?.Id;
            string bucket = (User.Identity.Name == "admin" ? "admin" : userId);
            ProbeData probe = null;
            List<Stream> streams;
            string thumbnailKey = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), System.IO.Path.GetExtension(file.FileName));
            string attachmentKey = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), System.IO.Path.GetExtension(file.FileName));
            bool isVideo = file.ContentType.Contains("video");


            using var stream0 = file.OpenReadStream();
            var out0 = await FFMPEGService.ConvertVideoToMp4(stream0, transpose: 2);

            using var stream1 = out0;
            await CDNService.UploadObjectAsync(attachmentKey, stream1);

            if (isVideo)
            {
                using var stream2 = file.OpenReadStream();
                probe = await FFMPEGService.Probe(stream2);

                using var stream3 = file.OpenReadStream();
                streams = await FFMPEGService.ExtractImagesFromVideo(stream3, fps: 1 / (double.TryParse(probe.Format.Duration, out double _duration) ? _duration : 1.0));
                if (streams.Count > 0)
                {
                    await CDNService.UploadObjectAsync(thumbnailKey, streams.FirstOrDefault());
                }
            }

            // prepare attachment from probe data
            var attachment = new Models.DOOHDB.Attachment
            {
                AttachmentKey = attachmentKey,
                FileName = file.FileName,
                Thumbnail = isVideo ? thumbnailKey : null,
                Size = file.Length,
                ContentType = file.ContentType,
                AspectRatio = probe != null ? probe.Streams.FirstOrDefault().DisplayAspectRatio : null,
                Duration = probe != null ? (double.TryParse(probe.Format.Duration, out double duration) ? duration : 0) : null,
                Height = probe != null ? (int.TryParse(probe.Streams.FirstOrDefault().Height, out int height) ? height : 0) : null,
                Width = probe != null ? (int.TryParse(probe.Streams.FirstOrDefault().Width, out int width) ? width : 0) : null,
                BitRate = probe != null ? (int.TryParse(probe.Format.BitRate, out int bitRate) ? bitRate : 0) : null,
                CodecName = probe != null ? probe.Streams.FirstOrDefault().DisplayAspectRatio : null,
                FrameRate = probe != null ? probe.Streams.FirstOrDefault().DisplayAspectRatio : null,
            };

            // commit then return attachment, user needs to be notified new key assigned
            return await DOOHDBService.CreateAttachment(attachment);
        }

        [Authorize]
        [HttpPost("multiple")] // Upload multiple objects to S3.
        public async Task<List<Models.DOOHDB.Attachment>> UploadObjectsAsync(IEnumerable<IFormFile> files)
        {
            var userId = IdentityContext.Users.Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault()?.Id;
            List<Models.DOOHDB.Attachment> attachments = new List<Models.DOOHDB.Attachment>();
            foreach (var file in files)
            {

                string bucket = (User.Identity.Name == "admin" ? "admin" : userId);
                ProbeData probe = null;
                List<Stream> streams;
                string thumbnailKey = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), System.IO.Path.GetExtension(file.FileName));
                string attachmentKey = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), System.IO.Path.GetExtension(file.FileName));
                bool isVideo = file.ContentType.Contains("video");


                using var stream0 = file.OpenReadStream();
                var out0 = await FFMPEGService.ConvertVideoToMp4(stream0, transpose: 2);

                using var stream1 = out0;
                await CDNService.UploadObjectAsync(attachmentKey, stream1);

                if (isVideo)
                {
                    using var stream2 = file.OpenReadStream();
                    probe = await FFMPEGService.Probe(stream2);

                    using var stream3 = file.OpenReadStream();
                    streams = await FFMPEGService.ExtractImagesFromVideo(stream3, fps: 1 / (double.TryParse(probe.Format.Duration, out double _duration) ? _duration : 1.0));
                    if (streams.Count > 0)
                    {
                        await CDNService.UploadObjectAsync(thumbnailKey, streams.FirstOrDefault());
                    }
                }

                // prepare attachment from probe data
                var attachment = new Models.DOOHDB.Attachment
                {
                    AttachmentKey = attachmentKey,
                    FileName = file.FileName,
                    Thumbnail = isVideo ? thumbnailKey : null,
                    Size = file.Length,
                    ContentType = file.ContentType,
                    AspectRatio = probe != null ? probe.Streams.FirstOrDefault().DisplayAspectRatio : null,
                    Duration = probe != null ? (double.TryParse(probe.Format.Duration, out double duration) ? duration : 0) : null,
                    Height = probe != null ? (int.TryParse(probe.Streams.FirstOrDefault().Height, out int height) ? height : 0) : null,
                    Width = probe != null ? (int.TryParse(probe.Streams.FirstOrDefault().Width, out int width) ? width : 0) : null,
                    BitRate = probe != null ? (int.TryParse(probe.Format.BitRate, out int bitRate) ? bitRate : 0) : null,
                    CodecName = probe != null ? probe.Streams.FirstOrDefault().DisplayAspectRatio : null,
                    FrameRate = probe != null ? probe.Streams.FirstOrDefault().DisplayAspectRatio : null,
                };

                // commit then return attachment, user needs to be notified new key assigned
                attachment = await DOOHDBService.CreateAttachment(attachment);
                attachments.Add(attachment);
            }
            return attachments;
        }

        [Authorize]
        [HttpDelete("delete/{**key}")] // Delete an object from S3.
        public async Task<IActionResult> DeleteObjectAsync(string key)
        {
            try
            {
                await DOOHDBService.DeleteAttachment(key);
                await CDNService.DeleteObjectAsync(key);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
