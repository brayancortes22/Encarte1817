using System;
using System.Globalization;

namespace Utilities.Helpers
{
    public interface IDatetimeHelper
    {
        DateTime GetCurrentUtcDateTime();
        DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId);
        DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId);
        string FormatDateTime(DateTime dateTime, string format = null);
        int CalculateAge(DateTime birthDate);
        bool IsWeekend(DateTime date);
        bool IsBusinessHour(DateTime dateTime, int startHour = 9, int endHour = 17);
    }

    public class DatetimeHelper : IDatetimeHelper
    {
        public DateTime GetCurrentUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        public DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }

        public DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
        }

        public string FormatDateTime(DateTime dateTime, string format = null)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = "yyyy-MM-dd HH:mm:ss";

            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }

        public int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            // Ajustar si el cumpleaños aún no ha ocurrido este año
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public bool IsBusinessHour(DateTime dateTime, int startHour = 9, int endHour = 17)
        {
            if (IsWeekend(dateTime))
                return false;

            int hour = dateTime.Hour;
            return hour >= startHour && hour < endHour;
        }
    }
}