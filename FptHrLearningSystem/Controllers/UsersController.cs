using FptHrLearningSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FptHrLearningSystem.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            //check admin 
            var context = new ApplicationDbContext();
            var username = User.Identity.Name;
            ApplicationUser userProfile = null;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                      ViewBag.displayMenu = "Yes";
                    if (!string.IsNullOrEmpty(username))
                    {
                        var user = context.Users.SingleOrDefault(u => u.UserName == username);
                        ViewBag.Birthday = user.Birthday;
                        ViewBag.LastName = user.LastName;
                        ViewBag.FirstName = user.FirstName;
                        ViewBag.Usertype = user.UserType;
                        ViewBag.PhoneNumber = user.PhoneNumber;
                        ViewBag.Email = user.Email;
                        ViewBag.Address = user.Address;
                        ViewBag.Gender = user.Gender;
                        ViewBag.Id = user.Id;
                        userProfile = user;
                    }

                    return View(userProfile);
                }
            }
            return View();

            /*return View();*/

        }
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin" || s[0].ToString() == "Trainee"|| s[0].ToString() == "Trainer"|| s[0].ToString() == "Staff")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}