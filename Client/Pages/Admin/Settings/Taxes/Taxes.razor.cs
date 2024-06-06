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
    public partial class Taxes
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
        protected bool isLoading = true;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Tax> taxes;

        //protected RadzenDataGrid<DOOH.Server.Models.DOOHDB.Tax> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }


        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }

        protected async System.Threading.Tasks.Task Load()
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                var doOHDBGetTaxesResult = await DOOHDBService.GetTaxes(filter: $@"(contains(TaxName,""{search}""))", orderby: $"", top: null, skip: null, count: true, expand: "Tax1");
                count = doOHDBGetTaxesResult.Count;
                taxes = doOHDBGetTaxesResult.Value.AsODataEnumerable();
            }
            catch (System.Exception doOHDBGetTaxesException)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Taxes" });
            }

            isLoading = false;
            StateHasChanged();
        }


        //protected async Task Search(ChangeEventArgs args)
        //{
        //    search = $"{args.Value}";

        //    await grid0.GoToPage(0);

        //    await grid0.Reload();

        //    StateHasChanged();
        //}

        //protected async Task Grid0LoadData(LoadDataArgs args)
        //{
        //    isLoading = true;
        //    StateHasChanged();
        //    try
        //    {
        //        var result = await DOOHDBService.GetTaxes(filter: $@"(contains(TaxName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Tax1", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
        //        taxes = result.Value.AsODataEnumerable();
        //        count = result.Count;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Taxes" });
        //    }
        //    finally
        //    {
        //        isLoading = false;
        //        StateHasChanged();
        //    }
        //}

        protected async Task AddParentTaxClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTax>("Add Tax", null);
            await Load();

        }

        protected async Task AddChildTaxClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Tax tax)
        {
            await DialogService.OpenAsync<AddTax>("Add Tax", new Dictionary<string, object> { { "ParentTaxId", tax.TaxId } });
            await Load();

        }

        protected async Task EditTaxClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Tax tax)
        {
            await DialogService.OpenAsync<EditTax>("Edit Tax", new Dictionary<string, object> { {"TaxId", tax.TaxId} });
            await Load();
        }

        protected async Task DeleteTaxClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Tax tax)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteTax(taxId:tax.TaxId);

                    if (deleteResult != null)
                    {
                        //await grid0.Reload();
                        await Load();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Tax"
                });
            }
        }
        void Log(string eventName, string value)
        {
            //console.Log($"{eventName}: {value}");
            JSRuntime.InvokeVoidAsync("console.log", $"{eventName}: {value}");
        }

        void OnChange(TreeEventArgs args)
        {
            Log("Change", $"Item Text: {args.Text}");
        }

        void OnExpand(TreeExpandEventArgs args)
        {
            Log("Expand", $"Text: {args.Text}");
        }

        void OnCollapse(TreeEventArgs args)
        {
            Log("Collapse", $"Text: {args.Text}");
        }
    }
}