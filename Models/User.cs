namespace Vista_Subdivision
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public string Role { get; set; } = null!; // Admin, Homeowner, Subdivision Official
        public string? Language { get; set; }
        public string? Timezone { get; set; } 
    }
}