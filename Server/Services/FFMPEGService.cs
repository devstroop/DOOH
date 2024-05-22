using DOOH.Server.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DOOH.Server.Services
{
    public class FFMPEGService
    {
        private readonly HttpClient _httpClient;

        public FFMPEGService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("FFMpeg:Endpoint"));
        }

        public async Task<ProbeData> Probe(Stream stream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");

            var response = await _httpClient.PostAsync("/probe", content);
            response.EnsureSuccessStatusCode();
            //return await response.Content.ReadAsStringAsync();
            var jsonString = await response.Content.ReadAsStringAsync();
            return Extensions.JsonExtensions.DeserializeSnakeCase<ProbeData>(jsonString);
        }

        public async Task<Stream> ConvertAudioToMp3(Stream stream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");

            var response = await _httpClient.PostAsync("/convert/audio/to/mp3", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> ConvertAudioToWav(Stream stream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");

            var response = await _httpClient.PostAsync("/convert/audio/to/wav", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> ConvertVideoToMp4(Stream stream, int? transpose = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");
            var response = await _httpClient.PostAsync($"/convert/video/to/mp4{(transpose != null ? $"?transpose={transpose}" : "")}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> ConvertImageToJpg(Stream stream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");

            var response = await _httpClient.PostAsync("/convert/image/to/jpg", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> ExtractAudioFromVideo(Stream stream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");

            var response = await _httpClient.PostAsync("/video/extract/audio", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<List<Stream>> ExtractImagesFromVideo(Stream stream, double fps = 1.0)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "file");

            var response = await _httpClient.PostAsync($"/video/extract/images?fps={fps}", content);
            response.EnsureSuccessStatusCode();
            // sample1: {"totalfiles":2,"description":"Extracted image files and URLs to download them. By default, downloading image also deletes the image from server. Note that port 3000 in the URL may not be the same as the real port, especially if server is running on Docker/Kubernetes.","files":[{"name":"01b50617-0001.png","url":"http://ffmpeg-api.devstroop.com:3000/video/extract/download/01b50617-0001.png"},{"name":"01b50617-0002.png","url":"http://ffmpeg-api.devstroop.com:3000/video/extract/download/01b50617-0002.png"}]}
            var json = await Radzen.HttpResponseMessageExtensions.ReadAsync<Dictionary<string, dynamic>>(response);
            var files = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json["files"].ToString());
            List<Stream> streams = new List<Stream>();

            foreach (var file in files)
            {
                var response2 = await _httpClient.GetAsync(file["url"]);
                response2.EnsureSuccessStatusCode();
                streams.Add(await response2.Content.ReadAsStreamAsync());
            }
            return streams;
        }

        public async Task<Stream> DownloadVideoExtract(string filename)
        {
            var response = await _httpClient.GetAsync($"/video/extract/download/{filename}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<string> GetEndpoints()
        {
            var response = await _httpClient.GetAsync("/endpoints");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
