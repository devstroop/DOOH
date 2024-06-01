
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace DOOH.Client
{
    public partial class DOOHDBService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public DOOHDBService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/DOOHDB/");
        }


        public async System.Threading.Tasks.Task ExportAdboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdboards(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Adboard>> GetAdboards(Query query)
        {
            return await GetAdboards(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Adboard>> GetAdboards(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Adboards");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboards(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Adboard>>(response);
        }

        partial void OnCreateAdboard(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> CreateAdboard(DOOH.Server.Models.DOOHDB.Adboard adboard = default(DOOH.Server.Models.DOOHDB.Adboard))
        {
            var uri = new Uri(baseUri, $"Adboards");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboard), Encoding.UTF8, "application/json");

            OnCreateAdboard(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Adboard>(response);
        }

        partial void OnDeleteAdboard(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdboard(int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"Adboards({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdboardByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> GetAdboardByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"Adboards({adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Adboard>(response);
        }

        partial void OnUpdateAdboard(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdboard(int adboardId = default(int), DOOH.Server.Models.DOOHDB.Adboard adboard = default(DOOH.Server.Models.DOOHDB.Adboard))
        {
            var uri = new Uri(baseUri, $"Adboards({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", adboard.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboard), Encoding.UTF8, "application/json");

            OnUpdateAdboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAdboardImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdboardImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdboardImages(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardImage>> GetAdboardImages(Query query)
        {
            return await GetAdboardImages(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardImage>> GetAdboardImages(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AdboardImages");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardImages(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardImage>>(response);
        }

        partial void OnCreateAdboardImage(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> CreateAdboardImage(DOOH.Server.Models.DOOHDB.AdboardImage adboardImage = default(DOOH.Server.Models.DOOHDB.AdboardImage))
        {
            var uri = new Uri(baseUri, $"AdboardImages");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardImage), Encoding.UTF8, "application/json");

            OnCreateAdboardImage(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardImage>(response);
        }

        partial void OnDeleteAdboardImage(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdboardImage(int adboardImageId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardImages({adboardImageId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdboardImage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdboardImageByAdboardImageId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> GetAdboardImageByAdboardImageId(string expand = default(string), int adboardImageId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardImages({adboardImageId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardImageByAdboardImageId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardImage>(response);
        }

        partial void OnUpdateAdboardImage(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdboardImage(int adboardImageId = default(int), DOOH.Server.Models.DOOHDB.AdboardImage adboardImage = default(DOOH.Server.Models.DOOHDB.AdboardImage))
        {
            var uri = new Uri(baseUri, $"AdboardImages({adboardImageId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", adboardImage.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardImage), Encoding.UTF8, "application/json");

            OnUpdateAdboardImage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAdboardNetworksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardnetworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardnetworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdboardNetworksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardnetworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardnetworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdboardNetworks(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardNetwork>> GetAdboardNetworks(Query query)
        {
            return await GetAdboardNetworks(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardNetwork>> GetAdboardNetworks(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AdboardNetworks");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardNetworks(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardNetwork>>(response);
        }

        partial void OnCreateAdboardNetwork(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> CreateAdboardNetwork(DOOH.Server.Models.DOOHDB.AdboardNetwork adboardNetwork = default(DOOH.Server.Models.DOOHDB.AdboardNetwork))
        {
            var uri = new Uri(baseUri, $"AdboardNetworks");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardNetwork), Encoding.UTF8, "application/json");

            OnCreateAdboardNetwork(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardNetwork>(response);
        }

        partial void OnDeleteAdboardNetwork(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdboardNetwork(int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardNetworks({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdboardNetwork(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdboardNetworkByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> GetAdboardNetworkByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardNetworks({adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardNetworkByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardNetwork>(response);
        }

        partial void OnUpdateAdboardNetwork(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdboardNetwork(int adboardId = default(int), DOOH.Server.Models.DOOHDB.AdboardNetwork adboardNetwork = default(DOOH.Server.Models.DOOHDB.AdboardNetwork))
        {
            var uri = new Uri(baseUri, $"AdboardNetworks({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", adboardNetwork.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardNetwork), Encoding.UTF8, "application/json");

            OnUpdateAdboardNetwork(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAdboardWifisToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardwifis/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardwifis/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdboardWifisToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardwifis/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardwifis/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdboardWifis(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardWifi>> GetAdboardWifis(Query query)
        {
            return await GetAdboardWifis(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardWifi>> GetAdboardWifis(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AdboardWifis");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardWifis(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardWifi>>(response);
        }

        partial void OnCreateAdboardWifi(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> CreateAdboardWifi(DOOH.Server.Models.DOOHDB.AdboardWifi adboardWifi = default(DOOH.Server.Models.DOOHDB.AdboardWifi))
        {
            var uri = new Uri(baseUri, $"AdboardWifis");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardWifi), Encoding.UTF8, "application/json");

            OnCreateAdboardWifi(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardWifi>(response);
        }

        partial void OnDeleteAdboardWifi(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdboardWifi(int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardWifis({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdboardWifi(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdboardWifiByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> GetAdboardWifiByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardWifis({adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardWifiByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardWifi>(response);
        }

        partial void OnUpdateAdboardWifi(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdboardWifi(int adboardId = default(int), DOOH.Server.Models.DOOHDB.AdboardWifi adboardWifi = default(DOOH.Server.Models.DOOHDB.AdboardWifi))
        {
            var uri = new Uri(baseUri, $"AdboardWifis({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", adboardWifi.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardWifi), Encoding.UTF8, "application/json");

            OnUpdateAdboardWifi(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAdvertisementsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/advertisements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/advertisements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdvertisementsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/advertisements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/advertisements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdvertisements(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Advertisement>> GetAdvertisements(Query query)
        {
            return await GetAdvertisements(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Advertisement>> GetAdvertisements(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Advertisements");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdvertisements(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Advertisement>>(response);
        }

        partial void OnCreateAdvertisement(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> CreateAdvertisement(DOOH.Server.Models.DOOHDB.Advertisement advertisement = default(DOOH.Server.Models.DOOHDB.Advertisement))
        {
            var uri = new Uri(baseUri, $"Advertisements");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(advertisement), Encoding.UTF8, "application/json");

            OnCreateAdvertisement(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Advertisement>(response);
        }

        partial void OnDeleteAdvertisement(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdvertisement(int advertisementId = default(int))
        {
            var uri = new Uri(baseUri, $"Advertisements({advertisementId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdvertisement(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdvertisementByAdvertisementId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> GetAdvertisementByAdvertisementId(string expand = default(string), int advertisementId = default(int))
        {
            var uri = new Uri(baseUri, $"Advertisements({advertisementId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdvertisementByAdvertisementId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Advertisement>(response);
        }

        partial void OnUpdateAdvertisement(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdvertisement(int advertisementId = default(int), DOOH.Server.Models.DOOHDB.Advertisement advertisement = default(DOOH.Server.Models.DOOHDB.Advertisement))
        {
            var uri = new Uri(baseUri, $"Advertisements({advertisementId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", advertisement.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(advertisement), Encoding.UTF8, "application/json");

            OnUpdateAdvertisement(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAnalyticsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/analytics/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/analytics/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAnalyticsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/analytics/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/analytics/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAnalytics(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Analytic>> GetAnalytics(Query query)
        {
            return await GetAnalytics(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Analytic>> GetAnalytics(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Analytics");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAnalytics(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Analytic>>(response);
        }

        partial void OnCreateAnalytic(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Analytic> CreateAnalytic(DOOH.Server.Models.DOOHDB.Analytic analytic = default(DOOH.Server.Models.DOOHDB.Analytic))
        {
            var uri = new Uri(baseUri, $"Analytics");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(analytic), Encoding.UTF8, "application/json");

            OnCreateAnalytic(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Analytic>(response);
        }

        partial void OnDeleteAnalytic(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAnalytic(int analyticId = default(int))
        {
            var uri = new Uri(baseUri, $"Analytics({analyticId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAnalytic(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAnalyticByAnalyticId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Analytic> GetAnalyticByAnalyticId(string expand = default(string), int analyticId = default(int))
        {
            var uri = new Uri(baseUri, $"Analytics({analyticId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAnalyticByAnalyticId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Analytic>(response);
        }

        partial void OnUpdateAnalytic(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAnalytic(int analyticId = default(int), DOOH.Server.Models.DOOHDB.Analytic analytic = default(DOOH.Server.Models.DOOHDB.Analytic))
        {
            var uri = new Uri(baseUri, $"Analytics({analyticId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", analytic.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(analytic), Encoding.UTF8, "application/json");

            OnUpdateAnalytic(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAttachmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/attachments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/attachments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAttachmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/attachments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/attachments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAttachments(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Attachment>> GetAttachments(Query query)
        {
            return await GetAttachments(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Attachment>> GetAttachments(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Attachments");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAttachments(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Attachment>>(response);
        }

        partial void OnCreateAttachment(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Attachment> CreateAttachment(DOOH.Server.Models.DOOHDB.Attachment attachment = default(DOOH.Server.Models.DOOHDB.Attachment))
        {
            var uri = new Uri(baseUri, $"Attachments");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(attachment), Encoding.UTF8, "application/json");

            OnCreateAttachment(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Attachment>(response);
        }

        partial void OnDeleteAttachment(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAttachment(string attachmentKey = default(string))
        {
            var uri = new Uri(baseUri, $"Attachments('{Uri.EscapeDataString(attachmentKey.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAttachment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAttachmentByAttachmentKey(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Attachment> GetAttachmentByAttachmentKey(string expand = default(string), string attachmentKey = default(string))
        {
            var uri = new Uri(baseUri, $"Attachments('{Uri.EscapeDataString(attachmentKey.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAttachmentByAttachmentKey(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Attachment>(response);
        }

        partial void OnUpdateAttachment(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAttachment(string attachmentKey = default(string), DOOH.Server.Models.DOOHDB.Attachment attachment = default(DOOH.Server.Models.DOOHDB.Attachment))
        {
            var uri = new Uri(baseUri, $"Attachments('{Uri.EscapeDataString(attachmentKey.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", attachment.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(attachment), Encoding.UTF8, "application/json");

            OnUpdateAttachment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportBillingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/billings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/billings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportBillingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/billings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/billings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetBillings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Billing>> GetBillings(Query query)
        {
            return await GetBillings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Billing>> GetBillings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Billings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBillings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Billing>>(response);
        }

        partial void OnCreateBilling(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Billing> CreateBilling(DOOH.Server.Models.DOOHDB.Billing billing = default(DOOH.Server.Models.DOOHDB.Billing))
        {
            var uri = new Uri(baseUri, $"Billings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(billing), Encoding.UTF8, "application/json");

            OnCreateBilling(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Billing>(response);
        }

        partial void OnDeleteBilling(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteBilling(int billingId = default(int))
        {
            var uri = new Uri(baseUri, $"Billings({billingId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteBilling(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetBillingByBillingId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Billing> GetBillingByBillingId(string expand = default(string), int billingId = default(int))
        {
            var uri = new Uri(baseUri, $"Billings({billingId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBillingByBillingId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Billing>(response);
        }

        partial void OnUpdateBilling(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateBilling(int billingId = default(int), DOOH.Server.Models.DOOHDB.Billing billing = default(DOOH.Server.Models.DOOHDB.Billing))
        {
            var uri = new Uri(baseUri, $"Billings({billingId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", billing.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(billing), Encoding.UTF8, "application/json");

            OnUpdateBilling(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportBrandsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportBrandsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetBrands(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Brand>> GetBrands(Query query)
        {
            return await GetBrands(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Brand>> GetBrands(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Brands");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBrands(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Brand>>(response);
        }

        partial void OnCreateBrand(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Brand> CreateBrand(DOOH.Server.Models.DOOHDB.Brand brand = default(DOOH.Server.Models.DOOHDB.Brand))
        {
            var uri = new Uri(baseUri, $"Brands");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(brand), Encoding.UTF8, "application/json");

            OnCreateBrand(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Brand>(response);
        }

        partial void OnDeleteBrand(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteBrand(int brandId = default(int))
        {
            var uri = new Uri(baseUri, $"Brands({brandId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteBrand(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetBrandByBrandId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Brand> GetBrandByBrandId(string expand = default(string), int brandId = default(int))
        {
            var uri = new Uri(baseUri, $"Brands({brandId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBrandByBrandId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Brand>(response);
        }

        partial void OnUpdateBrand(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateBrand(int brandId = default(int), DOOH.Server.Models.DOOHDB.Brand brand = default(DOOH.Server.Models.DOOHDB.Brand))
        {
            var uri = new Uri(baseUri, $"Brands({brandId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", brand.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(brand), Encoding.UTF8, "application/json");

            OnUpdateBrand(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCampaignsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCampaignsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCampaigns(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Campaign>> GetCampaigns(Query query)
        {
            return await GetCampaigns(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Campaign>> GetCampaigns(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Campaigns");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCampaigns(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Campaign>>(response);
        }

        partial void OnCreateCampaign(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Campaign> CreateCampaign(DOOH.Server.Models.DOOHDB.Campaign campaign = default(DOOH.Server.Models.DOOHDB.Campaign))
        {
            var uri = new Uri(baseUri, $"Campaigns");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaign), Encoding.UTF8, "application/json");

            OnCreateCampaign(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Campaign>(response);
        }

        partial void OnDeleteCampaign(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCampaign(int campaignId = default(int))
        {
            var uri = new Uri(baseUri, $"Campaigns({campaignId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCampaign(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCampaignByCampaignId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Campaign> GetCampaignByCampaignId(string expand = default(string), int campaignId = default(int))
        {
            var uri = new Uri(baseUri, $"Campaigns({campaignId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCampaignByCampaignId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Campaign>(response);
        }

        partial void OnUpdateCampaign(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCampaign(int campaignId = default(int), DOOH.Server.Models.DOOHDB.Campaign campaign = default(DOOH.Server.Models.DOOHDB.Campaign))
        {
            var uri = new Uri(baseUri, $"Campaigns({campaignId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", campaign.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaign), Encoding.UTF8, "application/json");

            OnUpdateCampaign(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCampaignAdboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaignadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaignadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCampaignAdboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaignadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaignadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCampaignAdboards(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.CampaignAdboard>> GetCampaignAdboards(Query query)
        {
            return await GetCampaignAdboards(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.CampaignAdboard>> GetCampaignAdboards(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"CampaignAdboards");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCampaignAdboards(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.CampaignAdboard>>(response);
        }

        partial void OnCreateCampaignAdboard(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> CreateCampaignAdboard(DOOH.Server.Models.DOOHDB.CampaignAdboard campaignAdboard = default(DOOH.Server.Models.DOOHDB.CampaignAdboard))
        {
            var uri = new Uri(baseUri, $"CampaignAdboards");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaignAdboard), Encoding.UTF8, "application/json");

            OnCreateCampaignAdboard(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.CampaignAdboard>(response);
        }

        partial void OnDeleteCampaignAdboard(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCampaignAdboard(int campaignId = default(int), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"CampaignAdboards(CampaignId={campaignId},AdboardId={adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCampaignAdboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCampaignAdboardByCampaignIdAndAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> GetCampaignAdboardByCampaignIdAndAdboardId(string expand = default(string), int campaignId = default(int), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"CampaignAdboards(CampaignId={campaignId},AdboardId={adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCampaignAdboardByCampaignIdAndAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.CampaignAdboard>(response);
        }

        partial void OnUpdateCampaignAdboard(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCampaignAdboard(int campaignId = default(int), int adboardId = default(int), DOOH.Server.Models.DOOHDB.CampaignAdboard campaignAdboard = default(DOOH.Server.Models.DOOHDB.CampaignAdboard))
        {
            var uri = new Uri(baseUri, $"CampaignAdboards(CampaignId={campaignId},AdboardId={adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", campaignAdboard.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaignAdboard), Encoding.UTF8, "application/json");

            OnUpdateCampaignAdboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCategories(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Category>> GetCategories(Query query)
        {
            return await GetCategories(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Category>> GetCategories(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Categories");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCategories(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Category>>(response);
        }

        partial void OnCreateCategory(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Category> CreateCategory(DOOH.Server.Models.DOOHDB.Category category = default(DOOH.Server.Models.DOOHDB.Category))
        {
            var uri = new Uri(baseUri, $"Categories");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

            OnCreateCategory(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Category>(response);
        }

        partial void OnDeleteCategory(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCategory(int categoryId = default(int))
        {
            var uri = new Uri(baseUri, $"Categories({categoryId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCategory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCategoryByCategoryId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Category> GetCategoryByCategoryId(string expand = default(string), int categoryId = default(int))
        {
            var uri = new Uri(baseUri, $"Categories({categoryId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCategoryByCategoryId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Category>(response);
        }

        partial void OnUpdateCategory(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCategory(int categoryId = default(int), DOOH.Server.Models.DOOHDB.Category category = default(DOOH.Server.Models.DOOHDB.Category))
        {
            var uri = new Uri(baseUri, $"Categories({categoryId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", category.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

            OnUpdateCategory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCitiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/cities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/cities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCitiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/cities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/cities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCities(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.City>> GetCities(Query query)
        {
            return await GetCities(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.City>> GetCities(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Cities");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCities(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.City>>(response);
        }

        partial void OnCreateCity(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.City> CreateCity(DOOH.Server.Models.DOOHDB.City city = default(DOOH.Server.Models.DOOHDB.City))
        {
            var uri = new Uri(baseUri, $"Cities");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(city), Encoding.UTF8, "application/json");

            OnCreateCity(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.City>(response);
        }

        partial void OnDeleteCity(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCity(string cityName = default(string))
        {
            var uri = new Uri(baseUri, $"Cities('{Uri.EscapeDataString(cityName.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCity(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCityByCityName(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.City> GetCityByCityName(string expand = default(string), string cityName = default(string))
        {
            var uri = new Uri(baseUri, $"Cities('{Uri.EscapeDataString(cityName.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCityByCityName(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.City>(response);
        }

        partial void OnUpdateCity(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCity(string cityName = default(string), DOOH.Server.Models.DOOHDB.City city = default(DOOH.Server.Models.DOOHDB.City))
        {
            var uri = new Uri(baseUri, $"Cities('{Uri.EscapeDataString(cityName.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", city.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(city), Encoding.UTF8, "application/json");

            OnUpdateCity(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCountriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/countries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/countries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCountriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/countries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/countries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCountries(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Country>> GetCountries(Query query)
        {
            return await GetCountries(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Country>> GetCountries(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Countries");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCountries(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Country>>(response);
        }

        partial void OnCreateCountry(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Country> CreateCountry(DOOH.Server.Models.DOOHDB.Country country = default(DOOH.Server.Models.DOOHDB.Country))
        {
            var uri = new Uri(baseUri, $"Countries");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(country), Encoding.UTF8, "application/json");

            OnCreateCountry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Country>(response);
        }

        partial void OnDeleteCountry(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCountry(string countryName = default(string))
        {
            var uri = new Uri(baseUri, $"Countries('{Uri.EscapeDataString(countryName.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCountry(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCountryByCountryName(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Country> GetCountryByCountryName(string expand = default(string), string countryName = default(string))
        {
            var uri = new Uri(baseUri, $"Countries('{Uri.EscapeDataString(countryName.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCountryByCountryName(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Country>(response);
        }

        partial void OnUpdateCountry(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCountry(string countryName = default(string), DOOH.Server.Models.DOOHDB.Country country = default(DOOH.Server.Models.DOOHDB.Country))
        {
            var uri = new Uri(baseUri, $"Countries('{Uri.EscapeDataString(countryName.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", country.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(country), Encoding.UTF8, "application/json");

            OnUpdateCountry(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportDisplaysToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/displays/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/displays/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportDisplaysToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/displays/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/displays/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetDisplays(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Display>> GetDisplays(Query query)
        {
            return await GetDisplays(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Display>> GetDisplays(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Displays");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDisplays(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Display>>(response);
        }

        partial void OnCreateDisplay(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Display> CreateDisplay(DOOH.Server.Models.DOOHDB.Display display = default(DOOH.Server.Models.DOOHDB.Display))
        {
            var uri = new Uri(baseUri, $"Displays");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(display), Encoding.UTF8, "application/json");

            OnCreateDisplay(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Display>(response);
        }

        partial void OnDeleteDisplay(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteDisplay(int displayId = default(int))
        {
            var uri = new Uri(baseUri, $"Displays({displayId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteDisplay(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetDisplayByDisplayId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Display> GetDisplayByDisplayId(string expand = default(string), int displayId = default(int))
        {
            var uri = new Uri(baseUri, $"Displays({displayId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDisplayByDisplayId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Display>(response);
        }

        partial void OnUpdateDisplay(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateDisplay(int displayId = default(int), DOOH.Server.Models.DOOHDB.Display display = default(DOOH.Server.Models.DOOHDB.Display))
        {
            var uri = new Uri(baseUri, $"Displays({displayId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", display.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(display), Encoding.UTF8, "application/json");

            OnUpdateDisplay(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportEarningsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/earnings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/earnings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportEarningsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/earnings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/earnings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetEarnings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Earning>> GetEarnings(Query query)
        {
            return await GetEarnings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Earning>> GetEarnings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Earnings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEarnings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Earning>>(response);
        }

        partial void OnCreateEarning(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Earning> CreateEarning(DOOH.Server.Models.DOOHDB.Earning earning = default(DOOH.Server.Models.DOOHDB.Earning))
        {
            var uri = new Uri(baseUri, $"Earnings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(earning), Encoding.UTF8, "application/json");

            OnCreateEarning(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Earning>(response);
        }

        partial void OnDeleteEarning(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteEarning(int earningId = default(int))
        {
            var uri = new Uri(baseUri, $"Earnings({earningId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteEarning(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetEarningByEarningId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Earning> GetEarningByEarningId(string expand = default(string), int earningId = default(int))
        {
            var uri = new Uri(baseUri, $"Earnings({earningId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEarningByEarningId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Earning>(response);
        }

        partial void OnUpdateEarning(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateEarning(int earningId = default(int), DOOH.Server.Models.DOOHDB.Earning earning = default(DOOH.Server.Models.DOOHDB.Earning))
        {
            var uri = new Uri(baseUri, $"Earnings({earningId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", earning.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(earning), Encoding.UTF8, "application/json");

            OnUpdateEarning(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMotherboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/motherboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/motherboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMotherboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/motherboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/motherboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMotherboards(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Motherboard>> GetMotherboards(Query query)
        {
            return await GetMotherboards(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Motherboard>> GetMotherboards(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Motherboards");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMotherboards(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Motherboard>>(response);
        }

        partial void OnCreateMotherboard(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> CreateMotherboard(DOOH.Server.Models.DOOHDB.Motherboard motherboard = default(DOOH.Server.Models.DOOHDB.Motherboard))
        {
            var uri = new Uri(baseUri, $"Motherboards");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(motherboard), Encoding.UTF8, "application/json");

            OnCreateMotherboard(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Motherboard>(response);
        }

        partial void OnDeleteMotherboard(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMotherboard(int motherboardId = default(int))
        {
            var uri = new Uri(baseUri, $"Motherboards({motherboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMotherboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMotherboardByMotherboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> GetMotherboardByMotherboardId(string expand = default(string), int motherboardId = default(int))
        {
            var uri = new Uri(baseUri, $"Motherboards({motherboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMotherboardByMotherboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Motherboard>(response);
        }

        partial void OnUpdateMotherboard(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMotherboard(int motherboardId = default(int), DOOH.Server.Models.DOOHDB.Motherboard motherboard = default(DOOH.Server.Models.DOOHDB.Motherboard))
        {
            var uri = new Uri(baseUri, $"Motherboards({motherboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", motherboard.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(motherboard), Encoding.UTF8, "application/json");

            OnUpdateMotherboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportProvidersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/providers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/providers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportProvidersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/providers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/providers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetProviders(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Provider>> GetProviders(Query query)
        {
            return await GetProviders(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Provider>> GetProviders(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Providers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetProviders(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Provider>>(response);
        }

        partial void OnCreateProvider(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Provider> CreateProvider(DOOH.Server.Models.DOOHDB.Provider provider = default(DOOH.Server.Models.DOOHDB.Provider))
        {
            var uri = new Uri(baseUri, $"Providers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(provider), Encoding.UTF8, "application/json");

            OnCreateProvider(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Provider>(response);
        }

        partial void OnDeleteProvider(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteProvider(int providerId = default(int))
        {
            var uri = new Uri(baseUri, $"Providers({providerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteProvider(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetProviderByProviderId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Provider> GetProviderByProviderId(string expand = default(string), int providerId = default(int))
        {
            var uri = new Uri(baseUri, $"Providers({providerId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetProviderByProviderId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Provider>(response);
        }

        partial void OnUpdateProvider(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateProvider(int providerId = default(int), DOOH.Server.Models.DOOHDB.Provider provider = default(DOOH.Server.Models.DOOHDB.Provider))
        {
            var uri = new Uri(baseUri, $"Providers({providerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", provider.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(provider), Encoding.UTF8, "application/json");

            OnUpdateProvider(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStates(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.State>> GetStates(Query query)
        {
            return await GetStates(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.State>> GetStates(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"States");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStates(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.State>>(response);
        }

        partial void OnCreateState(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.State> CreateState(DOOH.Server.Models.DOOHDB.State state = default(DOOH.Server.Models.DOOHDB.State))
        {
            var uri = new Uri(baseUri, $"States");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(state), Encoding.UTF8, "application/json");

            OnCreateState(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.State>(response);
        }

        partial void OnDeleteState(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteState(string stateName = default(string))
        {
            var uri = new Uri(baseUri, $"States('{Uri.EscapeDataString(stateName.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteState(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStateByStateName(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.State> GetStateByStateName(string expand = default(string), string stateName = default(string))
        {
            var uri = new Uri(baseUri, $"States('{Uri.EscapeDataString(stateName.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStateByStateName(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.State>(response);
        }

        partial void OnUpdateState(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateState(string stateName = default(string), DOOH.Server.Models.DOOHDB.State state = default(DOOH.Server.Models.DOOHDB.State))
        {
            var uri = new Uri(baseUri, $"States('{Uri.EscapeDataString(stateName.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", state.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(state), Encoding.UTF8, "application/json");

            OnUpdateState(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTaxesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/taxes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/taxes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTaxesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/taxes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/taxes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTaxes(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Tax>> GetTaxes(Query query)
        {
            return await GetTaxes(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Tax>> GetTaxes(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Taxes");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTaxes(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Tax>>(response);
        }

        partial void OnCreateTax(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Tax> CreateTax(DOOH.Server.Models.DOOHDB.Tax tax = default(DOOH.Server.Models.DOOHDB.Tax))
        {
            var uri = new Uri(baseUri, $"Taxes");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tax), Encoding.UTF8, "application/json");

            OnCreateTax(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Tax>(response);
        }

        partial void OnDeleteTax(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTax(int taxId = default(int))
        {
            var uri = new Uri(baseUri, $"Taxes({taxId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTax(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTaxByTaxId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Tax> GetTaxByTaxId(string expand = default(string), int taxId = default(int))
        {
            var uri = new Uri(baseUri, $"Taxes({taxId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTaxByTaxId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Tax>(response);
        }

        partial void OnUpdateTax(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTax(int taxId = default(int), DOOH.Server.Models.DOOHDB.Tax tax = default(DOOH.Server.Models.DOOHDB.Tax))
        {
            var uri = new Uri(baseUri, $"Taxes({taxId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", tax.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tax), Encoding.UTF8, "application/json");

            OnUpdateTax(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}