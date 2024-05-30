using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Models
{
    public partial class AdboardModels
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.AdboardModel> adboardModels;

        protected int GridColumnSize => adboardModel != null ? 7 : 12;
        protected int GridColumnSizeForm => adboardModel != null ? 5 : 0;

        protected RadzenDataGrid<DOOH.Server.Models.DOOHDB.AdboardModel> grid0;
        protected int count;
        protected bool isEdit = true;

        protected string search = "";

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
                var result = await DOOHDBService.GetAdboardModels(filter: $@"(contains(Model,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Display($expand=Brand),Motherboard($expand=Brand)", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                adboardModels = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Adboard Models" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            adboardModel = new DOOH.Server.Models.DOOHDB.AdboardModel();
        }

        protected async Task EditRow(DOOH.Server.Models.DOOHDB.AdboardModel args)
        {
            isEdit = true;
            adboardModel = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.AdboardModel adboardModel)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteAdboardModel(adboardModelId:adboardModel.AdboardModelId);

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
                    Detail = $"Unable to delete AdboardModel"
                });
            }
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.AdboardModel adboardModel;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Display> displaysForDisplayId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Motherboard> motherboardsForMotherboardId;


        protected int displaysForDisplayIdCount;
        protected DOOH.Server.Models.DOOHDB.Display displaysForDisplayIdValue;
        protected async Task displaysForDisplayIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetDisplays(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Model, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}", expand: "Brand");
                displaysForDisplayId = result.Value.AsODataEnumerable();
                displaysForDisplayIdCount = result.Count;

                if (!object.Equals(adboardModel.DisplayId, null))
                {
                    var valueResult = await DOOHDBService.GetDisplays(filter: $"DisplayId eq {adboardModel.DisplayId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        displaysForDisplayIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Display" });
            }
        }

        protected int motherboardsForMotherboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Motherboard motherboardsForMotherboardIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task motherboardsForMotherboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetMotherboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Rom, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}", expand: "Brand");
                motherboardsForMotherboardId = result.Value.AsODataEnumerable();
                motherboardsForMotherboardIdCount = result.Count;

                if (!object.Equals(adboardModel.MotherboardId, null))
                {
                    var valueResult = await DOOHDBService.GetMotherboards(filter: $"MotherboardId eq {adboardModel.MotherboardId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        motherboardsForMotherboardIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Motherboard" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await DOOHDBService.UpdateAdboardModel(adboardModelId:adboardModel.AdboardModelId, adboardModel) : await DOOHDBService.CreateAdboardModel(adboardModel);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            adboardModel = null;
            motherboardsForMotherboardIdValue = null;
            displaysForDisplayIdValue = null;
        }
    }
}