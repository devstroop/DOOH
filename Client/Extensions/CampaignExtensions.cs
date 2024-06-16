namespace DOOH.Client.Extensions
{
    public static class CampaignExtensions
    {
        // GetBudgetTypeLabel method
        public static string GetBudgetTypeLabel(this DOOH.Server.Models.DOOHDB.Campaign campaign)
        {
            return campaign.BudgetType switch
            {
                1 => "Total",
                2 => "Daily",
                _ => "Unknown"
            };
        }

        public static string GetBudgetLabel(this DOOH.Server.Models.DOOHDB.Campaign campaign)
        {
            return campaign.BudgetType switch
            {
                1 => $"$₹{campaign.Budget}",
                2 => $"$₹{campaign.Budget} per day",
                _ => "Unknown"
            };
        }
    }
}
