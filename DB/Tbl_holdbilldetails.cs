using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tbl_holdbilldetails
    {
        [Key]
        public Int64 Transid {  get; set; }
        public int customer {  get; set; }
        public DateOnly date {  get; set; }
        public double amount { get; set; }
        public double netamount { get; set; }
        public int Location { get; set; }
        public int emp { get; set; }
        public Int64 Dayid { get; set; }
        public Int64 Shiftid { get; set;}
        public Int64 Locatiionid { get; set; }
        public Int64   Transactionid { get; set; }
        public string cancelsts { get; set; }
        public string HoldType { get; set; }
    }
}
