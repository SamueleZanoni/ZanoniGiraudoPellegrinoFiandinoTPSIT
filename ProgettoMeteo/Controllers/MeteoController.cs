using Microsoft.AspNetCore.Mvc;
using ProgettoMeteo.Models;
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

        // GET: /Meteo/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Meteo/Index
        [HttpPost]
        public async Task<IActionResult> Index(string nomeCitta)
        {
            if (string.IsNullOrEmpty(nomeCitta))
            {
                ViewBag.Error = "Inserisci una città valida.";
                return View();
            }

            try
            {
                var meteoData = await _apiService.OttieniMeteoPerCitta(nomeCitta);
                return View(meteoData);  // Passiamo i dati meteo alla vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Errore nel recupero dei dati meteo: " + ex.Message;
                return View();
            }
        }
    }
}
