using System.ComponentModel.DataAnnotations;

namespace Vista_Subdivision.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementID { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateOnly? DatePosted { get; set; }  // This is required!

        [Required]
        public int AuthorID { get; set; }
    }
}
