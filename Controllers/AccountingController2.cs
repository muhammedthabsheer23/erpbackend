using Loginproject.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Loginproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController2 : ControllerBase
    {
        private readonly Dbconnection _connection;

        public AccountingController2(Dbconnection connection)
        {
            _connection = connection;

        }
        private static string GetInvtypeName(int InvTypeId)
        {
            return InvTypeId switch
            {
                1 => "Other Sale",
                3 => "Main Sale",
                6 => "Alteration",
                7 => "Quotation",
            };
        }
        private static string GetInvTypereturnName(int invtypeId)
        {
            return invtypeId switch
            {
                1 => "Other Sale",
                3 => "Main Sale",
                6 => "Alteration",
                7 => "Quotation",
            };
        }

            private static string GetPaymodeName(int paymodeId)
        {
            return paymodeId switch
            {
                1 => "CASH",
                2 => "CREDIT",
                3 => "MULTIPAYMODE",
                4 => "BENEFIT ",
                5 => "CARD",
                _ => "UNKNOWN"
            };
        }
     

        private static string GetPaymodeName(string narration)
        {
            var paymodeNames = new Dictionary<string, string>
            {
                { "Cash", "CASH" },
                { "Card", "CARD" },
                { "Benefit ", "BENEFIT " },
                { "Credit ", "CREDIT" }

            };

            return paymodeNames.TryGetValue(narration, out var name) ? name : narration;
        }

        private static string GetPaymodereturnName(int paymodeId)
        {
            return paymodeId switch
            {
                1 => "CASH",
                2 => "CREDIT",
                3 => "MULTIPAYMODE",
                4 => "BENEFIT",
                5 => "CARD",
                _ => "unknown"
            };
        }
     
        [HttpGet]
        [Route("combined-summary")]

        public async Task<ActionResult<CombinedSummary>> GetCombinedSummary(string dayid, string shiftid, string userid, string counter, string location, DateTime invdate, int Transactionid, int invtype)
        {
            if (!long.TryParse(dayid, out long dayidLong) ||
                !long.TryParse(shiftid, out long shiftidLong) ||
                !long.TryParse(userid, out long useridLong) ||
                 !long.TryParse(userid, out long invtypeLong) ||
                !long.TryParse(counter, out long counterLong) ||
                !long.TryParse(location, out long locationLong))
            {
                return BadRequest("Invalid query parameters");
            }

            // Define a sales category based on invtype
            string salesCategory = invtype switch
            {
                1 => "Other Sale",
                3 => "Main Sale",
                6 => "Alteration",
                7 => "Quotation",
          

                _ => "Unknown Sale" // Optional: handle unknown invtype values
            };
            var salesDetails = await _connection.Tbl_salesdetails
      .Where(sd => sd.dayid == dayidLong
                   && sd.shiftid == shiftidLong
                   && sd.userid == useridLong
                   && sd.counter == counterLong
                   && sd.Location == locationLong
                   && sd.cancel_flg == ""
                   && sd.Invtype == invtype // filter based on invtype
                   && sd.paymode != 3)
      .GroupBy(sd => new { sd.paymode, sd.Invtype }) // Group by paymode and invtype
      .Select(g => new
      {
          PaymodeId = g.Key.paymode,
          Amount = (decimal)g.Sum(sd => (double)sd.netamount),
          Count = g.Count(),
          InvTypeId = g.Key.Invtype
      })
      .ToListAsync();
            var transactionDetails = await _connection.Tbl_transactions
      .Where(t => t.date.Date == invdate.Date
                  && t.Trans_id == Transactionid
                  && t.narration != null && t.narration != "")
      .GroupBy(t => t.narration)
      .Select(g => new
      {
          Paymode = GetPaymodeName(g.Key),  // Keep this if g.Key is a string
          Amount = (decimal)g.Sum(t => (double)t.amount_rs),
          Count = g.Count(),
          salesCategory = "Some Default Category" // Adjust this logic if you need category info
      })
      .ToListAsync();


            // Combine and group sales details and transaction details
            var combinedSalesSummary = salesDetails.Select(sd => new Salessummary
            {
                Paymode = GetPaymodeName(sd.PaymodeId),
                Amount = sd.Amount,
                Count = sd.Count,
                SalesCategory = GetInvtypeName(sd.InvTypeId), // Set SalesCategory to invtype name
                InvTypeName = GetInvtypeName(sd.InvTypeId)    // Add InvTypeName
            })
           .Union(transactionDetails.Select(td => new Salessummary
           {
               Paymode = td.Paymode,
               Amount = td.Amount,
               Count = td.Count,
               SalesCategory = td.salesCategory
           }))
            .GroupBy(s => new { s.Paymode, s.SalesCategory, s.InvTypeName }) // Group by paymode, sales category, and invtype
            .Select(g => new Salessummary
            {
                Paymode = g.Key.Paymode,
                Amount = g.Sum(x => x.Amount),
                Count = g.Sum(x => x.Count),
                SalesCategory = g.Key.SalesCategory,
                InvTypeName = g.Key.InvTypeName // Include InvTypeName in final output
            })
            .ToList();

            // Filter and categorize sales details based on invtype
            var salesReturnDetails = await _connection.Tbl_salesreturndetails
                .Where(sr => sr.dayid == dayidLong
                             && sr.shiftid == shiftidLong
                             && sr.userid == useridLong
                             && sr.counter == counterLong
                             && sr.location == locationLong
                             && sr.cancel_sts == "")
                .GroupBy(sr => sr.paymode)
                .Select(gs => new
                {
                    Paymode = GetPaymodereturnName(gs.Key),
                    Amount = (decimal)gs.Sum(sr => (double)sr.netamount),
                    Count = gs.Count()
                })
                .ToListAsync();

            var salesReturnSummary = salesReturnDetails.Select(sr => new Salesreturnsummary
            {
                Paymode = sr.Paymode,
                Amount = sr.Amount,
                Count = sr.Count,
            }).ToList();

            // Fetch employee summary
            var employeeSummary = await _connection.Tbl_salesdetails
                .Join(_connection.Tbl_employee, s => s.Salesman, e => e.Id, (s, e) => new { s, e })
                .Where(se => se.s.dayid == dayidLong
                             && se.s.shiftid == shiftidLong
                             && se.s.userid == useridLong
                             && se.e.Status == "Active")
                .GroupBy(se => se.e.Name)
                .Select(g => new Employee_summary
                {
                    EmployeeName = g.Key,
                    TotalTransactions = g.Count(),
                    TotalNetAmount = g.Sum(se => se.s.netamount)
                })
                .ToListAsync();
            var productsummary = await _connection.Tbl_salemaster
                .Join(_connection.tbl_item_master,
                      sm => sm.item_id,
                      im => im.Id,
                      (sm, im) => new { sm, im })
                .Join(_connection.Tbl_salesdetails,
                      sm_im => sm_im.sm.transid,
                      sd => sd.Transactionid,
                      (sm_im, sd) => new { sm_im.sm, sm_im.im, sd })
                .Where(sm_im_sd_ic => sm_im_sd_ic.sd.dayid == dayidLong
                                     && sm_im_sd_ic.sd.shiftid == shiftidLong
                                     && sm_im_sd_ic.sd.userid == useridLong
                                     && sm_im_sd_ic.sd.counter == counterLong
                                     && sm_im_sd_ic.sd.Location == locationLong)
                .GroupBy(sm_im_sd_ic => sm_im_sd_ic.im.Name)
                .OrderBy(g => g.Key)
                .Select(g => new ProductSummary
                {
                    ProductName = g.Key,
                    ProductQuantity = g.Sum(x => (int)x.sm.qty),
                    ProductNetAmount = g.Sum(x => Convert.ToDecimal(x.sm.amount))
                })
                .ToListAsync();



            var itemCategorySummary = await _connection.Tbl_salemaster
                .Join(_connection.Tbl_salesdetails,
                      sm => sm.Transactionid,
                      sd => sd.Transactionid,
                      (sm, sd) => new { sm, sd })
                .Join(_connection.tbl_item_master,
                      sm_sd => sm_sd.sm.item_id,
                      im => im.Id,
                      (sm_sd, im) => new { sm_sd.sm, sm_sd.sd, im })
                .Join(_connection.item_category,
                      sm_sd_im => sm_sd_im.im.category,
                      ic => ic.Id,
                      (sm_sd_im, ic) => new { sm_sd_im.sm, sm_sd_im.sd, ic })
                .Where(sm_sd_im_ic => sm_sd_im_ic.sd.cancel_flg == ""
                                     && sm_sd_im_ic.sd.dayid == dayidLong
                                     && sm_sd_im_ic.sd.shiftid == shiftidLong
                                     && sm_sd_im_ic.sd.userid == useridLong
                                     && sm_sd_im_ic.sd.counter == counterLong
                                     && sm_sd_im_ic.sd.Location == locationLong)
                .GroupBy(sm_sd_im_ic => sm_sd_im_ic.ic.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalQuantity = (int)g.Sum(x => (double)x.sm.qty),
                    TotalNetAmount = (decimal)g.Sum(x => (double)x.sm.netamount)
                })
                .ToListAsync();
            var combinedSummary = new CombinedSummary
            {
                ProductSummary = productsummary,
                SalesSummary = combinedSalesSummary,
                SalesReturnSummary = salesReturnSummary,
                ItemCategorySummary = itemCategorySummary.Select(ic => new ItemCategorySummary
                {
                    CategoryName = ic.CategoryName,
                    TotalQuantity = ic.TotalQuantity,
                    TotalNetAmount = ic.TotalNetAmount
                }).ToList(),
                EmployeeSummary = employeeSummary
            };

            return Ok(combinedSummary);
        }


      


        [HttpGet]
        [Route("api/GetPurchaseSummary")]
        public IActionResult GetPurchaseSummary(DateTime startDate, DateTime endDate)
        {
            var purchases = (from s in _connection.Tbl_puchasedetails
                             where s.invdate >= startDate && s.invdate <= endDate
                             group s by s.invdate.Date into g
                             select new
                             {
                                 InvDate = g.Key,
                                 TotalSales = g.Count(),
                                 TotalNetAmount = g.Sum(x => x.netamount),
                                 CashSales = g.Sum(x => x.paymode == 1 ? x.netamount : 0),
                                 VisaCardSales = g.Sum(x => x.paymode == 4 ? x.netamount : 0),
                                 BenefitSale = g.Sum(x => x.paymode == 3 ? x.netamount : 0),
                                 CreditSale = g.Sum(x => x.paymode == 2 ? x.netamount : 0)
                             }).ToList();

            var purchaseReturns = (from sr in _connection.Tbl_puchasereturndetails
                                   where sr.invdate >= startDate && sr.invdate <= endDate
                                   group sr by sr.invdate.Date into g
                                   select new
                                   {
                                       InvDate = g.Key,
                                       TotalReturnAmount = g.Sum(x => x.netamount),
                                       CashPurchaseReturn = g.Sum(x => x.paymode == 1 ? x.netamount : 0),
                                       VisaCardPurchaseReturn = g.Sum(x => x.paymode == 4 ? x.netamount : 0),
                                       BenefitPurchaseReturn = g.Sum(x => x.paymode == 3 ? x.netamount : 0),
                                       CreditPurchaseReturn = g.Sum(x => x.paymode == 2 ? x.netamount : 0)
                                   }).ToList();

            var result = from p in purchases
                         join pr in purchaseReturns on p.InvDate equals pr.InvDate into prGroup
                         from pr in prGroup.DefaultIfEmpty()
                         select new PurchaseSummary
                         {
                             InvDate = p.InvDate,
                             TotalSales = p.TotalSales,
                             TotalNetAmount = p.TotalNetAmount - (pr?.TotalReturnAmount ?? 0),
                             CashPurchase = p.CashSales - (pr?.CashPurchaseReturn ?? 0),
                             VisaCardPurchase = p.VisaCardSales - (pr?.VisaCardPurchaseReturn ?? 0),
                             BenefitPayPurchase = p.BenefitSale - (pr?.BenefitPurchaseReturn ?? 0),
                             CreditPurchase = p.CreditSale - (pr?.CreditPurchaseReturn ?? 0)
                         };

            return Ok(result.OrderBy(r => r.InvDate));
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await GetSummaryAsync();
            return Ok(summary);
        }
        private async Task<List<SummaryDto>> GetSummaryAsync()
        {
            var today = DateTime.Today;
            var year = today.Year;
            var month = today.Month;

            // Adjust the queries to handle possible null results
            var purchaseReturnQuery = from p in _connection.Tbl_puchasereturndetails
                                      group p by 1 into g
                                      select new SummaryDto
                                      {
                                          Source = "PurchaseReturn",
                                          NetAmountToday = g.Where(p => p.invdate.Date == today).Sum(p => (decimal?)p.netamount) ?? 0,
                                          NetAmountThisMonth = g.Where(p => p.invdate.Year == year && p.invdate.Month == month).Sum(p => (decimal?)p.netamount) ?? 0,
                                          NetAmountThisYear = g.Where(p => p.invdate.Year == year).Sum(p => (decimal?)p.netamount) ?? 0
                                      };

            var purchaseQuery = from p in _connection.Tbl_puchasedetails
                                group p by 1 into g
                                select new SummaryDto
                                {
                                    Source = "Purchase",
                                    NetAmountToday = g.Where(p => p.invdate.Date == today).Sum(p => (decimal?)p.netamount) ?? 0,
                                    NetAmountThisMonth = g.Where(p => p.invdate.Year == year && p.invdate.Month == month).Sum(p => (decimal?)p.netamount) ?? 0,
                                    NetAmountThisYear = g.Where(p => p.invdate.Year == year).Sum(p => (decimal?)p.netamount) ?? 0
                                };

            var salesQuery = from s in _connection.Tbl_salesdetails
                             group s by 1 into g
                             select new SummaryDto
                             {
                                 Source = "Sales",
                                 NetAmountToday = g.Where(s => s.invdate.Date == today).Sum(s => (decimal?)s.netamount) ?? 0,
                                 NetAmountThisMonth = g.Where(s => s.invdate.Year == year && s.invdate.Month == month).Sum(s => (decimal?)s.netamount) ?? 0,
                                 NetAmountThisYear = g.Where(s => s.invdate.Year == year).Sum(s => (decimal?)s.netamount) ?? 0
                             };

            var salesReturnQuery = from sr in _connection.Tbl_salesreturndetails
                                   group sr by 1 into g
                                   select new SummaryDto
                                   {
                                       Source = "SalesReturn",
                                       NetAmountToday = g.Where(sr => sr.invdate.Date == today).Sum(sr => (decimal?)sr.netamount) ?? 0,
                                       NetAmountThisMonth = g.Where(sr => sr.invdate.Year == year && sr.invdate.Month == month).Sum(sr => (decimal?)sr.netamount) ?? 0,
                                       NetAmountThisYear = g.Where(sr => sr.invdate.Year == year).Sum(sr => (decimal?)sr.netamount) ?? 0
                                   };

            var purchaseReturnSummary = await purchaseReturnQuery.FirstOrDefaultAsync() ?? new SummaryDto { Source = "PurchaseReturn", NetAmountToday = 0, NetAmountThisMonth = 0, NetAmountThisYear = 0 };
            var purchaseSummary = await purchaseQuery.FirstOrDefaultAsync() ?? new SummaryDto { Source = "Purchase", NetAmountToday = 0, NetAmountThisMonth = 0, NetAmountThisYear = 0 };
            var salesSummary = await salesQuery.FirstOrDefaultAsync() ?? new SummaryDto { Source = "Sales", NetAmountToday = 0, NetAmountThisMonth = 0, NetAmountThisYear = 0 };
            var salesReturnSummary = await salesReturnQuery.FirstOrDefaultAsync() ?? new SummaryDto { Source = "SalesReturn", NetAmountToday = 0, NetAmountThisMonth = 0, NetAmountThisYear = 0 };

            var result = new List<SummaryDto>
    {
        purchaseReturnSummary,
        purchaseSummary,
        salesSummary,
        salesReturnSummary
    };

            return result;
        }


        [HttpGet]
        [Route("productsales")]

      
        public async Task<IActionResult> GetProductSalesSummary(DateTime startDate, DateTime endDate)
        {
            var salesDataQuery = from sm in _connection.Tbl_salemaster
                                 join im in _connection.tbl_item_master on sm.item_id equals im.Id
                                 join ic in _connection.item_category on im.category equals ic.Id
                                 where sm.cancelsts == "" && sm.invoice_date.Date >= startDate.Date && sm.invoice_date.Date <= endDate.Date
                                 group new { sm, im, ic } by new
                                 {
                                     im.Name,
                                     Category = ic.Name,
                                     im.Barcode
                                 } into g
                                 select new
                                 {
                                     ProductName = g.Key.Name,
                                     Category = g.Key.Category,
                                     Barcode = g.Key.Barcode,
                                     ProductQuantity = g.Sum(x => x.sm.qty),
                                     ProductNetAmount = g.Sum(x => x.sm.netamount)
                                 };

            var salesReturnDataQuery = from sm in _connection.Tbl_salesreturnmaster
                                       join im in _connection.tbl_item_master on sm.item_id equals im.Id
                                       join ic in _connection.item_category on im.category equals ic.Id
                                       where sm.invoice_date.Date >= startDate.Date && sm.invoice_date.Date <= endDate.Date
                                       group new { sm, im, ic } by new
                                       {
                                           im.Name,
                                           Category = ic.Name,
                                           im.Barcode
                                       } into g
                                       select new
                                       {
                                           ProductName = g.Key.Name,
                                           Category = g.Key.Category,
                                           Barcode = g.Key.Barcode,
                                           ProductQuantity = g.Sum(x => x.sm.qty),
                                           ProductNetAmount = g.Sum(x => x.sm.netamount)
                                       };

            var salesData = await salesDataQuery.ToListAsync();
            var salesReturnData = await salesReturnDataQuery.ToListAsync();

            var result = from s in salesData
                         join sr in salesReturnData
                         on new { s.ProductName, s.Category, s.Barcode } equals new { sr.ProductName, sr.Category, sr.Barcode } into joined
                         from sr in joined.DefaultIfEmpty()
                         select new ProductSalesSummary
                         {
                             ProductName = s.ProductName,
                             Category = s.Category,
                             Barcode = s.Barcode,
                             NetProductQuantity = s.ProductQuantity - (sr?.ProductQuantity ?? 0),
                             NetProductNetAmount = s.ProductNetAmount - (sr?.ProductNetAmount ?? 0)
                         };
            var sortedResult = result.OrderBy(r => r.ProductName).ToList();

            return Ok(sortedResult);
        }

     
    }
}








