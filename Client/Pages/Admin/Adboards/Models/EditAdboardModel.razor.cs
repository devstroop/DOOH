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

namespace DOOH.Client.Pages.Admin.Adboards.Models
{
    public partial class EditAdboardModel
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
        public int AdboardModelId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            adboardModel = await DOOHDBService.GetAdboardModelByAdboardModelId(adboardModelId:AdboardModelId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.AdboardModel adboardModel;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> attachmentsForAttachmentKey;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Display> displaysForDisplayId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Motherboard> motherboardsForMotherboardId;


        protected int attachmentsForAttachmentKeyCount;
        protected DOOH.Server.Models.DOOHDB.Attachment attachmentsForAttachmentKeyValue;
        protected async Task attachmentsForAttachmentKeyLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAttachments(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AttachmentKey, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                attachmentsForAttachmentKey = result.Value.AsODataEnumerable();
                attachmentsForAttachmentKeyCount = result.Count;

                if (!object.Equals(adboardModel.AttachmentKey, null))
                {
                    var valueResult = await DOOHDBService.GetAttachments(filter: $"AttachmentKey eq '{adboardModel.AttachmentKey}'");
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

        protected int displaysForDisplayIdCount;
        protected DOOH.Server.Models.DOOHDB.Display displaysForDisplayIdValue;
        protected async Task displaysForDisplayIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetDisplays(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Model, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                displaysForDisplayId = result.Value.AsODataEnumerable();
                displaysForDisplayIdCount = result.Count;

                if (!object.Equals(adboardModel.DisplayId, null))
                {
                    var valueResult = await DOOHDBService.GetDisplays(filter: $"DisplayId eq {adboardModel.DisplayId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        displaysForDisplayIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Display" });
            }
        }

        protected int motherboardsForMotherboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Motherboard motherboardsForMotherboardIdValue;
        protected async Task motherboardsForMotherboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetMotherboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Model, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                motherboardsForMotherboardId = result.Value.AsODataEnumerable();
                motherboardsForMotherboardIdCount = result.Count;

                if (!object.Equals(adboardModel.MotherboardId, null))
                {
                    var valueResult = await DOOHDBService.GetMotherboards(filter: $"MotherboardId eq {adboardModel.MotherboardId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        motherboardsForMotherboardIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Motherboard" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateAdboardModel(adboardModelId:AdboardModelId, adboardModel);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(adboardModel);
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


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            adboardModel = await DOOHDBService.GetAdboardModelByAdboardModelId(adboardModelId:AdboardModelId);
        }

        DOOH.Client.Components.UploadComponent uploadBrandLogo;

        protected void OnUploadComplete(Attachment attachment)
        {
            adboardModel.AttachmentKey = attachment.AttachmentKey;
            adboardModel.Attachment = attachment;
            StateHasChanged();
        }
    }
}