using Microsoft.AspNetCore.Mvc;
using WebApplicationIntern2.Models;
using WebApplicationIntern2.Services;
using System.Globalization;
using System.Diagnostics;

namespace WebApplicationIntern2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? selectedDate, string? searchCurrency)
        {
            var model = new ExchangeRateResponse
            {
                Success = true,
                Date = DateTime.Now
            };

            try
            {
                // Using Connected Service approach as requested by mentor
                using var nbrmService = new NBRMServiceWrapper();
                List<ExchangeRateRow> exchangeRates;

                if (!string.IsNullOrEmpty(selectedDate) && DateTime.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    // Get exchange rates for specific date using Connected Service
                    exchangeRates = await nbrmService.GetExchangeRatesForDateAsync(parsedDate.ToString("dd.MM.yyyy"));
                    model.Date = parsedDate;
                }
                else
                {
                    // Get current exchange rates using Connected Service
                    exchangeRates = await nbrmService.GetExchangeRatesAsync();
                }

                // Apply search filter if specified
                if (!string.IsNullOrEmpty(searchCurrency))
                {
                    exchangeRates = exchangeRates
                        .Where(r => r.Code.Contains(searchCurrency.ToUpper()) || 
                                   r.Country.Contains(searchCurrency, StringComparison.OrdinalIgnoreCase) ||
                                   r.Currency.Contains(searchCurrency, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    ViewBag.SearchCurrency = searchCurrency;
                }

                model.ExchangeRates = exchangeRates.OrderBy(r => r.Code).ToList();
                ViewBag.SelectedDate = selectedDate;
                ViewBag.Message = $"Connected Service: Found {exchangeRates.Count} exchange rates from NBRM";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exchange rates from NBRM");
                model.Success = false;
                model.ErrorMessage = ex.Message;
                ViewBag.Message = "Could not retrieve exchange rates from NBRM";
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
