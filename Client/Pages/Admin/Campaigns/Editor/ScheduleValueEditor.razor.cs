using DOOH.Server.Models.DOOHDB;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class ScheduleValueEditor
    {
        [Parameter] public int CampaignId { get; set; }
        [Parameter] public DateTime Date { get; set; } = DateTime.Today;
        [Parameter] public Server.Models.DOOHDB.Schedule Schedule { get; set; }
        [Parameter] public IEnumerable<Server.Models.DOOHDB.Adboard> Adboards { get; set; }
        [Parameter] public IEnumerable<int> SelectedAdboardIds { get; set; } = new List<int>();

        [Inject] protected DialogService Dialog { get; set; }
        
        [Inject] protected NotificationService NotificationService { get; set; }

        RadzenDropDownDataGrid<IEnumerable<int>> grid;
        protected override async Task OnParametersSetAsync()
        {
            Schedule = Schedule ?? new Server.Models.DOOHDB.Schedule()
            {
                ScheduleId = 0,
                CampaignId = CampaignId,
                Date = Date,
                Rotation = 1
            };
            StateHasChanged();
        }

        private async Task OnSubmit(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            try
            {
                if (!SelectedAdboardIds.Any())
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Please select at least one adboard"
                    });
                    return;
                }
                // Suggest a suitable label for the schedule, contains no of rotations and adboards
                var label
                    = $"{schedule.Rotation} {(schedule.Rotation > 1 ? "rotations" : "rotation")} / {SelectedAdboardIds.Count()} {(SelectedAdboardIds.Count() > 1 ? "adboards" : "adboard")}";
                // schedule.Label = $"{schedule.Rotation} {(schedule.Rotation > 1 ? "rotations" : "rotation")}";
                schedule.Label = label;
                var adboards = Adboards.Where(x => SelectedAdboardIds.Contains(x.AdboardId)).ToList();
                var scheduleAdboards = adboards.Select(x => new ScheduleAdboard
                {
                    AdboardId = x.AdboardId,
                    ScheduleId = schedule.ScheduleId
                }).ToList();
                schedule.ScheduleAdboards = scheduleAdboards;
                Dialog.Close(schedule);

            }
            catch (Exception exception)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to save"
                });
            }
        }
    }
}
