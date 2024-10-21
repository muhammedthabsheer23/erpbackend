namespace Loginproject.DB
{
    public class Salesreturn
    {
        public Int64 Id {  get; set; }
        public string srno { get; set; }
        public  DateTime invdate { get; set; }
        public int paymode {  get; set; }
        public Int64 cust_id {  get; set; }
        public decimal netamount {  get; set; }
        public string vchno { get;}
        public int voucher_seriesid { get; set; }
        public int salesman { get;set; }
        public Int64 dayid { get; set; }
        public int userid { get; set;}
        public Int64 shiftid {  get; set; }
        public Int64 saleretunid {  get; set; }
        public int counter {  get; set; }
        public int location { get; set; }
        public string cancel_sts { get; set; }
        public Int64 Transactionid { get; set; }
        public int Invtype { get; set; }

    }
}
