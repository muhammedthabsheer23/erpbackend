namespace Loginproject.DB
{
    public class PurchaseReturn
    {
        public Int64 Id { get; set; }
        public string prno { get; set; }
        public DateTime invdate { get; set; }
        public int paymode { get; set; }
        public double netamount { get; set; }
    }
}