using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using static System.Net.Mime.MediaTypeNames;
using Markdig;

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

            var pollResponse = await MeteoInquinamento(latitudine, longitudine);
            dynamic infoPoll = JsonConvert.DeserializeObject<dynamic>(pollResponse);

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
                WindSpeed = (float)meteoData["wind"]["speed"],
                Visibility = (float)meteoData["visibility"],

                AQI = infoPoll["list"][0]["main"]["aqi"],
                CO = infoPoll["list"][0]["components"]["co"],
                NO = infoPoll["list"][0]["components"]["no"],
                NO2 = infoPoll["list"][0]["components"]["no2"],
                O3 = infoPoll["list"][0]["components"]["o3"],
                SO2 = infoPoll["list"][0]["components"]["so2"],
                PM2_5 = infoPoll["list"][0]["components"]["pm2_5"],
                PM10 = infoPoll["list"][0]["components"]["pm10"],
                NH3 = infoPoll["list"][0]["components"]["nh3"]
            };
        }

        public async Task<string> Meteo5Giorni(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api.openweathermap.org/data/2.5/forecast?lat={latitudine}&lon={longitudine}&appid={apiKey}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> MeteoInquinamento(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://api.openweathermap.org/data/2.5/air_pollution?lat={latitudine}&lon={longitudine}&appid={apiKey}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
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

        public async Task<string> ApiAi(string testo)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=AIzaSyB8KPMuCtIvpKl4LEAVUwM7-FAe7BoUGTs");
            var content = new StringContent($"{{\r\n    \"contents\": \r\n    [{{\r\n        \"parts\":\r\n            [{{\"text\": \"{testo}.\"}}]\r\n    }}]\r\n}}", System.Text.Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            dynamic data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            string risTesto = data["candidates"][0]["content"]["parts"][0]["text"].ToString();

            return Markdown.ToHtml(risTesto);
        }
    }
}
