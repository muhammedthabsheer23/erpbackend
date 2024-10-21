using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Tbl_Username
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        public string User_Name { get; set; }

        [Required]
        public string pwd { get; set; }

        [Required]
        public int Employeeid { get; set; }

        [Required]
        public int Counter { get; set; }

        [Required]
        public int Location { get; set; }

        [Required]
        public int Branch { get; set; }

        [Required]
        public string Active { get; set; }
    }
}
