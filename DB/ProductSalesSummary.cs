public class ProductSalesSummary
{
    public string ProductName { get; set; }
    public string Category { get; set; }
    public string Barcode { get; set; }
    public float NetProductQuantity { get; set; }  // Changed from double to decimal
    public float NetProductNetAmount { get; set; }  // Changed from double to decimal
}
