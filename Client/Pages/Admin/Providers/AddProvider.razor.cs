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
            provider = new DOOH.Server.Models.DOOHDB.Provider();
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Provider provider;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.City> citiesForCityName;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.State> statesForStateName;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Country> countriesForCountryName;


        protected int citiesForCityNameCount;
        protected DOOH.Server.Models.DOOHDB.City citiesForCityNameValue;
        protected async Task citiesForCityNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCities(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CityName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                citiesForCityName = result.Value.AsODataEnumerable();
                citiesForCityNameCount = result.Count;

                if (!object.Equals(provider.CityName, null))
                {
                    var valueResult = await DOOHDBService.GetCities(filter: $"CityName eq '{provider.CityName}'");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        citiesForCityNameValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load City" });
            }
        }

        protected int statesForStateNameCount;
        protected DOOH.Server.Models.DOOHDB.State statesForStateNameValue;
        protected async Task statesForStateNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetStates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(StateName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                statesForStateName = result.Value.AsODataEnumerable();
                statesForStateNameCount = result.Count;

                if (!object.Equals(provider.StateName, null))
                {
                    var valueResult = await DOOHDBService.GetStates(filter: $"StateName eq '{provider.StateName}'");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        statesForStateNameValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load State" });
            }
        }

        protected int countriesForCountryNameCount;
        protected DOOH.Server.Models.DOOHDB.Country countriesForCountryNameValue;
        protected async Task countriesForCountryNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCountries(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CountryName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                countriesForCountryName = result.Value.AsODataEnumerable();
                countriesForCountryNameCount = result.Count;

                if (!object.Equals(provider.CountryName, null))
                {
                    var valueResult = await DOOHDBService.GetCountries(filter: $"CountryName eq '{provider.CountryName}'");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        countriesForCountryNameValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Country" });
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