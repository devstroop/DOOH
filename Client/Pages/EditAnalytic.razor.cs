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
    public partial class EditAnalytic
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
        public int AnalyticId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            analytic = await DOOHDBService.GetAnalyticByAnalyticId(analyticId:AnalyticId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Analytic analytic;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboardsForAdboardId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> advertisementsForAdvertisementId;


        protected int adboardsForAdboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Adboard adboardsForAdboardIdValue;
        protected async Task adboardsForAdboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(DisplaySerial, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                adboardsForAdboardId = result.Value.AsODataEnumerable();
                adboardsForAdboardIdCount = result.Count;

                if (!object.Equals(analytic.AdboardId, null))
                {
                    var valueResult = await DOOHDBService.GetAdboards(filter: $"AdboardId eq {analytic.AdboardId}");
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

        protected int advertisementsForAdvertisementIdCount;
        protected DOOH.Server.Models.DOOHDB.Advertisement advertisementsForAdvertisementIdValue;
        protected async Task advertisementsForAdvertisementIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdvertisements(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AttachmentKey, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                advertisementsForAdvertisementId = result.Value.AsODataEnumerable();
                advertisementsForAdvertisementIdCount = result.Count;

                if (!object.Equals(analytic.AdvertisementId, null))
                {
                    var valueResult = await DOOHDBService.GetAdvertisements(filter: $"AdvertisementId eq {analytic.AdvertisementId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        advertisementsForAdvertisementIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Advertisement" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateAnalytic(analyticId:AnalyticId, analytic);
                if (result != null)
                {
                    DialogService.Close(analytic);
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
    }
}