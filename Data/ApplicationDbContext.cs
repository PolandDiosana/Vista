using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<EventCalendar> EventCalendars { get; set; }
        public DbSet<FacilityReservation> FacilityReservations { get; set; }
    }
}
