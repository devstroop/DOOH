using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Authentication
{
    public partial class Register
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

        private DOOH.Server.Models.ApplicationUser user;
        private bool isBusy;
        private bool errorVisible;
        private string error;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = new DOOH.Server.Models.ApplicationUser();
        }

        private async Task FormSubmit()
        {
            try
            {
                isBusy = true;

                await Security.Register(user.Email, user.Password);

                DialogService.Close(true);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }

            isBusy = false;
        }

        private async Task CancelClick()
        {
            DialogService.Close(false);
        }
    }
}