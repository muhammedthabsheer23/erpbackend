using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tbl_user
    {
        [Key]
        [ScaffoldColumn(false)]
        [Exclude]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Employeeid { get; set; }
        [Required]
        public string Salesseries { get; set; }
        [Required]
        public string Salesretseries { get; set; }
        [Required]
        public int Counter { get; set;}
        [Required]
        public string Location { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public string Isdayend { get; set; }
        [Required]
        public string Active { get; set; }
        [Required]
        public string Discount { get; set; }
        [Required]
        public string Setmaster { get; set; }
    }
}
