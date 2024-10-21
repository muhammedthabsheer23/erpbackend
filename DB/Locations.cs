using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Locations
    {
        [Key]
        public  Int64 LocationId { get; set; }
        public String Location { get; set; }
        public string Company { get; set; }
        public string Address {  get; set; }
        public int BranchId { get; set; }

    }
}
