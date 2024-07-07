using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOOH.Client.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Providers
{
    public partial class AddProvider
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

        protected override async Task OnInitializedAsync()
        {
            provider = new DOOH.Server.Models.DOOHDB.Provider()
            {
                ProviderId = 0,
                Country = "India"
            };
        }
        protected bool errorVisible;

        protected DOOH.Server.Models.DOOHDB.Provider provider;
        
        
        
        // FetchAddressClick
        private async Task FetchAddressClick(MouseEventArgs args)
        {
            
            var result = await DialogService.OpenAsync<GeoPicker>("Fetch Address", new Dictionary<string, object>(), new DialogOptions()
            {
                Draggable = true,
                Resizable = true,
                CloseDialogOnEsc = true,
                CloseDialogOnOverlayClick = true
            });
            if (result != null)
            {
                
            }

        }
        
        protected bool IsSaving { get; set; } = false;
        protected async Task FormSubmit()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();
                var result = await DOOHDBService.CreateProvider(provider);
                DialogService.Close(provider);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        [Inject]
        protected SecurityService Security { get; set; }
    }
}