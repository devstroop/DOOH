namespace DOOH.Server.Models.Enums
{
    public enum BudgetType
    {
        Total = 1,
        Daily = 2
    }

    public static class BudgetTypeExtensions
    {
        public static string ToFriendlyString(this BudgetType budgetType)
        {
            return budgetType switch
            {
                BudgetType.Total => "Total",
                BudgetType.Daily => "Daily",
                _ => string.Empty
            };
        }


        public static string GetColor(this BudgetType? budgetType)
        {
            return budgetType.HasValue ? budgetType.Value.GetColor() : "transparent";
        }

        public static string GetColor(this BudgetType budgetType)
        {
            return budgetType switch
            {
                BudgetType.Total => "darkgoldenrod",
                BudgetType.Daily => "green",
                _ => "transparent"
            };
        }
    }
}
