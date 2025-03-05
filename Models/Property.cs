public class Property
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } // Navigation Property
    public string LotNumber { get; set; }
    public string BlockNumber { get; set; }
    public string HouseType { get; set; }
    public string SubdivisionName { get; set; }
    public string CityMunicipality { get; set; }
    public string StreetName { get; set; }
    public string Province { get; set; }
    public string Phase { get; set; }
}
