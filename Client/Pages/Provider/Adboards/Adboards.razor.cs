using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using DOOH.Client.Layout;
using DOOH.Client.Templates;
using DOOH.Client.Pages.Admin.Adboards.Wifis;

namespace DOOH.Client.Pages.Provider.Adboards
{
    public partial class Adboards
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
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }

        [Inject]
        protected IConfiguration Configuration { get; set; }

        [CascadingParameter]
        protected AdminLayout Layout { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards = new List<Server.Models.DOOHDB.Adboard>();

        protected RadzenDataList<DOOH.Server.Models.DOOHDB.Adboard> list0;
        protected int adboardsCount;


        protected bool isAdboardsLoading = false;
        protected string search { get; set; } = "";


        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await adboardsLoadData(new LoadDataArgs());
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await adboardsLoadData(new LoadDataArgs());
            }
        }

        string SanitizeSearch(string input)
        {
            return input?.Replace("'", "''");
        }

        protected async Task adboardsLoadData(LoadDataArgs args)
        {
            try
            {
                isAdboardsLoading = true;

                // Sanitize the search input
                string SanitizeSearch(string input)
                {
                    return input?.Replace("'", "''");
                }

                string sanitizedSearch = SanitizeSearch(search);

                // Construct the search filter
                string searchFilter = !string.IsNullOrEmpty(sanitizedSearch)
                    ? $"(contains(SignName, '{sanitizedSearch}') or contains(Address, '{sanitizedSearch}') or contains(City, '{sanitizedSearch}') or contains(State, '{sanitizedSearch}') or contains(Country, '{sanitizedSearch}'))"
                    : null;

                // Combine filters
                string combinedFilter;
                if (!string.IsNullOrEmpty(searchFilter) && !string.IsNullOrEmpty(args.Filter))
                {
                    combinedFilter = $"{searchFilter} and {args.Filter}";
                }
                else if (!string.IsNullOrEmpty(searchFilter))
                {
                    combinedFilter = searchFilter;
                }
                else
                {
                    combinedFilter = args.Filter;
                }

                // Get adboards
                var result = await DOOHDBService.GetAdboards(
                    top: args.Top,
                    skip: args.Skip,
                    count: (args.Top != null && args.Skip != null),
                    filter: combinedFilter,
                    orderby: args.OrderBy,
                    expand: "AdboardImages,Category,Display($expand=Brand),Motherboard($expand=Brand),AdboardWifis,AdboardNetworks,AdboardStatuses"
                );

                // Process the result
                adboards = result.Value.AsODataEnumerable();

                foreach (var adboard in adboards)
                {
                    await Console.Out.WriteLineAsync(adboard.Category.CategoryColor);
                }

                adboardsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
            finally
            {
                isAdboardsLoading = false;
            }
        }


        protected async void DetailsSplitButtonClicked(RadzenSplitButtonItem item, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {

            if (item.Text == "Wifi")
            {
                var dialogResult = await DialogService.OpenAsync<ConfigureAdboardWifi>("Configure Wifi", new Dictionary<string, object>() { { "AdboardId", adboard.AdboardId } });
                if (dialogResult != null)
                {
                    await adboardsLoadData(new LoadDataArgs() { });
                    StateHasChanged();
                }
            }
        }

        protected async void ToggleAdboardIsActive(dynamic args, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            try
            {
                adboard.IsActive = !adboard.IsActive;
                await DOOHDBService.UpdateAdboard(adboardId: adboard.AdboardId, adboard: adboard);
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to update" });
            }
            finally
            {
                StateHasChanged();
            }
        }
        protected async void RefreshClick(MouseEventArgs args)
        {
            await adboardsLoadData(new LoadDataArgs() { });
            StateHasChanged();
        }

        protected async Task Search(MouseEventArgs args)
        {
            await list0.GoToPage(0);

            await list0.Reload();
        }

        protected async Task NetworkClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            var adboardNetwork = adboard.AdboardNetworks.FirstOrDefault();
            var parameters = new Dictionary<string, object>() { { "AdboardNetwork", adboardNetwork } };
            var options = new DialogOptions() { Width="360px", Draggable = true, CloseDialogOnOverlayClick = true, CloseDialogOnEsc = true };
            await DialogService.OpenAsync<AdboardNetworkTemplate>("Network", parameters, options);
        }

        protected async Task MapClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            var markers = new List<Tuple<string, GoogleMapPosition>>();
            var position = new GoogleMapPosition() { Lat = adboard.Latitude, Lng = adboard.Longitude };
            markers.Add(new Tuple<string, GoogleMapPosition>($"#{adboard.AdboardId}", position));
            var parameters = new Dictionary<string, object>();
            parameters.Add("Markers", markers);
            parameters.Add("Center", position);
            parameters.Add("Zoom", 18);
            var options = new DialogOptions() { Width = "800px", Height = "600px", Draggable = true, CloseDialogOnOverlayClick = true, CloseDialogOnEsc = true };
            await DialogService.OpenAsync<GoogleMapTemplate>("Map", parameters, options);
        }
    }
}