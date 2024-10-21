namespace Loginproject.DB
{
    public class CombinedSalesSummary
    {
        public IEnumerable<Salessummary> SalesSummaries { get; set; }
        public IEnumerable<Salesreturnsummary> SalesReturnSummaries { get; set; }
    }
}
