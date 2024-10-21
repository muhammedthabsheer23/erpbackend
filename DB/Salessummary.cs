namespace Loginproject.DB
{
    public class Salessummary
    {
        public string Paymode { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public string SalesCategory { get; set; } // Existing property for sales category
        public string InvTypeName { get; set; }

    }
}
