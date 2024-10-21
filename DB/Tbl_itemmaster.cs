namespace Loginproject.DB
{
    public class Tbl_itemmaster
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }    
        public string Code {  get; set; }
        public string Barcode {  get; set; }
        public int grp {  get; set; }
        public int company {  get; set; }
        public int category {  get; set; }
        public int sub_category { get; set; }
        public int unit { get; set; }
        public int product_type {  get; set; }
        public decimal margin_amnt { get; set; }
        public int purchase_vat {  get; set; }
        public int sales_vat { get; set; }
        public decimal salemrp { get; set; }
        public int vendorid { get; set; }
    }
}
