namespace Loginproject.DB
{
    public class Purchase
    {
        public Int64 Id { get; set; }
        public string pno { get; set; }
        public DateTime invdate { get; set; }
        public int paymode { get; set; }
        public double netamount { get; set; }
    }
}
