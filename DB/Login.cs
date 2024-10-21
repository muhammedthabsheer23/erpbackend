using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Login
    {
  
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public int Branch {  get; set; }
    }
}
