
using System.Net.Http;
using System.Text;

namespace DOOH.Adboard
{
    public partial class DOOHDBService
    {
        private readonly HttpClient httpClient;

        public DOOHDBService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
        }


        partial void OnGetAdboardByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> GetAdboardByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = Extensions.ODataExtensions.GetODataUri(uri: new Uri($"{httpClient.BaseAddress}odata/DOOHDB/Adboards({adboardId})"), filter: null, top: null, skip: null, orderby: null, expand: expand, select: null, count: null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Extensions.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.Adboard>(response);
        }

        partial void OnGetAdboardWifiByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> GetAdboardWifiByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = Extensions.ODataExtensions.GetODataUri(uri: new Uri(httpClient.BaseAddress, $"odata/DOOHDB/AdboardWifis({adboardId})"), filter: null, top: null, skip: null, orderby: null, expand: expand, select: null, count: null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Extensions.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardWifi>(response);
        }

        partial void OnUpdateAdboardWifiByAdboardId(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> UpdateAdboardWifiByAdboardId(int adboardId = default(int), DOOH.Server.Models.DOOHDB.AdboardWifi adboardWifi = default(DOOH.Server.Models.DOOHDB.AdboardWifi))
        {
            var uri = new Uri(httpClient.BaseAddress, $"odata/DOOHDB/AdboardWifis({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Extensions.ODataJsonSerializer.Serialize(adboardWifi), Encoding.UTF8, "application/json");
            OnUpdateAdboardWifiByAdboardId(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }


        partial void OnGetAdvertisements(HttpRequestMessage requestMessage);

        public async Task<Models.ODataServiceResult<DOOH.Server.Models.DOOHDB.Advertisement>> GetAdvertisements(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(httpClient.BaseAddress, $"odata/DOOHDB/Advertisements");
            uri = Extensions.ODataExtensions.GetODataUri(uri: uri, filter: filter, top: top, skip: skip, orderby: orderby, expand: expand, select: select, count: count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdvertisements(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Extensions.HttpResponseMessageExtensions.ReadAsync<Models.ODataServiceResult<DOOH.Server.Models.DOOHDB.Advertisement>>(response);
        }

        public async Task<string> GetPresignedUrl(string key)
        {
            var uri = new Uri(httpClient.BaseAddress, $"api/cdn/object/presigned/{key}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.Content.ReadAsStringAsync();
        }
        
        
        // AdboardStatuses
        
        partial void OnGetAdboardStatuses(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardStatus>> GetAdboardStatuses(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(httpClient.BaseAddress, $"AdboardStatuses");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardStatuses(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DOOH.Server.Models.DOOHDB.AdboardStatus>>(response);
        }

        partial void OnCreateAdboardStatus(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> CreateAdboardStatus(DOOH.Server.Models.DOOHDB.AdboardStatus adboardStatus = default(DOOH.Server.Models.DOOHDB.AdboardStatus))
        {
            var uri = new Uri(httpClient.BaseAddress, $"AdboardStatuses");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardStatus), Encoding.UTF8, "application/json");

            OnCreateAdboardStatus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardStatus>(response);
        }

        partial void OnDeleteAdboardStatus(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdboardStatus(int adboardId = default(int))
        {
            var uri = new Uri(httpClient.BaseAddress, $"AdboardStatuses({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdboardStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdboardStatusByAdboardId(HttpRequestMessage requestMessage);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> GetAdboardStatusByAdboardId(string expand = default(string), int adboardId = default(int))
        {
            var uri = new Uri(httpClient.BaseAddress, $"AdboardStatuses({adboardId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdboardStatusByAdboardId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DOOH.Server.Models.DOOHDB.AdboardStatus>(response);
        }

        partial void OnUpdateAdboardStatus(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdboardStatus(int adboardId = default(int), DOOH.Server.Models.DOOHDB.AdboardStatus adboardStatus = default(DOOH.Server.Models.DOOHDB.AdboardStatus))
        {
            var uri = new Uri(httpClient.BaseAddress, $"AdboardStatuses({adboardId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(adboardStatus), Encoding.UTF8, "application/json");

            OnUpdateAdboardStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}
