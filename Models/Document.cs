namespace Vista_Subdivision
{
    public class Document
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!; // Navigation Property
        public string ProofOfOwnership { get; set; } = null!;
        public string ValidID { get; set; } = null!;
        public string UtilityBill { get; set; } = null!;
    }
}
