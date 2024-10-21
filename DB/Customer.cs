namespace Loginproject.DB
{
    public class Customer
    {
        public string sno { get; set; }
        public DateTime invdate { get; set; }
        public Decimal netamount { get; set; }
        public Int64 cust_id { get; set; }
        public int Salesman { get; set; }
        public Int64 PId { get; set; }
        public string custname { get; set; }
        public string paymode { get; set; }
    }
}
