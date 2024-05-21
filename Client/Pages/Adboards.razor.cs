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
    public partial class Adboards
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards;

        protected RadzenDataGrid<DOOH.Server.Models.DOOHDB.Adboard> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

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
                var result = await DOOHDBService.GetAdboards(filter: $@"(contains(DisplaySerial,""{search}"") or contains(MotherboardSerial,""{search}"") or contains(AttachmentKey,""{search}"") or contains(Address,""{search}"") or contains(CityName,""{search}"") or contains(StateName,""{search}"") or contains(CountryName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "AdboardModel,Provider,Category,Attachment,City,State,Country", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                adboards = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Adboards" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAdboard>("Add Adboard", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<DOOH.Server.Models.DOOHDB.Adboard> args)
        {
            await DialogService.OpenAsync<EditAdboard>("Edit Adboard", new Dictionary<string, object> { {"AdboardId", args.Data.AdboardId} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteAdboard(adboardId:adboard.AdboardId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Adboard"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await DOOHDBService.ExportAdboardsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "AdboardModel,Provider,Category,Attachment,City,State,Country",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Adboards");
            }

            if (args == null || args.Value == "xlsx")
            {
                await DOOHDBService.ExportAdboardsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "AdboardModel,Provider,Category,Attachment,City,State,Country",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Adboards");
            }
        }
    }
}