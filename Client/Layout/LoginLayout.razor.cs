using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;


namespace DOOH.Client.Layout
{
    public partial class LoginLayout
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
        protected DOOHDBService DOOHDBService { get; set; }
        protected DOOH.Server.Models.DOOHDB.Company Company { get; set; }
        private bool showLoading = false;
        public bool ShowLoading
        {
            get => showLoading; set
            {
                showLoading = value;
                InvokeAsync(() => StateHasChanged());
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
                Company = await DOOHDBService.GetCompanyByKey(key: "company");
                ShowLoading = false;
                StateHasChanged();
                return;
            }
            catch { }
            Company = Company ?? new DOOH.Server.Models.DOOHDB.Company() { Key = "company" };
            ShowLoading = false;
            StateHasChanged();

        }
    }
}