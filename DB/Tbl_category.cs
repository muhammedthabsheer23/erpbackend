namespace Loginproject.DB
{
    public class Tbl_category
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int64 Company_id { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
