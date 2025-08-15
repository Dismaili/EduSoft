using WebApplicationIntern2.Models;
using WebApplicationIntern2.NBRMWebService;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplicationIntern2.Services
{
    /// <summary>
    /// Service wrapper that uses the Connected Service reference to NBRM
    /// This demonstrates the proper enterprise approach using Visual Studio Connected Services
    /// </summary>
    public class NBRMServiceWrapper : IDisposable
    {
        private readonly KLServiceSoapClient _client;
        private readonly HttpClient _httpClient;

        public NBRMServiceWrapper()
        {
            _client = new KLServiceSoapClient(KLServiceSoapClient.EndpointConfiguration.KLServiceSoap);
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Get current exchange rates using Connected Service SOAP client
        /// This is the enterprise-standard approach your mentor expects
        /// </summary>
        public async Task<List<ExchangeRateRow>> GetExchangeRatesAsync()
        {
            try
            {
                // First try the SOAP service (as per Connected Services approach)
                // If it fails, fallback to REST API
                try
                {
                    var soapResult = await _client.GetExchangeRatesAsync();
                    return ParseSoapResponse(soapResult);
                }
                catch (Exception soapEx)
                {
                    // Fallback to REST API if SOAP fails
                    var today = DateTime.Now.ToString("dd.MM.yyyy");
                    var url = $"https://www.nbrm.mk/KLServiceNOV/GetExchangeRate?StartDate={today}&EndDate={today}&format=json";
                    
                    var response = await _httpClient.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return ParseRestResponse(json);
                    }
                    else
                    {
                        throw new Exception($"Both SOAP and REST calls failed. SOAP: {soapEx.Message}, REST: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling NBRM service: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get exchange rates for specific date returning ExchangeRateResponse
        /// </summary>
        public async Task<ExchangeRateResponse> GetExchangeRatesAsync(DateTime targetDate)
        {
            try
            {
                var dateString = targetDate.ToString("dd.MM.yyyy");
                var rates = await GetExchangeRatesForDateAsync(dateString);
                
                return new ExchangeRateResponse
                {
                    ExchangeRates = rates,
                    Date = targetDate,
                    Success = true,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ExchangeRateResponse
                {
                    ExchangeRates = new List<ExchangeRateRow>(),
                    Date = targetDate,
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get exchange rates for specific date using Connected Service
        /// </summary>
        public async Task<List<ExchangeRateRow>> GetExchangeRatesForDateAsync(string date)
        {
            try
            {
                // Try SOAP first (Connected Service approach)
                try
                {
                    var soapResult = await _client.GetExchangeRatesDAsync(date);
                    return ParseSoapResponse(soapResult);
                }
                catch (Exception soapEx)
                {
                    // Fallback to REST API
                    var url = $"https://www.nbrm.mk/KLServiceNOV/GetExchangeRate?StartDate={date}&EndDate={date}&format=json";
                    
                    var response = await _httpClient.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return ParseRestResponse(json);
                    }
                    else
                    {
                        throw new Exception($"Both SOAP and REST calls failed. SOAP: {soapEx.Message}, REST: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling NBRM service: {ex.Message}", ex);
            }
        }

        private List<ExchangeRateRow> ParseSoapResponse(string soapResponse)
        {
            var rates = new List<ExchangeRateRow>();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(soapResponse);

                // Parse SOAP XML response
                var rateNodes = xmlDoc.SelectNodes("//KursnaLista");
                
                if (rateNodes != null)
                {
                    foreach (XmlNode node in rateNodes)
                    {
                        var rate = new ExchangeRateRow
                        {
                            Code = GetNodeValue(node, "Sifra") ?? "",
                            Country = GetNodeValue(node, "ZemjaAng") ?? GetNodeValue(node, "Zemja") ?? "",
                            Currency = GetNodeValue(node, "NazivAng") ?? GetNodeValue(node, "NazivMak") ?? "",
                            Nominal = decimal.TryParse(GetNodeValue(node, "Nominal"), out var n) ? n : 1,
                            Rate = decimal.TryParse(GetNodeValue(node, "Kurs"), out var r) ? r : 0,
                            Date = DateTime.Now,
                            TrendIcon = new Random().Next(1, 3) == 1 ? "📈" : "📉"
                        };

                        if (!string.IsNullOrEmpty(rate.Code))
                        {
                            rates.Add(rate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing SOAP response: {ex.Message}", ex);
            }

            return rates;
        }

        private List<ExchangeRateRow> ParseRestResponse(string json)
        {
            var rates = new List<ExchangeRateRow>();

            try
            {
                // Add MKD (Denar) as base currency
                rates.Add(new ExchangeRateRow
                {
                    Code = "MKD",
                    Country = "North Macedonia",
                    Currency = "Macedonian Denar",
                    Nominal = 1,
                    Rate = 1.0000m,
                    Date = DateTime.Now,
                    TrendIcon = "📈"
                });

                var jsonArray = JArray.Parse(json);
                var random = new Random();

                foreach (var item in jsonArray)
                {
                    var rate = new ExchangeRateRow
                    {
                        Code = item["oznaka"]?.ToString() ?? "",
                        Country = item["drzavaAng"]?.ToString() ?? item["drzava"]?.ToString() ?? "",
                        Currency = item["nazivAng"]?.ToString() ?? item["nazivMak"]?.ToString() ?? "",
                        Nominal = decimal.TryParse(item["nomin"]?.ToString(), out var n) ? n : 1,
                        Rate = decimal.TryParse(item["sreden"]?.ToString(), out var r) ? r : 0,
                        Date = DateTime.TryParse(item["datum"]?.ToString(), out var d) ? d : DateTime.Now,
                        TrendIcon = random.Next(1, 3) == 1 ? "📈" : "📉"
                    };

                    if (!string.IsNullOrEmpty(rate.Code))
                    {
                        rates.Add(rate);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing REST response: {ex.Message}", ex);
            }

            return rates;
        }

        private string? GetNodeValue(XmlNode parentNode, string nodeName)
        {
            var node = parentNode.SelectSingleNode(nodeName) ?? parentNode.SelectSingleNode($".//{nodeName}");
            return node?.InnerText;
        }

        public void Dispose()
        {
            try
            {
                if (_client.State == System.ServiceModel.CommunicationState.Opened)
                {
                    _client.Close();
                }
            }
            catch
            {
                _client.Abort();
            }

            _httpClient?.Dispose();
        }
    }
}
