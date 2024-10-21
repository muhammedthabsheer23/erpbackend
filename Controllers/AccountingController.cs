using Loginproject.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Loginproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountingController : ControllerBase
    {
        private readonly Dbconnection _connection;

        public AccountingController(Dbconnection connection)
        {
            _connection = connection;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PAYMODTS>>> Getpaymodts()
        {
            if (_connection.tbl_paymod_master == null)
            {
                return NotFound();
            }
            return await _connection.tbl_paymod_master.ToListAsync();
        }
        [HttpGet]
        [Route("getaccount")]
        public async Task<ActionResult<IEnumerable<Tbl_Account>>> Getaccounts()
        {
            if (_connection.tblAccounts == null)
            {
                return NotFound();
            }
            return await _connection.tblAccounts.ToListAsync();


        }



        [HttpPost]
        [Route("Tbl_user")]
        public async Task<ActionResult<Tbl_Username>> Create(Tbl_Username tbl_User)
        {
            _connection.Tbl_user.Add(tbl_User);
            await _connection.SaveChangesAsync();
            return tbl_User;

        }
        [HttpPost]
        [Route("Tblemployee")]
        public async Task<ActionResult<Tbl_Employee>> Createemployee(Tbl_Employee tbl_Employee)
        {
            _connection.Tbl_employee.Add(tbl_Employee);
            await _connection.SaveChangesAsync();
            return tbl_Employee;

        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(Login log)
        {
            var tempUser = _connection.Tbl_user.FirstOrDefault(u => u.User_Name == log.Username && u.pwd == log.Password && u.Active == "active" && u.Branch ==log.Branch);
            if (tempUser != null)
            {
                var token = CreateJwtToken();
                return Ok(new { status = true, Message = "sucess", Data = new { Token = token } });
            }
            return BadRequest();
        }
        private string CreateJwtToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"1"),
                new Claim(JwtRegisteredClaimNames.Email,"thabsheer"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom secret key for authentication"));
            var token = new JwtSecurityToken(
                issuer: "https://example.com",
                audience: "https://example.com",
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet]
        [Route("counter")]
        public async Task<ActionResult<IEnumerable<Countersettings>>> Getcounter()
        {
            if (_connection.countersettings == null)
            {
                return NotFound();
            }
            return await _connection.countersettings.ToListAsync();


        }
        [HttpGet]
        [Route("dayshift")]
        public async Task<ActionResult<IEnumerable<Dayshift>>> Getdayshift()
        {
            if (_connection.DAYSHIFT == null)
            {
                return NotFound();
            }
            return await _connection.DAYSHIFT.ToListAsync();


        }

        [HttpGet]
        [Route("tblsalesdetail")]
        public async Task<ActionResult<IEnumerable<Tblsaledetail>>> Gettblsalesdetail()
        {
            if (_connection.Tbl_salesdetails == null)
            {
                return NotFound();
            }
            return await _connection.Tbl_salesdetails.ToListAsync();

            //var Sale = await _connection.tblSalesdetails.ToListAsync();
            //return Sale;
        }

        [HttpPost]
        [Route("tbldetails")]
        public async Task<ActionResult<Tblsaledetail>> Createtblsale(Tblsaledetail tblsaledetail)
        {
            _connection.Tbl_salesdetails.Add(tblsaledetail);
            await _connection.SaveChangesAsync();
            return tblsaledetail;

        }
        //[HttpGet("summarytransaction")]
        //public IActionResult GetTransactionSummary(int transactionid, DateTime date)
        //{
        //    var result = _connection.Tbl_transactions
        //        .Where(t => t.Trans_id == transactionid && t.date==date && t.narration != null && t.narration != "")
        //        .GroupBy(t => t.narration)
        //        .Select(g => new Multipaymode
        //        {
        //            Amount = g.Sum(t => t.amount_rs),
        //            Count = g.Count(),
        //            Narration = g.Key
        //        })
        //        .ToList();

        //    return Ok(result);
        //}
        [HttpGet]
        [Route("customer")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            var customerList = await (from ts in _connection.Tbl_salesdetails

                                      join c in _connection.tbl_paymod_master on ts.paymode equals c.Id
                                      join ac in _connection.tblAccounts on ts.cust_id equals ac.id

                                      select new Customer()
                                      {
                                          sno = ts.sno,
                                          netamount = ts.netamount,
                                          invdate = ts.invdate,
                                          custname = ts.custname,
                                          paymode = c.Name,
                                          Salesman = ts.Salesman,
                                          PId = c.Id,
                                          cust_id = ac.id,

                                      }).ToListAsync();

            return Ok(customerList);
        }


        private static string GetPaymodeName(int paymodeId)
        {
            return paymodeId switch
            {
                1 => "CASH",
                2 => "CREDIT",
                3 => "MULTIPAYMODE",
                4 => "BENEFIT PAY",
                5 => "VISA CARD",
                _ => "UNKNOWN"
            };
        }

        private static string GetPaymodeName(string paymodeId)
        {
            var paymodeNames = new Dictionary<string, string>
        {
            { "1", "Cash" },
            { "2", "Card" },
            { "3", "Online" }
            // Add other mappings as necessary
        };

            return paymodeNames.TryGetValue(paymodeId, out var name) ? name : "Unknown";
        }
        [HttpGet]
        [Route("sales-summary")]
        public async Task<ActionResult<IEnumerable<Salessummary>>> GetSalesSummary(int dayid, int shiftid, int userid, int counter, int location)
        {
            var salesDetails = await _connection.Tbl_salesdetails
                .Where(sd => sd.dayid == dayid
                             && sd.shiftid == shiftid
                             && sd.userid == userid
                             && sd.counter == counter
                             && sd.Location == location
                             && sd.cancel_flg == "")
                .GroupBy(sd => sd.paymode)
                .Select(g => new
                {
                    PaymodeId = g.Key,
                    Amount = g.Sum(sd => sd.netamount),
                    Count = g.Count() // Invoke Count() method
                })
                .ToListAsync();

            var multiPaymodeDetails = salesDetails
                .Where(sd => GetPaymodeName(sd.PaymodeId) == "MULTIPAYMODE")
                .Select(sd => new
                {
                    sd.PaymodeId,
                    sd.Amount,
                    sd.Count
                })
                .ToList();



            var combinedSalesSummary = salesDetails.Select(sd => new Salessummary
            {
                Paymode = GetPaymodeName(sd.PaymodeId),
                Amount = sd.Amount,
                Count = sd.Count
            })

            .Union(multiPaymodeDetails.Select(mp => new Salessummary
            {
                Paymode = "MULTIPAYMODE",
                Amount = mp.Amount,
                Count = mp.Count
            }))
            .ToList();

            return Ok(combinedSalesSummary);
        }


        private static string GetPaymodereturnNames(int paymodeId)
        {
            return paymodeId switch
            {
                1 => "CASH",
                2 => "CREDIT",
                3 => "MULTIPAYMODE",
                4 => "BENEFIT PAY",
                5 => "VISA CARD",
                _ => "UNKNOWN"
            };
        }

        private static string GetPaymodereturnName(int paymodeId)
        {
            return paymodeId switch
            {
                1 => "cash",
                2 => "card",
                _ => "unknown"
            };
        }

        [HttpGet]
        [Route("salesreturn")]
        public async Task<ActionResult<IEnumerable<Salesreturnsummary>>> GetSalesReturn(int dayid, int shiftid, int userid, int counter, int location)
        {
            var salesreturn = await _connection.Tbl_salesreturndetails
                .Where(sr => sr.dayid == dayid
                             && sr.shiftid == shiftid
                             && sr.userid == userid
                             && sr.counter == counter
                             && sr.location == location
                             && sr.cancel_sts == "")
                .GroupBy(sr => sr.paymode)
                .Select(gs => new
                {
                    Paymode = GetPaymodereturnName(gs.Key),
                    Amount = gs.Sum(sr => sr.netamount),
                    Count = gs.Count()
                })
                .ToListAsync();

            var salesreturnsummary = salesreturn.Select(sr => new Salesreturnsummary
            {
                Paymode = sr.Paymode,
                //Amount = sr.Amount,
                Count = sr.Count
            });

            return Ok(salesreturnsummary);
        }

        [HttpGet]
        [Route("dayendreport")]
        public async Task<ActionResult<IEnumerable<Dayendreport>>> Getdayend()
        {
            try
            {
                var dayendList = await (from ts in _connection.DAYSHIFT
                                        join c in _connection.Tbl_user on ts.USERID equals c.Id
                                        join ac in _connection.countersettings on ts.counter equals ac.id
                                        join tl in _connection.Tbl_location on ts.Branch equals tl.LocationId
                                        join sd in _connection.Tbl_salesdetails on ts.DAYID equals sd.dayid
                                        group new { ts, c, ac, tl, sd } by new
                                        {
                                            ts.DAYID,
                                            ts.SHIFTID,
                                            ts.USERID,
                                            c.User_Name,
                                            ac.id,
                                            ac.counter,
                                            tl.LocationId,
                                            tl.Location,
                                            ts.starttime,
                                            ts.endtime,
                                            sd.Transactionid,
                                            sd.invdate,
                                            sd.Invtype
                                        } into grouped

                                        select new Dayendreport()
                                        {
                                            Id = grouped.Key.USERID,
                                            Name = grouped.Key.User_Name,
                                            DAYID = grouped.Key.DAYID,
                                            SHIFTID = grouped.Key.SHIFTID,
                                            counterid = grouped.Key.id,
                                            countername = grouped.Key.counter,
                                            LocationId = grouped.Key.LocationId,
                                            starttime = grouped.Key.starttime.ToString("yyyy/MM/dd hh:mm tt"),
                                            endtime = grouped.Key.endtime.ToString("yyyy/MM/dd hh:mm tt"),
                                            transactionid = grouped.Key.Transactionid,
                                            invdate = grouped.Key.invdate,
                                            invtypeid = grouped.Key.Invtype,
                                            Location = grouped.Key.Location,
                                        }).ToListAsync();

                if (dayendList == null || dayendList.Count == 0)
                {
                    return NotFound("No day-end reports found.");
                }

                return Ok(dayendList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //       public async Task<ActionResult<IEnumerable<Dayendreport>>> Getdayend()
        //{
        //    try
        //    {
        //        var dayendList = await (from ts in _connection.DAYSHIFT
        //                                join c in _connection.Tbl_user on ts.USERID equals c.Id
        //                                join ac in _connection.countersettings on ts.counter equals ac.id
        //                                join tl in _connection.Tbl_location on ts.Branch equals tl.LocationId
        //                                join sd in _connection.Tbl_salesdetails on ts.DAYID equals sd.dayid
        //                                select new Dayendreport()
        //                                {
        //                                    Id = c.Id,
        //                                    Name = c.User_Name,
        //                                    DAYID = ts.DAYID,
        //                                    SHIFTID = ts.SHIFTID,
        //                                    counterid = ac.id,
        //                                    countername = ac.counter,
        //                                    LocationId = tl.LocationId,
        //                                    starttime = ts.starttime.ToString("MM/dd/yyyy hh:mm tt"), // Format to show date and time with AM/PM
        //                                    endtime = ts.endtime.ToString("MM/dd/yyyy hh:mm tt"),     // Format to show date and time with AM/PM
        //                                    transactionid = sd.Transactionid,
        //                                    invdate = sd.invdate,
        //                                    invtypeid = sd.Invtype,
        //                                    Location = tl.Location,
        //                                }).ToListAsync();

        //        if (dayendList == null || dayendList.Count == 0)
        //        {
        //            return NotFound("No day-end reports found.");
        //        }

        //        return Ok(dayendList);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        [HttpGet("counters")]
        public async Task<IActionResult> GetCounters()
        {
            var counters = await _connection.countersettings.Select(c => new
            {
                c.id
                ,
                c.counter
            }).ToListAsync();
            return Ok(counters);
        }
        [HttpGet]
        [Route("dayday")]
        public IActionResult GetSalesSummary(DateTime startDate, DateTime endDate)
        {
            // Validate the provided dates
            if (startDate == default(DateTime) || endDate == default(DateTime) || startDate > endDate)
            {
                return BadRequest("Invalid start date or end date.");
            }

            // SalesData grouped by date (ignoring time)
            var salesData = (from s in _connection.Tbl_salesdetails
                             join pm in _connection.tbl_paymod_master on s.paymode equals pm.Id
                             where s.invdate >= startDate && s.invdate <= endDate && s.paymode != 3
                             group s by s.invdate.Date into g
                             select new
                             {
                                 InvDate = g.Key,
                                 TotalNetAmount = g.Sum(x => x.netamount),
                                 CashSales = g.Sum(x => x.paymode == 1 ? x.netamount : 0),
                                 CreditSales = g.Sum(x => x.paymode == 2 ? x.netamount : 0),
                                 BenefitSales = g.Sum(x => x.paymode == 4 ? x.netamount : 0),
                                 CardSales = g.Sum(x => x.paymode == 5 ? x.netamount : 0)
                             }).ToList();

            // TransactionData grouped by date (ignoring time)
            var transactionData = (from tt in _connection.Tbl_transactions
                                   join sd in _connection.Tbl_salesdetails on tt.Trans_id equals sd.Transactionid
                                   where sd.paymode == 3 && tt.date >= startDate && tt.date <= endDate
                                         && !string.IsNullOrEmpty(tt.narration)
                                         && new[] { "CASH", "CREDIT", "BENEFIT", "CARD" }.Contains(tt.narration)
                                   group tt by sd.invdate.Date into g
                                   select new
                                   {
                                       InvDate = g.Key,
                                       TotalNetAmount = g.Sum(x => Convert.ToDecimal(x.amount_rs)),  // Convert to decimal
                                       AdditionalCashSales = g.Sum(x => x.narration == "CASH" ? Convert.ToDecimal(x.amount_rs) : 0),  // Convert to decimal
                                       AdditionalCreditSales = g.Sum(x => x.narration == "CREDIT" ? Convert.ToDecimal(x.amount_rs) : 0),  // Convert to decimal
                                       AdditionalBenefitSales = g.Sum(x => x.narration == "BENEFIT" ? Convert.ToDecimal(x.amount_rs) : 0),  // Convert to decimal
                                       AdditionalCardSales = g.Sum(x => x.narration == "CARD" ? Convert.ToDecimal(x.amount_rs) : 0)  // Convert to decimal
                                   }).ToList();

            // SalesReturnData grouped by date (ignoring time)
            var salesReturnData = (from sr in _connection.Tbl_salesreturndetails
                                   join pm in _connection.tbl_paymod_master on sr.paymode equals pm.Id
                                   where sr.invdate >= startDate && sr.invdate <= endDate && sr.paymode != 3 && sr.cancel_sts == ""
                                   group sr by sr.invdate.Date into g
                                   select new
                                   {
                                       InvDate = g.Key,
                                       TotalReturnAmount = g.Sum(x => x.netamount),
                                       CashSalesReturn = g.Sum(x => x.paymode == 1 ? x.netamount : 0),
                                       CreditSalesReturn = g.Sum(x => x.paymode == 2 ? x.netamount : 0),
                                       BenefitSalesReturn = g.Sum(x => x.paymode == 4 ? x.netamount : 0),
                                       CardSalesReturn = g.Sum(x => x.paymode == 5 ? x.netamount : 0)
                                   }).ToList();

            // Combine results by date
            var result = from s in salesData
                         join t in transactionData on s.InvDate equals t.InvDate into tempT
                         from t in tempT.DefaultIfEmpty()
                         join sr in salesReturnData on s.InvDate equals sr.InvDate into tempSR
                         from sr in tempSR.DefaultIfEmpty()
                         select new
                         {
                             InvDate = s.InvDate.ToString("yyyy-MM-dd"), // Format date as string (optional)
                             NetTotalAmount = (s.TotalNetAmount as decimal? ?? 0M) + (t?.TotalNetAmount ?? 0M) - (sr?.TotalReturnAmount ?? 0M),
                             CashSales = (s.CashSales as decimal? ?? 0M) + (t?.AdditionalCashSales ?? 0M) - (sr?.CashSalesReturn ?? 0M),
                             CreditSales = (s.CreditSales as decimal? ?? 0M) + (t?.AdditionalCreditSales ?? 0M) - (sr?.CreditSalesReturn ?? 0M),
                             BenefitSales = (s.BenefitSales as decimal? ?? 0M) + (t?.AdditionalBenefitSales ?? 0M) - (sr?.BenefitSalesReturn ?? 0M),
                             CardSales = (s.CardSales as decimal? ?? 0M) + (t?.AdditionalCardSales ?? 0M) - (sr?.CardSalesReturn ?? 0M)
                         };

            return Ok(result);
        }

    }
}


