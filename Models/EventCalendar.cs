using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Vista_Subdivision
{
    public class EventCalendar
    {
        [Key]
        public int EventID { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? EventDate { get; set; }
        public string? EventTime { get; set; }
        public string? EventType { get; set; }
        public int OrganizerID { get; set; }
    }
}
