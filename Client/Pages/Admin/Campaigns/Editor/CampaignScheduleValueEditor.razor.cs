using Microsoft.AspNetCore.Components;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignScheduleValueEditor
    {
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


        protected DOOH.Server.Models.DOOHDB.CampaignSchedule _schedule;

        protected override void OnParametersSet()
        {
            if (ScheduleId > 0)
            {
                _schedule = DOOHDBService.GetCampaignScheduleByScheduleId(scheduleId: ScheduleId).Result;
            }
            else
            {
                _schedule = new DOOH.Server.Models.DOOHDB.CampaignSchedule();
                _schedule.Rotation = Rotation;
                _schedule.Label = $"{Rotation} {(Rotation > 1 ? "rotations" : "rotation")}";
                _schedule.Start = Start;
                _schedule.End = End;
            }
        }

        protected async Task OnSubmit(DOOH.Server.Models.DOOHDB.CampaignSchedule schedule)
        {
            schedule.Label = $"{schedule.Rotation} {(schedule.Rotation > 1 ? "rotations" : "rotation")}";
            if (schedule.ScheduleId > 0)
            {
                await DOOHDBService.UpdateCampaignSchedule(ScheduleId, schedule);
            }
            else
            {
                await DOOHDBService.CreateCampaignSchedule(schedule);
            }
        }
    }
}
