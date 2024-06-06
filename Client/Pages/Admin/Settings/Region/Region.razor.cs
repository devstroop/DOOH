using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Settings.Region
{
    public partial class Region
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Country> countries;
        protected IEnumerable<DOOH.Server.Models.DOOHDB.State> states;
        protected IEnumerable<DOOH.Server.Models.DOOHDB.City> cities;

        protected override async Task OnInitializedAsync()
        {
            await LoadCountries();
            await LoadStates();
            await LoadCities();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadCountries();
                await LoadStates();
                await LoadCities();
            }
        }



        protected async Task LoadCountries()
        {
            var result = await DOOHDBService.GetCountries();
            countries = result.Value.AsODataEnumerable();
        }

        protected async Task LoadStates()
        {
            var result = await DOOHDBService.GetStates();
            states = result.Value.AsODataEnumerable();
        }

        protected async Task LoadCities()
        {
            var result = await DOOHDBService.GetCities();
            cities = result.Value.AsODataEnumerable();
        }

        // AddCountryClick
        protected async Task AddCountryClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddCountry>("Add Country");
            await LoadCountries();
            StateHasChanged();
        }

        protected async Task DeleteCountryClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Country country)
        {
            try
            {
                // Confirm
                var result = await DialogService.Confirm("Are you sure?", "Delete Country", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                if (result == true)
                {
                    var resultDelete = await DOOHDBService.DeleteCountry(countryName: country.CountryName);
                    if (resultDelete != null)
                    {
                        await LoadCountries();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to delete Country" });
            }
            finally
            {
                StateHasChanged();
            }
        }

        protected async Task AddStateClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Country country)
        {
            await DialogService.OpenAsync<AddState>("Add State", new Dictionary<string, object>() { { "CountryName", country.CountryName } });
            await LoadStates();
            StateHasChanged();
        }

        protected async Task DeleteStateClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.State state)
        {
            try
            {
                var result = await DialogService.Confirm("Are you sure?", "Delete State", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                if (result == true)
                {
                    var resultDelete = await DOOHDBService.DeleteState(stateName: state.StateName);
                    if (resultDelete != null)
                    {
                        await LoadStates();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to delete State" });
            }
            finally
            {
                StateHasChanged();
            }
        }

        protected async Task AddCityClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.State state)
        {
            await DialogService.OpenAsync<AddCity>("Add City", new Dictionary<string, object>() { { "StateName", state.StateName } });
            await LoadCities();
            StateHasChanged();
        }

        protected async Task DeleteCityClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.City city)
        {
            try
            {
                var result = await DialogService.Confirm("Are you sure?", "Delete City", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                if (result == true)
                {
                    var resultDelete = await DOOHDBService.DeleteCity(cityName: city.CityName);
                    if (resultDelete != null)
                    {
                        await LoadCities();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to delete City" });
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}