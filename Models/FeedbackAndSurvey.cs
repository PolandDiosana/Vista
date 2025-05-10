using System.ComponentModel.DataAnnotations;

namespace Vista_Subdivision.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime SubmissionDate { get; set; }
    }

    public class Poll
    {
        public int PollId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ExpirationDate { get; set; }

        public List<PollOption> Options { get; set; }
    }

    public class PollOption
    {
        public int PollOptionId { get; set; }

        [Required]
        public string Text { get; set; }

        public int PollId { get; set; }
        public Poll Poll { get; set; }
    }
}