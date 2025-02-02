using System;

namespace ProgettoMeteo.Services
{
    public class UtilityService
    {
        private static readonly string[] WeekDayNames =
        {
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };

        private static readonly string[] MonthNames =
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };


        public string GetDate(long dateUnix, int timezoneOffset)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(dateUnix + timezoneOffset).UtcDateTime;
            string weekDayName = WeekDayNames[(int)date.DayOfWeek];
            string monthName = MonthNames[date.Month - 1];

            return $"{weekDayName} {date.Day}, {monthName}";
        }

        public (string WeekDay, string DateAndMonth) GetDateParts(long dateUnix, int timezoneOffset)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(dateUnix + timezoneOffset).UtcDateTime;
            string weekDayName = WeekDayNames[(int)date.DayOfWeek];
            string monthName = MonthNames[date.Month - 1];
            string dayMonth = $"{date.Day} {monthName}";

            return (weekDayName, dayMonth);
        }

        public string GetTime(long timeUnix, int timezoneOffset)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(timeUnix + timezoneOffset).UtcDateTime;
            int hours = date.Hour;
            int minutes = date.Minute;
            string period = hours >= 12 ? "PM" : "AM";

            return $"{(hours % 12 == 0 ? 12 : hours % 12):D2}:{minutes:D2} {period}";
        }

        public string GetHours(long timeUnix, int timezoneOffset)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(timeUnix + timezoneOffset).UtcDateTime;
            int hours = date.Hour;
            string period = hours >= 12 ? "PM" : "AM";

            return $"{(hours % 12 == 0 ? 12 : hours % 12)} {period}";
        }

        public double MpsToKmh(double mps)
        {
            return mps * 3600 / 1000;
        }

        public string GetAqiLevel(int aqi)
        {
            return aqi switch
            {
                1 => "Good",
                2 => "Fair",
                3 => "Moderate",
                4 => "Poor",
                5 => "Very Poor",
                _ => "Unknown AQI level"
            };
        }
    }
}
