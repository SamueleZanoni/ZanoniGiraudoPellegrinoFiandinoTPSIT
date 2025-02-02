namespace ProgettoMeteo.Models
{
    public class FiveDayForecast
    {
        public string Icon { get; set; }
        public float Temperature { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public int Timezone { get; set; }
        public long Dt { get; set; }
    }

    public class DailyForecast
    {
        public string Date { get; set; }
        public float MaxTemperature { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Timezone { get; set; }
        public long Dt { get; set; }
        public int Speed { get; set; }
        public int Direction { get; set; }
    }
}
