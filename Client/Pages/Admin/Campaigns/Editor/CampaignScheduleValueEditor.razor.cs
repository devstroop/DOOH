using Microsoft.AspNetCore.Components;
using Radzen;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignScheduleValueEditor
    {
        [Parameter]
        public int CampaignId { get; set; }
        
        [Parameter]
        public int ScheduleId { get; set; } = 0;
        
        [Parameter]
        public int Rotation { get; set; } = 1;

        [Parameter]
        public DateTime Start { get; set; }

        [Parameter]
        public DateTime End { get; set; }

        [Inject]
        protected DOOHDBService DOOHDBService { get; set; }
        
        [Inject]
        protected DialogService Dialog { get; set; }
        
        [Inject]
        protected NotificationService NotificationService { get; set; }


        protected DOOH.Server.Models.DOOHDB.CampaignSchedule _schedule;
        
        
        private bool IsSaving { get; set; } = false;

        protected override async Task OnParametersSetAsync()
        {
            if (ScheduleId == 0)
            {
                _schedule = new DOOH.Server.Models.DOOHDB.CampaignSchedule();
                _schedule.CampaignId = CampaignId;
                _schedule.ScheduleId = ScheduleId;
                _schedule.Rotation = Rotation;
                _schedule.Label = $"{Rotation} {(Rotation > 1 ? "rotations" : "rotation")}";
                _schedule.Start = Start;
                _schedule.End = End;
            }
            else
            {
                _schedule = await DOOHDBService.GetCampaignScheduleByScheduleId(scheduleId: ScheduleId);
            }
        }

        protected async Task OnSubmit(DOOH.Server.Models.DOOHDB.CampaignSchedule schedule)
        {
            try
            {
                IsSaving = true;
                StateHasChanged();
                schedule.Label = $"{schedule.Rotation} {(schedule.Rotation > 1 ? "rotations" : "rotation")}";
                // if (schedule.ScheduleId > 0)
                // {
                //     await DOOHDBService.UpdateCampaignSchedule(ScheduleId, schedule);
                // }
                // else
                // {
                //     schedule = await DOOHDBService.CreateCampaignSchedule(schedule);
                // }
                Dialog.Close(schedule);

            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to save"
                });
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }
    }
}
