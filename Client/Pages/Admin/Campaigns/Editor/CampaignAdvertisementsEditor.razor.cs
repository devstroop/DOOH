using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Text.Json.Nodes;

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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> advertisements;

        protected int advertisementsCount;


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

    }
}
