﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Text.Json.Nodes;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignAdboardsEditor
    {
        [Parameter]
        public int CampaignId { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected DOOHDBService DOOHDBService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected RadzenDataList<DOOH.Server.Models.DOOHDB.Adboard> list0;
        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards;
        protected IEnumerable<int> selectedAdboardIds = new List<int>();
        protected int Zoom { get; set; } = 12;
        protected bool showSelectedAdboardsOnly { get; set; } = false;

        protected int adboardsCount;
        protected bool isAdboardsLoading = false;
        protected string search { get; set; } = "";
        protected GoogleMapPosition CenterPosition { get; set; } = new GoogleMapPosition() { Lat = 42.6977, Lng = 23.3219 };

        protected async Task Search(MouseEventArgs args)
        {
            await list0.GoToPage(0);

            await list0.Reload();


        }
        protected async Task FetchCurrentLocation(MouseEventArgs args)
        {
            var result = await JSRuntime.InvokeAsync<JsonArray>("getCoords");
            await JSRuntime.InvokeVoidAsync("console.log", (double)result[0]);
            await JSRuntime.InvokeVoidAsync("console.log", (double)result[1]);
            CenterPosition = new GoogleMapPosition() { Lat = (double)result[0], Lng = (double)result[1] };
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
                    ? $"(contains(Address, '{sanitizedSearch}') or contains(CityName, '{sanitizedSearch}') or contains(StateName, '{sanitizedSearch}') or contains(CountryName, '{sanitizedSearch}'))"
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
                    expand: "AdboardImages,Category,Display($expand=Brand),Motherboard($expand=Brand),AdboardWifis,AdboardNetworks"
                );

                // Process the result
                adboards = result.Value.AsODataEnumerable();
                selectedAdboardIds = selectedAdboardIds.Where(x => adboards.Any(y => y.AdboardId == x)).ToList();

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



        RadzenGoogleMap map;

        async void OnMapClick(GoogleMapClickEventArgs args)
        {
            //console.Log($"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
            await JSRuntime.InvokeVoidAsync("console.log", $"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
        }

        async Task OnMarkerClick(RadzenGoogleMapMarker marker)
        {
            var adboardId = Convert.ToInt32(marker.Title);
            var adboard = adboards.FirstOrDefault(x => x.AdboardId == adboardId);
            var message = $"<img src=\"{adboard.AdboardImages.FirstOrDefault().Image}\" class=\"\" style=\"height: 120px; width: auto; object-fit: cover; border-radius: 4px;\"/>" +
                $"<br/><br/>" +
                $"ID: {adboardId}" +
                $"<br/>" +
                $"{adboard.Address.Trim()}" +
                $"<br/>" +
                $"Price <b>₹{adboard.BaseRatePerSecond ?? 0}</b>/sec";

            var code = $@"
var map = Radzen['{map.UniqueID}'].instance;
var marker = map.markers.find(m => m.title == '{marker.Title}');
if(window.infoWindow) {{window.infoWindow.close();}}
window.infoWindow = new google.maps.InfoWindow({{content: '{message}'}});
setTimeout(() => window.infoWindow.open(map, marker), 200);
";

            await JSRuntime.InvokeVoidAsync("eval", code);
        }

        protected async Task OnSelectAdboard(DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            if (!selectedAdboardIds.Contains(adboard.AdboardId))
            {
                selectedAdboardIds = selectedAdboardIds.Append(adboard.AdboardId).ToList();
                StateHasChanged();
            }
        }

        protected async Task OnUnselectAdboard(DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            if (selectedAdboardIds.Contains(adboard.AdboardId))
            {
                selectedAdboardIds = selectedAdboardIds.Where(x => x != adboard.AdboardId).ToList();
                StateHasChanged();
            }
        }

        protected async Task ClearSelectedAdboards()
        {
            selectedAdboardIds = new List<int>();
            StateHasChanged();
        }


        protected string selectedAdboardLabel => $"{selectedAdboardIds.Count()} selected!";

        protected bool showSelectedAdboardLabel => selectedAdboardIds.Count() > 0;
    }
}