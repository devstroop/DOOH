using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using DOOH.Client.Layout;

namespace DOOH.Client.Pages.Admin.Adboards
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
        protected SecurityService Security { get; set; }

        [Inject]
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }

        [CascadingParameter]
        protected AdminLayout Layout { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards;

        protected RadzenDataList<DOOH.Server.Models.DOOHDB.Adboard> list0;
        protected int adboardsCount;


        protected bool isAdboardsLoading = false;
        protected string search = "";


        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            //Layout.ShowLoading = true;
        }


        protected async Task adboardsLoadData(LoadDataArgs args)
        {
            try
            {
                isAdboardsLoading = true;

                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy, expand: "Category,AdboardModel($expand=Display($expand=Brand),Motherboard($expand=Brand))");

                adboards = result.Value.AsODataEnumerable();

                foreach (var adboard in adboards)
                {
                    await Console.Out.WriteLineAsync(adboard.Category.CategoryColor);
                }

                adboardsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
            finally
            {
                isAdboardsLoading = false;
            }
        }

        protected async void DetailsSplitButtonClicked(RadzenSplitButtonItem item, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            if (item.Text == "Edit")
            {
                var dialogResult = await DialogService.OpenAsync<EditAdboard>("Edit Adboard", new Dictionary<string, object>() { { "AdboardId", adboard.AdboardId } });
                if (dialogResult != null)
                {
                    await adboardsLoadData(new LoadDataArgs() { });
                    StateHasChanged();
                }
            }
            else if (item.Text == "Delete")
            {
                var dialogResult = await DialogService.Confirm("Are you sure?", "Delete Adboard");
                if (dialogResult == true)
                {
                    try
                    {
                        await DOOHDBService.DeleteAdboard(adboardId: adboard.AdboardId);
                        await adboardsLoadData(new LoadDataArgs() { });
                    }
                    catch (Exception)
                    {
                        NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete" });
                    }
                    finally
                    {
                        StateHasChanged();
                    }
                }
            }
        }

        protected async void ToggleAdboardIsActive(dynamic args, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            try
            {
                adboard.IsActive = !adboard.IsActive;
                await DOOHDBService.UpdateAdboard(adboardId: adboard.AdboardId, adboard: adboard);
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to update" });
            }
            finally
            {
                StateHasChanged();
            }
        }

        protected async void AddAdboardClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAdboard>("Add Adboard", null);
            if (dialogResult != null)
            {
                await adboardsLoadData(new LoadDataArgs() { });
                StateHasChanged();
            }
        }

        // RefreshClick
        protected async void RefreshClick(MouseEventArgs args)
        {
            await adboardsLoadData(new LoadDataArgs() { });
            StateHasChanged();
        }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await list0.GoToPage(0);

            await list0.Reload();
        }
    }
}