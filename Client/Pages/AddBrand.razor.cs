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
    public partial class AddBrand
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

        protected override async Task OnInitializedAsync()
        {
            brand = new DOOH.Server.Models.DOOHDB.Brand();
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Brand brand;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> attachmentsForBrandLogoAttachmentKey;


        protected int attachmentsForBrandLogoAttachmentKeyCount;
        protected DOOH.Server.Models.DOOHDB.Attachment attachmentsForBrandLogoAttachmentKeyValue;
        protected async Task attachmentsForBrandLogoAttachmentKeyLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAttachments(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AttachmentKey, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                attachmentsForBrandLogoAttachmentKey = result.Value.AsODataEnumerable();
                attachmentsForBrandLogoAttachmentKeyCount = result.Count;

                if (!object.Equals(brand.BrandLogo_AttachmentKey, null))
                {
                    var valueResult = await DOOHDBService.GetAttachments(filter: $"AttachmentKey eq '{brand.BrandLogo_AttachmentKey}'");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        attachmentsForBrandLogoAttachmentKeyValue = firstItem;
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
                var result = await DOOHDBService.CreateBrand(brand);
                DialogService.Close(brand);
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

        DOOH.Client.Components.UploadComponent uploadBrandLogo;

        protected void OnUploadComplete(Attachment attachment)
        {
            brand.BrandLogo_AttachmentKey = attachment.AttachmentKey;
            brand.Attachment = attachment;
            StateHasChanged();
        }

    }
}