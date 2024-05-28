using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Settings.Countries
{
    public partial class EditCountry
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

        [Parameter]
        public string CountryName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            country = await DOOHDBService.GetCountryByCountryName(countryName: CountryName);
        }
        protected bool errorVisible;
        protected Server.Models.DOOHDB.Country country;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateCountry(countryName: CountryName, country);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                    hasChanges = true;
                    canEdit = false;
                    return;
                }
                DialogService.Close(country);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            country = await DOOHDBService.GetCountryByCountryName(countryName: CountryName);
        }
    }
}