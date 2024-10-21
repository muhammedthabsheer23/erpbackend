namespace Loginproject.DB
{
    public class Tbl_cashinout
    {
        public Int64 Id { get; set; }
        public string Type{ get; set; }
        public double Amount { get; set; }
        public int userid { get; set; }
        public int counter {  get; set; }
        public int dayid {  get; set; }
        public int  shiftid { get; set; }
        public string description { get; set; }
        public int Location {  get; set; }
        public DateTime Date {  get; set; }
        public Int64 Transid { get; set; }
        public string cancel_sts { get; set; }
    }
}
