namespace Loginproject.DB
{
    public class Monthend_report
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int counterid { get; set; }
        public string countername { get; set; }
        public Int64 LocationId { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public Int64 transactionId { get; set; }
        public string Location { get; set; }
        public decimal TotalNetAmount { get; set; }
    }
}
