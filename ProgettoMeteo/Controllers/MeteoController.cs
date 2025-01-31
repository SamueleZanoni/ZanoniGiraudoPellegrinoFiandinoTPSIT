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
        public IActionResult PaginaAi()
        {
            return View();
        }

        // POST: /Meteo/Index
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

                return View(meteoData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Errore nel recupero dei dati meteo: " + ex.Message;
                return View();
            }
        }
    }
}
