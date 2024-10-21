using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tbl_recieptdetailss
    {
        [Key]
        public DateOnly Dates {  get; set; }
        public Int64 Debit_acnt { get; set; }
        public double Debitamnt { get; set; }
        public Int64 Credit_acnt { get; set; }
        public double Creditamnt { get; set;}
        public Int64 transactionid { get; set; }
        public int counter { get; set; }
        public int userid { get; set; }
        public int dayid { get; set; }
        public int shiftid { get; set; }
        public int Location {  get; set; }
        public Int64 OrderId { get; set; }
    }
}
