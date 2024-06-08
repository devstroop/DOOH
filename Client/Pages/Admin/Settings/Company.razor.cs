using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static System.Net.Mime.MediaTypeNames;

namespace DOOH.Client.Pages.Admin.Settings
{
    public partial class Company
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

        [Parameter]
        public int Id { get; set; } = 1;

        protected bool isLoading = true;
        protected bool isEditing = false;
        protected bool isNew = false;

        protected override async Task OnInitializedAsync()
        {
            await Fetch();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Fetch();
                isLoading = false;
                StateHasChanged();
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
                company = await DOOHDBService.GetCompanyById(id: Id);
                LogoDarkImages = new List<string> { company.LogoDark };
                LogoLightImages = new List<string> { company.LogoLight };
                FaviconImages = new List<string> { company.Favicon };
                return;
            }
            catch { }
            company = company ?? new DOOH.Server.Models.DOOHDB.Company() { Id=Id };
            LogoDarkImages = LogoDarkImages ?? new List<string>();
            LogoLightImages = LogoLightImages ?? new List<string>();
            FaviconImages = FaviconImages ?? new List<string>();
        }

        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Company company;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.City> citiesForCityName;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.State> statesForStateName;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Country> countriesForCountryName;


        protected int citiesForCityNameCount;
        protected DOOH.Server.Models.DOOHDB.City citiesForCityNameValue;
        protected async Task citiesForCityNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCities(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(CityName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                citiesForCityName = result.Value.AsODataEnumerable();
                citiesForCityNameCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load City" });
            }
        }

        protected int statesForStateNameCount;
        protected DOOH.Server.Models.DOOHDB.State statesForStateNameValue;
        protected async Task statesForStateNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetStates(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(StateName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                statesForStateName = result.Value.AsODataEnumerable();
                statesForStateNameCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load State" });
            }
        }

        protected int countriesForCountryNameCount;
        protected DOOH.Server.Models.DOOHDB.Country countriesForCountryNameValue;


        protected async Task countriesForCountryNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCountries(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(CountryName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                countriesForCountryName = result.Value.AsODataEnumerable();
                countriesForCountryNameCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Country" });
            }
        }

        protected async Task SaveClick(MouseEventArgs args)
        {
            try
            {
                company.LogoDark = LogoDarkImages.FirstOrDefault();
                company.LogoLight = LogoLightImages.FirstOrDefault();
                company.Favicon = FaviconImages.FirstOrDefault();
                await DOOHDBService.UpdateCompany(id: Id, company);
                isEditing = false;
                StateHasChanged();
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Company updated" });
            }
            catch (System.Exception doohDBServiceException)
            {
                errorVisible = true;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to update Company" });
            }
        }

        protected async Task EditClick(MouseEventArgs args)
        {
            isEditing = true;
            StateHasChanged();
        }

        protected async Task CancelClick(MouseEventArgs args)
        {
            await Fetch();
            isEditing = false;
            StateHasChanged();
        }


        protected void OnRefreshImage() => StateHasChanged();
        protected List<string> LogoDarkImages = new List<string>();
        protected List<string> LogoLightImages = new List<string>();
        protected List<string> FaviconImages = new List<string>();

        protected async void OnAddDarkLogo(string image)
        {
            LogoDarkImages = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteDarkLogo(string image)
        {
            LogoDarkImages.Clear();
            StateHasChanged();
        }

        protected async void OnAddLightLogo(string image)
        {
            LogoLightImages = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteLightLogo(string image)
        {
            LogoLightImages.Clear();
            StateHasChanged();
        }

        protected async void OnAddFavicon(string image)
        {
            FaviconImages = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteFavicon(string image)
        {
            FaviconImages.Clear();
            StateHasChanged();
        }
    }
}