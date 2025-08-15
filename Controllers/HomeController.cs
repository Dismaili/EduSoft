using Microsoft.AspNetCore.Mvc;
using WebApplicationIntern2.Models;
using WebApplicationIntern2.Services;
using System.Diagnostics;

namespace WebApplicationIntern2.Controllers
{
    public class HomeController : Controller
    {
        private readonly NBRMServiceWrapper _nbrmService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(NBRMServiceWrapper nbrmService, ILogger<HomeController> logger)
        {
            _nbrmService = nbrmService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(DateTime? selectedDate = null, string searchCurrency = "")
        {
            try
            {
                // Use selected date or default to today
                var targetDate = selectedDate ?? DateTime.Now;
                
                // Set ViewBag values for form persistence
                ViewBag.SelectedDate = targetDate.ToString("yyyy-MM-dd");
                ViewBag.SearchCurrency = searchCurrency;

                // Get exchange rates from NBRM service
                var exchangeRates = await _nbrmService.GetExchangeRatesAsync(targetDate);

                if (exchangeRates?.Success == true && exchangeRates.ExchangeRates.Any())
                {
                    // Filter by currency if search term is provided
                    if (!string.IsNullOrWhiteSpace(searchCurrency))
                    {
                        var filteredRates = exchangeRates.ExchangeRates.Where(r => 
                            r.Code.Contains(searchCurrency, StringComparison.OrdinalIgnoreCase) ||
                            r.Country.Contains(searchCurrency, StringComparison.OrdinalIgnoreCase)
                        ).ToList();

                        exchangeRates.ExchangeRates = filteredRates;

                        if (!filteredRates.Any())
                        {
                            ViewBag.Message = $"No currencies found matching '{searchCurrency}'";
                        }
                    }

                    if (exchangeRates.ExchangeRates.Any())
                    {
                        ViewBag.Message = $"Successfully loaded {exchangeRates.ExchangeRates.Count} exchange rates for {targetDate:MMMM dd, yyyy}";
                    }
                }
                else
                {
                    ViewBag.Message = "Unable to retrieve exchange rates from NBRM. Please try again later.";
                    exchangeRates = new ExchangeRateResponse { Success = false, ErrorMessage = "Service unavailable" };
                }

                return View(exchangeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exchange rates");
                ViewBag.Message = "An error occurred while retrieving exchange rates.";
                
                var errorResponse = new ExchangeRateResponse 
                { 
                    Success = false, 
                    ErrorMessage = ex.Message 
                };
                
                return View(errorResponse);
            }
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
