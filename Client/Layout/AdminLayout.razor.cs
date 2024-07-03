using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Layout
{
    public partial class AdminLayout
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

        bool sidebarExpanded = true;

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected DOOHDBService DOOHDBService { get; set; }
        protected DOOH.Server.Models.DOOHDB.Company Company { get; set; }

        protected string AdminLogo { get; set; } = "vectors/Admin-Blue.svg";

        private bool showLoading = false;
        public bool ShowLoading
        {
            get => showLoading; set
            {
                showLoading = value;
                InvokeAsync(() => StateHasChanged());
            }
        }

        void SidebarToggleClick()
        {
            sidebarExpanded = !sidebarExpanded;
        }
        protected void ProfileMenuClick(RadzenProfileMenuItem args)
        {
            if (args.Value == "Logout")
            {
                Security.Logout();
            }
        }

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

        protected override async Task OnParametersSetAsync()
        {
            await Fetch();
        }

        protected async Task Fetch()
        {
            try
            {
                var result = await DOOHDBService.GetCompanies();
                Company = result.Value.FirstOrDefault(x => x.Key == "company", null) ?? await DOOHDBService.CreateCompany(new DOOH.Server.Models.DOOHDB.Company() { Key = "company" });
            }
            catch { }
            ShowLoading = false;
            StateHasChanged();

        }
    }
}