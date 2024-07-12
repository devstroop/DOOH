using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Providers
{
    public partial class Providers
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] protected TooltipService TooltipService { get; set; }
        [Inject] protected ContextMenuService ContextMenuService { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] public DOOHDBService DOOHDBService { get; set; }
        [Inject] protected SecurityService Security { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Provider> providers;
        protected RadzenDataGrid<DOOH.Server.Models.DOOHDB.Provider> grid0;
        protected int count;
        protected string search = "";
        protected bool IsLoading = false;

        protected override async Task OnInitializedAsync()
        {
            if(grid0 != null) await grid0.Reload();
        }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";
            await grid0.GoToPage(0);
            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                IsLoading = true;
                var filter = $@"(contains(ContactName,""{search}"") or contains(CompanyName,""{search}"") or contains(Email,""{search}"") or contains(Phone,""{search}"") or contains(Address,""{search}"") or contains(City,""{search}"") or contains(State,""{search}"") or contains(Country,""{search}"") or contains(UserId,""{search}"")) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}";
                var result = await DOOHDBService.GetProviders(filter, args.OrderBy, "", args.Top, args.Skip, args.Top != null && args.Skip != null);
                providers = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load Providers, Exception: " + ex.Message });
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddProvider>("Add Provider", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<DOOH.Server.Models.DOOHDB.Provider> args)
        {
            await DialogService.OpenAsync<EditProvider>("Edit Provider", new Dictionary<string, object> { { "ProviderId", args.Data.ProviderId } });
            await grid0.Reload();
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            var query = new Query
            {
                Filter = string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter,
                OrderBy = grid0.Query.OrderBy,
                Expand = "City,State,Country",
                Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
            };

            if (args?.Value == "csv")
            {
                await DOOHDBService.ExportProvidersToCSV(query, "Providers");
            }
            else if (args == null || args.Value == "xlsx")
            {
                await DOOHDBService.ExportProvidersToExcel(query, "Providers");
            }
        }
    }
}
