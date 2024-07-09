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
        private readonly Services.CDNService cdnService;
        private readonly ApplicationIdentityDbContext identityContext;
        private readonly Services.FFMPEGService ffmpegService;
        
        public CDNController(Services.CDNService cdnService, DOOHDBService doohdbService, ApplicationIdentityDbContext identityContext, Services.FFMPEGService ffmpegService)
        {
            this.cdnService = cdnService ?? throw new ArgumentNullException(nameof(cdnService));
            // this.DOOHDBService = DOOHDBService ?? throw new ArgumentNullException(nameof(DOOHDBService));
            this.identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
            this.ffmpegService = ffmpegService ?? throw new ArgumentNullException(nameof(ffmpegService));
        }

        [Authorize]
        [HttpGet("objects")] // List objects in S3.
        public async Task<IActionResult> ListObjectsAsync()
        {
            try
            {
                var objects = await cdnService.ListObjectsAsync();
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("objects/{**directory}")] // List objects in S3.
        public async Task<IActionResult> ListObjectsFromDirectoryAsync(string directory)
        {
            try
            {
                var objects = await cdnService.ListObjectsAsync(directory);
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("objects/presigned")] // List presigned object URLs in S3.
        public async Task<IActionResult> ListPresignedObjectUrlsAsync()
        {
            try
            {
                var objects = await cdnService.ListPresignedObjectUrlsAsync(TimeSpan.FromMinutes(15));
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("object/{**key}")] // Get an object from S3.
        public async Task<IActionResult> GetObjectAsync(string key)
        {
            try
            {
                var stream = await cdnService.GetObjectAsync(key);
                return Ok(stream);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("metadata/{**key}")] // Get an object metadata from S3.
        public async Task<IActionResult> GetObjectMetadataAsync(string key)
        {
            try
            {
                var metadata = await cdnService.GetObjectMetadataAsync(key);
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("probe/{**key}")] // Probe an object in S3.
        public async Task<IActionResult> GetProbeAsync(string key)
        {
            try
            {
                var stream = await cdnService.GetObjectAsync(key);
                var probe = await ffmpegService.Probe(stream);
                return Ok(probe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("object/presigned/{**key}")] // Get a presigned URL for an object in S3.
        public IActionResult GetPresignedObjectUrlAsync(string key)
        {
            try
            {
                var presignedUrl = cdnService.GetPresignedObjectUrl(key, TimeSpan.FromMinutes(15));
                return Ok(presignedUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("single")] // Upload an object to S3.
        public async Task<Dictionary<string, string>> UploadObjectAsync(IFormFile file)
        {
            var userId = identityContext.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name))?.Id;
            var bucket = (User.Identity?.Name?.ToLower() == "admin" ? "admin" : userId);
            var contentType = file.ContentType;
            var thumbnail = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), "thumbnail.png");
            var key = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), System.IO.Path.GetExtension(file.FileName));


            await using var stream0 = file.OpenReadStream();
            var (out0, probeData) = await ffmpegService.ConvertVideoToMp4(stream0, transpose: 2);


            var metadata = new Dictionary<string, string>
            {
                { "key", key },
                { "filename", file.FileName },
                { "thumbnail", thumbnail },
                { "size", probeData.Format.Size },
                { "content_type", contentType },
                { "aspect_ratio", probeData.Streams.FirstOrDefault()?.DisplayAspectRatio },
                { "duration", probeData.Format.Duration },
                { "codec", probeData.Streams.FirstOrDefault()?.CodecName },
                { "height", probeData.Streams.FirstOrDefault()?.Height },
                { "width", probeData.Streams.FirstOrDefault()?.Width },
                { "bit_rate", probeData.Format.BitRate },
                { "frame_rate", probeData.Streams.FirstOrDefault()?.RFrameRate },
                { "format_name", probeData.Format.FormatName },
                { "owner", userId }
            };


            await using var stream1 = out0;
            await cdnService.UploadObjectAsync(key, stream1, metadata);

            if (!contentType.Contains("video")) return metadata;
            await using var stream3 = file.OpenReadStream();
            var streams = await ffmpegService.ExtractImagesFromVideo(stream3, fps: 1 / (double.TryParse(probeData.Format.Duration, out double _duration) ? _duration : 1.0));
            if (streams.Count > 0)
            {
                await cdnService.UploadObjectAsync(thumbnail, streams.FirstOrDefault());
            }


            return metadata;
        }

        [Authorize]
        [HttpPost("multiple")] // Upload multiple objects to S3.
        public async Task<List<Dictionary<string, string>>> UploadObjectsAsync(IEnumerable<IFormFile> files)
        {
            var userId = identityContext.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name))?.Id;
            var metadatas = new List<Dictionary<string, string>>();
            foreach (var file in files)
            {
                var bucket = (User.Identity?.Name?.ToLower() == "admin" ? "admin" : userId);
                
                var contentType = file.ContentType;

                var thumbnail = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), "thumbnail.png");
                var key = bucket + "/" + Path.ChangeExtension(Path.GetRandomFileName(), System.IO.Path.GetExtension(file.FileName));

                await using var stream0 = file.OpenReadStream();
                var (out0, probeData) = await ffmpegService.ConvertVideoToMp4(stream0, transpose: 2);
                var metadata = new Dictionary<string, string>
                {
                    { "key", key },
                    { "filename", file.FileName },
                    { "thumbnail", thumbnail },
                    { "size", probeData.Format.Size },
                    { "content_type", contentType },
                    { "aspect_ratio", probeData.Streams.FirstOrDefault()?.DisplayAspectRatio },
                    { "duration", probeData.Format.Duration },
                    { "codec", probeData.Streams.FirstOrDefault()?.CodecName },
                    { "height", probeData.Streams.FirstOrDefault()?.Height },
                    { "width", probeData.Streams.FirstOrDefault()?.Width },
                    { "bit_rate", probeData.Format.BitRate },
                    { "frame_rate", probeData.Streams.FirstOrDefault()?.RFrameRate },
                    { "format_name", probeData.Format.FormatName },
                    { "owner", userId }
                };


                await using var stream1 = out0;
                await cdnService.UploadObjectAsync(key, stream1, metadata);

                if (!contentType.Contains("video")) continue;
                await using var stream3 = file.OpenReadStream();
                var streams = await ffmpegService.ExtractImagesFromVideo(stream3, fps: 1 / (double.TryParse(probeData.Format.Duration, out var duration) ? duration : 1.0));
                if (streams.Count > 0)
                {
                    await cdnService.UploadObjectAsync(thumbnail, streams.FirstOrDefault());
                }
            }
            return metadatas;
        }

        [Authorize]
        [HttpDelete("delete/{**key}")] // Delete an object from S3.
        public async Task<IActionResult> DeleteObjectAsync(string key)
        {
            try
            {
                var metadata = await cdnService.GetObjectMetadataAsync(key);
                await cdnService.DeleteObjectAsync(key);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
