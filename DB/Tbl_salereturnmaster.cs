using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tbl_salereturnmaster
    {
        [Key]
        public Int64 transid {  get; set; }
        public Int64 item_id { get; set; }
        public float qty { get; set; }    
        public float price { get; set; }
        public float amount { get; set; }
        public DateTime invoice_date { get; set; }
        public float netamount { get; set; }
        public string Barcode { get; set; }
    }
}
