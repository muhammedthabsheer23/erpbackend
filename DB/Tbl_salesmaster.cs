using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loginproject.DB
{
    public class Tbl_salesmaster
    {
        [Key]
        public Int64 transid { get; set; }
        public int voucher_seriesid { get; set; }
        public string vch_no { get; set; }
        public Int64 Party_id { get; set; }
        public Int64 item_id { get; set; }
        public float qty { get; set; }
        public int unit { get; set; }
        public float price { get; set; }
        public float amount { get; set; }
        public DateTime invoice_date { get; set; }
        public DateOnly dates { get; set; }
        public string Location { get; set; }
        public string narration { get; set; }
        public string cancelsts { get; set; }
        public float netamount { get; set; }
        public string sno { get; set; }
        public Int64 Transactionid { get; set; }
    }
}
