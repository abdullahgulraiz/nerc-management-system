using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace NERCManagementSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new NercDBInitializer());
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberType> MemberTypes { get; set; }
        public DbSet<OrganizerType> OrganizerTypes { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<OrganizerTask> OrganizerTasks { get; set; }
    }

    public class NercDBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            /* Create roles */
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roles = new List<IdentityRole>();
            var adminRole = new IdentityRole("Admin");
            roles.Add(adminRole); roles.Add(new IdentityRole("Organizer")); roles.Add(new IdentityRole("Team"));
            foreach (var role in roles)
            {
                roleManager.Create(role);
            }
            /* Create an admin user */
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            var admin = new ApplicationUser() { UserName = "admin", PasswordHash = (new PasswordHasher()).HashPassword("nerc") };
            userManager.Create(admin);
            userManager.AddToRole(admin.Id, adminRole.Name);
            /* Create Organizer Tasks */
            var tasks = new List<OrganizerTask>();
            tasks.Add(new OrganizerTask { Name = "Web & IT" }); tasks.Add(new OrganizerTask { Name = "Human Resources" });
            foreach (var task in tasks)
            {
                context.OrganizerTasks.Add(task);
            }
            /* Create Organizer Types */
            var types = new List<OrganizerType>();
            types.Add(new OrganizerType { Name = "Organizer" }); types.Add(new OrganizerType { Name = "Faculty" });
            types.Add(new OrganizerType { Name = "Conducting Student" });
            foreach (var type in types)
            {
                context.OrganizerTypes.Add(type);
            }
        }
    }
}