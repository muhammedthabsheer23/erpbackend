using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class PAYMODTS
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Post_ledger { get; set; }
        public Int64 Post_Ledger_id { get; set; }
        public Int64 Company_id { get; set; }
    }
}
