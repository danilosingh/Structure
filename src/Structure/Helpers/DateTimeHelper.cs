using System;
using System.Threading;

namespace Structure.Helpers
{
    public static class DateTimeHelper
    {
        public static string GetMonthName(int month)
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        public static string GetMonthYearDescription(DateTime date)
        {
            return GetMonthName(date.Month) + "/" + date.ToString("yy");
        }

        public static bool IsNullOrEmpty(Nullable<DateTime> date, bool validarDataMinima = true)
        {
            if (date == null || (validarDataMinima && date == DateTime.MinValue))
            {
                return true;
            }

            return false;
        }

        public static bool IsMinimal(DateTime date)
        {
            return date <= DateTime.MinValue;
        }

        public static DateTime? GetGreaterDate(DateTime? dataA, DateTime? dataB)
        {
            return GreaterThan(dataA, dataB) ? dataA : dataB;
        }

        public static bool GreaterThan(DateTime? date, DateTime? dataComparacao)
        {
            return Nullable.Compare(date, dataComparacao) > 0;
        }

        public static bool GreaterThanOrEqual(DateTime? date, DateTime? dataComparacao)
        {
            return Nullable.Compare(date, dataComparacao) >= 0;
        }

        public static bool NotEqual(DateTime? date, DateTime? dataComparacao)
        {
            return Nullable.Compare(date, dataComparacao) != 0;
        }

        public static string FormatDateTime(DateTime date)
        {
            return date.ToString("dd/MM/yyy HH:mm");
        }

        public static DateTimeOffset GetDatetimeWithUtc(DateTime baseDatetime, decimal utc)
        {
            DateTime date = DateTime.SpecifyKind(baseDatetime.ToUniversalTime(), DateTimeKind.Unspecified);
            return new DateTimeOffset(date.AddHours((double)utc), new TimeSpan((int)utc, 0, 0));
        }

        public static DateTimeOffset GetNowWithUtc(decimal utc)
        {
            return GetDatetimeWithUtc(DateTime.Now, utc);
        }
    }
}
