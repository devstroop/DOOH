using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Settings.Cities
{
    public partial class EditCity
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
        public string CityName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            city = await DOOHDBService.GetCityByCityName(cityName:CityName);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.City city;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.State> statesForStateName;


        protected int statesForStateNameCount;
        protected DOOH.Server.Models.DOOHDB.State statesForStateNameValue;
        protected async Task statesForStateNameLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetStates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(StateName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                statesForStateName = result.Value.AsODataEnumerable();
                statesForStateNameCount = result.Count;

                if (!object.Equals(city.StateName, null))
                {
                    var valueResult = await DOOHDBService.GetStates(filter: $"StateName eq '{city.StateName}'");
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
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateCity(cityName:CityName, city);
                if (result != null)
                {
                    DialogService.Close(city);
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
    }
}