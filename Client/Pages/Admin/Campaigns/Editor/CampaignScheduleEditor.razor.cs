using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignScheduleEditor
    {
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected IList<DOOH.Server.Models.DOOHDB.CampaignSchedule> campaignSchedules = new List<DOOH.Server.Models.DOOHDB.CampaignSchedule>();
        RadzenScheduler<DOOH.Server.Models.DOOHDB.CampaignSchedule> scheduler;


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
                DOOH.Server.Models.DOOHDB.CampaignSchedule data = await DialogService.OpenAsync<CampaignScheduleEditor>("Add Schedule",
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

            var data = await DialogService.OpenAsync<CampaignScheduleEditor>("Edit Schedule", new Dictionary<string, object> { { "Schedule", copy } });

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
