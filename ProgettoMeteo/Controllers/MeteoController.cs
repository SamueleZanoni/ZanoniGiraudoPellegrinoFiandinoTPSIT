using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using ProgettoMeteo.Models;
using ProgettoMeteo.Services;
using System.Threading.Tasks;

namespace ProgettoMeteo.Controllers
{
    public class MeteoController : Controller
    {
        private readonly Api _apiService;
        
        public MeteoController(Api apiService)
        {
            _apiService = apiService;
        }

        ////GET: /Meteo/DomandeMeteo
        //[HttpGet]
        //public IActionResult DomandeMeteo()
        //{
        //    return View();
        //}

        //// POST: /Meteo/DomandeMeteo
        //[HttpPost]
        //public async Task<IActionResult> DomandeMeteo(string citta, string testo)
        //{
        //    if (string.IsNullOrEmpty(testo))
        //    {
        //        ViewBag.Error = "Inserisci un testo valido.";
        //        return View();
        //    }
        //    try
        //    {
        //        var data = await _apiService.ApiAi(testo, citta);
        //        ViewData["RispostaMeteo"] = data;

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Errore nel recupero del testo: " + ex.Message;
        //        return View();
        //    }
        //}

        // GET: /Meteo/PaginaAi
        [HttpGet]
        public IActionResult PaginaAi()
        {
            return View();
        }

        // POST: /Meteo/PaginaAi
        [HttpPost]
        public async Task<IActionResult> PaginaAi(string testo)
        {
            if (string.IsNullOrEmpty(testo))
            {
                ViewBag.Error = "Inserisci un testo valido.";
                return View();
            }
            try
            {
                var data = await _apiService.ApiAi(testo);
                ViewData["Risposta"] = data;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Errore nel recupero del testo: " + ex.Message;
                return View();
            }
        }

        // GET: /Meteo/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Meteo/Index
        [HttpPost]
        public async Task<IActionResult> Index(string? nomeCitta, float? latitudine, float? longitudine)
        {
            try
            {
                if (latitudine.HasValue && longitudine.HasValue)
                {
                    var meteo = await _apiService.ReverseGeocoding(latitudine.Value, longitudine.Value);

                    return View(meteo);
                }
                else if (!string.IsNullOrEmpty(nomeCitta))
                {
                    var meteoData = await _apiService.OttieniMeteoPerCitta(nomeCitta);
                    return View(meteoData);
                }
                else
                {
                    ViewBag.Error = "Inserisci una città valida o permetti l'accesso alla tua posizione.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Errore nel recupero dei dati meteo: " + ex.Message;
                return View();
            }
        }
        public IActionResult Mappe()
        {


            ViewData["MappaNuvole"] = $"https://tile.openweathermap.org/map/clouds_new/0/0/0.png?appid={_apiService.apiKey}";
            ViewData["MappaPressione"] = $"https://tile.openweathermap.org/map/pressure_new/0/0/0.png?appid={_apiService.apiKey}";
            ViewData["MappaVento"] = $"https://tile.openweathermap.org/map/wind_new/0/0/0.png?appid={_apiService.apiKey}";
            ViewData["MappaTemperatura"] = $"https://tile.openweathermap.org/map/temp_new/0/0/0.png?appid={_apiService.apiKey}";
            ViewData["MappaPrecipitazioni"] = $"https://tile.openweathermap.org/map/precipitation_new/0/0/0.png?appid={_apiService.apiKey}";

            return View();
        }
    }
}
