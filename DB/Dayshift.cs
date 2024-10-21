using System.ComponentModel.DataAnnotations;

namespace Loginproject.DB
{
    public class Dayshift
    {
        [Key]
        public Int64 DAYID { get; set; }
        public int  SHIFTID {  get; set; }
        public int USERID { get; set; }
        public double OP_BALANCE { get; set; }
        public double CL_BALANCE { get; set; }
        public double DIFFERENCE { get; set; }
        public string Status { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int counter { get; set; }
        public int Branch { get; set; }
    }
}
