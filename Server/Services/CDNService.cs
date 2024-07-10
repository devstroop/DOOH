using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace DOOH.Server.Services
{
    public class CDNService
    {
        private readonly Amazon.S3.IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;
        private readonly string _bucket;

        public CDNService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var serviceUrl = configuration.GetValue<string>("R2:ServiceURL");
            var accessKey = configuration.GetValue<string>("R2:AccessKey");
            var secretKey = configuration.GetValue<string>("R2:SecretKey");
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(credentials, new AmazonS3Config { ServiceURL = serviceUrl });
            _bucket = configuration.GetValue<string>("R2:Bucket");
        }

        /// <summary>
        /// Lists objects in S3.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Amazon.S3.Model.S3Object>> ListObjectsAsync(string directory = null, CancellationToken cancellationToken = default)
        {
            var request = new Amazon.S3.Model.ListObjectsV2Request
            {
                BucketName = _bucket
            };

            var response = await _s3Client.ListObjectsV2Async(request, cancellationToken);
            return (string.IsNullOrWhiteSpace(directory) || directory?.Trim() == "/") ? response.S3Objects : response.S3Objects.Where(x => x.Key.StartsWith(directory));
        }

        /// <summary>
        /// Lists presigned object URLs in S3.
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<string>> ListPresignedObjectUrlsAsync(TimeSpan duration, CancellationToken cancellationToken = default)
        {
            var request = new Amazon.S3.Model.ListObjectsV2Request
            {
                BucketName = _bucket
            };

            var response = await _s3Client.ListObjectsV2Async(request, cancellationToken);
            return response.S3Objects.Select(obj => GetPresignedObjectUrl(obj.Key, duration)).ToList();
        }

        /// <summary>
        /// Gets an object from S3.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Stream> GetObjectAsync(string key, CancellationToken cancellationToken = default)
        {
            var request = new Amazon.S3.Model.GetObjectRequest
            {
                BucketName = _bucket,
                Key = key
            };
            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            return response.ResponseStream;
        }

        /// <summary>
        /// Get Metadata of an object in S3.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AmazonS3Exception"></exception>
        public async Task<MetadataCollection> GetObjectMetadataAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var request = new Amazon.S3.Model.GetObjectMetadataRequest
            {
                BucketName = _bucket,
                Key = key
            };
            try
            {
                var response = await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
                return response.Metadata;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;
                throw;
            }
        }
        
        
        /// <summary>
        /// Generates a presigned URL for an object in S3.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetPresignedObjectUrl(string key, TimeSpan duration)
        {
            var request = new Amazon.S3.Model.GetPreSignedUrlRequest
            {
                Key = key,
                Expires = DateTime.UtcNow.Add(duration)
            };

            var url = _s3Client.GetPreSignedURL(request);
            var serviceUrl = _configuration.GetValue<string>("R2:ServiceURL");
            var domain = _configuration.GetValue<string>("R2:Domain");
            return url.Replace(serviceUrl, domain);
        }

        /// <summary>
        /// Uploads an object to S3.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="stream"></param>
        /// <param name="metadata"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Dictionary<string, string>> UploadObjectAsync(string key, Stream stream, Dictionary<string, string> metadata = null, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _bucket,
                Key = key,
                InputStream = stream,
                DisablePayloadSigning = true,
                //TODO: ObjectLockRetainUntilDate = DateTime.UtcNow.AddMinutes(10)
            };
            foreach (var each in metadata ?? new Dictionary<string, string>())
            {
                request.Metadata.Add(each.Key, each.Value);
            }
            var response = await _s3Client.PutObjectAsync(request, cancellationToken);
            return response?.HttpStatusCode == HttpStatusCode.OK ? response.ResponseMetadata.Metadata.ToDictionary() : null;
        }

        /// <summary>
        /// Deletes an object from S3.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteObjectAsync(string key, CancellationToken cancellationToken = default)
        {
            var request = new Amazon.S3.Model.DeleteObjectRequest
            {
                BucketName = _bucket,
                Key = key
            };
            var response = await _s3Client.DeleteObjectAsync(request, cancellationToken);
            return response?.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
