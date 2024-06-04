using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using DOOH.Server.Models.DOOHDB;

namespace DOOH.Client.Pages
{
    public partial class EditAdvertisement
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public DOOHDBService DOOHDBService { get; set; }

        [Parameter]
        public int AdvertisementId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            advertisement = await DOOHDBService.GetAdvertisementByAdvertisementId(advertisementId:AdvertisementId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Advertisement advertisement;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Campaign> campaignsForCampaignId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> attachmentsForAttachmentKey;


        protected int campaignsForCampaignIdCount;
        protected DOOH.Server.Models.DOOHDB.Campaign campaignsForCampaignIdValue;
        protected async Task campaignsForCampaignIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCampaigns(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CampaignName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                campaignsForCampaignId = result.Value.AsODataEnumerable();
                campaignsForCampaignIdCount = result.Count;

                if (!object.Equals(advertisement.CampaignId, null))
                {
                    var valueResult = await DOOHDBService.GetCampaigns(filter: $"CampaignId eq {advertisement.CampaignId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        campaignsForCampaignIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Campaign" });
            }
        }

        protected int attachmentsForAttachmentKeyCount;
        protected DOOH.Server.Models.DOOHDB.Attachment attachmentsForAttachmentKeyValue;
        protected async Task attachmentsForAttachmentKeyLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAttachments(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AttachmentKey, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                attachmentsForAttachmentKey = result.Value.AsODataEnumerable();
                attachmentsForAttachmentKeyCount = result.Count;

                if (!object.Equals(advertisement.AttachmentKey, null))
                {
                    var valueResult = await DOOHDBService.GetAttachments(filter: $"AttachmentKey eq '{advertisement.AttachmentKey}'");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        attachmentsForAttachmentKeyValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Attachment" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateAdvertisement(advertisementId:AdvertisementId, advertisement);
                if (result != null)
                {
                    DialogService.Close(advertisement);
                }   
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        [Inject]
        protected SecurityService Security { get; set; }

        DOOH.Client.Components.UploadComponent uploadDD;

        protected void OnUploadComplete(Attachment attachment)
        {
            advertisement.AttachmentKey = attachment.AttachmentKey;
            advertisement.Attachment = attachment;
            advertisement.Duration = attachment.Duration ?? 0.0;
            StateHasChanged();
        }
    }
}