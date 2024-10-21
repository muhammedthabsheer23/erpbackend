namespace Loginproject.DB
{
    public class Dayendreport
    {
        public Int64 DAYID { get; set; }
        public Int64 SHIFTID { get; set; }
        public Int64 Id { get; set; }
        public string countername{ get; set; }
        public int Branch {  get; set; }
        public  string starttime { get; set; }
        public  string endtime { get; set; }
        public int paymode { get; set; }
        public Decimal netamount { get; set; }
        public string Name { get; set; }
        public DateTime invdate { get; set; }
        public double salereturnnetamount { get; set; }
        public string salereturnpaymode {  get; set; }
        public int counterid { get; set; }
        public Int64 LocationId { get; set; }
        public String Location { get; set; }
        public Int64 transactionid { get; set; }
        public int invtypeid { get; set; }
    }
}
