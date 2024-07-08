
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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardNetwork), Encoding.UTF8, "application/json");

            OnUpdateAdboardNetwork(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAdboardStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdboardStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdboardStatuses(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardStatus>> GetAdboardStatuses(Query query)
        {
            return await GetAdboardStatuses(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardStatus>> GetAdboardStatuses(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AdboardStatuses");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardStatuses(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardStatus>>(response);
        }

        partial void OnCreateAdboardStatus(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> CreateAdboardStatus(DOOH.Server.Models.DOOHDB.AdboardStatus adboardStatus = default(DOOH.Server.Models.DOOHDB.AdboardStatus))
        {
            var uri = new Uri(baseUri, $"AdboardStatuses");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardStatus), Encoding.UTF8, "application/json");

            OnCreateAdboardStatus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardStatus>(response);
        }

        partial void OnDeleteAdboardStatus(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdboardStatus(int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardStatuses({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdboardStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdboardStatusByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> GetAdboardStatusByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"AdboardStatuses({adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardStatusByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardStatus>(response);
        }

        partial void OnUpdateAdboardStatus(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdboardStatus(int adboardId = default(int), DOOH.Server.Models.DOOHDB.AdboardStatus adboardStatus = default(DOOH.Server.Models.DOOHDB.AdboardStatus))
        {
            var uri = new Uri(baseUri, $"AdboardStatuses({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardStatus), Encoding.UTF8, "application/json");

            OnUpdateAdboardStatus(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(analytic), Encoding.UTF8, "application/json");

            OnUpdateAnalytic(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaignAdboard), Encoding.UTF8, "application/json");

            OnUpdateCampaignAdboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCampaignCriteriaToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigncriteria/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigncriteria/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCampaignCriteriaToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigncriteria/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigncriteria/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCampaignCriteria(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.CampaignCriterion>> GetCampaignCriteria(Query query)
        {
            return await GetCampaignCriteria(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.CampaignCriterion>> GetCampaignCriteria(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"CampaignCriteria");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCampaignCriteria(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.CampaignCriterion>>(response);
        }

        partial void OnCreateCampaignCriterion(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> CreateCampaignCriterion(DOOH.Server.Models.DOOHDB.CampaignCriterion campaignCriterion = default(DOOH.Server.Models.DOOHDB.CampaignCriterion))
        {
            var uri = new Uri(baseUri, $"CampaignCriteria");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaignCriterion), Encoding.UTF8, "application/json");

            OnCreateCampaignCriterion(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.CampaignCriterion>(response);
        }

        partial void OnDeleteCampaignCriterion(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCampaignCriterion(int campaignCriteriaId = default(int))
        {
            var uri = new Uri(baseUri, $"CampaignCriteria({campaignCriteriaId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCampaignCriterion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCampaignCriterionByCampaignCriteriaId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> GetCampaignCriterionByCampaignCriteriaId(string expand = default(string), int campaignCriteriaId = default(int))
        {
            var uri = new Uri(baseUri, $"CampaignCriteria({campaignCriteriaId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCampaignCriterionByCampaignCriteriaId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.CampaignCriterion>(response);
        }

        partial void OnUpdateCampaignCriterion(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCampaignCriterion(int campaignCriteriaId = default(int), DOOH.Server.Models.DOOHDB.CampaignCriterion campaignCriterion = default(DOOH.Server.Models.DOOHDB.CampaignCriterion))
        {
            var uri = new Uri(baseUri, $"CampaignCriteria({campaignCriteriaId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(campaignCriterion), Encoding.UTF8, "application/json");

            OnUpdateCampaignCriterion(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

            OnUpdateCategory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCompaniesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCompaniesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/companies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/companies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCompanies(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Company>> GetCompanies(Query query)
        {
            return await GetCompanies(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Company>> GetCompanies(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Companies");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompanies(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Company>>(response);
        }

        partial void OnCreateCompany(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Company> CreateCompany(DOOH.Server.Models.DOOHDB.Company company = default(DOOH.Server.Models.DOOHDB.Company))
        {
            var uri = new Uri(baseUri, $"Companies");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(company), Encoding.UTF8, "application/json");

            OnCreateCompany(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Company>(response);
        }

        partial void OnDeleteCompany(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCompany(string key = default(string))
        {
            var uri = new Uri(baseUri, $"Companies('{Uri.EscapeDataString(key.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCompany(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCompanyByKey(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Company> GetCompanyByKey(string expand = default(string), string key = default(string))
        {
            var uri = new Uri(baseUri, $"Companies('{Uri.EscapeDataString(key.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompanyByKey(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Company>(response);
        }

        partial void OnUpdateCompany(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCompany(string key = default(string), DOOH.Server.Models.DOOHDB.Company company = default(DOOH.Server.Models.DOOHDB.Company))
        {
            var uri = new Uri(baseUri, $"Companies('{Uri.EscapeDataString(key.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(company), Encoding.UTF8, "application/json");

            OnUpdateCompany(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(earning), Encoding.UTF8, "application/json");

            OnUpdateEarning(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportFaqsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/faqs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/faqs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportFaqsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/faqs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/faqs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetFaqs(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Faq>> GetFaqs(Query query)
        {
            return await GetFaqs(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Faq>> GetFaqs(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Faqs");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetFaqs(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Faq>>(response);
        }

        partial void OnCreateFaq(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Faq> CreateFaq(DOOH.Server.Models.DOOHDB.Faq faq = default(DOOH.Server.Models.DOOHDB.Faq))
        {
            var uri = new Uri(baseUri, $"Faqs");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(faq), Encoding.UTF8, "application/json");

            OnCreateFaq(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Faq>(response);
        }

        partial void OnDeleteFaq(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteFaq(int faqId = default(int))
        {
            var uri = new Uri(baseUri, $"Faqs({faqId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteFaq(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetFaqByFaqId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Faq> GetFaqByFaqId(string expand = default(string), int faqId = default(int))
        {
            var uri = new Uri(baseUri, $"Faqs({faqId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetFaqByFaqId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Faq>(response);
        }

        partial void OnUpdateFaq(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateFaq(int faqId = default(int), DOOH.Server.Models.DOOHDB.Faq faq = default(DOOH.Server.Models.DOOHDB.Faq))
        {
            var uri = new Uri(baseUri, $"Faqs({faqId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(faq), Encoding.UTF8, "application/json");

            OnUpdateFaq(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(motherboard), Encoding.UTF8, "application/json");

            OnUpdateMotherboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/pages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/pages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/pages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/pages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPages(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Page>> GetPages(Query query)
        {
            return await GetPages(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Page>> GetPages(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Pages");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPages(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Page>>(response);
        }

        partial void OnCreatePage(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Page> CreatePage(DOOH.Server.Models.DOOHDB.Page page = default(DOOH.Server.Models.DOOHDB.Page))
        {
            var uri = new Uri(baseUri, $"Pages");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(page), Encoding.UTF8, "application/json");

            OnCreatePage(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Page>(response);
        }

        partial void OnDeletePage(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePage(string slag = default(string))
        {
            var uri = new Uri(baseUri, $"Pages('{Uri.EscapeDataString(slag.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPageBySlag(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Page> GetPageBySlag(string expand = default(string), string slag = default(string))
        {
            var uri = new Uri(baseUri, $"Pages('{Uri.EscapeDataString(slag.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPageBySlag(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Page>(response);
        }

        partial void OnUpdatePage(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePage(string slag = default(string), DOOH.Server.Models.DOOHDB.Page page = default(DOOH.Server.Models.DOOHDB.Page))
        {
            var uri = new Uri(baseUri, $"Pages('{Uri.EscapeDataString(slag.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(page), Encoding.UTF8, "application/json");

            OnUpdatePage(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(provider), Encoding.UTF8, "application/json");

            OnUpdateProvider(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSchedulesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/schedules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/schedules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSchedulesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/schedules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/schedules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSchedules(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Schedule>> GetSchedules(Query query)
        {
            return await GetSchedules(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Schedule>> GetSchedules(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Schedules");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchedules(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Schedule>>(response);
        }

        partial void OnCreateSchedule(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Schedule> CreateSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule = default(DOOH.Server.Models.DOOHDB.Schedule))
        {
            var uri = new Uri(baseUri, $"Schedules");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(schedule), Encoding.UTF8, "application/json");

            OnCreateSchedule(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Schedule>(response);
        }

        partial void OnDeleteSchedule(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSchedule(int scheduleId = default(int))
        {
            var uri = new Uri(baseUri, $"Schedules({scheduleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSchedule(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetScheduleByScheduleId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Schedule> GetScheduleByScheduleId(string expand = default(string), int scheduleId = default(int))
        {
            var uri = new Uri(baseUri, $"Schedules({scheduleId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetScheduleByScheduleId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Schedule>(response);
        }

        partial void OnUpdateSchedule(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSchedule(int scheduleId = default(int), DOOH.Server.Models.DOOHDB.Schedule schedule = default(DOOH.Server.Models.DOOHDB.Schedule))
        {
            var uri = new Uri(baseUri, $"Schedules({scheduleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(schedule), Encoding.UTF8, "application/json");

            OnUpdateSchedule(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportScheduleAdboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/scheduleadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/scheduleadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportScheduleAdboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/scheduleadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/scheduleadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetScheduleAdboards(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.ScheduleAdboard>> GetScheduleAdboards(Query query)
        {
            return await GetScheduleAdboards(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.ScheduleAdboard>> GetScheduleAdboards(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ScheduleAdboards");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetScheduleAdboards(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.ScheduleAdboard>>(response);
        }

        partial void OnCreateScheduleAdboard(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> CreateScheduleAdboard(DOOH.Server.Models.DOOHDB.ScheduleAdboard scheduleAdboard = default(DOOH.Server.Models.DOOHDB.ScheduleAdboard))
        {
            var uri = new Uri(baseUri, $"ScheduleAdboards");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(scheduleAdboard), Encoding.UTF8, "application/json");

            OnCreateScheduleAdboard(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.ScheduleAdboard>(response);
        }

        partial void OnDeleteScheduleAdboard(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteScheduleAdboard(int scheduleId = default(int), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"ScheduleAdboards(ScheduleId={scheduleId},AdboardId={adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteScheduleAdboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetScheduleAdboardByScheduleIdAndAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> GetScheduleAdboardByScheduleIdAndAdboardId(string expand = default(string), int scheduleId = default(int), int adboardId = default(int))
        {
            var uri = new Uri(baseUri, $"ScheduleAdboards(ScheduleId={scheduleId},AdboardId={adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetScheduleAdboardByScheduleIdAndAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.ScheduleAdboard>(response);
        }

        partial void OnUpdateScheduleAdboard(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateScheduleAdboard(int scheduleId = default(int), int adboardId = default(int), DOOH.Server.Models.DOOHDB.ScheduleAdboard scheduleAdboard = default(DOOH.Server.Models.DOOHDB.ScheduleAdboard))
        {
            var uri = new Uri(baseUri, $"ScheduleAdboards(ScheduleId={scheduleId},AdboardId={adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(scheduleAdboard), Encoding.UTF8, "application/json");

            OnUpdateScheduleAdboard(httpRequestMessage);

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


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tax), Encoding.UTF8, "application/json");

            OnUpdateTax(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportUploadsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/uploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/uploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportUploadsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/uploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/uploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetUploads(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Upload>> GetUploads(Query query)
        {
            return await GetUploads(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Upload>> GetUploads(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Uploads");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUploads(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.Upload>>(response);
        }

        partial void OnCreateUpload(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Upload> CreateUpload(DOOH.Server.Models.DOOHDB.Upload upload = default(DOOH.Server.Models.DOOHDB.Upload))
        {
            var uri = new Uri(baseUri, $"Uploads");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(upload), Encoding.UTF8, "application/json");

            OnCreateUpload(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Upload>(response);
        }

        partial void OnDeleteUpload(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteUpload(string key = default(string))
        {
            var uri = new Uri(baseUri, $"Uploads('{Uri.EscapeDataString(key.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteUpload(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetUploadByKey(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Upload> GetUploadByKey(string expand = default(string), string key = default(string))
        {
            var uri = new Uri(baseUri, $"Uploads('{Uri.EscapeDataString(key.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUploadByKey(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Upload>(response);
        }

        partial void OnUpdateUpload(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateUpload(string key = default(string), DOOH.Server.Models.DOOHDB.Upload upload = default(DOOH.Server.Models.DOOHDB.Upload))
        {
            var uri = new Uri(baseUri, $"Uploads('{Uri.EscapeDataString(key.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(upload), Encoding.UTF8, "application/json");

            OnUpdateUpload(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportUserInformationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/userinformations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/userinformations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportUserInformationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/userinformations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/userinformations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetUserInformations(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.UserInformation>> GetUserInformations(Query query)
        {
            return await GetUserInformations(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.UserInformation>> GetUserInformations(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"UserInformations");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUserInformations(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.UserInformation>>(response);
        }

        partial void OnCreateUserInformation(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> CreateUserInformation(DOOH.Server.Models.DOOHDB.UserInformation userInformation = default(DOOH.Server.Models.DOOHDB.UserInformation))
        {
            var uri = new Uri(baseUri, $"UserInformations");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(userInformation), Encoding.UTF8, "application/json");

            OnCreateUserInformation(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.UserInformation>(response);
        }

        partial void OnDeleteUserInformation(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteUserInformation(string userId = default(string))
        {
            var uri = new Uri(baseUri, $"UserInformations('{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteUserInformation(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetUserInformationByUserId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> GetUserInformationByUserId(string expand = default(string), string userId = default(string))
        {
            var uri = new Uri(baseUri, $"UserInformations('{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUserInformationByUserId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.UserInformation>(response);
        }

        partial void OnUpdateUserInformation(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateUserInformation(string userId = default(string), DOOH.Server.Models.DOOHDB.UserInformation userInformation = default(DOOH.Server.Models.DOOHDB.UserInformation))
        {
            var uri = new Uri(baseUri, $"UserInformations('{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(userInformation), Encoding.UTF8, "application/json");

            OnUpdateUserInformation(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportScheduleAdvertisementsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/scheduleadvertisements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/scheduleadvertisements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportScheduleAdvertisementsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/scheduleadvertisements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/scheduleadvertisements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetScheduleAdvertisements(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement>> GetScheduleAdvertisements(Query query)
        {
            return await GetScheduleAdvertisements(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement>> GetScheduleAdvertisements(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ScheduleAdvertisements");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetScheduleAdvertisements(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement>>(response);
        }

        partial void OnCreateScheduleAdvertisement(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> CreateScheduleAdvertisement(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement scheduleAdvertisement = default(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement))
        {
            var uri = new Uri(baseUri, $"ScheduleAdvertisements");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(scheduleAdvertisement), Encoding.UTF8, "application/json");

            OnCreateScheduleAdvertisement(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement>(response);
        }

        partial void OnDeleteScheduleAdvertisement(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteScheduleAdvertisement(int scheduleId = default(int), int advertisementId = default(int))
        {
            var uri = new Uri(baseUri, $"ScheduleAdvertisements(ScheduleId={scheduleId},AdvertisementId={advertisementId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteScheduleAdvertisement(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetScheduleAdvertisementByScheduleIdAndAdvertisementId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> GetScheduleAdvertisementByScheduleIdAndAdvertisementId(string expand = default(string), int scheduleId = default(int), int advertisementId = default(int))
        {
            var uri = new Uri(baseUri, $"ScheduleAdvertisements(ScheduleId={scheduleId},AdvertisementId={advertisementId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetScheduleAdvertisementByScheduleIdAndAdvertisementId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement>(response);
        }

        partial void OnUpdateScheduleAdvertisement(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateScheduleAdvertisement(int scheduleId = default(int), int advertisementId = default(int), DOOH.Server.Models.DOOHDB.ScheduleAdvertisement scheduleAdvertisement = default(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement))
        {
            var uri = new Uri(baseUri, $"ScheduleAdvertisements(ScheduleId={scheduleId},AdvertisementId={advertisementId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(scheduleAdvertisement), Encoding.UTF8, "application/json");

            OnUpdateScheduleAdvertisement(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}