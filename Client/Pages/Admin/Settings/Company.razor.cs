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

        private string Key = "company";
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
            }
        }

        //protected override async Task OnParametersSetAsync()
        //{
        //    await Fetch();
        //}


        protected async Task Fetch()
        {
            try
            {
                company = await DOOHDBService.GetCompanyByKey(key: Key);
                Icons = new List<string> { company.Icon };
                Logos = new List<string> { company.Logo };
                AdminLogos = new List<string> { company.AdminLogo };
                ProviderLogos = new List<string> { company.ProviderLogo };
                LoginLogos = new List<string> { company.LoginLogo };
                isLoading = false;
                return;
            }
            catch { }
            company = company ?? new DOOH.Server.Models.DOOHDB.Company() { Key=Key };
            Icons = Icons ?? new List<string>();
            Logos = Logos ?? new List<string>();
            AdminLogos = AdminLogos ?? new List<string>();
            ProviderLogos = ProviderLogos ?? new List<string>();
            LoginLogos = LoginLogos ?? new List<string>();
            isNew = true;
            isLoading = false;
            StateHasChanged();
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
                company.Icon = Icons.FirstOrDefault();
                company.Logo = Logos.FirstOrDefault();
                company.AdminLogo = AdminLogos.FirstOrDefault();
                company.ProviderLogo = ProviderLogos.FirstOrDefault();
                company.LoginLogo = LoginLogos.FirstOrDefault();
                if (isNew)
                {
                    await DOOHDBService.CreateCompany(company);
                }
                else
                {
                    await DOOHDBService.UpdateCompany(key: Key, company);
                }
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Company information updated!" });
            }
            catch (System.Exception doohDBServiceException)
            {
                errorVisible = true;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to update Company" });
            }
            finally
            {
                isEditing = false;
                StateHasChanged();
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
        protected List<string> Icons = new List<string>();
        protected List<string> Logos = new List<string>();
        protected List<string> AdminLogos = new List<string>();
        protected List<string> ProviderLogos = new List<string>();
        protected List<string> LoginLogos = new List<string>();


        protected async void OnAddIcon(string image)
        {
            if (String.IsNullOrEmpty(image)) { return; }
            Icons = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteIcon(string image)
        {
            Icons = new List<string>();
            StateHasChanged();
        }
        protected async void OnAddLoginLogo(string image)
        {
            if (String.IsNullOrEmpty(image)) { return; }
            LoginLogos = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteLoginLogo(string image)
        {
            LoginLogos = new List<string>();
            StateHasChanged();
        }
        protected async void OnAddLogo(string image)
        {
            if (String.IsNullOrEmpty(image)) { return; }
            Logos = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteLogo(string image)
        {
            Logos = new List<string>();
            StateHasChanged();
        }

        protected async void OnAddAdminLogo(string image)
        {
            if (String.IsNullOrEmpty(image)) { return; }
            AdminLogos = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteAdminLogo(string image)
        {
            AdminLogos = new List<string>();
            StateHasChanged();
        }

        protected async void OnAddProviderLogo(string image)
        {
            if (String.IsNullOrEmpty(image)) { return; }
            ProviderLogos = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteProviderLogo(string image)
        {
            ProviderLogos = new List<string>();
            StateHasChanged();
        }





        private async Task<bool> CheckImageUrlAsync(string url)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>("checkImage", url);
            }
            catch
            {
                return false;
            }
        }
    }
}