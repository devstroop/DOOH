using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Settings.Taxes
{
    public partial class AddTax
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
        public int? ParentTaxId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tax = new DOOH.Server.Models.DOOHDB.Tax { TaxId = 0, ParentTaxId = ParentTaxId };
        }

        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Tax tax;

        //protected IEnumerable<DOOH.Server.Models.DOOHDB.Tax> taxesForParentTaxId;


        //protected int taxesForParentTaxIdCount;
        //protected DOOH.Server.Models.DOOHDB.Tax taxesForParentTaxIdValue;
        //protected async Task taxesForParentTaxIdLoadData(LoadDataArgs args)
        //{
        //    try
        //    {
        //        var result = await DOOHDBService.GetTaxes(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(TaxName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
        //        taxesForParentTaxId = result.Value.AsODataEnumerable();
        //        taxesForParentTaxIdCount = result.Count;

        //        if (!object.Equals(tax.ParentTaxId, null))
        //        {
        //            var valueResult = await DOOHDBService.GetTaxes(filter: $"TaxId eq {tax.ParentTaxId}");
        //            var firstItem = valueResult.Value.FirstOrDefault();
        //            if (firstItem != null)
        //            {
        //                taxesForParentTaxIdValue = firstItem;
        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Tax1" });
        //    }
        //}
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.CreateTax(tax);
                DialogService.Close(tax);
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