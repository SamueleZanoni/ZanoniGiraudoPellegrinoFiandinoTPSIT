
namespace ProgettoMeteo.Models
{
    public class Api
    {
        public async Task<string> MeteoCorrente(float latitudine, float longitudine)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.openweathermap.org/data/2.5/weather?lat={latitudine}&lon={longitudine}&appid=bec2cee97778ae672a64740c7aa3657d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> MeteoGeoLocation(string nomeCitta)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://api.openweathermap.org/geo/1.0/direct?q={nomeCitta}&limit=2&appid=bec2cee97778ae672a64740c7aa3657d");
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
    }
}
