using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Loginproject.DB
{
    public class Dbconnection:DbContext
    {
        public Dbconnection(DbContextOptions<Dbconnection>options):base(options) 
        {
            
        }

        //public DbSet<Tbl_user> Users { get; set; }
        public DbSet<Tbl_Username> Tbl_user {  get; set; }
        public  DbSet<PAYMODTS> tbl_paymod_master { get; set; }
        public DbSet<Tblsaledetail> Tbl_salesdetails { get; set; }
        public DbSet<Tbl_Employee>Tbl_employee{ get; set; }
        public DbSet<Tbl_Account>tblAccounts { get; set; }
        public DbSet<Dayshift>DAYSHIFT { get; set; }
        public DbSet<Countersettings>countersettings { get; set; }
        public DbSet<Salesreturn>Tbl_salesreturndetails { get; set; }
        public DbSet<Locations> Tbl_location {  get; set; }
        public DbSet<Tbltransaction>Tbl_transactions { get; set; }
        public DbSet<Tbl_itemmaster> tbl_item_master { get; set; }
        public DbSet<Tbl_salesmaster>Tbl_salemaster{ get; set; }
        public DbSet<Tbl_category>item_category { get; set; }
        public DbSet<Purchase> Tbl_puchasedetails { get; set; }
        public DbSet<PurchaseReturn> Tbl_puchasereturndetails { get; set; }
        public DbSet<Tbl_salereturnmaster> Tbl_salesreturnmaster { get; set; }
        public DbSet<Tbl_invoicetype>InvoiceType { get; set; }
        public DbSet<Tbl_cashinout> Cashinout { get; set; }
        public DbSet<Tbl_holdbilldetails>Holdbilldetails { get; set; }
        public DbSet<Tbl_recieptdetailss>Tbl_receiptdetails { get; set; }



    }
}
