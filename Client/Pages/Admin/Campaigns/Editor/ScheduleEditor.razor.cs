using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Radzen;
namespace DOOH.Client.Pages.Admin.Campaigns.Editor;

public partial class ScheduleEditor
{
    
    [Parameter] public int CampaignId { get; set; } = 0;
    [Parameter] public DateTime Min { get; set; } = DateTime.Today;
    [Parameter] public DateTime Max { get; set; } = DateTime.Today.AddDays(90);
    [Parameter] public IEnumerable<DOOH.Server.Models.DOOHDB.Schedule> Data { get; set; } = new List<DOOH.Server.Models.DOOHDB.Schedule>();
    [Parameter] public IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> SelectedAdboards { get; set; } = new List<DOOH.Server.Models.DOOHDB.Adboard>();
    [Parameter] public EventCallback<DOOH.Server.Models.DOOHDB.Schedule> Add { get; set; } = EventCallback<DOOH.Server.Models.DOOHDB.Schedule>.Empty;
    [Parameter] public EventCallback<DOOH.Server.Models.DOOHDB.Schedule> Update { get; set; } = EventCallback<DOOH.Server.Models.DOOHDB.Schedule>.Empty;
    [Parameter] public EventCallback<DOOH.Server.Models.DOOHDB.Schedule> Delete { get; set; } = EventCallback<DOOH.Server.Models.DOOHDB.Schedule>.Empty;

    [Inject] protected DialogService DialogService { get; set; }

    [Inject] protected DOOHDBService DoohDbService { get; set; }

    [Inject] protected NotificationService NotificationService { get; set; }

    [Inject] protected IJSRuntime JsRuntime { get; set; }
    
    [Inject] protected IStringLocalizer<ScheduleEditor> L { get; set; }

    private Month StartMonth { get; set; } = (Month)DateTime.Today.Month;
    private DateTime SelectedDate { get; set; } = DateTime.Now;


    void OnSlotRender(SchedulerSlotRenderEventArgs args)
    {
        // Highlight today in month view
        if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
        {
            args.Attributes["style"] = "background: rgba(70,255,40,.2);";
        }
        
        if ((args.Start < Min || args.Start > Max) || (args.End < Min || args.End > Max) || (args.Start > args.End))
        {
            // disabled grey
            args.Attributes["style"] = "background: rgba(200,200,200,.2);";
        }

        // Highlight working hours (9-18)
        // if ((args.View.Text == "Week" || args.View.Text == "Day")) // && args.Start.Hour > 8 && args.Start.Hour < 19)
        // {
        //     args.Attributes["style"] = "background: rgba(255,220,40,.2);";
        // }
    }

    async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
    {
        if (args.View.Text != "Year")
        {
            if ((args.Start < Min || args.Start > Max) || (args.End < Min || args.End > Max) || (args.Start > args.End))
            {
                // NotificationService.Notify(NotificationSeverity.Error, "Invalid Date", "Selected date is out of campaign date range");
                return;
            }

            if(Data.Any(x => x.Date.Date == args.Start.Date))
            {
                await OnScheduleEdit(Data.First(x => x.Date.Date == args.Start.Date));
                return;
            }
            
            DOOH.Server.Models.DOOHDB.Schedule data = await DialogService.OpenAsync<ScheduleValueEditor>("Add Schedule",
                new Dictionary<string, object> { { "CampaignId", CampaignId }, { "Adboards", SelectedAdboards }, { "Date", args.Start } });

            if (data != null)
            {
                Data = Data.Where(x => x.ScheduleId != data.ScheduleId).Append(data);
                await Add.InvokeAsync(data);
            }
        }
    }

