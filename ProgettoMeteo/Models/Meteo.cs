using static System.Net.Mime.MediaTypeNames;

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
        public float Visibility { get; set; }

        // Nuovi parametri per l'inquinamento
        public int AQI { get; set; } // Air Quality Index
        public float CO { get; set; }  // Carbon Monoxide
        public float NO { get; set; }  // Nitric Oxide
        public float NO2 { get; set; } // Nitrogen Dioxide
        public float O3 { get; set; }  // Ozone
        public float SO2 { get; set; } // Sulfur Dioxide
        public float PM2_5 { get; set; } // Particulate Matter (PM 2.5)
        public float PM10 { get; set; } // Particulate Matter (PM 10)
        public float NH3 { get; set; }  // Ammonia

        public string ConvertiIcona(string icona)
        {
            return $"~/images/weather_icons/{icona}.png";
        }
    }
}
