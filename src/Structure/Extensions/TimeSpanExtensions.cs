using System;

namespace Structure.Extensions
{
    public static class TimeSpanExtensions
    {
        public static long GetTruncateTotalDays(this TimeSpan timeSpan)
        {
            return Convert.ToInt64(Math.Truncate(timeSpan.TotalDays));
        }

        public static string FormatHourAndMinute(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm"); 
        }
    }
}
