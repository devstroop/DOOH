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
    public partial class PublicLayout
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

        protected void ProfileMenuClick(RadzenProfileMenuItem args)
        {
            if (args.Value == "Logout")
            {
                Security.Logout();
            }
        }

        protected async Task DashboardClick(MouseEventArgs args)
        {
            if (Security.IsInRole("Admin"))
            {
                NavigationManager.NavigateTo("/admin/dashboard");
            }
            else if (Security.IsInRole("Provider"))
            {
                NavigationManager.NavigateTo("/provider/dashboard");
            }
            else
            {
                return;
            }
        }
    }
}