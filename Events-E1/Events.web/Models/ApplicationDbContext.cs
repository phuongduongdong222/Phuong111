using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
//using Events.Data;

namespace Events.web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Course> Courses { get; set; }

        public IDbSet<CourseCategory> CourseCategories { get; set; }

        public IDbSet<CoursesAssigned> CoursesAssigned { get; set; }
        //public IDbSet<Event> Events { get; set; }
        //public IDbSet<Comment> Comments { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}