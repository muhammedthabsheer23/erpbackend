namespace Loginproject.DB
{
    public class Tbltransaction
    {
        public Int64 id { get; set; }
        public string vch_type { get; set; }
        public int vch_series { get; set; }
        public string narration { get; set; }
        public decimal amount_rs { get; set; }
        public DateTime date { get; set; }
        public Int64 Trans_id { get; set; }
        public string cancel_sts {get; set;}
    }
}
