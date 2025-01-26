using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace ProgettoMeteo.Models
{
    public class Api
    {
        private readonly string apiKey = "bec2cee97778ae672a64740c7aa3657d";

        public async Task<string> MeteoCorrente(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitudine}&lon={longitudine}&units=metric&appid={apiKey}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Meteo> OttieniMeteoPerCitta(string nomeCitta)
        {
            nomeCitta = nomeCitta.Trim();

            var geoResponse = await MeteoGeoLocation(nomeCitta);
            var deserializedData = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(geoResponse);

            if (deserializedData == null || deserializedData.Count == 0)
            {
                throw new Exception("Città non trovata.");
            }

            float latitudine = (float)deserializedData[0]["lat"];
            float longitudine = (float)deserializedData[0]["lon"];

            var response = await MeteoCorrente(latitudine, longitudine);
            dynamic meteoData = JsonConvert.DeserializeObject<dynamic>(response);

            return new Meteo
            {
                Location = $"{meteoData["name"]}, {meteoData["sys"]["country"]}",
                Description = meteoData["weather"][0]["description"],
                Temperature = (float)meteoData["main"]["temp"],
                FeelsLike = (float)meteoData["main"]["feels_like"],
                Humidity = (int)meteoData["main"]["humidity"],
                Pressure = (int)meteoData["main"]["pressure"],
                Icon = meteoData["weather"][0]["icon"],
                Sunrise = UnixTimeToDateTime((long)meteoData["sys"]["sunrise"]),
                Sunset = UnixTimeToDateTime((long)meteoData["sys"]["sunset"]),
                WindSpeed = (float)meteoData["wind"]["speed"]
            };
        }

        public async Task<string> MeteoGeoLocation(string nomeCitta)
        {
            var client = new HttpClient();
            var url = $"http://api.openweathermap.org/geo/1.0/direct?q={nomeCitta}&limit=1&appid={apiKey}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static string UnixTimeToDateTime(long unixTime)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
            return dateTime.ToString("HH:mm"); // Formato 24 ore
        }
    }
}
