using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tblsaledetail
    {
        [Key]
        [ScaffoldColumn(false)]
        [Exclude]
        public Int64 Id {  get; set; }
        public string sno { get; set; }
        public DateTime invdate{ get; set; }
        public decimal netamount { get; set; }
        public Int64 cust_id { get; set;}
        public int Salesman { get; set; }
        public int paymode { get;set; }
        public Int64 shiftid {  get; set; }
        public Int64 saleretunid { get; set; }
        public int counter {  get; set; }
        public string custname { get; set; }
        public Int64 dayid { get; set; }
        public int Location {  get; set; }
        public int userid {  get; set; }
        public string cancel_flg {  get; set; }
        public Int64 Transactionid { get; set; }
        public int Invtype { get; set; }
    }
}
