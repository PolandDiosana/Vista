namespace Vista_Subdivision
{
    public class EmergencyContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!; // Navigation Property
        public string ContactName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}