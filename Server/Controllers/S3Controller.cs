using DOOH.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime;
using Amazon.S3;
using System.Net;
using System.Text.Json;
using Amazon.S3.Model;
using DOOH.Server.Models;

namespace DOOH.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly ApplicationIdentityDbContext _identityContext;
        private readonly Services.FFMPEGService _ffmpegService;
        private readonly DOOHDBService _doohdbService;
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucket;

        public S3Controller(DOOHDBService doohdbService, ApplicationIdentityDbContext identityContext,
            Services.FFMPEGService ffmpegService, IConfiguration configuration)
        {
            _doohdbService = doohdbService ?? throw new ArgumentNullException(nameof(doohdbService));
            _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
            _ffmpegService = ffmpegService ?? throw new ArgumentNullException(nameof(ffmpegService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var serviceUrl = _configuration.GetValue<string>("R2:ServiceURL");
            var accessKey = _configuration.GetValue<string>("R2:AccessKey");
            var secretKey = _configuration.GetValue<string>("R2:SecretKey");
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(credentials, new AmazonS3Config { ServiceURL = serviceUrl });
            _bucket = _configuration.GetValue<string>("R2:Bucket");
        }

        [Authorize]
        [HttpGet("objects")]
        public async Task<IActionResult> ListObjectsAsync()
        {
            var request = new Amazon.S3.Model.ListObjectsV2Request { BucketName = _bucket };
            var response = await _s3Client.ListObjectsV2Async(request);
            var userId = _identityContext.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name))?.Id;
            var objects = response.S3Objects.Where(x => !x.Key.EndsWith(".thumbnail.png"));
            objects = User.IsInRole("Admin") ? objects : objects.Where(obj => obj.Key.StartsWith(userId!));
            // List<CustomS3ObjectModel> result = new List<CustomS3ObjectModel>();
            // foreach (var each in objects ?? new List<S3Object>())
            // {
            //     var customObject = new CustomS3ObjectModel
            //     {
            //         ETag = each.ETag,
            //         Key = each.Key,
            //         LastModified = each.LastModified,
            //         Owner = JsonSerializer.Serialize(each.Owner),
            //         RestoreStatus = JsonSerializer.Serialize(each.RestoreStatus),
            //         Size = each.Size,
            //         StorageClass = JsonSerializer.Serialize(each.StorageClass),
            //         BucketName = each.BucketName
            //     };
            //     result.Add(customObject);
            // }
            return Ok(objects);
        }
        
        [HttpDelete("object/{**key}")]
        [Authorize]
        public async Task<IActionResult> DeleteObjectAsync(string key)
        {
            try
            {
                var request = new Amazon.S3.Model.DeleteObjectRequest
                {
                    BucketName = _bucket,
                    Key = key
                };

                var response = await _s3Client.DeleteObjectAsync(request);
                return Ok($"Object {key} deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete object {key}: {ex.Message}");
            }
        }

        
        [HttpGet("presigned")]
        public async Task<IActionResult> ListPresignedObjectUrlsAsync()
        {
            return await HandleS3RequestAsync(async () =>
            {
                var request = new Amazon.S3.Model.ListObjectsV2Request { BucketName = _bucket };
                var response = await _s3Client.ListObjectsV2Async(request);
                var objects = response.S3Objects.Select(obj => GetPresignedObjectUrl(obj.Key)).ToList();
                return Ok(objects);
            });
        }

        [HttpGet("object/{**key}")]
        public async Task<IActionResult> GetObjectAsync(string key)
        {
            return await HandleS3RequestAsync(async () =>
            {
                var request = new Amazon.S3.Model.GetObjectRequest { BucketName = _bucket, Key = key };
                var response = await _s3Client.GetObjectAsync(request);
                return File(response.ResponseStream, response.Headers.ContentType, key);
            });
        }

        [HttpPost("single")]
        [Authorize]
        public async Task<IActionResult> UploadObjectAsync(IFormFile file)
        {
            var userId = _identityContext.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name))?.Id;
            var bucket = User.Identity?.Name?.ToLower() == "admin" ? "admin" : userId;
            var contentType = file.ContentType;
            var key = $"{bucket}/{Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName))}";

            await using var stream0 = file.OpenReadStream();
            var (out0, probeData) = await _ffmpegService.ConvertVideoToMp4(stream0, transpose: 2);
            var metadata = CreateMetadata(file, key, probeData, userId);

            await using var stream1 = out0;
            await UploadObjectToS3Async(key, stream1, metadata);

            if (contentType.Contains("video"))
            {
                var thumbnail = $"{key}.thumbnail.png";
                await using var stream3 = file.OpenReadStream();
                var streams = await _ffmpegService.ExtractImagesFromVideo(stream3,
                    fps: 1 / (double.TryParse(probeData.Format.Duration, out double duration) ? duration : 1.0));
                if (streams.Any())
                {
                    await UploadObjectToS3Async(thumbnail, streams.First());
                }

                metadata.Thumbnail = thumbnail;
            }

            return Ok(metadata);
        }

        [HttpPost("multiple")]
        [Authorize]
        public async Task<IActionResult> UploadObjectsAsync(IEnumerable<IFormFile> files)
        {
            var userId = _identityContext.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name))?.Id;
            var metadatas = new List<MediaMetadata>();

            foreach (var file in files)
            {
                var bucket = User.Identity?.Name?.ToLower() == "admin" ? "admin" : userId;
                var contentType = file.ContentType;
                var key =
                    $"{bucket}/{Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName))}";

                await using var stream0 = file.OpenReadStream();
                var (out0, probeData) = await _ffmpegService.ConvertVideoToMp4(stream0, transpose: 2);
                var metadata = CreateMetadata(file, key, probeData, userId);

                await using var stream1 = out0;
                await UploadObjectToS3Async(key, stream1, metadata);

                if (contentType.Contains("video"))
                {
                    var thumbnail = $"{key}.thumbnail.png";
                    await using var stream3 = file.OpenReadStream();
                    var streams = await _ffmpegService.ExtractImagesFromVideo(stream3,
                        fps: 1 / (double.TryParse(probeData.Format.Duration, out double duration) ? duration : 1.0));
                    if (streams.Any())
                    {
                        await UploadObjectToS3Async(thumbnail, streams.First());
                    }

                    metadata.Thumbnail = thumbnail;
                }

                metadatas.Add(metadata);
            }

            return Ok(metadatas);
        }

        [HttpGet("probe/{**key}")]
        public async Task<IActionResult> GetProbeAsync(string key)
        {
            return await HandleS3RequestAsync(async () =>
            {
                var request = new Amazon.S3.Model.GetObjectRequest { BucketName = _bucket, Key = key };
                var response = await _s3Client.GetObjectAsync(request);
                await using var stream = response.ResponseStream;
                var probe = await _ffmpegService.Probe(stream);
                return Ok(probe);
            });
        }
        
        [HttpGet("metadata/{**key}")]
        public async Task<IActionResult> GetMetadataAsync(string key)
        {
            return await HandleS3RequestAsync(async () =>
            {
                var request = new Amazon.S3.Model.GetObjectRequest { BucketName = _bucket, Key = key };
                var response = await _s3Client.GetObjectAsync(request);
                var result = response.Metadata.Keys.ToDictionary<string, string, dynamic>(eachKey => eachKey.Replace("x-amz-meta-", ""), eachKey => response.Metadata[eachKey]);
                return Ok(result);
            });
        }

        [HttpGet("object/presigned/{**key}")]
        public IActionResult GetPresignedObjectUrl(string key)
        {
            try
            {
                var duration = TimeSpan.FromMinutes(15);
                var request = new Amazon.S3.Model.GetPreSignedUrlRequest
                {
                    BucketName = _bucket,
                    Key = key,
                    Expires = DateTime.UtcNow.Add(duration)
                };

                var url = _s3Client.GetPreSignedURL(request);
                var serviceUrl = _configuration.GetValue<string>("R2:ServiceURL");
                var domain = _configuration.GetValue<string>("R2:Domain");
                return Ok(url.Replace(serviceUrl, domain));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task UploadObjectToS3Async(string key, Stream stream, MediaMetadata metadata = null)
        {
            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _bucket,
                Key = key,
                InputStream = stream,
                DisablePayloadSigning = true
                //TODO: ObjectLockRetainUntilDate = DateTime.UtcNow.AddMinutes(10)
            };

            if (metadata != null)
            {;
                foreach (var each in metadata.ToDictionary())
                {
                    request.Metadata.Add(each.Key, each.Value);
                }
            }

            var response = await _s3Client.PutObjectAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to upload object {key}");
            }
        }

        private async Task<IActionResult> HandleS3RequestAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private MediaMetadata CreateMetadata(IFormFile file, string key, ProbeData probeData, string userId)
        {
            var aspectRatio = probeData?.Streams?.FirstOrDefault()?.DisplayAspectRatio;
            var bitRate = probeData?.Format?.BitRate;
            var codec = probeData?.Streams?.FirstOrDefault()?.CodecName;
            var contentType = file.ContentType;
            var duration = probeData?.Format?.Duration;
            var frameRate = probeData?.Streams?.FirstOrDefault()?.RFrameRate;
            var height = int.TryParse(probeData?.Streams?.FirstOrDefault()?.Height, out int heightOut) ? heightOut : 0;
            var size = probeData?.Format?.Size;
            var width = int.TryParse(probeData?.Streams?.FirstOrDefault()?.Width, out int widthOut) ? widthOut : 0;
            return new MediaMetadata
            (
                aspectRatio: aspectRatio,
                bitRate: bitRate,
                codec: codec,
                contentType: contentType,
                createdAt: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                duration: duration,
                filename: file.FileName,
                frameRate: frameRate,
                height: height,
                key: key,
                owner: userId,
                size: size,
                thumbnail: $"{key}.thumbnail.png",
                width: width
            );
        }
    }
}