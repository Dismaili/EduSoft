namespace WebApplicationIntern2.Models
{
    public class ExchangeRateResponse
    {
        public List<ExchangeRateRow> ExchangeRates { get; set; } = new List<ExchangeRateRow>();
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
