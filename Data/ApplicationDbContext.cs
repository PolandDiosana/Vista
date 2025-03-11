using Microsoft.EntityFrameworkCore;

namespace Vista_Subdivision.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    }
}
