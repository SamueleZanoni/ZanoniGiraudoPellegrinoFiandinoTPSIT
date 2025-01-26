using System.Net.Http;
using System;
using System.Text.Json;
using Newtonsoft.Json;
using ProgettoMeteo.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProgettoMeteo.Models
{
    public class Luogo
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
    }
    public class Api
    {
        #region "OpenWeather"
        public async Task<string> MeteoCorrente(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.openweathermap.org/data/2.5/weather?lat={latitudine}&lon={longitudine}&appid=bec2cee97778ae672a64740c7aa3657d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Meteo> OttieniMeteoPerCitta(string nomeCitta)
        {
            var geoResponse = await MeteoGeoLocation(nomeCitta);

            // Deserializzazione della risposta JSON in un array di dizionari (dato che l'API può restituire più risultati)
            var deserializedData1 = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(geoResponse);

            if (deserializedData1 == null || deserializedData1.Count == 0)
            {
                throw new Exception("Città non trovata.");
            }

            // Prendi la prima città dalla lista dei risultati
            float latitudine = (float)deserializedData1[0]["lat"];
            float longitudine = (float)deserializedData1[0]["lon"];

            var response = await MeteoCorrente(latitudine, longitudine);

            dynamic deserializedData2 = JsonConvert.DeserializeObject<dynamic>(response);

            var dati = new Meteo
            {
                Location = $"{deserializedData2["name"]}, {deserializedData2["sys"]["country"]}",
                Description = deserializedData2["weather"][0]["description"],
                Temperature = (float)deserializedData2["main"]["temp"],
                FeelsLike = (float)deserializedData2["main"]["feels_like"],
                Humidity = (int)deserializedData2["main"]["humidity"],
                Pressure = (int)deserializedData2["main"]["pressure"],
                Icon = deserializedData2["weather"][0]["icon"],
                Sunrise = UnixTimeToDateTime((long)deserializedData2["sys"]["sunrise"]),
                Sunset = UnixTimeToDateTime((long)deserializedData2["sys"]["sunset"]),
                WindSpeed = (float)deserializedData2["wind"]["speed"]
            };

            return dati;
        }

        public async Task<string> MeteoGeoLocation(string nomeCitta)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://api.openweathermap.org/geo/1.0/direct?q={nomeCitta}&limit=1&appid=bec2cee97778ae672a64740c7aa3657d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Meteo5Giorni(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api.openweathermap.org/data/2.5/forecast?lat={latitudine}&lon={longitudine}&appid=bec2cee97778ae672a64740c7aa3657d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> MeteoInquinamento(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://api.openweathermap.org/data/2.5/air_pollution?lat={latitudine}&lon={longitudine}&appid=bec2cee97778ae672a64740c7aa3657d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> MeteoMappe(string tipoMappa)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://tile.openweathermap.org/map/{tipoMappa}/0/0/0.png?appid=bec2cee97778ae672a64740c7aa3657d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        private static string UnixTimeToDateTime(long unixTime)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
            return dateTime.ToString("hh:mm tt"); // Formato 12 ore (AM/PM)
        }
    }
}
