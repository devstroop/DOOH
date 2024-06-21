using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Text.Json.Nodes;
using FFmpegBlazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignAdvertisementsEditor
    {
        [Parameter]
        public int CampaignId { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }
        
        [Inject]
        protected DialogService DialogService { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> advertisements;

        protected int advertisementsCount;


        
        FFMPEG ffMpeg;
        byte[] videoBuffer;
        string logMessages = string.Empty;
        string progressMessage = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            // Wire-up events
            if (FFmpegFactory.Runtime == null)
            {
                FFmpegFactory.Logger += LogToConsole;
                FFmpegFactory.Progress += ProgressChange;
            }

            // Initialize Library
            await FFmpegFactory.Init(JSRuntime);
        }
        private void LogToConsole(Logs message)
        {
            var logMessage = $"{message.Type} {message.Message}";
            Console.WriteLine(logMessage);
            LogToUi(logMessage);
        }

        private void LogToUi(string message)
        {
            logMessages += $"{message}\r\n";
            // Re-render DOM
            StateHasChanged();
        }

        private async void ProgressChange(Progress message)
        {
        }
        public void Dispose()
        {
            FFmpegFactory.Logger -= LogToConsole;
            FFmpegFactory.Progress -= ProgressChange;
        }

        protected async Task advertisementsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdvertisements(top: args.Top, skip: args.Skip, filter: $@"(CampaignId eq {CampaignId}) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", orderby: args.OrderBy, expand: "Attachment");

                advertisements = result.Value.AsODataEnumerable();
                advertisementsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        protected async Task OnAdMediaClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            try
            {
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }






        RadzenUpload upload;
        RadzenUpload uploadDD;

        int progress;
        bool showProgress;
        bool showComplete;
        string completionMessage;
        bool cancelUpload = false;

        void CompleteUpload(UploadCompleteEventArgs args)
        {
            if (!args.Cancelled)
                completionMessage = "Upload Complete!";
            else
                completionMessage = "Upload Cancelled!";

            showProgress = false;
            showComplete = true;
        }

        void TrackProgress(UploadProgressArgs args)
        {
            showProgress = true;
            showComplete = false;
            progress = args.Progress;

            // cancel upload
            args.Cancel = cancelUpload;

            // reset cancel flag
            cancelUpload = false;
        }

        void CancelUpload()
        {
            cancelUpload = true;
        }

        int customParameter = 1;

        void OnChange(UploadChangeEventArgs args, string name)
        {
            foreach (var file in args.Files)
            {
                //console.Log($"File: {file.Name} / {file.Size} bytes");
            }

            //console.Log($"{name} changed");
        }

        void OnProgress(UploadProgressArgs args, string name)
        {
            //console.Log($"{args.Progress}% '{name}' / {args.Loaded} of {args.Total} bytes.");

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    //console.Log($"Uploaded: {file.Name} / {file.Size} bytes");
                }
            }
        }

        void OnComplete(UploadCompleteEventArgs args)
        {
            //console.Log($"Server response: {args.RawResponse}");
        }

        void OnClientChange(UploadChangeEventArgs args)
        {
            //console.Log($"Client-side upload changed");

            foreach (var file in args.Files)
            {
                //console.Log($"File: {file.Name} / {file.Size} bytes");

                try
                {
                    long maxFileSize = 10 * 1024 * 1024;
                    // read file
                    var stream = file.OpenReadStream(maxFileSize);
                    stream.Close();
                }
                catch (Exception ex)
                {
                    //console.Log($"Client-side file read error: {ex.Message}");
                }
            }
        }

        // OnHoverClick
        void OnHoverClick(string args)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Hover", Detail = args });
        }

        // DeleteAdvertisement
        protected async Task DeleteAdvertisement(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var result = await DOOHDBService.DeleteAdvertisement(advertisement.AdvertisementId);
                    if (result != null)
                    {
                        await advertisementsLoadData(new LoadDataArgs { Top = 10 });
                        NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = "Advertisement deleted successfully" });
                    }
                }
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete" });
            }
        }
        
        
    }
}
