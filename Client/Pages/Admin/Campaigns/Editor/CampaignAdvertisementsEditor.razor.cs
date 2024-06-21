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
        
        [Parameter]
        public Action Refresh { get; set; }

        [Inject]
        protected IJSRuntime Runtime { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DOOH.Client.DOOHDBService DbService { get; set; }
        
        [Inject]
        protected DialogService DialogService { get; set; }

        private IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> advertisements;

        private int advertisementsCount;


        private async Task AdvertisementsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DbService.GetAdvertisements(top: args.Top, skip: args.Skip, filter: $@"(CampaignId eq {CampaignId}) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", orderby: args.OrderBy, expand: "Attachment");

                advertisements = result.Value.AsODataEnumerable();
                advertisementsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        private RadzenUpload upload;
        private RadzenUpload uploadDd;

        private int progress;
        private string progressMessage = string.Empty;
        private bool showProgress;
        private bool cancelUpload;

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
            Refresh?.Invoke();
        }


        private void OnHoverClick(string args)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Hover", Detail = args });
        }

        private async Task DeleteAdvertisement(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var result = await DbService.DeleteAdvertisement(advertisement.AdvertisementId);
                    if (result != null)
                    {
                        await AdvertisementsLoadData(new LoadDataArgs { Top = 10 });
                        NotificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Success, Summary = "Success",
                            Detail = "Advertisement deleted successfully"
                        });
                    }
                }
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage
                    { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete" });
            }
            finally
            {
                Refresh?.Invoke();
            }
        }
        
        
    }
}
