using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vista_Subdivision.Models
{
    public class FacilityReservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public required string Facility { get; set;}

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
