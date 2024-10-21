namespace Loginproject.DB
{
    public class SalesSummaryResponse
    {
        public string SummaryType { get; set; }
        public string Paymode { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
    }
}
