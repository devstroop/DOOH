using DOOH.Server.Models.DOOHDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DOOH.Client.Templates;

public partial class ScheduleTemplate : ComponentBase
{
    [Parameter] public Schedule Context { get; set; }
    [Parameter] public EventCallback<Schedule> Edit { get; set; } = EventCallback<Schedule>.Empty;
    [Parameter] public EventCallback<Schedule> Update { get; set; } = EventCallback<Schedule>.Empty;
    [Parameter] public EventCallback<Schedule> Delete { get; set; } = EventCallback<Schedule>.Empty;

    private async Task RemoveRotationClick(MouseEventArgs args)
    {
        if (Context.Rotation > 1)
        {
            Context.Rotation -= 1;
            await Update.InvokeAsync(Context);
        }
        else
        {
            await Delete.InvokeAsync(Context);
        }
    }

    private async Task AddRotationClick(MouseEventArgs args)
    {
        Context.Rotation += 1;
        await Update.InvokeAsync(Context);
    }

    private async Task EditClick(MouseEventArgs args)
    {
        await Edit.InvokeAsync(Context);
    }

    private async Task DeleteClick(MouseEventArgs args)
    {
        await Delete.InvokeAsync(Context);
    }
}