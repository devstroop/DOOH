using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Pages
{
    public partial class Privacy
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
        public DOOHDBService DOOHDBService { get; set; }

        protected DOOH.Server.Models.DOOHDB.Page _page;

        protected bool isEditing = false;
        protected bool isNew = false;

        protected override async Task OnInitializedAsync()
        {
            await Fetch();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Fetch();
            }
        }

        private async Task Fetch()
        {
            try
            {
                _page = await DOOHDBService.GetPageBySlag(slag: "privacy");
                return;
            }
            catch { }

            _page = _page ?? new DOOH.Server.Models.DOOHDB.Page { Slag = "privacy" };
            isNew = true;
        }

        protected async Task SaveClick(MouseEventArgs args)
        {
            try
            {
                if (isNew)
                {
                    await DOOHDBService.CreatePage(_page);
                }
                else
                {
                    await DOOHDBService.UpdatePage(_page.Slag, _page);
                }
                isNew = false;
                isEditing = false;
                NotificationService.Notify(NotificationSeverity.Success, "Success", "Privacy policy saved!");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "An error has occurred");
            }
        }

        protected async Task CancelClick(MouseEventArgs args)
        {
            await OnInitializedAsync();
            isEditing = false;
            StateHasChanged();
        }

        protected async Task EditClick(MouseEventArgs args)
        {
            isEditing = true;
            StateHasChanged();
        }
    }
}