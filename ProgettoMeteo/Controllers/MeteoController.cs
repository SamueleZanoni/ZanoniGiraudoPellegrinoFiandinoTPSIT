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

        [HttpPost]
        public async Task<IActionResult> PaginaAi(string testo)
        {
            if (string.IsNullOrEmpty(testo))
            {
                TempData["Error"] = "Inserisci un testo valido.";
                return RedirectToAction(nameof(PaginaAi));
            }
            try
            {
                var data = await _apiService.ApiAi(testo);
                TempData["Risposta"] = data;
                return RedirectToAction(nameof(PaginaAi));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nel recupero del testo: " + ex.Message;
                return RedirectToAction(nameof(PaginaAi));
            }
        }

        [HttpGet]
        public IActionResult PaginaAi()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            if (TempData["Risposta"] != null)
            {
                ViewData["Risposta"] = TempData["Risposta"];
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? nomeCitta, float? latitudine, float? longitudine)
        {
            try
            {
                if (latitudine.HasValue && longitudine.HasValue)
                {
                    var meteo = await _apiService.ReverseGeocoding(latitudine.Value, longitudine.Value);
                    return RedirectToAction("Index", "Meteo", new { nomeCitta = meteo.Location });
                }
                else if (!string.IsNullOrEmpty(nomeCitta))
                {
                    var meteoData = await _apiService.OttieniMeteoPerCitta(nomeCitta);
                    return RedirectToAction("Index", "Meteo", new { nomeCitta = nomeCitta });
                }
                else
                {
                    TempData["Error"] = "Inserisci una città valida o permetti l'accesso alla tua posizione.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nel recupero dei dati meteo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(string? nomeCitta)
        {
            if (!string.IsNullOrEmpty(nomeCitta))
            {
                // Qui puoi recuperare nuovamente i dati meteo se necessario
                var meteoData = await _apiService.OttieniMeteoPerCitta(nomeCitta);
                return View(meteoData);
            }

            // Gestisci l'eventuale messaggio di errore
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View();
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
