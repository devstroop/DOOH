using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Provider.Adboards.Wifis
{
    public partial class ConfigureAdboardWifi
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
        [Inject]
        protected SecurityService Security { get; set; }

        [Parameter]
        public int AdboardId { get; set; }

        private bool isNew = false;
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.AdboardWifi adboardWifi;

        protected override async Task OnParametersSetAsync()
        {
            var adboard = await DOOHDBService.GetAdboardByAdboardId(adboardId: AdboardId, expand: "AdboardWifis");
            if ((adboard?.AdboardWifis?.Count ?? 0) > 0)
            {
                adboardWifi = adboard.AdboardWifis.FirstOrDefault();
            }
            else
            {
                adboardWifi = new DOOH.Server.Models.DOOHDB.AdboardWifi() { AdboardId = AdboardId };
                isNew = true;
            } 
        }

        protected bool IsSaving { get; set; } = false;
        protected async Task FormSubmit()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();
                if (isNew)
                {
                    adboardWifi = await DOOHDBService.CreateAdboardWifi(adboardWifi);
                    DialogService.Close(adboardWifi);
                }
                else
                {
                    var result = await DOOHDBService.UpdateAdboardWifi(adboardId: AdboardId, adboardWifi);
                    if (result != null)
                    {
                        DialogService.Close(adboardWifi);
                    }
                    else
                    {
                        errorVisible = true;
                    }
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


    }
}