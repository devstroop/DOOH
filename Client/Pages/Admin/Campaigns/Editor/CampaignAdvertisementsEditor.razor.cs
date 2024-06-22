using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Text.Json.Nodes;
using DOOH.Server.Models.DOOHDB;
using FFmpegBlazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignAdvertisementsEditor
    {
        [Parameter]
        public int CampaignId { get; set; }
        
        [Parameter]
        public EventCallback<Attachment> AddAttachment { get; set; }
        [Parameter]
        public EventCallback<int> DeleteAdvertisementByAdvertisementId { get; set; }
        
        [Parameter]
        public Action Refresh { get; set; }
        
        [Parameter]
        public IEnumerable<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

        [Inject]
        protected IJSRuntime Runtime { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DOOH.Client.DOOHDBService DbService { get; set; }
        
        [Inject]
        protected DialogService DialogService { get; set; }


        private int AdvertisementsCount => Advertisements?.Count() ?? 0;

        private RadzenUpload upload;
        private RadzenUpload uploadDd;

        private int progress;
        private string progressMessage = string.Empty;
        private bool showProgress;
        private bool cancelUpload;

        [Inject]
        protected DOOH.Client.DOOHDBService DoohdbService { get; set; }


        private void OnChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                //console.Log($"File: {file.Name} / {file.Size} bytes");
            }

            //console.Log($"{name} changed");
        }

        private void OnProgress(UploadProgressArgs args)
        {
            showProgress = true;
            progress = args.Progress;
            progressMessage = $"{args.Progress}% / {args.Loaded} of {args.Total} bytes.";
            args.Cancel = cancelUpload;
            Refresh?.Invoke();
        }

        private void OnComplete(UploadCompleteEventArgs args)
        {
            showProgress = false;
            progress = 100;
            progressMessage = !args.Cancelled ? "Upload Complete!" : "Upload Cancelled!";

            var rawResponse = args.RawResponse;
            // log
            Runtime.InvokeVoidAsync("console.log", rawResponse);
            // TODO: Deserialize
            Refresh?.Invoke();
        }
        
        private void OnHoverClick(string args)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Hover", Detail = args });
        }

        private async Task DeleteAdvertisement(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            await DeleteAdvertisementByAdvertisementId.InvokeAsync(advertisement.AdvertisementId);
        }


        // ImportClick
        private async Task ImportClick(MouseEventArgs args)
        {
            // OpenDialog
            await DialogService.OpenAsync<ImportAttachmment>("Import Attachmment", new Dictionary<string, object>() { { "Import", EventCallback.Factory.Create<Attachment>(this, OnImportAttachment) } });
            Refresh?.Invoke();
        }
        
        // ImportAttachment
        private async Task OnImportAttachment(Attachment attachment)
        {
            await AddAttachment.InvokeAsync(attachment);
        }
        
    }
}