    async Task OnScheduleSelect(SchedulerAppointmentSelectEventArgs<DOOH.Server.Models.DOOHDB.Schedule> args)
    {
        if (args.Data == null)
        {
            return;
        }
        
        // var start = args.Start;
        // var end = args.End;
        //
        // if ((start < Min || start > Max) || (end < Min || end > Max) || (start > end))
        // {
        //     NotificationService.Notify(NotificationSeverity.Error, "Cannot Edit", "Older schedules cannot be edited");
        //     return;
        // }

        var data = await DialogService.OpenAsync<ScheduleValueEditor>("Edit Schedule", new Dictionary<string, object> { { "CampaignId", CampaignId }, { "Schedule", args.Data }, { "Adboards", SelectedAdboards }, { "SelectedAdboardIds", args.Data.ScheduleAdboards.Select(x => x.AdboardId) }  });

        if (data != null)
        {
            args.Data.Date = data.Date;
            args.Data.Rotation = data.Rotation;
            args.Data.Label = data.Label;
            args.Data.ScheduleAdboards = data.ScheduleAdboards;
        }
        
        Data = Data.Where(x => x.ScheduleId != args.Data.ScheduleId).Append(args.Data);
        await Update.InvokeAsync(args.Data);
    }

    void OnScheduleRender(SchedulerAppointmentRenderEventArgs<DOOH.Server.Models.DOOHDB.Schedule> args)
    {
        var scheduleSlotClass = "schedule-slot";
        
        if (args.Data.Rotation == 0)
        {
            args.Attributes["style"] = "background: red";
            scheduleSlotClass = $"{scheduleSlotClass} schedule-slot-error";
        }

        if ((args.Start < Min || args.Start > Max) || (args.End < Min || args.End > Max) || (args.Start > args.End))
        {
            args.Attributes["style"] = "background: rgba(200,200,200, 0.5);";
            scheduleSlotClass = $"{scheduleSlotClass} schedule-slot-disabled";
        }
        args.Attributes["class"] = scheduleSlotClass;
    }

    async Task OnScheduleMove(SchedulerAppointmentMoveEventArgs args)
    {
        var draggedAppointment = Data.FirstOrDefault(x => x == args.Appointment.Data);

        if (draggedAppointment != null)
        {
            var date = draggedAppointment.Date + args.TimeSpan;
            
            if (date < Min || date > Max)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Cannot move", "Cannot be moved to this date.");
                return;
            }

            if (Data.Any(x => x.Date.Date == date))
            {
                NotificationService.Notify(NotificationSeverity.Error, "Cannot move", "Another schedule already exists on this date.");
                return;
            }
            

            draggedAppointment.Date += args.TimeSpan;

            draggedAppointment.Label = draggedAppointment.Label;
            
            
            Data = Data.Where(x => x.ScheduleId != draggedAppointment.ScheduleId).Append(draggedAppointment);
            await Update.InvokeAsync(draggedAppointment);
            
        }
    }

    private async Task OnScheduleModify(DOOH.Server.Models.DOOHDB.Schedule data)
    {
        Data = Data.Where(x => x.ScheduleId != data.ScheduleId).Append(data);
        await Update.InvokeAsync(data);
        
    }
    
    private async Task OnScheduleDelete(DOOH.Server.Models.DOOHDB.Schedule data)
    {
        var result = await DialogService.Confirm(L["DeleteConfirmMessage"], L["DeleteConfirmTitle"]);
        if (result == true)
        {
            Data = Data.Where(x => x.ScheduleId != data.ScheduleId);
            await Delete.InvokeAsync(data);
            
        }
    }
    
    private async Task OnScheduleEdit(DOOH.Server.Models.DOOHDB.Schedule data)
    {
        var result = await DialogService.OpenAsync<ScheduleValueEditor>("Edit Schedule", new Dictionary<string, object> { { "CampaignId", CampaignId }, { "Schedule", data }, { "Adboards", SelectedAdboards }, { "SelectedAdboardIds", data.ScheduleAdboards.Select(x => x.AdboardId) }  });

        if (result != null)
        {
            data.Date = result.Date;
            data.Rotation = result.Rotation;
            data.Label = result.Label;
            data.ScheduleAdboards = result.ScheduleAdboards;
        }
        
        Data = Data.Where(x => x.ScheduleId != data.ScheduleId).Append(data);
        await Update.InvokeAsync(data);
        
    }
}