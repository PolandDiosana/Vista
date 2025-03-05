public class Document
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } // Navigation Property
    public string ProofOfOwnership { get; set; }
    public string ValidID { get; set; }
    public string UtilityBill { get; set; }
}
