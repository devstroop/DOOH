using Amazon.S3.Model;
using DOOH.Server.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

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

        public async Task<IEnumerable<S3Object>> GetObjectsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("objects");
                response.EnsureSuccessStatusCode(); // Throws on non-success status codes
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<S3Object>>(json);
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                Console.WriteLine($"Error fetching objects: {ex.Message}");
                throw;
            }
        }
        
        // ProbeData
        public async Task<ProbeData> GetProbe(string key)
        {
            var response = await httpClient.GetAsync($"probe/{key}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProbeData>(jsonString);
        }
        // Metadata
        public async Task<Dictionary<string, dynamic>> GetMetadataAsync(string key)
        {
            var response = await httpClient.GetAsync($"metadata/{key}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
        }
        
        public async Task DeleteObjectAsync(string key)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"object/{key}");
                response.EnsureSuccessStatusCode(); // Throws on non-success status codes
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                Console.WriteLine($"Error deleting object {key}: {ex.Message}");
                throw;
            }
        }


        public async Task<string> GetPresignedUrlAsync(string key)
        {
            try
            {
                var response = await httpClient.GetAsync($"presigned/{key}");
                response.EnsureSuccessStatusCode(); // Throws on non-success status codes
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                Console.WriteLine($"Error fetching presigned URL for {key}: {ex.Message}");
                throw;
            }
        }

        public async Task<byte[]> DownloadObjectAsync(string key)
        {
            try
            {
                var response = await httpClient.GetAsync($"object/{key}");
                response.EnsureSuccessStatusCode(); // Throws on non-success status codes
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                Console.WriteLine($"Error downloading object {key}: {ex.Message}");
                throw;
            }
        }

        public async Task<string> UploadObjectAsync(Stream fileStream, string fileName)
        {
            try
            {
                var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                content.Add(fileContent, "file", fileName);

                var response = await httpClient.PostAsync("single", content);
                response.EnsureSuccessStatusCode(); // Throws on non-success status codes

                var responseJson = await response.Content.ReadAsStringAsync();
                var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);
                return metadata["key"]; // Return the key of the uploaded object
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                Console.WriteLine($"Error uploading object {fileName}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<string>> UploadObjectsAsync(IEnumerable<Stream> fileStreams, IEnumerable<string> fileNames)
        {
            try
            {
                var uploadTasks = new List<Task<string>>();

                foreach (var (fileStream, fileName) in fileStreams.Zip(fileNames, (stream, name) => (stream, name)))
                {
                    uploadTasks.Add(UploadObjectAsync(fileStream, fileName));
                }

                var results = await Task.WhenAll(uploadTasks);
                return results;
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                Console.WriteLine($"Error uploading objects: {ex.Message}");
                throw;
            }
        }
    }
}
