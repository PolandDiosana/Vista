namespace Vista_Subdivision
{
    public class Property
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!; // Navigation Property
        public string LotNumber { get; set; } = null!;
        public string BlockNumber { get; set; } = null!;
        public string HouseType { get; set; } = null!;
        public string SubdivisionName { get; set; } = null!;
        public string CityMunicipality { get; set; } = null!;
        public string StreetName { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Phase { get; set; } = null!;
    }
}