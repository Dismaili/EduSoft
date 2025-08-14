namespace WebApplicationIntern2.Models
{
    public class ExchangeRateRow
    {
        public string Code { get; set; } = string.Empty;
        public decimal Nominal { get; set; }
        public decimal Rate { get; set; }
        public string Country { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string TrendIcon { get; set; } = "📈"; // Default up
    }
}
