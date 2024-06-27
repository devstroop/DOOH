using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Text.Json.Nodes;
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
            
            var url = advertisement.Upload.GetUrl();
            await DialogService.OpenAsync<Player>($"#{advertisement.AdvertisementId}", new Dictionary<string, object>() { { "Src", url } }, new DialogOptions() { Width = "400px" });
        }

        private async Task DeleteAdvertisement(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            await Delete.InvokeAsync(advertisement.AdvertisementId);
        }


        // ImportClick
        private async Task ImportClick(MouseEventArgs args)
        {
            // OpenDialog
            var result = await DialogService.OpenAsync<ImportFromUploads>("Import from Uploads", options: new DialogOptions()
            {
                Width = "100%",
                Height = "100%"
            });
            if (result != null)
            {
                if (result is Upload upload)
                {
                    var advertisement = new Advertisement
                    {
                        AdvertisementId = 0,
                        CampaignId = CampaignId,
                        UploadKey = upload.Key,
                        Upload = upload
                    };
                    advertisement = await DoohdbService.CreateAdvertisement(advertisement);
                    await Add.InvokeAsync(advertisement);
                }
            }
        }
        
    }
}
