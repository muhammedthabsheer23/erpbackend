 namespace Loginproject.DB
    {
        public class DayReport
        {
            public DateTime InvDate { get; set; }
            public int TotalSales { get; set; }
            public decimal NetTotalAmount { get; set; }
            public decimal CashSales { get; set; }
            public decimal VisaCardSales { get; set; }
            public decimal BenefitSale { get; set; }
            public decimal CreditSale { get; set; }
        }
    }

