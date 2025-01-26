namespace ProgettoMeteo.Models
{
    public class Meteo
    {
        public string Location { get; set; }
        public string Description { get; set; }
        public float Temperature { get; set; }
        public float FeelsLike { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public string Icon { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public float WindSpeed { get; set; }
    }
}
