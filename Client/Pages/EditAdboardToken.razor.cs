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
    public partial class EditAdboardToken
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
        public int AdboardId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            adboardToken = await DOOHDBService.GetAdboardTokenByAdboardId(adboardId:AdboardId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.AdboardToken adboardToken;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboardsForAdboardId;


        protected int adboardsForAdboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Adboard adboardsForAdboardIdValue;
        protected async Task adboardsForAdboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(DisplaySerial, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                adboardsForAdboardId = result.Value.AsODataEnumerable();
                adboardsForAdboardIdCount = result.Count;

                if (!object.Equals(adboardToken.AdboardId, null))
                {
                    var valueResult = await DOOHDBService.GetAdboards(filter: $"AdboardId eq {adboardToken.AdboardId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        adboardsForAdboardIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Adboard" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateAdboardToken(adboardId:AdboardId, adboardToken);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(adboardToken);
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

            adboardToken = await DOOHDBService.GetAdboardTokenByAdboardId(adboardId:AdboardId);
        }
    }
}