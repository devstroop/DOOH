namespace DOOH.Server.Models.Enums
{
    public enum BudgetType
    {
        Undefined = 0,
        Total = 1,
        Daily = 2
    }

    public static class BudgetTypeExtensions
    {
        public static string ToFriendlyString(this BudgetType budgetType)
        {
            switch (budgetType)
            {
                case BudgetType.Total:
                    return "Total";
                case BudgetType.Daily:
                    return "Daily";
                default:
                    return string.Empty;
            }
        }


        public static string GetColor(this BudgetType budgetType)
        {
            switch (budgetType)
            {
                case BudgetType.Total:
                    return "darkgoldenrod";
                case BudgetType.Daily:
                    return "green";
                default:
                    return "transparent";
            }
        }
    }
}
