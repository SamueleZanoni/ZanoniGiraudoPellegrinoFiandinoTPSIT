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
                1 => "Good: Air quality is considered satisfactory, and air pollution poses little or no risk.",
                2 => "Fair: Air quality is acceptable; however, for some pollutants there may be a moderate health concern for sensitive people.",
                3 => "Moderate: Sensitive groups may experience health effects. The general public is unlikely to be affected.",
                4 => "Poor: Everyone may begin to experience health effects; sensitive groups may experience more serious effects.",
                5 => "Very Poor: Emergency conditions with serious risks for the entire population.",
                _ => "Unknown AQI level"
            };
        }
    }
}
