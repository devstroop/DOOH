using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Amazon.S3.Model;
using DOOH.Client.Components;
using DOOH.Client.Extensions;
using DOOH.Server.Models.DOOHDB;
using FFmpegBlazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignAdvertisementsEditor
    {
        [Parameter]
        public int CampaignId { get; set; }
        
        [Parameter]
        public EventCallback<Advertisement> Add { get; set; }
        [Parameter]
        public EventCallback<int> Delete { get; set; }
        
        [Parameter]
        public Action Refresh { get; set; }
        
        [Parameter]
        public IEnumerable<Advertisement> Data { get; set; } = new List<Advertisement>();

        [Inject]
        protected IJSRuntime Runtime { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DOOH.Client.DOOHDBService DbService { get; set; }
        
        [Inject]
        protected DialogService DialogService { get; set; }


        private int AdvertisementsCount => Data?.Count() ?? 0;


        [Inject]
        protected DOOH.Client.DOOHDBService DoohdbService { get; set; }


        private async Task OnHoverClick(MouseEventArgs args, Advertisement advertisement)
        {
            
            var url = advertisement.Key.GetUrl();
            await DialogService.OpenAsync<Player>($"#{advertisement.AdvertisementId}", new Dictionary<string, object>() { { "Src", url } }, new DialogOptions() { Width = "400px", CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true });
        }

        private async Task DeleteAdvertisement(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            await Delete.InvokeAsync(advertisement.AdvertisementId);
        }


        // ImportClick
        private async Task UploadClick(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<Upload>("Upload", null);
            if (result != null)
            {
                Console.WriteLine(JsonSerializer.Serialize(result));
            }
        }
        private async Task ImportClick(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<Client.Pages.Uploads>("Import from Uploads", new Dictionary<string, object>()
            {
                { "Selectable", true }
            });
            if (result != null)
            {
                Console.WriteLine(JsonSerializer.Serialize(result));
            }
        }
    }
}
