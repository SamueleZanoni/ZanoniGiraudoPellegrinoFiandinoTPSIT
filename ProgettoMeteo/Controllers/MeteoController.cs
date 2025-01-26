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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CercaCitta(string nomeCitta)
        {
            if (string.IsNullOrWhiteSpace(nomeCitta))
            {
                ViewBag.ErrorMessage = "Inserisci una città valida.";
                return View("Index");
            }

            try
            {
                var meteo = await _apiService.OttieniMeteoPerCitta(nomeCitta);
                return View("Meteo", meteo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Errore durante il recupero dei dati: " + ex.Message;
                return View("Index");
            }
        }
    }
}
