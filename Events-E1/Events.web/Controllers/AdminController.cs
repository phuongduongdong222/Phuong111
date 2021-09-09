using Events.web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Events.web.Controllers
{
    [MyCustomAuthorize(Roles = "Administrator")] //Set role for admin only
    public class AdminController : BaseController //Announce class 'AdminController' from 'BaseController'
    {
        public ActionResult Trainer() //Publicly get action of 'Trainer' to set in other files
        {
            List<ApplicationUser> Trainers = new List<ApplicationUser>();
            var role = Db.Roles
                .Where(r => r.Name == "Trainer")
                .FirstOrDefault();
            if (role != null)
            {
                var users = Db.Users
                    .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                    .ToList();
                return View(users);
            }
            return View();
        }

        public ActionResult TrainingStaff() //Publicly get action of 'TrainingStaff' to set in other files
        {
            List<ApplicationUser> Trainers = new List<ApplicationUser>();
            var role = Db.Roles
                .Where(r => r.Name == "TrainingStaff")
                .FirstOrDefault();
            if (role != null)
            {
                var users = Db.Users
                    .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                    .ToList();
                return View(users);
            }
            return View();
        }

        [HttpGet] //Send data using a query string
        public ActionResult Edit(string id) //Publicly get action of 'Edit' to set in other files
        {
            var accountToEdit = this.Db.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (accountToEdit == null)
            {
                var PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                return this.Redirect(PreviousUrl);
            }
            return this.View(accountToEdit);
        }

        [HttpPost] //Request method via Http page
        public ActionResult Edit(string id, ApplicationUser user) //Get 'Edit' action to set in other files
        {
            var accountToEdit = this.Db.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (accountToEdit == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (user != null && this.ModelState.IsValid)
            {
                accountToEdit.FullName = user.FullName;
                accountToEdit.Age = user.Age;
                accountToEdit.DateofBirth = user.DateofBirth;
                accountToEdit.Education = user.Education;
                accountToEdit.ToeicScore = user.ToeicScore;
                accountToEdit.Location = user.Location;

                this.Db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return this.View(user);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            var user = Db.Users.Find(id);
            if (user == null)
            {
                return Redirect(url);
            }
            Db.Users.Remove(user);
            var assign = Db.CoursesAssigned
                .Where(a => a.UserId == id)
                .ToList();
            if (assign != null)
            {
                foreach (var item in assign)
                {
                    Db.CoursesAssigned.Remove(item);
                }
            }
            Db.SaveChanges();
            return Redirect(url);
        }

        public ActionResult Role() //Publicly get 'Role' action to detail the role list
        {
            var Roles = Db.Roles.ToList();
            return View(Roles);
        }

        public ActionResult RoleCreate() //Publicly get 'RoleCreate' action to create new roles
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost] //Request method via Http page
        public ActionResult RoleCreate(IdentityRole Role) //Get 'RoleCreate' action to save into database
        {
            Db.Roles.Add(Role);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}