namespace Vista_Subdivision.Models
{
    public class UserRegistration
    {
        // User
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Profile { get; set; }

        // Property
        public string? LotNumber { get; set; }
        public string? BlockNumber { get; set; }
        public string? HouseType { get; set; }
        public string? SubdivisionName { get; set; }
        public string? CityMunicipality { get; set; }
        public string? StreetName { get; set; }
        public string? Province { get; set; }
        public string? Phase { get; set; }
    }
}
