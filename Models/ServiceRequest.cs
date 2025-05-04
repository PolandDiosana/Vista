using System.ComponentModel.DataAnnotations;

namespace Vista_Subdivision.Models
{
    public class ServiceRequest
    {
        [Key]
        public int RequestID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter a request title")]
        public string ReqTitle { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Street name is required")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Priority level is required")]
        public string PriorityLevel { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        public string DateSubmitted { get; set; }

        public string Status { get; set; }
    }
}
