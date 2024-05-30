namespace DOOH.Client.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetTimeAgo(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                return "Just now";
            }
            if (timeSpan <= TimeSpan.FromMinutes(1))
            {
                return $"{timeSpan.Seconds} seconds ago";
            }
            if (timeSpan <= TimeSpan.FromMinutes(2))
            {
                return "A minute ago";
            }
            if (timeSpan <= TimeSpan.FromHours(1))
            {
                return $"{timeSpan.Minutes} minutes ago";
            }
            if (timeSpan <= TimeSpan.FromHours(2))
            {
                return "An hour ago";
            }
            if (timeSpan <= TimeSpan.FromDays(1))
            {
                return $"{timeSpan.Hours} hours ago";
            }
            if (timeSpan <= TimeSpan.FromDays(2))
            {
                return "Yesterday";
            }
            if (timeSpan <= TimeSpan.FromDays(30))
            {
                return $"{timeSpan.Days} days ago";
            }
            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}
