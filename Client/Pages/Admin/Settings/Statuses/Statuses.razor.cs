using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Settings.Statuses
{
    public partial class Statuses
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Status> statuses;

        protected RadzenDataGrid<DOOH.Server.Models.DOOHDB.Status> grid0;
        protected int count;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetStatuses(filter: $"{args.Filter}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                statuses = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Statuses" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddStatus>("Add Status", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DOOH.Server.Models.DOOHDB.Status args)
        {
            await DialogService.OpenAsync<EditStatus>("Edit Status", new Dictionary<string, object> { {"StatusId", args.StatusId} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Status status)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteStatus(statusId:status.StatusId);

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
                    Detail = $"Unable to delete Status"
                });
            }
        }
    }
}