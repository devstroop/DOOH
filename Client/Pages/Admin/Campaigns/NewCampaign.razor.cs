using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Collections;
using Radzen.Blazor.Rendering;
using System.Text.Json.Nodes;

namespace DOOH.Client.Pages.Admin.Campaigns
{
    public partial class NewCampaign
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
            campaign = new DOOH.Server.Models.DOOHDB.Campaign();
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Campaign campaign;

        protected int selectedTabIndex { get; set; } = 1;
        protected bool isAdboardsLoading = false;
        protected string search { get; set; } = "";
        RadzenScheduler<DOOH.Server.Models.DOOHDB.CampaignSchedule> scheduler;
        protected IList<DOOH.Server.Models.DOOHDB.CampaignSchedule> campaignSchedules = new List<DOOH.Server.Models.DOOHDB.CampaignSchedule>();

        protected async Task FormSubmit()
        {
            try
            {
                campaign.CampaignSchedules = campaignSchedules;
                var result = await DOOHDBService.CreateCampaign(campaign);
                DialogService.Close(campaign);
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


        [Inject]
        protected SecurityService Security { get; set; }

        protected GoogleMapPosition CenterPosition { get; set; } = new GoogleMapPosition() { Lat = 42.6977, Lng = 23.3219 };
        protected int Zoom { get; set; } = 12;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards;

        protected int adboardsCount;

        protected RadzenDataList<DOOH.Server.Models.DOOHDB.Adboard> list0;

        protected async Task Search(MouseEventArgs args)
        {
            await list0.GoToPage(0);

            await list0.Reload();


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var result = await JSRuntime.InvokeAsync<JsonArray>("getCoords");
                CenterPosition = new GoogleMapPosition() { Lat = Convert.ToDouble(result[0]), Lng = Convert.ToDouble(result[1]) };
                await JSRuntime.InvokeVoidAsync("console.log", result);
            }
        }


        void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour < 19)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }
        }

        async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            // Console Log, after Serialize
            await JSRuntime.InvokeVoidAsync("console.log", args.Start.ToString("yyyy-MM-ddTHH:mm:ss"), args.End.ToString("yyyy-MM-ddTHH:mm:ss"));
            if (args.View.Text != "Year")
            {
                DOOH.Server.Models.DOOHDB.CampaignSchedule data = await DialogService.OpenAsync<AddCampaignSchedule>("Add Schedule",
                    new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

                if (data != null)
                {
                    campaignSchedules.Add(data);
                    // Either call the Reload method or reassign the Data property of the Scheduler
                    await scheduler.Reload();
                }
            }
        }

        async Task OnScheduleSelect(SchedulerAppointmentSelectEventArgs<DOOH.Server.Models.DOOHDB.CampaignSchedule> args)
        {

            var copy = new DOOH.Server.Models.DOOHDB.CampaignSchedule
            {
                Start = args.Data.Start,
                End = args.Data.End,
                Rotation = args.Data.Rotation,
                Label = args.Data.Label
            };

            var data = await DialogService.OpenAsync<EditCampaignSchedule>("Edit Schedule", new Dictionary<string, object> { { "Schedule", copy } });

            if (data != null)
            {
                // Update the appointment
                args.Data.Start = data.Start;
                args.Data.End = data.End;
                args.Data.Rotation = data.Rotation;
                args.Data.Label = data.Label;
            }

            await scheduler.Reload();
        }

        void OnScheduleRender(SchedulerAppointmentRenderEventArgs<DOOH.Server.Models.DOOHDB.CampaignSchedule> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.Rotation == 0)
            {
                args.Attributes["style"] = "background: red";
            }
        }

        async Task OnScheduleMove(SchedulerAppointmentMoveEventArgs args)
        {
            var draggedAppointment = campaignSchedules.FirstOrDefault(x => x == args.Appointment.Data);

            if (draggedAppointment != null)
            {
                draggedAppointment.Rotation = draggedAppointment.Rotation;

                draggedAppointment.Start = draggedAppointment.Start + args.TimeSpan;

                draggedAppointment.End = draggedAppointment.End + args.TimeSpan;

                draggedAppointment.Label = draggedAppointment.Label;

                await scheduler.Reload();
            }
        }
    }
}