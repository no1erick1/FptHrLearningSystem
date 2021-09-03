using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FptHrLearningSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<ClassProfile> ClassProfiles { get; set; }
        public DbSet<Classroom> classrooms { get; set; }
        public DbSet<TraineeClassroom> TraineeClassrooms { get; set; }
        public DbSet<TrainerCourse> TrainerCourses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }
       

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
       
    }
}