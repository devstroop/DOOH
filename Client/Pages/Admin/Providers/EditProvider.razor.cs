using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Providers
{
    public partial class EditProvider
    {
        [Inject]
        protected IJSRuntime Runtime { get; set; }

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
        public DOOHDBService DatabaseService { get; set; }

        [Parameter]
        public int ProviderId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            provider = await DatabaseService.GetProviderByProviderId(providerId:ProviderId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Provider provider;


        protected bool IsSaving { get; set; } = false;
        protected async Task FormSubmit()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();
                var result = await DatabaseService.UpdateProvider(providerId:ProviderId, provider);
                if (result != null)
                {
                    DialogService.Close(provider);
                }
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

        protected async Task DeleteButtonClick(MouseEventArgs args)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DatabaseService.DeleteProvider(ProviderId);
                    if (deleteResult != null)
                    {
                        DialogService.Close(provider);
                    }
                }
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete Provider" });
            }
        }


        [Inject]
        protected SecurityService Security { get; set; }
    }
}