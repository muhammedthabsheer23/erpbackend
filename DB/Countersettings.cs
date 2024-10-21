using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Countersettings
    {
        [Key]
        public int id {  get; set; }
        public string counter { get; set; }
        public int series{ get; set; }
    }
}
