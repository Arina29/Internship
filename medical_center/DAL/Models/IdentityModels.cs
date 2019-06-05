using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MED.DAL.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    {
       
        public ApplicationDbContext()
            : base("medicineDb", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            //Database.SetInitializer(new ContextSeed());
        }

        public DbSet<Doctors> Doctor { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientBook> PatientBook { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedule  { get; set; }
        public DbSet<DoctorReview> DoctorReviews { get; set; }
        public DbSet<PatientAnalysis> PatientAnalysis { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}