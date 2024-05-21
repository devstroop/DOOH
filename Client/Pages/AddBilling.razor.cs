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
    public partial class AddBilling
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
            billing = new DOOH.Server.Models.DOOHDB.Billing();
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Billing billing;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Analytic> analyticsForAnalyticId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Tax> taxesForTaxId;


        protected int analyticsForAnalyticIdCount;
        protected DOOH.Server.Models.DOOHDB.Analytic analyticsForAnalyticIdValue;
        protected async Task analyticsForAnalyticIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAnalytics(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                analyticsForAnalyticId = result.Value.AsODataEnumerable();
                analyticsForAnalyticIdCount = result.Count;

                if (!object.Equals(billing.AnalyticId, null))
                {
                    var valueResult = await DOOHDBService.GetAnalytics(filter: $"AnalyticId eq {billing.AnalyticId}");
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

        protected int taxesForTaxIdCount;
        protected DOOH.Server.Models.DOOHDB.Tax taxesForTaxIdValue;
        protected async Task taxesForTaxIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetTaxes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(TaxName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                taxesForTaxId = result.Value.AsODataEnumerable();
                taxesForTaxIdCount = result.Count;

                if (!object.Equals(billing.TaxId, null))
                {
                    var valueResult = await DOOHDBService.GetTaxes(filter: $"TaxId eq {billing.TaxId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        taxesForTaxIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Tax" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.CreateBilling(billing);
                DialogService.Close(billing);
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
    }
}