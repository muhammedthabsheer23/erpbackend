namespace Loginproject.DB
{
    public class PurchaseSummary
    {
        public DateTime InvDate { get; set; }
        public int TotalSales { get; set; }
        public double TotalNetAmount { get; set; }
        public double NetTotalAmount { get; set; }
        public double CashPurchase { get; set; }
        public double VisaCardPurchase { get; set; }
        public double BenefitPayPurchase { get; set; }
        public double CreditPurchase { get; set; }
    }
}
