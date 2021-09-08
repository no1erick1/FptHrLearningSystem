using FptHrLearningSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(FptHrLearningSystem.Startup))]
namespace FptHrLearningSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        //private void createRolesandUsers()
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        //    // In Startup iam creating first Admin Role and creating a default Admin User
        //    if (!roleManager.RoleExists("Admin"))
        //    {

        //        // first we create Admin rool
        //        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //        role.Name = "Admin";
        //        roleManager.Create(role);

        //        //Here we create a Admin super user who will maintain the website                        
        //        var user = new ApplicationUser();
        //        user.UserName = "Duong";
        //        user.Email = "thanhduong@gmail.com";

        //        string userPWD = "Hades281220@";
        //        var chkUser = UserManager.Create(user, userPWD);

        //        //Add default User to Role Admin
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = UserManager.AddToRole(user.Id, "Admin");
        //        }
        //    }

        //}

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "admin@fpt.edu.vn";
                user.Email = "admin@fpt.edu.vn";
                user.UserType = "Admin";
                user.FirstName = "Huỳnh";
                user.LastName = "Thanh Dương";

                string userPWD = "Admin2812@";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Staff"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Staff";
                roleManager.Create(role);
                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "staff@fpt.edu.vn";
                user.Email = "staff@fpt.edu.vn";
                user.UserType = "Staff";
                user.FirstName = "Huỳnh";
                user.LastName = "Thanh Hải";
                string userPWD = "Staff2812@";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Staff");

                }
            }

            // creating Creating Employee role     
            if (!roleManager.RoleExists("Trainee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Trainee";
                roleManager.Create(role);
                //Here we create a Admin super user who will maintain the website                   

                var user = new Trainee();
                user.UserName = "trainee@fpt.edu.vn";
                user.Email = "trainee@fpt.edu.vn";
                user.UserType = "Trainee";
                user.FirstName = "Huỳnh";
                user.LastName = "Thanh Phát";
                string userPWD = "Trainee2812@";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Trainee");

                }

            }
            // creating Creating Employee role     
            if (!roleManager.RoleExists("Trainer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Trainer";
                roleManager.Create(role);
                //Here we create a Admin super user who will maintain the website                   

                var user = new Trainer();
                user.UserName = "trainer@fpt.edu.vn";
                user.Email = "trainer@fpt.edu.vn";
                user.UserType = "Trainer";
                user.FirstName = "Huỳnh";
                user.LastName = "Thanh Tùng";
                string userPWD = "Trainer2812@";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Trainer");

                }
            }
        }
    }
}
