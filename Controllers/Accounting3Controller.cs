using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Loginproject.DB;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Metrics;
using System.Transactions;

namespace Loginproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Accounting3Controller : ControllerBase
    {

        private readonly Dbconnection _connection;

        public Accounting3Controller(Dbconnection connection)
        {
            _connection = connection;

        }
        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary(
            int dayid = 0,
            int shiftid = 0,
            int userid = 0,
            int counter = 0,
            int location = 0,
            int transactionid = 0)
        {
            var dayidLong = (long)dayid;
            var shiftidLong = (long)shiftid;
            var useridLong = (long)userid;
            var counterLong = (long)counter;
            var locationLong = (long)location;




            var othersaleData = from sd in _connection.Tbl_salesdetails
                                join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                                join it in _connection.InvoiceType on sd.Invtype equals it.Id
                                where (dayid == 0 || sd.dayid == dayid)
                                    && (shiftid == 0 || sd.shiftid == shiftid)
                                    && (userid == 0 || sd.userid == userid)
                                    && (counter == 0 || sd.counter == counter)
                                    && (location == 0 || sd.Location == location)
                                    && it.Id == 1
                                    && string.IsNullOrEmpty(sd.cancel_flg)

                                group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                                select new
                                {
                                    InvDate = g.Key.Date,
                                    InvoiceType = g.Key.Name,
                                    Paymode = g.Key.paymode,
                                    TotalSales = g.Count(),
                                    TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                    CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                    CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                    BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                    CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                    CashSalesCount = g.Count(x => x.pm.Id == 1),
                                    CardSalesCount = g.Count(x => x.pm.Id == 5),
                                    BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                    CreditSaleCount = g.Count(x => x.pm.Id == 2)
                                };

            var MainData = from sd in _connection.Tbl_salesdetails
                           join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                           join it in _connection.InvoiceType on sd.Invtype equals it.Id
                           where (dayid == 0 || sd.dayid == dayid)
                               && (shiftid == 0 || sd.shiftid == shiftid)
                               && (userid == 0 || sd.userid == userid)
                               && (counter == 0 || sd.counter == counter)
                               && (location == 0 || sd.Location == location)
                               && it.Id == 3
                               && string.IsNullOrEmpty(sd.cancel_flg)

                           group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                           select new
                           {
                               InvDate = g.Key.Date,
                               InvoiceType = g.Key.Name,
                               Paymode = g.Key.paymode,
                               TotalSales = g.Count(),
                               TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                               CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                               CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                               BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                               CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                               CashSalesCount = g.Count(x => x.pm.Id == 1),
                               CardSalesCount = g.Count(x => x.pm.Id == 5),
                               BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                               CreditSaleCount = g.Count(x => x.pm.Id == 2)
                           };
            var alterationData = from sd in _connection.Tbl_salesdetails
                                 join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                                 join it in _connection.InvoiceType on sd.Invtype equals it.Id
                                 where (dayid == 0 || sd.dayid == dayid)
                                     && (shiftid == 0 || sd.shiftid == shiftid)
                                     && (userid == 0 || sd.userid == userid)
                                     && (counter == 0 || sd.counter == counter)
                                     && (location == 0 || sd.Location == location)
                                     && it.Id == 6
                                     && string.IsNullOrEmpty(sd.cancel_flg)

                                 group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                                 select new
                                 {
                                     InvDate = g.Key.Date,
                                     InvoiceType = g.Key.Name,
                                     Paymode = g.Key.paymode,
                                     TotalSales = g.Count(),
                                     TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                     CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                     CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                     BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                     CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                     CashSalesCount = g.Count(x => x.pm.Id == 1),
                                     CardSalesCount = g.Count(x => x.pm.Id == 5),
                                     BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                     CreditSaleCount = g.Count(x => x.pm.Id == 2)
                                 };
            var QutationData = from sd in _connection.Tbl_salesdetails
                               join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                               join it in _connection.InvoiceType on sd.Invtype equals it.Id
                               where (dayid == 0 || sd.dayid == dayid)
                                   && (shiftid == 0 || sd.shiftid == shiftid)
                                   && (userid == 0 || sd.userid == userid)
                                   && (counter == 0 || sd.counter == counter)
                                   && (location == 0 || sd.Location == location)
                                   && it.Id == 7
                                   && string.IsNullOrEmpty(sd.cancel_flg)

                               group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                               select new
                               {
                                   InvDate = g.Key.Date,
                                   InvoiceType = g.Key.Name,
                                   Paymode = g.Key.paymode,
                                   TotalSales = g.Count(),
                                   TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                   CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                   CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                   BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                   CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                   CashSalesCount = g.Count(x => x.pm.Id == 1),
                                   CardSalesCount = g.Count(x => x.pm.Id == 5),
                                   BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                   CreditSaleCount = g.Count(x => x.pm.Id == 2)
                               };


            // Transaction Data where paymode is 3
            var transactionData = from t in _connection.Tbl_transactions
                                  join sd in _connection.Tbl_salesdetails on t.Trans_id equals sd.Transactionid
                                  where sd.paymode == 3
                                      && (transactionid == 0 || t.Trans_id == transactionid)
                                      && !string.IsNullOrEmpty(t.narration)
                                      && new[] { "BENEFIT", "CASH", "CARD", "CREDIT" }.Contains(t.narration)
                                  group t by t.date.Date into g
                                  select new
                                  {
                                      Date = g.Key,
                                      TotalNetAmount = g.Sum(x => (decimal)x.amount_rs),
                                      AdditionalCashSales = g.Where(x => x.narration == "CASH").Sum(x => (decimal)x.amount_rs),
                                      AdditionalCardSales = g.Where(x => x.narration == "CARD").Sum(x => (decimal)x.amount_rs),
                                      AdditionalBenefitSale = g.Where(x => x.narration == "BENEFIT").Sum(x => (decimal)x.amount_rs),
                                      AdditionalCreditSale = g.Where(x => x.narration == "CREDIT").Sum(x => (decimal)x.amount_rs),
                                      CashtransCount = g.Count(x => x.narration == "CASH"),
                                      CardtransCount = g.Count(x => x.narration == "CARD"),
                                      BenefittransCount = g.Count(x => x.narration == "BENEFIT"),
                                      CredittransCount = g.Count(x => x.narration == "CREDIT")
                                  };

            // Sales Return Data
            var salesReturnData = from sr in _connection.Tbl_salesreturndetails
                                  join pm in _connection.tbl_paymod_master on sr.paymode equals pm.Id
                                  where (dayid == 0 || sr.dayid == dayid)
                                      && (shiftid == 0 || sr.shiftid == shiftid)
                                      && (userid == 0 || sr.userid == userid)
                                      && (counter == 0 || sr.counter == counter)
                                      && (location == 0 || sr.location == location)
                                      && string.IsNullOrEmpty(sr.cancel_sts)
                                      && sr.paymode != 3

                                  group sr by sr.invdate.Date into g
                                  select new
                                  {
                                      InvDate = g.Key,
                                      TotalReturnAmount = g.Sum(x => (decimal?)x.netamount) ?? 0,
                                      CashSalesReturn = g.Where(x => x.paymode == 1).Sum(x => (decimal?)x.netamount) ?? 0,
                                      CardSalesReturn = g.Where(x => x.paymode == 5).Sum(x => (decimal?)x.netamount) ?? 0,
                                      BenefitSalesReturn = g.Where(x => x.paymode == 4).Sum(x => (decimal?)x.netamount) ?? 0,
                                      CreditSalesReturn = g.Where(x => x.paymode == 2).Sum(x => (decimal?)x.netamount) ?? 0
                                  };
            var result = from sd in othersaleData.ToList()
                         join td in transactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                         from td in tdGroup.DefaultIfEmpty()
                         join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                         from srd in srdGroup.DefaultIfEmpty()
                         select new
                         {
                             sd.InvDate,
                             sd.InvoiceType,
                             sd.TotalSales,
                             NetTotalAmount = sd.Paymode == 3
                     ? (sd.TotalNetAmount + (td?.TotalNetAmount ?? 0)) - (srd?.TotalReturnAmount ?? 0)
                     : sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                             CashSales = sd.Paymode == 3
                ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                             CardSales = sd.Paymode == 3
                ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                             BenefitSale = sd.Paymode == 3
                  ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                  : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                             CreditSale = sd.Paymode == 3
                 ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                 : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                             CreditCount = sd.Paymode == 3
                  ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                  : sd.CreditSaleCount,
                             BenefitCount = sd.Paymode == 3
                  ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                  : sd.BenefitSaleCount,
                             CashCount = sd.Paymode == 3
                ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                : sd.CashSalesCount,
                             CardCount = sd.Paymode == 3
                ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                : sd.CardSalesCount,
                         };
            // Group combined results by InvoiceType
            var groupedResult = from r in result
                                group r by r.InvoiceType into g
                                select new
                                {
                                    InvoiceType = g.Key,
                                    TotalSales = g.Sum(x => x.TotalSales),
                                    NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                    CashSales = g.Sum(x => x.CashSales),
                                    CardSales = g.Sum(x => x.CardSales),
                                    BenefitSale = g.Sum(x => x.BenefitSale),
                                    CreditSale = g.Sum(x => x.CreditSale),
                                    CreditCount = g.Sum(x => x.CreditCount),
                                    BenefitCount = g.Sum(x => x.BenefitCount),
                                    CashCount = g.Sum(x => x.CashCount),
                                    CardCount = g.Sum(x => x.CardCount)
                                };

            var Mainresult = from sd in MainData.ToList()
                             join td in transactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                             from td in tdGroup.DefaultIfEmpty()
                             join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                             from srd in srdGroup.DefaultIfEmpty()
                             select new
                             {
                                 sd.InvDate,
                                 sd.InvoiceType,
                                 sd.TotalSales,
                                 NetTotalAmount = sd.Paymode == 3
                         ? (sd.TotalNetAmount + (td?.TotalNetAmount ?? 0)) - (srd?.TotalReturnAmount ?? 0)
                         : sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                 CashSales = sd.Paymode == 3
                    ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                    : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                 CardSales = sd.Paymode == 3
                    ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                    : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                 BenefitSale = sd.Paymode == 3
                      ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                      : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                 CreditSale = sd.Paymode == 3
                     ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                     : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                 CreditCount = sd.Paymode == 3
                      ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                      : sd.CreditSaleCount,
                                 BenefitCount = sd.Paymode == 3
                      ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                      : sd.BenefitSaleCount,
                                 CashCount = sd.Paymode == 3
                    ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                    : sd.CashSalesCount,
                                 CardCount = sd.Paymode == 3
                    ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                    : sd.CardSalesCount,
                             };
            // Group combined results by InvoiceType
            var MaingroupedResult = from r in Mainresult
                                    group r by r.InvoiceType into g
                                    select new
                                    {
                                        InvoiceType = g.Key,
                                        TotalSales = g.Sum(x => x.TotalSales),
                                        NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                        CashSales = g.Sum(x => x.CashSales),
                                        CardSales = g.Sum(x => x.CardSales),
                                        BenefitSale = g.Sum(x => x.BenefitSale),
                                        CreditSale = g.Sum(x => x.CreditSale),
                                        CreditCount = g.Sum(x => x.CreditCount),
                                        BenefitCount = g.Sum(x => x.BenefitCount),
                                        CashCount = g.Sum(x => x.CashCount),
                                        CardCount = g.Sum(x => x.CardCount)
                                    };
            var Alterationresult = from sd in alterationData.ToList()
                                   join td in transactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                                   from td in tdGroup.DefaultIfEmpty()
                                   join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                                   from srd in srdGroup.DefaultIfEmpty()
                                   select new
                                   {
                                       sd.InvDate,
                                       sd.InvoiceType,
                                       sd.TotalSales,
                                       NetTotalAmount = sd.Paymode == 3
                               ? (sd.TotalNetAmount + (td?.TotalNetAmount ?? 0)) - (srd?.TotalReturnAmount ?? 0)
                               : sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                       CashSales = sd.Paymode == 3
                          ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                          : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                       CardSales = sd.Paymode == 3
                          ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                          : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                       BenefitSale = sd.Paymode == 3
                            ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                            : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                       CreditSale = sd.Paymode == 3
                           ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                           : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                       CreditCount = sd.Paymode == 3
                            ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                            : sd.CreditSaleCount,
                                       BenefitCount = sd.Paymode == 3
                            ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                            : sd.BenefitSaleCount,
                                       CashCount = sd.Paymode == 3
                          ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                          : sd.CashSalesCount,
                                       CardCount = sd.Paymode == 3
                          ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                          : sd.CardSalesCount,
                                   };
            // Group combined results by InvoiceType
            var AlterationgroupedResult = from r in Alterationresult
                                          group r by r.InvoiceType into g
                                          select new
                                          {
                                              InvoiceType = g.Key,
                                              TotalSales = g.Sum(x => x.TotalSales),
                                              NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                              CashSales = g.Sum(x => x.CashSales),
                                              CardSales = g.Sum(x => x.CardSales),
                                              BenefitSale = g.Sum(x => x.BenefitSale),
                                              CreditSale = g.Sum(x => x.CreditSale),
                                              CreditCount = g.Sum(x => x.CreditCount),
                                              BenefitCount = g.Sum(x => x.BenefitCount),
                                              CashCount = g.Sum(x => x.CashCount),
                                              CardCount = g.Sum(x => x.CardCount)
                                          };
            var Quotationresult = from sd in QutationData.ToList()
                                  join td in transactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                                  from td in tdGroup.DefaultIfEmpty()
                                  join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                                  from srd in srdGroup.DefaultIfEmpty()
                                  select new
                                  {
                                      sd.InvDate,
                                      sd.InvoiceType,
                                      sd.TotalSales,
                                      NetTotalAmount = sd.Paymode == 3
                              ? (sd.TotalNetAmount + (td?.TotalNetAmount ?? 0)) - (srd?.TotalReturnAmount ?? 0)
                              : sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                      CashSales = sd.Paymode == 3
                         ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                         : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                      CardSales = sd.Paymode == 3
                         ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                         : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                      BenefitSale = sd.Paymode == 3
                           ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                           : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                      CreditSale = sd.Paymode == 3
                          ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                          : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                      CreditCount = sd.Paymode == 3
                           ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                           : sd.CreditSaleCount,
                                      BenefitCount = sd.Paymode == 3
                           ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                           : sd.BenefitSaleCount,
                                      CashCount = sd.Paymode == 3
                         ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                         : sd.CashSalesCount,
                                      CardCount = sd.Paymode == 3
                         ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                         : sd.CardSalesCount,
                                  };
            // Group combined results by InvoiceType
            var QuotationgroupedResult = from r in Quotationresult
                                         group r by r.InvoiceType into g
                                         select new
                                         {
                                             InvoiceType = g.Key,
                                             TotalSales = g.Sum(x => x.TotalSales),
                                             NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                             CashSales = g.Sum(x => x.CashSales),
                                             CardSales = g.Sum(x => x.CardSales),
                                             BenefitSale = g.Sum(x => x.BenefitSale),
                                             CreditSale = g.Sum(x => x.CreditSale),
                                             CreditCount = g.Sum(x => x.CreditCount),
                                             BenefitCount = g.Sum(x => x.BenefitCount),
                                             CashCount = g.Sum(x => x.CashCount),
                                             CardCount = g.Sum(x => x.CardCount)
                                         };


            // Additional summary data
            var employeeSummary = await _connection.Tbl_salesdetails
                .Join(_connection.Tbl_employee, s => s.Salesman, e => e.Id, (s, e) => new { s, e })
                .Where(se => (dayid == 0 || se.s.dayid == dayidLong)
                             && (shiftid == 0 || se.s.shiftid == shiftidLong)
                             && (userid == 0 || se.s.userid == useridLong)
                             && string.IsNullOrEmpty(se.s.cancel_flg)
                             && se.e.Status == "Active")
                .GroupBy(se => se.e.Name)
                .Select(g => new Employee_summary
                {
                    EmployeeName = g.Key,
                    TotalTransactions = g.Count(),
                    TotalNetAmount = g.Sum(se => se.s.netamount)
                })
                .ToListAsync();
            var cashInOutData = from c in _connection.Cashinout
                                where (dayid == 0 || c.dayid == dayid)
                                    && (shiftid == 0 || c.shiftid == shiftid)
                                    && (counter == 0 || c.counter == counter)
                                    && (userid == 0 || c.userid == userid)
                                    && c.cancel_sts == ""
                                group c by new { c.Type, c.description } into g
                                select new
                                {
                                    Type = g.Key.Type,
                                    Description = g.Key.description,
                                    TotalAmount = g.Sum(x => (decimal)x.Amount) // Explicitly cast to decimal
                                };

            var cashInOutResults = cashInOutData
                .GroupBy(x => x.Description) // Group only by Description
                .Select(g => new
                {
                    Description = g.Key,
                    CashInAmount = g.Where(x => x.Type == "CASH IN").Sum(x => x.TotalAmount),
                    CashOutAmount = g.Where(x => x.Type == "CASH OUT").Sum(x => x.TotalAmount),
                    TotalAmount = g.Where(x => x.Type == "CASH OUT").Sum(x => x.TotalAmount) - g.Where(x => x.Type == "CASH IN").Sum(x => x.TotalAmount)
                    // Subtract CASH OUT from CASH IN
                })
                .ToList();

            var balance = from d in _connection.DAYSHIFT
                          where (dayid == 0 || d.DAYID == dayid)
                                    && (shiftid == 0 || d.SHIFTID == shiftid)
                                    && (counter == 0 || d.counter == counter)
                                    && (userid == 0 || d.USERID == userid)
                          select new
                          {
                              openningbalance = d.OP_BALANCE,
                              closingbalance = d.CL_BALANCE
                          };

            var Alterationquery = from tr in _connection.Tbl_receiptdetails
                                  join hb in _connection.Holdbilldetails on tr.OrderId equals hb.Transactionid
                                  join tt in _connection.Tbl_transactions on tr.transactionid equals tt.Trans_id
                                  where (dayid == 0 || tr.dayid == dayidLong)
                                      && (shiftid == 0 || tr.shiftid == shiftidLong)
                                      && (location == 0 || tr.Location == locationLong)
                                      && (transactionid == 0 || tr.transactionid == transactionid)
                                      && (counter == 0 || tr.counter == counterLong)
                                      && tt.cancel_sts == ""
                                      && hb.HoldType == "Alteration"
                                      && tt.narration != null
                                      && tt.narration != ""
                                  select new
                                  {
                                      tr.Debitamnt,
                                      tt.narration
                                  };

            var alterationresultList = Alterationquery.ToList();

            var joborderquery = from tr in _connection.Tbl_receiptdetails
                                join hb in _connection.Holdbilldetails on tr.OrderId equals hb.Transactionid
                                join tt in _connection.Tbl_transactions on tr.transactionid equals tt.Trans_id
                                where (dayid == 0 || tr.dayid == dayidLong)
                                    && (shiftid == 0 || tr.shiftid == shiftidLong)
                                    && (location == 0 || tr.Location == locationLong)
                                    && (transactionid == 0 || tr.transactionid == transactionid)
                                    && (counter == 0 || tr.counter == counterLong)
                                    && tt.cancel_sts == ""
                                    && hb.HoldType == "JobOrder"
                                    && tt.narration != null
                                    && tt.narration != ""
                                select new
                                {
                                    tr.Debitamnt,
                                    tt.narration
                                };

            var joborderresultList = joborderquery.ToList();


            var productsummary = await _connection.Tbl_salemaster
                .Join(_connection.tbl_item_master, sm => sm.item_id, im => im.Id, (sm, im) => new { sm, im })
                .Join(_connection.Tbl_salesdetails, sm_im => sm_im.sm.transid, sd => sd.Transactionid, (sm_im, sd) => new { sm_im.sm, sm_im.im, sd })
                .Where(sm_im_sd_ic => (dayid == 0 || sm_im_sd_ic.sd.dayid == dayidLong)
                                     && (shiftid == 0 || sm_im_sd_ic.sd.shiftid == shiftidLong)
                                     && (userid == 0 || sm_im_sd_ic.sd.userid == useridLong)
                                     && (counter == 0 || sm_im_sd_ic.sd.counter == counterLong)
                                     && (location == 0 || sm_im_sd_ic.sd.Location == locationLong)
                                      && string.IsNullOrEmpty(sm_im_sd_ic.sd.cancel_flg))
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
                .Join(_connection.Tbl_salesdetails, sm => sm.Transactionid, sd => sd.Transactionid, (sm, sd) => new { sm, sd })
                .Join(_connection.tbl_item_master, sm_sd => sm_sd.sm.item_id, im => im.Id, (sm_sd, im) => new { sm_sd.sm, sm_sd.sd, im })
                .Join(_connection.item_category, sm_sd_im => sm_sd_im.im.category, ic => ic.Id, (sm_sd_im, ic) => new { sm_sd_im.sm, sm_sd_im.sd, ic })
                .Where(sm_sd_im_ic => string.IsNullOrEmpty(sm_sd_im_ic.sd.cancel_flg)
                                     && (dayid == 0 || sm_sd_im_ic.sd.dayid == dayidLong)
                                     && (shiftid == 0 || sm_sd_im_ic.sd.shiftid == shiftidLong)
                                     && (userid == 0 || sm_sd_im_ic.sd.userid == useridLong)
                                     && (counter == 0 || sm_sd_im_ic.sd.counter == counterLong)
                                     && (location == 0 || sm_sd_im_ic.sd.Location == locationLong)
                                      && string.IsNullOrEmpty(sm_sd_im_ic.sd.cancel_flg))
                .GroupBy(sm_sd_im_ic => sm_sd_im_ic.ic.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalQuantity = (int)g.Sum(x => x.sm.qty),
                    NetTotalAmount = g.Sum(x => (decimal)x.sm.amount)
                })

                .ToListAsync();

            var combinedSummary = new
            {
                obcb = balance.ToList(),
                Alteration = Alterationquery.ToList(),
                JobOrder = joborderresultList.ToList(),
                Cashinout = cashInOutResults.ToList(),
                SalesReturnSummary = salesReturnData.ToList(),
                OtherSalesSummary = groupedResult.ToList(),
                AlterationSummary = AlterationgroupedResult.ToList(),
                Quotationsummary = QuotationgroupedResult.ToList(),
                Mainsalesummary = MaingroupedResult.ToList(),
                EmployeeSummary = employeeSummary,
                ProductSummary = productsummary,
                ItemCategorySummary = itemCategorySummary
            };

            return Ok(combinedSummary);
        }
        [HttpGet("GetMonthSalesSummary")]
        public async Task<IActionResult> GetSalesSummary(int month, int year)
        {
            // Calculate the start date and end date based on the selected month and year
            var startDate = new DateTime(year, month, 1); // First day of the month
            var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month

            // LINQ Query to calculate Sales Data
            {
                var othersaleData = from sd in _connection.Tbl_salesdetails
                                    join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                                    join it in _connection.InvoiceType on sd.Invtype equals it.Id
                                    where
                                         sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                        && it.Id == 1
                                        && string.IsNullOrEmpty(sd.cancel_flg)

                                    group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                                    select new
                                    {
                                        InvDate = g.Key.Date,
                                        InvoiceType = g.Key.Name,
                                        Paymode = g.Key.paymode,
                                        TotalSales = g.Count(),
                                        TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                        CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                        CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                        BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                        CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                        CashSalesCount = g.Count(x => x.pm.Id == 1),
                                        CardSalesCount = g.Count(x => x.pm.Id == 5),
                                        BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                        CreditSaleCount = g.Count(x => x.pm.Id == 2)
                                    };



                // Transaction Data where paymode is 3
                var othertransactionData = from t in _connection.Tbl_transactions
                                           join sd in _connection.Tbl_salesdetails on t.Trans_id equals sd.Transactionid
                                           where
                                           sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                           && sd.paymode == 3
                                            && sd.Invtype == 1
                                               && !string.IsNullOrEmpty(t.narration)
                                               && new[] { "BENEFIT", "CASH", "CARD", "CREDIT" }.Contains(t.narration)
                                           group t by t.date.Date into g
                                           select new
                                           {
                                               Date = g.Key,
                                               TotalNetAmount = g.Sum(x => (decimal)x.amount_rs),
                                               AdditionalCashSales = g.Where(x => x.narration == "CASH").Sum(x => (decimal)x.amount_rs),
                                               AdditionalCardSales = g.Where(x => x.narration == "CARD").Sum(x => (decimal)x.amount_rs),
                                               AdditionalBenefitSale = g.Where(x => x.narration == "BENEFIT").Sum(x => (decimal)x.amount_rs),
                                               AdditionalCreditSale = g.Where(x => x.narration == "CREDIT").Sum(x => (decimal)x.amount_rs),
                                               CashtransCount = g.Count(x => x.narration == "CASH"),
                                               CardtransCount = g.Count(x => x.narration == "CARD"),
                                               BenefittransCount = g.Count(x => x.narration == "BENEFIT"),
                                               CredittransCount = g.Count(x => x.narration == "CREDIT")
                                           };

                // Sales Return Data
                var salesReturnData = from sr in _connection.Tbl_salesreturndetails
                                      join pm in _connection.tbl_paymod_master on sr.paymode equals pm.Id
                                      where sr.invdate.Date >= startDate.Date && sr.invdate.Date <= endDate.Date
                                      && string.IsNullOrEmpty(sr.cancel_sts)
                                          && sr.paymode != 3

                                      group sr by sr.invdate.Date into g
                                      select new
                                      {
                                          InvDate = g.Key,
                                          TotalReturnAmount = g.Sum(x => (decimal?)x.netamount) ?? 0,
                                          CashSalesReturn = g.Where(x => x.paymode == 1).Sum(x => (decimal?)x.netamount) ?? 0,
                                          CardSalesReturn = g.Where(x => x.paymode == 5).Sum(x => (decimal?)x.netamount) ?? 0,
                                          BenefitSalesReturn = g.Where(x => x.paymode == 4).Sum(x => (decimal?)x.netamount) ?? 0,
                                          CreditSalesReturn = g.Where(x => x.paymode == 2).Sum(x => (decimal?)x.netamount) ?? 0
                                      };
                var result = from sd in othersaleData.ToList()
                             join td in othertransactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                             from td in tdGroup.DefaultIfEmpty()
                             join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                             from srd in srdGroup.DefaultIfEmpty()
                             select new
                             {
                                 sd.InvDate,
                                 sd.InvoiceType,
                                 sd.TotalSales,
                                 NetTotalAmount = sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                 CashSales = sd.Paymode == 3
                    ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                    : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                 CardSales = sd.Paymode == 3
                    ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                    : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                 BenefitSale = sd.Paymode == 3
                      ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                      : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                 CreditSale = sd.Paymode == 3
                     ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                     : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                 CreditCount = sd.Paymode == 3
                      ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                      : sd.CreditSaleCount,
                                 BenefitCount = sd.Paymode == 3
                      ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                      : sd.BenefitSaleCount,
                                 CashCount = sd.Paymode == 3
                    ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                    : sd.CashSalesCount,
                                 CardCount = sd.Paymode == 3
                    ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                    : sd.CardSalesCount,
                             };
                // Group combined results by InvoiceType
                var othergroupedResult = from r in result
                                         group r by r.InvoiceType into g
                                         select new
                                         {
                                             InvoiceType = g.Key,
                                             TotalSales = g.Sum(x => x.TotalSales),
                                             NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                             CashSales = g.Sum(x => x.CashSales),
                                             CardSales = g.Sum(x => x.CardSales),
                                             BenefitSale = g.Sum(x => x.BenefitSale),
                                             CreditSale = g.Sum(x => x.CreditSale),
                                             CreditCount = g.Sum(x => x.CreditCount),
                                             BenefitCount = g.Sum(x => x.BenefitCount),
                                             CashCount = g.Sum(x => x.CashCount),
                                             CardCount = g.Sum(x => x.CardCount)
                                         };
                var mainsaleData = from sd in _connection.Tbl_salesdetails
                                   join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                                   join it in _connection.InvoiceType on sd.Invtype equals it.Id
                                   where
                                        sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                       && it.Id == 3
                                       && string.IsNullOrEmpty(sd.cancel_flg)

                                   group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                                   select new
                                   {
                                       InvDate = g.Key.Date,
                                       InvoiceType = g.Key.Name,
                                       Paymode = g.Key.paymode,
                                       TotalSales = g.Count(),
                                       TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                       CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                       CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                       BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                       CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                       CashSalesCount = g.Count(x => x.pm.Id == 1),
                                       CardSalesCount = g.Count(x => x.pm.Id == 5),
                                       BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                       CreditSaleCount = g.Count(x => x.pm.Id == 2)
                                   };



                // Transaction Data where paymode is 3
                var maintransactionData = from t in _connection.Tbl_transactions
                                          join sd in _connection.Tbl_salesdetails on t.Trans_id equals sd.Transactionid
                                          where
                                          sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                          && sd.paymode == 3
                                           && sd.Invtype == 3
                                              && !string.IsNullOrEmpty(t.narration)
                                              && new[] { "BENEFIT", "CASH", "CARD", "CREDIT" }.Contains(t.narration)
                                          group t by t.date.Date into g
                                          select new
                                          {
                                              Date = g.Key,
                                              TotalNetAmount = g.Sum(x => (decimal)x.amount_rs),
                                              AdditionalCashSales = g.Where(x => x.narration == "CASH").Sum(x => (decimal)x.amount_rs),
                                              AdditionalCardSales = g.Where(x => x.narration == "CARD").Sum(x => (decimal)x.amount_rs),
                                              AdditionalBenefitSale = g.Where(x => x.narration == "BENEFIT").Sum(x => (decimal)x.amount_rs),
                                              AdditionalCreditSale = g.Where(x => x.narration == "CREDIT").Sum(x => (decimal)x.amount_rs),
                                              CashtransCount = g.Count(x => x.narration == "CASH"),
                                              CardtransCount = g.Count(x => x.narration == "CARD"),
                                              BenefittransCount = g.Count(x => x.narration == "BENEFIT"),
                                              CredittransCount = g.Count(x => x.narration == "CREDIT")
                                          };

                // Sales Return Data

                var mainresult = from sd in mainsaleData.ToList()
                                 join td in maintransactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                                 from td in tdGroup.DefaultIfEmpty()
                                 join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                                 from srd in srdGroup.DefaultIfEmpty()
                                 select new
                                 {
                                     sd.InvDate,
                                     sd.InvoiceType,
                                     sd.TotalSales,
                                     NetTotalAmount = sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                     CashSales = sd.Paymode == 3
                        ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                        : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                     CardSales = sd.Paymode == 3
                        ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                        : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                     BenefitSale = sd.Paymode == 3
                          ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                          : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                     CreditSale = sd.Paymode == 3
                         ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                         : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                     CreditCount = sd.Paymode == 3
                          ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                          : sd.CreditSaleCount,
                                     BenefitCount = sd.Paymode == 3
                          ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                          : sd.BenefitSaleCount,
                                     CashCount = sd.Paymode == 3
                        ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                        : sd.CashSalesCount,
                                     CardCount = sd.Paymode == 3
                        ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                        : sd.CardSalesCount,
                                 };
                // Group combined results by InvoiceType
                var maingroupedResult = from r in mainresult
                                        group r by r.InvoiceType into g
                                        select new
                                        {
                                            InvoiceType = g.Key,
                                            TotalSales = g.Sum(x => x.TotalSales),
                                            NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                            CashSales = g.Sum(x => x.CashSales),
                                            CardSales = g.Sum(x => x.CardSales),
                                            BenefitSale = g.Sum(x => x.BenefitSale),
                                            CreditSale = g.Sum(x => x.CreditSale),
                                            CreditCount = g.Sum(x => x.CreditCount),
                                            BenefitCount = g.Sum(x => x.BenefitCount),
                                            CashCount = g.Sum(x => x.CashCount),
                                            CardCount = g.Sum(x => x.CardCount)
                                        };
                var alterationsaleData = from sd in _connection.Tbl_salesdetails
                                         join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                                         join it in _connection.InvoiceType on sd.Invtype equals it.Id
                                         where
                                              sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                             && it.Id == 6
                                             && string.IsNullOrEmpty(sd.cancel_flg)

                                         group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                                         select new
                                         {
                                             InvDate = g.Key.Date,
                                             InvoiceType = g.Key.Name,
                                             Paymode = g.Key.paymode,
                                             TotalSales = g.Count(),
                                             TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                             CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                             CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                             BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                             CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                             CashSalesCount = g.Count(x => x.pm.Id == 1),
                                             CardSalesCount = g.Count(x => x.pm.Id == 5),
                                             BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                             CreditSaleCount = g.Count(x => x.pm.Id == 2)
                                         };



                // Transaction Data where paymode is 3
                var alterationtransactionData = from t in _connection.Tbl_transactions
                                                join sd in _connection.Tbl_salesdetails on t.Trans_id equals sd.Transactionid
                                                where
                                                sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                                && sd.paymode == 3
                                                 && sd.Invtype == 3
                                                    && !string.IsNullOrEmpty(t.narration)
                                                    && new[] { "BENEFIT", "CASH", "CARD", "CREDIT" }.Contains(t.narration)
                                                group t by t.date.Date into g
                                                select new
                                                {
                                                    Date = g.Key,
                                                    TotalNetAmount = g.Sum(x => (decimal)x.amount_rs),
                                                    AdditionalCashSales = g.Where(x => x.narration == "CASH").Sum(x => (decimal)x.amount_rs),
                                                    AdditionalCardSales = g.Where(x => x.narration == "CARD").Sum(x => (decimal)x.amount_rs),
                                                    AdditionalBenefitSale = g.Where(x => x.narration == "BENEFIT").Sum(x => (decimal)x.amount_rs),
                                                    AdditionalCreditSale = g.Where(x => x.narration == "CREDIT").Sum(x => (decimal)x.amount_rs),
                                                    CashtransCount = g.Count(x => x.narration == "CASH"),
                                                    CardtransCount = g.Count(x => x.narration == "CARD"),
                                                    BenefittransCount = g.Count(x => x.narration == "BENEFIT"),
                                                    CredittransCount = g.Count(x => x.narration == "CREDIT")
                                                };

                // Sales Return Data

                var alterationresult = from sd in alterationsaleData.ToList()
                                       join td in alterationtransactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                                       from td in tdGroup.DefaultIfEmpty()
                                       join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                                       from srd in srdGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           sd.InvDate,
                                           sd.InvoiceType,
                                           sd.TotalSales,
                                           NetTotalAmount = sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                           CashSales = sd.Paymode == 3
                              ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                              : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                           CardSales = sd.Paymode == 3
                              ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                              : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                           BenefitSale = sd.Paymode == 3
                                ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                                : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                           CreditSale = sd.Paymode == 3
                               ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                               : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                           CreditCount = sd.Paymode == 3
                                ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                                : sd.CreditSaleCount,
                                           BenefitCount = sd.Paymode == 3
                                ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                                : sd.BenefitSaleCount,
                                           CashCount = sd.Paymode == 3
                              ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                              : sd.CashSalesCount,
                                           CardCount = sd.Paymode == 3
                              ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                              : sd.CardSalesCount,
                                       };
                // Group combined results by InvoiceType
                var alterationgroupedResult = from r in alterationresult
                                              group r by r.InvoiceType into g
                                              select new
                                              {
                                                  InvoiceType = g.Key,
                                                  TotalSales = g.Sum(x => x.TotalSales),
                                                  NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                                  CashSales = g.Sum(x => x.CashSales),
                                                  CardSales = g.Sum(x => x.CardSales),
                                                  BenefitSale = g.Sum(x => x.BenefitSale),
                                                  CreditSale = g.Sum(x => x.CreditSale),
                                                  CreditCount = g.Sum(x => x.CreditCount),
                                                  BenefitCount = g.Sum(x => x.BenefitCount),
                                                  CashCount = g.Sum(x => x.CashCount),
                                                  CardCount = g.Sum(x => x.CardCount)
                                              };
                var quotationsaleData = from sd in _connection.Tbl_salesdetails
                                        join pm in _connection.tbl_paymod_master on sd.paymode equals pm.Id
                                        join it in _connection.InvoiceType on sd.Invtype equals it.Id
                                        where
                                             sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                            && it.Id == 7
                                            && string.IsNullOrEmpty(sd.cancel_flg)

                                        group new { sd, pm, it } by new { sd.invdate.Date, it.Name, sd.paymode } into g
                                        select new
                                        {
                                            InvDate = g.Key.Date,
                                            InvoiceType = g.Key.Name,
                                            Paymode = g.Key.paymode,
                                            TotalSales = g.Count(),
                                            TotalNetAmount = g.Sum(x => (decimal)x.sd.netamount),
                                            CashSalesAmount = g.Where(x => x.pm.Id == 1).Sum(x => (decimal)x.sd.netamount),
                                            CardSalesAmount = g.Where(x => x.pm.Id == 5).Sum(x => (decimal)x.sd.netamount),
                                            BenefitSaleAmount = g.Where(x => x.pm.Id == 4).Sum(x => (decimal)x.sd.netamount),
                                            CreditSaleAmount = g.Where(x => x.pm.Id == 2).Sum(x => (decimal)x.sd.netamount),
                                            CashSalesCount = g.Count(x => x.pm.Id == 1),
                                            CardSalesCount = g.Count(x => x.pm.Id == 5),
                                            BenefitSaleCount = g.Count(x => x.pm.Id == 4),
                                            CreditSaleCount = g.Count(x => x.pm.Id == 2)
                                        };



                // Transaction Data where paymode is 3
                var quotationtransactionData = from t in _connection.Tbl_transactions
                                               join sd in _connection.Tbl_salesdetails on t.Trans_id equals sd.Transactionid
                                               where
                                               sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                               && sd.paymode == 3
                                                && sd.Invtype == 7
                                                   && !string.IsNullOrEmpty(t.narration)
                                                   && new[] { "BENEFIT", "CASH", "CARD", "CREDIT" }.Contains(t.narration)
                                               group t by t.date.Date into g
                                               select new
                                               {
                                                   Date = g.Key,
                                                   TotalNetAmount = g.Sum(x => (decimal)x.amount_rs),
                                                   AdditionalCashSales = g.Where(x => x.narration == "CASH").Sum(x => (decimal)x.amount_rs),
                                                   AdditionalCardSales = g.Where(x => x.narration == "CARD").Sum(x => (decimal)x.amount_rs),
                                                   AdditionalBenefitSale = g.Where(x => x.narration == "BENEFIT").Sum(x => (decimal)x.amount_rs),
                                                   AdditionalCreditSale = g.Where(x => x.narration == "CREDIT").Sum(x => (decimal)x.amount_rs),
                                                   CashtransCount = g.Count(x => x.narration == "CASH"),
                                                   CardtransCount = g.Count(x => x.narration == "CARD"),
                                                   BenefittransCount = g.Count(x => x.narration == "BENEFIT"),
                                                   CredittransCount = g.Count(x => x.narration == "CREDIT")
                                               };

                // Sales Return Data

                var quotationresult = from sd in quotationsaleData.ToList()
                                      join td in quotationtransactionData.ToList() on sd.InvDate equals td.Date into tdGroup
                                      from td in tdGroup.DefaultIfEmpty()
                                      join srd in salesReturnData.ToList() on sd.InvDate equals srd.InvDate into srdGroup
                                      from srd in srdGroup.DefaultIfEmpty()
                                      select new
                                      {
                                          sd.InvDate,
                                          sd.InvoiceType,
                                          sd.TotalSales,
                                          NetTotalAmount = sd.TotalNetAmount - (srd?.TotalReturnAmount ?? 0),
                                          CashSales = sd.Paymode == 3
                             ? (sd.CashSalesAmount + (td?.AdditionalCashSales ?? 0)) - (srd?.CashSalesReturn ?? 0)
                             : sd.CashSalesAmount - (srd?.CashSalesReturn ?? 0),
                                          CardSales = sd.Paymode == 3
                             ? (sd.CardSalesAmount + (td?.AdditionalCardSales ?? 0)) - (srd?.CardSalesReturn ?? 0)
                             : sd.CardSalesAmount - (srd?.CardSalesReturn ?? 0),
                                          BenefitSale = sd.Paymode == 3
                               ? (sd.BenefitSaleAmount + (td?.AdditionalBenefitSale ?? 0)) - (srd?.BenefitSalesReturn ?? 0)
                               : sd.BenefitSaleAmount - (srd?.BenefitSalesReturn ?? 0),
                                          CreditSale = sd.Paymode == 3
                              ? (sd.CreditSaleAmount + (td?.AdditionalCreditSale ?? 0)) - (srd?.CreditSalesReturn ?? 0)
                              : sd.CreditSaleAmount - (srd?.CreditSalesReturn ?? 0),
                                          CreditCount = sd.Paymode == 3
                               ? (sd.CreditSaleCount + (td?.CredittransCount ?? 0))
                               : sd.CreditSaleCount,
                                          BenefitCount = sd.Paymode == 3
                               ? (sd.BenefitSaleCount + (td?.BenefittransCount ?? 0))
                               : sd.BenefitSaleCount,
                                          CashCount = sd.Paymode == 3
                             ? (sd.CashSalesCount + (td?.CashtransCount ?? 0))
                             : sd.CashSalesCount,
                                          CardCount = sd.Paymode == 3
                             ? (sd.CardSalesCount + (td?.CardtransCount ?? 0))
                             : sd.CardSalesCount,
                                      };
                // Group combined results by InvoiceType
                var quotationgroupedResult = from r in quotationresult
                                             group r by r.InvoiceType into g
                                             select new
                                             {
                                                 InvoiceType = g.Key,
                                                 TotalSales = g.Sum(x => x.TotalSales),
                                                 NetTotalAmount = g.Sum(x => x.NetTotalAmount),
                                                 CashSales = g.Sum(x => x.CashSales),
                                                 CardSales = g.Sum(x => x.CardSales),
                                                 BenefitSale = g.Sum(x => x.BenefitSale),
                                                 CreditSale = g.Sum(x => x.CreditSale),
                                                 CreditCount = g.Sum(x => x.CreditCount),
                                                 BenefitCount = g.Sum(x => x.BenefitCount),
                                                 CashCount = g.Sum(x => x.CashCount),
                                                 CardCount = g.Sum(x => x.CardCount)
                                             };
                var cashInData = from c in _connection.Cashinout
                                    where
                                    c.Date.Date >= startDate.Date && c.Date.Date <= endDate.Date
                                        && c.cancel_sts == "" && c.Type == "CASH IN"
                                 group c by new { c.Type, c.description } into g
                                    select new
                                    {
                                        Type = g.Key.Type,
                                        Description = g.Key.description,
                                        TotalAmount = g.Sum(x => (decimal)x.Amount) 
                                      
                                    };

                var cashInResults = cashInData
                    .GroupBy(x => x.Description) // Group only by Description
                    .Select(g => new
                    {
                        Description = g.Key,
                        CashInAmount = g.Where(x => x.Type == "CASH IN").Sum(x => x.TotalAmount),
                        TotalAmount =  g.Where(x => x.Type == "CASH IN").Sum(x => x.TotalAmount)
                        // Subtract CASH OUT from CASH IN
                    })
                    .ToList();
                var cashOutData = from c in _connection.Cashinout
                                    where
                                    c.Date.Date >= startDate.Date && c.Date.Date <= endDate.Date
                                        && c.cancel_sts == "" && c.Type=="CASH OUT"
                                    group c by new { c.Type, c.description } into g
                                    select new
                                    {
                                        Type = g.Key.Type,
                                        Description = g.Key.description,
                                        TotalAmount = g.Sum(x => (decimal)x.Amount) // Explicitly cast to decimal
                                    };

                var cashOutResults = cashOutData
                    .GroupBy(x => x.Description) // Group only by Description
                    .Select(g => new
                    {
                        Description = g.Key,
                        CashOutAmount = g.Where(x => x.Type == "CASH OUT").Sum(x => x.TotalAmount),
                        TotalAmount = g.Where(x => x.Type == "CASH OUT").Sum(x => x.TotalAmount) 
                        // Subtract CASH OUT from CASH IN
                    })
                    .ToList();

                var Employeesummary = from sd in _connection.Tbl_salesdetails
                                      join te in _connection.Tbl_employee on sd.Salesman equals te.Id
                                      where
                                      sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
                                      && sd.cancel_flg == ""
                                      && te.Status == "Active"
                                      group sd by te.Name into g
                                      select new
                                      {
                                          Employeename = g.Key,
                                          Totalamount = g.Sum(x => (decimal)x.netamount)

                                      };
                var categoryresult = from sm in _connection.Tbl_salemaster
                                     join sd in _connection.Tbl_salesdetails on sm.Transactionid equals sd.Transactionid
                                     join im in _connection.tbl_item_master on sm.item_id equals im.Id
                                     join c in _connection.item_category on im.category equals c.Id
                                     where sm.cancelsts == "" &&
                                           sd.invdate.Date >= startDate.Date &&
                                           sd.invdate.Date <= endDate.Date
                                     group sm by c.Name into g
                                     orderby g.Key
                                     select new
                                     {
                                         CategoryName = g.Key,
                                         TotalQuantity = g.Sum(x => x.qty),
                                         TotalNetAmount = g.Sum(x => x.netamount),
                                         TotalTransactions = g.Count()
                                     };

                var list = categoryresult.ToList();
                var products = from sm in _connection.Tbl_salemaster
                               join im in _connection.tbl_item_master on sm.item_id equals im.Id
                               join sd in _connection.Tbl_salesdetails on sm.transid equals sd.Transactionid
                               where sm.cancelsts == "" && sd.cancel_flg == "" && sd.invdate.Date >= startDate.Date
                               && sd.invdate.Date <= endDate.Date
                               group sd by im.Name into g
                               orderby g.Key
                               select new
                               {
                                   ProductName = g.Key,
                                   TotalQuantity = g.Count(),
                                   TotalNetAmount = g.Sum(x => x.netamount)
                               };
                var plist = products.ToList();

                var balance = _connection.DAYSHIFT
     .Where(d => d.starttime.Date >= startDate.Date && d.endtime.Date <= endDate.Date)
     .GroupBy(d => true)
     .Select(g => new
     {
         TotalOpeningBalance = g.Sum(d => d.OP_BALANCE),
         TotalClosingBalance = g.Sum(d => d.CL_BALANCE)
     })
     .FirstOrDefault();
               

                //var Alterationquery = from tr in _connection.Tbl_receiptdetails
                //                      join hb in _connection.Holdbilldetails on tr.OrderId equals hb.Transactionid
                //                      join tt in _connection.Tbl_transactions on tr.transactionid equals tt.Trans_id
                                    
                //                      where
                //                            hb. >= startDate.Date && hb.date.Date <= endDate.Date
                //                          && tt.cancel_sts == ""
                //                          && hb.HoldType == "Alteration"
                //                          && tt.narration != null
                //                          && tt.narration != ""
                //                      select new
                //                      {
                //                          tr.Debitamnt,
                //                          tt.narration
                //                      };

                //var alterationresultList = Alterationquery.ToList();



                //var joborderquery = from tr in _connection.Tbl_receiptdetails
                //                    join hb in _connection.Holdbilldetails on tr.OrderId equals hb.Transactionid
                //                    join tt in _connection.Tbl_transactions on tr.transactionid equals tt.Trans_id
                //                    where
                //                         tt.cancel_sts == ""
                //                        && hb.HoldType == "JobOrder"
                //                        && tt.narration != null
                //                        && tt.narration != ""
                //                    select new
                //                    {
                //                        tr.Debitamnt,
                //                        tt.narration
                //                    };

                //var joborderresultList = joborderquery.ToList();
                var combinedSummary = new
                {
                    obcb = balance,
                    Employeesummary = Employeesummary.ToList(),
                    Mainsalesummary = maingroupedResult.ToList(),
                    SalesReturnSummary = salesReturnData.ToList(),
                    OtherSalesSummary = othergroupedResult.ToList(),
                    AlterationSummary = alterationgroupedResult.ToList(),
                    Quotationsummary = quotationgroupedResult.ToList(),
                    Cashin = cashInData.ToList(),
                    Cashout=cashOutData.ToList(),
                    ItemCategorySummary = categoryresult.ToList(),
                    productSummary=products.ToList(),
                };

                return Ok(combinedSummary);
            }
        }
        [HttpGet]
        [Route("monthendreport")]
        public async Task<ActionResult<IEnumerable<Monthend_report>>> GetMonthEnd()
        {
            try
            {
                var monthEndList = await (from ts in _connection.DAYSHIFT
                                          join c in _connection.Tbl_user on ts.USERID equals c.Id
                                          join ac in _connection.countersettings on ts.counter equals ac.id
                                          join tl in _connection.Tbl_location on ts.Branch equals tl.LocationId
                                          join sd in _connection.Tbl_salesdetails on ts.DAYID equals sd.dayid
                                          group new { ts, c, ac, tl, sd } by new
                                          {
                                              Year = sd.invdate.Year,
                                              Month = sd.invdate.Month,
                                              ts.counter,
                                              ts.USERID,
                                          } into grouped
                                          select new
                                          {
                                              TotalNetAmount = grouped.Sum(x => x.sd.netamount),
                                              Id = grouped.Key.USERID,
                                              User_Name = grouped.Max(g => g.c.User_Name),
                                              Year = grouped.Key.Year,
                                              Month = grouped.Key.Month,
                                              counterid = grouped.Key.counter,
                                              countername = grouped.Max(g => g.ac.counter),
                                              LocationId = grouped.Max(g => g.tl.LocationId),
                                              starttime = grouped.Min(g => g.ts.starttime),
                                              endtime = grouped.Max(g => g.ts.endtime),
                                              transactionId = grouped.Max(g => g.sd.Transactionid),
                                              Location = grouped.Max(g => g.tl.Location),
                                              // Add other aggregated fields here as needed
                                          }).ToListAsync();

                var formattedResults = monthEndList.Select(x => new Monthend_report
                {
                    TotalNetAmount = x.TotalNetAmount,
                    Id = x.Id,
                    User_Name = x.User_Name,
                    Year = x.Year,
                    Month = x.Month,
                    counterid = x.counterid,
                    countername = x.countername,
                    LocationId = x.LocationId,
                    starttime = x.starttime.ToString("yyyy/MM/dd"),
                    endtime = x.endtime.ToString("yyyy/MM/dd"),
                    transactionId = x.transactionId,
                    Location = x.Location,
                    // Map other fields as needed
                }).ToList();

                if (formattedResults == null || formattedResults.Count == 0)
                {
                    return NotFound("No month-end reports found.");
                }

                return Ok(formattedResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
//        [HttpGet]
//        [Route("GetSalesSummary1")]
//        public IActionResult GetSalesSSummary(int month, int year)
//        {
//            // Calculate the start date and end date based on the selected month and year
//            var startDate = new DateTime(year, month, 1); // First day of the month
//            var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month

//            // Sales Data
//            var othersalesData = _connection.Tbl_salesdetails
//                .Where(s => s.paymode != 3 && s.Invtype == 1 && s.cancel_flg == ""
//                    && s.invdate.Date >= startDate.Date && s.invdate.Date <= endDate.Date)
//                .GroupBy(s => s.Invtype) // Group by Invtype to include it in the summary
//                .Select(g => new
//                {
//                    Invtype = g.Key,
//                    TotalNetAmount = g.Sum(s => s.netamount),
//                    CashSales = g.Sum(s => s.paymode == 1 ? s.netamount : 0),
//                    CreditSales = g.Sum(s => s.paymode == 2 ? s.netamount : 0),
//                    BenefitSales = g.Sum(s => s.paymode == 4 ? s.netamount : 0),
//                    CardSales = g.Sum(s => s.paymode == 5 ? s.netamount : 0)
//                }).FirstOrDefault();

//            // Transaction Data
//            var othertransactionData = _connection.Tbl_transactions
//                .Join(_connection.Tbl_salesdetails,
//                    tt => tt.Trans_id,
//                    sd => sd.Transactionid,
//                    (tt, sd) => new { tt, sd })
//                .Where(joined => joined.sd.paymode == 3 && joined.sd.Invtype == 1
//                    && joined.tt.date >= startDate.Date && joined.tt.date <= endDate.Date
//                    && !string.IsNullOrEmpty(joined.tt.narration)
//                    && new[] { "CASH", "CREDIT", "BENEFIT", "CARD" }.Contains(joined.tt.narration))
//                .GroupBy(joined => joined.sd.Invtype) // Group by Invtype to include it in the summary
//                .Select(g => new
//                {
//                    Invtype = g.Key,
//                    TotalNetAmount = g.Sum(j => j.tt.amount_rs),
//                    AdditionalCashSales = g.Sum(j => j.tt.narration == "CASH" ? j.tt.amount_rs : 0),
//                    AdditionalCreditSales = g.Sum(j => j.tt.narration == "CREDIT" ? j.tt.amount_rs : 0),
//                    AdditionalBenefitSales = g.Sum(j => j.tt.narration == "BENEFIT" ? j.tt.amount_rs : 0),
//                    AdditionalCardSales = g.Sum(j => j.tt.narration == "CARD" ? j.tt.amount_rs : 0)
//                }).FirstOrDefault();

//            // Sales Return Data
//            var othersalesReturnData = _connection.Tbl_salesreturndetails
//                .Where(sr => sr.paymode != 3 && sr.Invtype == 1 && sr.cancel_sts == ""
//                    && sr.invdate.Date >= startDate.Date && sr.invdate.Date <= endDate.Date)
//                .GroupBy(sr => sr.Invtype) // Group by Invtype to include it in the summary
//                .Select(g => new
//                {
//                    Invtype = g.Key,
//                    TotalReturnAmount = g.Sum(sr => sr.netamount),
//                    CashSalesReturn = g.Sum(sr => sr.paymode == 1 ? sr.netamount : 0),
//                    CreditSalesReturn = g.Sum(sr => sr.paymode == 2 ? sr.netamount : 0),
//                    BenefitSalesReturn = g.Sum(sr => sr.paymode == 4 ? sr.netamount : 0),
//                    CardSalesReturn = g.Sum(sr => sr.paymode == 5 ? sr.netamount : 0)
//                }).FirstOrDefault();

//            // Combine Data
//            var otherresult = new
//            {
//                Invtype = othersalesData?.Invtype ?? othertransactionData?.Invtype ?? othersalesReturnData?.Invtype, // Include Invtype in the result
//                NetTotalAmount = (othersalesData?.TotalNetAmount ?? 0) + (othertransactionData?.TotalNetAmount ?? 0) - (othersalesReturnData?.TotalReturnAmount ?? 0),
//                CashSales = (othersalesData?.CashSales ?? 0) + (othertransactionData?.AdditionalCashSales ?? 0) - (othersalesReturnData?.CashSalesReturn ?? 0),
//                CreditSales = (othersalesData?.CreditSales ?? 0) + (othertransactionData?.AdditionalCreditSales ?? 0) - (othersalesReturnData?.CreditSalesReturn ?? 0),
//                BenefitSales = (othersalesData?.BenefitSales ?? 0) + (othertransactionData?.AdditionalBenefitSales ?? 0) - (othersalesReturnData?.BenefitSalesReturn ?? 0),
//                CardSales = (othersalesData?.CardSales ?? 0) + (othertransactionData?.AdditionalCardSales ?? 0) - (othersalesReturnData?.CardSalesReturn ?? 0)
//            };

//            // Return the combined summary (you can add other summaries here if needed)
          

//        var mainsalesData = _connection.Tbl_salesdetails
//  .Where(s => s.paymode != 3 && s.Invtype == 3 && s.cancel_flg == "" && s.invdate.Date >= startDate.Date && s.invdate.Date <= endDate.Date)
//  .GroupBy(s => s.Invtype) // Group by Invtype to include it in the summary
//  .Select(g => new
//  {
//      Invtype = g.Key,
//      TotalNetAmount = g.Sum(s => s.netamount),
//      CashSales = g.Sum(s => s.paymode == 1 ? s.netamount : 0),
//      CreditSales = g.Sum(s => s.paymode == 2 ? s.netamount : 0),
//      BenefitSales = g.Sum(s => s.paymode == 4 ? s.netamount : 0),
//      CardSales = g.Sum(s => s.paymode == 5 ? s.netamount : 0)
//  }).FirstOrDefault();

//                // Transaction Data
//                var maintransactionData = _connection.Tbl_transactions
//                    .Join(_connection.Tbl_salesdetails,
//                        tt => tt.Trans_id,
//                        sd => sd.Transactionid,
//                        (tt, sd) => new { tt, sd })
//                    .Where(joined => joined.sd.paymode == 3 && joined.sd.Invtype == 3
//                        && joined.tt.date >= startDate.Date && joined.tt.date <= endDate.Date
//                        && !string.IsNullOrEmpty(joined.tt.narration)
//                        && new[] { "CASH", "CREDIT", "BENEFIT", "CARD" }.Contains(joined.tt.narration))
//                    .GroupBy(joined => joined.sd.Invtype) // Group by Invtype to include it in the summary
//                    .Select(g => new
//                    {
//                        Invtype = g.Key,
//                        TotalNetAmount = g.Sum(j => j.tt.amount_rs),
//                        AdditionalCashSales = g.Sum(j => j.tt.narration == "CASH" ? j.tt.amount_rs : 0),
//                        AdditionalCreditSales = g.Sum(j => j.tt.narration == "CREDIT" ? j.tt.amount_rs : 0),
//                        AdditionalBenefitSales = g.Sum(j => j.tt.narration == "BENEFIT" ? j.tt.amount_rs : 0),
//                        AdditionalCardSales = g.Sum(j => j.tt.narration == "CARD" ? j.tt.amount_rs : 0)
//                    }).FirstOrDefault();

//                // Sales Return Data
//                var mainsalesReturnData = _connection.Tbl_salesreturndetails
//                    .Where(sr => sr.paymode != 3 && sr.Invtype == 3 && sr.cancel_sts == ""
//                        && sr.invdate.Date >= startDate.Date && sr.invdate.Date <= endDate.Date)
//                    .GroupBy(sr => sr.Invtype) // Group by Invtype to include it in the summary
//                    .Select(g => new
//                    {
//                        Invtype = g.Key,
//                        TotalReturnAmount = g.Sum(sr => sr.netamount),
//                        CashSalesReturn = g.Sum(sr => sr.paymode == 1 ? sr.netamount : 0),
//                        CreditSalesReturn = g.Sum(sr => sr.paymode == 2 ? sr.netamount : 0),
//                        BenefitSalesReturn = g.Sum(sr => sr.paymode == 4 ? sr.netamount : 0),
//                        CardSalesReturn = g.Sum(sr => sr.paymode == 5 ? sr.netamount : 0)
//                    }).FirstOrDefault();

//                // Combine Data
//                var mainresult = new
//                {
//                    Invtype = mainsalesData?.Invtype ?? maintransactionData?.Invtype ?? mainsalesReturnData?.Invtype, // Include Invtype in the result
//                    NetTotalAmount = (mainsalesData?.TotalNetAmount ?? 0) + (maintransactionData?.TotalNetAmount ?? 0) - (mainsalesReturnData?.TotalReturnAmount ?? 0),
//                    CashSales = (mainsalesData?.CashSales ?? 0) + (maintransactionData?.AdditionalCashSales ?? 0) - (mainsalesReturnData?.CashSalesReturn ?? 0),
//                    CreditSales = (mainsalesData?.CreditSales ?? 0) + (maintransactionData?.AdditionalCreditSales ?? 0) - (mainsalesReturnData?.CreditSalesReturn ?? 0),
//                    BenefitSales = (mainsalesData?.BenefitSales ?? 0) + (maintransactionData?.AdditionalBenefitSales ?? 0) - (mainsalesReturnData?.BenefitSalesReturn ?? 0),
//                    CardSales = (mainsalesData?.CardSales ?? 0) + (maintransactionData?.AdditionalCardSales ?? 0) - (mainsalesReturnData?.CardSalesReturn ?? 0)
//                };
//                var altersalesData = _connection.Tbl_salesdetails
//.Where(s => s.paymode != 3 && s.Invtype == 6 && s.cancel_flg == "" && s.invdate.Date >= startDate.Date && s.invdate.Date <= endDate.Date)
//.GroupBy(s => s.Invtype) // Group by Invtype to include it in the summary
//.Select(g => new
//{
//    Invtype = g.Key,
//    TotalNetAmount = g.Sum(s => s.netamount),
//    CashSales = g.Sum(s => s.paymode == 1 ? s.netamount : 0),
//    CreditSales = g.Sum(s => s.paymode == 2 ? s.netamount : 0),
//    BenefitSales = g.Sum(s => s.paymode == 4 ? s.netamount : 0),
//    CardSales = g.Sum(s => s.paymode == 5 ? s.netamount : 0)
//}).FirstOrDefault();

//                // Transaction Data
//                var altertransactionData = _connection.Tbl_transactions
//                    .Join(_connection.Tbl_salesdetails,
//                        tt => tt.Trans_id,
//                        sd => sd.Transactionid,
//                        (tt, sd) => new { tt, sd })
//                    .Where(joined => joined.sd.paymode == 3 && joined.sd.Invtype == 6
//                        && joined.tt.date >= startDate.Date && joined.tt.date <= endDate.Date
//                        && !string.IsNullOrEmpty(joined.tt.narration)
//                        && new[] { "CASH", "CREDIT", "BENEFIT", "CARD" }.Contains(joined.tt.narration))
//                    .GroupBy(joined => joined.sd.Invtype) // Group by Invtype to include it in the summary
//                    .Select(g => new
//                    {
//                        Invtype = g.Key,
//                        TotalNetAmount = g.Sum(j => j.tt.amount_rs),
//                        AdditionalCashSales = g.Sum(j => j.tt.narration == "CASH" ? j.tt.amount_rs : 0),
//                        AdditionalCreditSales = g.Sum(j => j.tt.narration == "CREDIT" ? j.tt.amount_rs : 0),
//                        AdditionalBenefitSales = g.Sum(j => j.tt.narration == "BENEFIT" ? j.tt.amount_rs : 0),
//                        AdditionalCardSales = g.Sum(j => j.tt.narration == "CARD" ? j.tt.amount_rs : 0)
//                    }).FirstOrDefault();

//                // Sales Return Data
//                var altersalesReturnData = _connection.Tbl_salesreturndetails
//                    .Where(sr => sr.paymode != 3 && sr.Invtype == 6 && sr.cancel_sts == ""
//                        && sr.invdate.Date >= startDate.Date && sr.invdate.Date <= endDate.Date)
//                    .GroupBy(sr => sr.Invtype) // Group by Invtype to include it in the summary
//                    .Select(g => new
//                    {
//                        Invtype = g.Key,
//                        TotalReturnAmount = g.Sum(sr => sr.netamount),
//                        CashSalesReturn = g.Sum(sr => sr.paymode == 1 ? sr.netamount : 0),
//                        CreditSalesReturn = g.Sum(sr => sr.paymode == 2 ? sr.netamount : 0),
//                        BenefitSalesReturn = g.Sum(sr => sr.paymode == 4 ? sr.netamount : 0),
//                        CardSalesReturn = g.Sum(sr => sr.paymode == 5 ? sr.netamount : 0)
//                    }).FirstOrDefault();

//                // Combine Data
//                var alterresult = new
//                {
//                    Invtype = altersalesData?.Invtype ?? altertransactionData?.Invtype ?? altersalesReturnData?.Invtype, // Include Invtype in the result
//                    NetTotalAmount = (altersalesData?.TotalNetAmount ?? 0) + (altertransactionData?.TotalNetAmount ?? 0) - (altersalesReturnData?.TotalReturnAmount ?? 0),
//                    CashSales = (altersalesData?.CashSales ?? 0) + (altertransactionData?.AdditionalCashSales ?? 0) - (altersalesReturnData?.CashSalesReturn ?? 0),
//                    CreditSales = (altersalesData?.CreditSales ?? 0) + (altertransactionData?.AdditionalCreditSales ?? 0) - (altersalesReturnData?.CreditSalesReturn ?? 0),
//                    BenefitSales = (altersalesData?.BenefitSales ?? 0) + (altertransactionData?.AdditionalBenefitSales ?? 0) - (altersalesReturnData?.BenefitSalesReturn ?? 0),
//                    CardSales = (altersalesData?.CardSales ?? 0) + (altertransactionData?.AdditionalCardSales ?? 0) - (altersalesReturnData?.CardSalesReturn ?? 0)
//                };
//                var quotsalesData = _connection.Tbl_salesdetails
//.Where(s => s.paymode != 3 && s.Invtype == 7 && s.cancel_flg == "" && s.invdate.Date >= startDate.Date && s.invdate.Date <= endDate.Date)
//.GroupBy(s => s.Invtype) // Group by Invtype to include it in the summary
//.Select(g => new
//{
//    Invtype = g.Key,
//    TotalNetAmount = g.Sum(s => s.netamount),
//    CashSales = g.Sum(s => s.paymode == 1 ? s.netamount : 0),
//    CreditSales = g.Sum(s => s.paymode == 2 ? s.netamount : 0),
//    BenefitSales = g.Sum(s => s.paymode == 4 ? s.netamount : 0),
//    CardSales = g.Sum(s => s.paymode == 5 ? s.netamount : 0)
//}).FirstOrDefault();

//                // Transaction Data
//                var quottransactionData = _connection.Tbl_transactions
//                    .Join(_connection.Tbl_salesdetails,
//                        tt => tt.Trans_id,
//                        sd => sd.Transactionid,
//                        (tt, sd) => new { tt, sd })
//                    .Where(joined => joined.sd.paymode == 3 && joined.sd.Invtype == 7
//                        && joined.tt.date >= startDate.Date && joined.tt.date <= endDate.Date
//                        && !string.IsNullOrEmpty(joined.tt.narration)
//                        && new[] { "CASH", "CREDIT", "BENEFIT", "CARD" }.Contains(joined.tt.narration))
//                    .GroupBy(joined => joined.sd.Invtype) // Group by Invtype to include it in the summary
//                    .Select(g => new
//                    {
//                        Invtype = g.Key,
//                        TotalNetAmount = g.Sum(j => j.tt.amount_rs),
//                        AdditionalCashSales = g.Sum(j => j.tt.narration == "CASH" ? j.tt.amount_rs : 0),
//                        AdditionalCreditSales = g.Sum(j => j.tt.narration == "CREDIT" ? j.tt.amount_rs : 0),
//                        AdditionalBenefitSales = g.Sum(j => j.tt.narration == "BENEFIT" ? j.tt.amount_rs : 0),
//                        AdditionalCardSales = g.Sum(j => j.tt.narration == "CARD" ? j.tt.amount_rs : 0)
//                    }).FirstOrDefault();

//                // Sales Return Data
//                var quotsalesReturnData = _connection.Tbl_salesreturndetails
//                    .Where(sr => sr.paymode != 3 && sr.Invtype == 7 && sr.cancel_sts == ""
//                        && sr.invdate.Date >= startDate.Date && sr.invdate.Date <= endDate.Date)
//                    .GroupBy(sr => sr.Invtype) // Group by Invtype to include it in the summary
//                    .Select(g => new
//                    {
//                        Invtype = g.Key,
//                        TotalReturnAmount = g.Sum(sr => sr.netamount),
//                        CashSalesReturn = g.Sum(sr => sr.paymode == 1 ? sr.netamount : 0),
//                        CreditSalesReturn = g.Sum(sr => sr.paymode == 2 ? sr.netamount : 0),
//                        BenefitSalesReturn = g.Sum(sr => sr.paymode == 4 ? sr.netamount : 0),
//                        CardSalesReturn = g.Sum(sr => sr.paymode == 5 ? sr.netamount : 0)
//                    }).FirstOrDefault();

//                // Combine Data
//                var quotresult = new
//                {
//                    Invtype = quotsalesData?.Invtype ?? quottransactionData?.Invtype ?? quotsalesReturnData?.Invtype, // Include Invtype in the result
//                    NetTotalAmount = (quotsalesData?.TotalNetAmount ?? 0) + (quottransactionData?.TotalNetAmount ?? 0) - (quotsalesReturnData?.TotalReturnAmount ?? 0),
//                    CashSales = (quotsalesData?.CashSales ?? 0) + (quottransactionData?.AdditionalCashSales ?? 0) - (quotsalesReturnData?.CashSalesReturn ?? 0),
//                    CreditSales = (quotsalesData?.CreditSales ?? 0) + (quottransactionData?.AdditionalCreditSales ?? 0) - (quotsalesReturnData?.CreditSalesReturn ?? 0),
//                    BenefitSales = (quotsalesData?.BenefitSales ?? 0) + (quottransactionData?.AdditionalBenefitSales ?? 0) - (quotsalesReturnData?.BenefitSalesReturn ?? 0),
//                    CardSales = (quotsalesData?.CardSales ?? 0) + (quottransactionData?.AdditionalCardSales ?? 0) - (quotsalesReturnData?.CardSalesReturn ?? 0)
//                };
//                var cashInData = _connection.Cashinout
//                      .Where(c => c.Date >= startDate.Date && c.Date <= endDate.Date && c.Type == "CASH IN")
                     
//                   .Select(c => new
//     {
//         Amount =c.Amount ,  
//         Description = c.description 
//     }).ToList();
//                var cashOutData = _connection.Cashinout
//     .Where(c => c.Type == "CASH OUT" && c.Date >= startDate.Date && c.Date <= endDate.Date)
//     .Select(c => new
//     {
//      Amount=  c.Amount,
//       Description= c.description
//     }).ToList();


//                var Employeesummary = from sd in _connection.Tbl_salesdetails
//                                      join te in _connection.Tbl_employee on sd.Salesman equals te.Id
//                                      where
//                                      sd.invdate.Date >= startDate.Date && sd.invdate.Date <= endDate.Date
//                                      && sd.cancel_flg == ""
//                                      && te.Status == "Active"
//                                      group sd by te.Name into g
//                                      select new
//                                      {
//                                          Employeename = g.Key,
//                                          Totalamount = g.Sum(x => (decimal)x.netamount)

//                                      };
//                var categoryresult = from sm in _connection.Tbl_salemaster
//                                     join sd in _connection.Tbl_salesdetails on sm.Transactionid equals sd.Transactionid
//                                     join im in _connection.tbl_item_master on sm.item_id equals im.Id
//                                     join c in _connection.item_category on im.category equals c.Id
//                                     where sm.cancelsts == "" &&
//                                           sd.invdate.Date >= startDate.Date &&
//                                           sd.invdate.Date <= endDate.Date
//                                     group sm by c.Name into g
//                                     orderby g.Key
//                                     select new
//                                     {
//                                         CategoryName = g.Key,
//                                         TotalQuantity = g.Sum(x => x.qty),
//                                         TotalNetAmount = g.Sum(x => x.netamount),
//                                         TotalTransactions = g.Count()
//                                     };

//                var list = categoryresult.ToList();
//                var balance = _connection.DAYSHIFT
//     .Where(d => d.starttime.Date >= startDate.Date && d.endtime.Date <= endDate.Date)
//     .GroupBy(d => true)
//     .Select(g => new
//     {
//         TotalOpeningBalance = g.Sum(d => d.OP_BALANCE),
//         TotalClosingBalance = g.Sum(d => d.CL_BALANCE)
//     })
//     .FirstOrDefault();

//                var salesReturnData = from sr in _connection.Tbl_salesreturndetails
//                                      join pm in _connection.tbl_paymod_master on sr.paymode equals pm.Id
//                                      where sr.invdate.Date >= startDate.Date && sr.invdate.Date <= endDate.Date
//                                      && string.IsNullOrEmpty(sr.cancel_sts)
//                                          && sr.paymode != 3

//                                      group sr by sr.invdate.Date into g
//                                      select new
//                                      {
//                                          InvDate = g.Key,
//                                          TotalReturnAmount = g.Sum(x => (decimal?)x.netamount) ?? 0,
//                                          CashSalesReturn = g.Where(x => x.paymode == 1).Sum(x => (decimal?)x.netamount) ?? 0,
//                                          CardSalesReturn = g.Where(x => x.paymode == 5).Sum(x => (decimal?)x.netamount) ?? 0,
//                                          BenefitSalesReturn = g.Where(x => x.paymode == 4).Sum(x => (decimal?)x.netamount) ?? 0,
//                                          CreditSalesReturn = g.Where(x => x.paymode == 2).Sum(x => (decimal?)x.netamount) ?? 0
//                                      };

//                var combinedSummary = new
//                {
//                    OtherSalesSummary = otherresult,
//                    AlterationSummary = alterresult,
//                    Mainsalesummary = mainresult,
//                    Quotationsummary = quotresult,
//                    SalesReturnSummary = salesReturnData.ToList(),
//                    obcb = balance,
//                    Employeesummary = Employeesummary.ToList(),
//                    CashOut = cashInData,
//                    Cashin = cashOutData,
//                    ItemCategorySummary = categoryresult.ToList(),
//                };

//                return Ok(combinedSummary);

//            }
        }

        }


