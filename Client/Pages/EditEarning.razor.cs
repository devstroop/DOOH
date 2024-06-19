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
    public partial class EditEarning
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
        public int EarningId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            earning = await DOOHDBService.GetEarningByEarningId(earningId:EarningId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Earning earning;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Provider> providersForProviderId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Analytic> analyticsForAnalyticId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboardsForAdboardId;


        protected int providersForProviderIdCount;
        protected DOOH.Server.Models.DOOHDB.Provider providersForProviderIdValue;
        protected async Task providersForProviderIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetProviders(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(ContactName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                providersForProviderId = result.Value.AsODataEnumerable();
                providersForProviderIdCount = result.Count;

                if (!object.Equals(earning.ProviderId, null))
                {
                    var valueResult = await DOOHDBService.GetProviders(filter: $"ProviderId eq {earning.ProviderId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        providersForProviderIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Provider" });
            }
        }

        protected int analyticsForAnalyticIdCount;
        protected DOOH.Server.Models.DOOHDB.Analytic analyticsForAnalyticIdValue;
        protected async Task analyticsForAnalyticIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAnalytics(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                analyticsForAnalyticId = result.Value.AsODataEnumerable();
                analyticsForAnalyticIdCount = result.Count;

                if (!object.Equals(earning.AnalyticId, null))
                {
                    var valueResult = await DOOHDBService.GetAnalytics(filter: $"AnalyticId eq {earning.AnalyticId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        analyticsForAnalyticIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Analytic" });
            }
        }

        protected int adboardsForAdboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Adboard adboardsForAdboardIdValue;
        protected async Task adboardsForAdboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(DisplaySerial, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                adboardsForAdboardId = result.Value.AsODataEnumerable();
                adboardsForAdboardIdCount = result.Count;

                if (!object.Equals(earning.AdboardId, null))
                {
                    var valueResult = await DOOHDBService.GetAdboards(filter: $"AdboardId eq {earning.AdboardId}");
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
                var result = await DOOHDBService.UpdateEarning(earningId:EarningId, earning);
                if (result != null)
                {
                    DialogService.Close(earning);
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