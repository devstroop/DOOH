using DOOH.Server.Models.DOOHDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DOOH.Client.Templates;

public partial class CampaignScheduleTemplate : ComponentBase
{
    [Parameter] public CampaignSchedule Context { get; set; }

    private void OnRemoveRotation()
    {
        Console.WriteLine("Remove rotation");
    }

    private void OnAddRotation()
    {
        Console.WriteLine("Add rotation");
    }

    private async Task OnDelete(MouseEventArgs args)
    {
        Console.WriteLine("Delete");
    }
}