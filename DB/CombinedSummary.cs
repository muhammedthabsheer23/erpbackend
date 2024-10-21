
namespace Loginproject.DB
{
    public class CombinedSummary
    {
        public List<Salessummary> SalesSummary { get; set; }
        public List<Salesreturnsummary> SalesReturnSummary { get; set; }
        public  List<ItemCategorySummary>ItemCategorySummary { get; set; }
        public List<Employee_summary> EmployeeSummary { get; set; } 
        public List<ProductSummary> ProductSummary { get; set; }
        public List<otherSaleSummary> OtherSaleSummary { get; set; }
        public List<MainSummary> QuotationSummary { get; set; }
        public List<Salessummary> MainSaleSummary { get; set; }
        public List<Salessummary> AlterationSummary { get; set; }
    }
}
