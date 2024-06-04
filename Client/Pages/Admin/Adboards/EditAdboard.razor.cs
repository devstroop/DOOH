using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using DOOH.Server.Models.DOOHDB;
using static System.Net.Mime.MediaTypeNames;
using DOOH.Client.Pages.Admin.Adboards.Displays;

namespace DOOH.Client.Pages.Admin.Adboards
{
    public partial class EditAdboard
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
        public int AdboardId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            adboard = await DOOHDBService.GetAdboardByAdboardId(adboardId:AdboardId, expand: "AdboardImages");
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Adboard adboard;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Provider> providersForProviderId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Category> categoriesForCategoryId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> attachmentsForAttachmentKey;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.City> citiesForCityName;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.State> statesForStateName;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Country> countriesForCountryName;


        protected int providersForProviderIdCount;
        protected DOOH.Server.Models.DOOHDB.Provider providersForProviderIdValue;
        protected async Task providersForProviderIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetProviders(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(ProviderName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                providersForProviderId = result.Value.AsODataEnumerable();
                providersForProviderIdCount = result.Count;

                if (!object.Equals(adboard.ProviderId, null))
                {
                    var valueResult = await DOOHDBService.GetProviders(filter: $"ProviderId eq {adboard.ProviderId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        providersForProviderIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Provider" });
            }
        }

        protected int categoriesForCategoryIdCount;
        protected DOOH.Server.Models.DOOHDB.Category categoriesForCategoryIdValue;
        protected async Task categoriesForCategoryIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCategories(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CategoryName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                categoriesForCategoryId = result.Value.AsODataEnumerable();
                categoriesForCategoryIdCount = result.Count;

                if (!object.Equals(adboard.CategoryId, null))
                {
                    var valueResult = await DOOHDBService.GetCategories(filter: $"CategoryId eq {adboard.CategoryId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        categoriesForCategoryIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Category" });
            }
        }

        protected int citiesForCityNameCount;
        protected DOOH.Server.Models.DOOHDB.City citiesForCityNameValue;
        protected async Task citiesForCityNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCities(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CityName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                citiesForCityName = result.Value.AsODataEnumerable();
                citiesForCityNameCount = result.Count;

                if (!object.Equals(adboard.CityName, null))
                {
                    var valueResult = await DOOHDBService.GetCities(filter: $"CityName eq '{adboard.CityName}'");
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

                if (!object.Equals(adboard.StateName, null))
                {
                    var valueResult = await DOOHDBService.GetStates(filter: $"StateName eq '{adboard.StateName}'");
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

                if (!object.Equals(adboard.CountryName, null))
                {
                    var valueResult = await DOOHDBService.GetCountries(filter: $"CountryName eq '{adboard.CountryName}'");
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
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateAdboard(adboardId:AdboardId, adboard);

                if (result != null)
                {
                    DialogService.Close(adboard);
                }

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



        [Inject]
        protected SecurityService Security { get; set; }


        protected IEnumerable<DOOH.Server.Models.DOOHDB.Display> displaysForDisplayId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Motherboard> motherboardsForMotherboardId;


        protected int displaysForDisplayIdCount;
        protected DOOH.Server.Models.DOOHDB.Display displaysForDisplayIdValue;
        protected async Task displaysForDisplayIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetDisplays(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(Model, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}", expand: "Brand");
                displaysForDisplayId = result.Value.AsODataEnumerable();
                displaysForDisplayIdCount = result.Count;

                if (!object.Equals(adboard.DisplayId, null))
                {
                    var valueResult = await DOOHDBService.GetDisplays(filter: $"DisplayId eq {adboard.DisplayId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        displaysForDisplayIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Display" });
            }
        }

        protected int motherboardsForMotherboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Motherboard motherboardsForMotherboardIdValue;
        protected async Task motherboardsForMotherboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetMotherboards(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(Rom, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}", expand: "Brand");
                motherboardsForMotherboardId = result.Value.AsODataEnumerable();
                motherboardsForMotherboardIdCount = result.Count;

                if (!object.Equals(adboard.MotherboardId, null))
                {
                    var valueResult = await DOOHDBService.GetMotherboards(filter: $"MotherboardId eq {adboard.MotherboardId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        motherboardsForMotherboardIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Motherboard" });
            }
        }

        protected void AdboardImagesChange(ICollection<DOOH.Server.Models.DOOHDB.AdboardImage> images)
        {
            adboard.AdboardImages = images;
            StateHasChanged();
        }

    }
}