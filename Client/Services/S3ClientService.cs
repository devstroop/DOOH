using System.Text.Json;
using System.Text.Json.Serialization;
using DOOH.Server.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DOOH.Client.Services
{
    public partial class S3ClientService
    {
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;

        public S3ClientService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {

            this.navigationManager = navigationManager;
            httpClient.BaseAddress = new Uri($"{navigationManager.BaseUri}api/s3/");
            this.httpClient = httpClient;
            
        }

        public async Task<IEnumerable<CustomS3ObjectModel>> GetObjectsAsync()
        {
            var response = await httpClient.GetAsync("objects");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(); // [{"checksumAlgorithm":[],"eTag":"\"af1e14c288cf17fee549fd2d3fb750f5\"","bucketName":"doohfy","key":"368a15eb-b1ab-4c66-b960-91d9fb5e16df/xbbain1i.mp4","lastModified":"2024-07-12T05:06:32.785+05:30","owner":null,"restoreStatus":null,"size":32510291,"storageClass":{"value":"STANDARD"}},{"checksumAlgorithm":[],"eTag":"\"af1e14c288cf17fee549fd2d3fb750f5\"","bucketName":"doohfy","key":"368a15eb-b1ab-4c66-b960-91d9fb5e16df/zyo5jegf.mp4","lastModified":"2024-07-12T05:12:37.719+05:30","owner":null,"restoreStatus":null,"size":32510291,"storageClass":{"value":"STANDARD"}}]
           
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            };

            var result = JsonSerializer.Deserialize<IEnumerable<CustomS3ObjectModel>>(json, options);
            return result; // result contains null values
        }
        
        // ProbeData
        public async Task<ProbeData> GetProbe(string key)
        {
            var response = await httpClient.GetAsync($"probe/{key}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProbeData>(jsonString);
        }
        // Metadata
        public async Task<MediaMetadata> GetMetadataAsync(string key)
        {
            var response = await httpClient.GetAsync($"metadata/{key}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MediaMetadata>(jsonString);
        }
        
        public async Task DeleteObjectAsync(string key)
        {
            var response = await httpClient.DeleteAsync($"object/{key}");
            response.EnsureSuccessStatusCode(); // Throws on non-success status codes
        }


        public async Task<string> GetPresignedUrlAsync(string key)
        {
            var response = await httpClient.GetAsync($"presigned/{key}");
            response.EnsureSuccessStatusCode(); // Throws on non-success status codes
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<byte[]> DownloadObjectAsync(string key)
        {
            var response = await httpClient.GetAsync($"object/{key}");
            response.EnsureSuccessStatusCode(); // Throws on non-success status codes
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<string> UploadObjectAsync(Stream fileStream, string fileName)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(fileStream);
            content.Add(fileContent, "file", fileName);

            var response = await httpClient.PostAsync("single", content);
            response.EnsureSuccessStatusCode(); // Throws on non-success status codes

            var responseJson = await response.Content.ReadAsStringAsync();
            var metadata = JsonSerializer.Deserialize<Dictionary<string, string>>(responseJson);
            return metadata["key"]; // Return the key of the uploaded object
        }

        public async Task<IEnumerable<string>> UploadObjectsAsync(IEnumerable<Stream> fileStreams, IEnumerable<string> fileNames)
        {
            var uploadTasks = new List<Task<string>>();

            foreach (var (fileStream, fileName) in fileStreams.Zip(fileNames, (stream, name) => (stream, name)))
            {
                uploadTasks.Add(UploadObjectAsync(fileStream, fileName));
            }

            var results = await Task.WhenAll(uploadTasks);
            return results;
        }
    }
}
