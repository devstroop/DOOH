using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages
{
    public partial class EditDisplay
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
        public int DisplayId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            display = await DOOHDBService.GetDisplayByDisplayId(displayId:DisplayId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Display display;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Brand> brandsForBrandId;


        protected int brandsForBrandIdCount;
        protected DOOH.Server.Models.DOOHDB.Brand brandsForBrandIdValue;
        protected async Task brandsForBrandIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetBrands(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(BrandName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                brandsForBrandId = result.Value.AsODataEnumerable();
                brandsForBrandIdCount = result.Count;

                if (!object.Equals(display.BrandId, null))
                {
                    var valueResult = await DOOHDBService.GetBrands(filter: $"BrandId eq {display.BrandId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        brandsForBrandIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Brand" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateDisplay(displayId:DisplayId, display);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(display);
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

            display = await DOOHDBService.GetDisplayByDisplayId(displayId:DisplayId);
        }
    }
}