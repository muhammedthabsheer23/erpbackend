using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tbl_Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Mob { get; set; }
        public string Email { get; set; }
        public string Area { get; set; }
        public double? Salary { get; set; } // Nullable type
        public string Status { get; set; }
        public string Commision { get; set; } // Fixed typo from 'commision'
        public int? Customer { get; set; } // Nullable type
        public string Allprivillage { get; set; }
        public int? LedgerId { get; set; } // Nullable type
        public double? CreditLimit { get; set; } // Nullable type
        public string InsuranceNo { get; set; } // Fixed typo from 'insuranceno'
        public decimal Salesman { get; set; }

    }
}
