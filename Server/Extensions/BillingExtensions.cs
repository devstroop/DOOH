using DOOH.Server.Models.DOOHDB;

namespace DOOH.Server.Extensions;

public static class BillingExtensions
{
    public static decimal Estimate(this Schedule schedule)
    {
        try
        {
            return schedule.Rotation * schedule.ScheduleCampaignAdboards.AsEnumerable().Select(x => x.CampaignAdboard.Adboard.BaseRatePerSecond).Sum() ?? 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }
    
}