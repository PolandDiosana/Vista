using System.ComponentModel.DataAnnotations;

namespace Vista_Subdivision.Models
{
    public class Security
    {
        [Key]
        public int SecurityID { get; set; }
        [Required]
        public int HomeownerID { get; set; }
        public required string VisitorName { get; set; }
        public string? VehicleInfo { get; set; }
        public DateOnly EntryDate { get; set; }
        public TimeOnly EntryTime { get; set; }
        public DateOnly? ExitDate { get; set; }
        public TimeOnly? ExitTime { get; set; }
    }
}
