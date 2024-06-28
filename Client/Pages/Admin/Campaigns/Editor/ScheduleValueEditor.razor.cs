using DOOH.Server.Models.DOOHDB;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class ScheduleValueEditor
    {
        [Parameter] public int CampaignId { get; set; }
        [Parameter] public Server.Models.DOOHDB.Schedule Schedule { get; set; }
        [Parameter] public IEnumerable<Server.Models.DOOHDB.Adboard> Adboards { get; set; }
        
        [Inject] protected DialogService Dialog { get; set; }
        
        [Inject] protected NotificationService NotificationService { get; set; }

        RadzenDropDownDataGrid<IEnumerable<int>> grid;
        private IEnumerable<int> selectedAdboardIds;
        protected override async Task OnParametersSetAsync()
        {
            Schedule = Schedule ?? new Server.Models.DOOHDB.Schedule()
            {
                ScheduleId = 0,
                CampaignId = CampaignId
            };
        }

        private async Task OnSubmit(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            try
            {
                schedule.Label = $"{schedule.Rotation} {(schedule.Rotation > 1 ? "rotations" : "rotation")}";
                schedule.ScheduleAdboards = selectedAdboardIds.Select(x => new ScheduleAdboard
                {
                    AdboardId = x,
                    ScheduleId = schedule.ScheduleId
                }).ToList();
                Dialog.Close(schedule);

            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to save"
                });
            }
        }
    }
}
