namespace Vista_Subdivision
{
    public class EmergencyContact
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? ContactName { get; set; }
        public string? Relationship { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } 
    }
}