namespace Vista_Subdivision
{
    public class EventCalendar
    {
        public int EventID { get; set; }
        public User User { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? EventDate { get; set; }
        public int OrganizerID { get; set; }
    }
}
