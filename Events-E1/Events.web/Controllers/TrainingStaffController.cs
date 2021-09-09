using Events.web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Events.web.Controllers
{
    [MyCustomAuthorize(Roles = "TrainingStaff")]
    public class TrainingStaffController : BaseController
    {
        public ActionResult Trainer()
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

        public ActionResult Trainee()
        {
            List<ApplicationUser> Trainees = new List<ApplicationUser>();
            var role = Db.Roles
                .Where(r => r.Name == "Trainee")
                .FirstOrDefault();
            if (role != null)
            {
                var users = Db.Users
                    .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                    .ToList();
                foreach (var user in users)
                {
                    var courseId = Db.CoursesAssigned
                        .Where(c => c.UserId == user.Id)
                        .Select(c => c.CourseId)
                        .ToList();
                    List<Course> x = new List<Course>();
                    foreach (var id in courseId)
                    {
                        var course = Db.Courses
                            .Where(c => c.Id == id)
                            .FirstOrDefault();
                        x.Add(course);
                    }
                    if (x != null)
                    {
                        user.courses = x.AsEnumerable();
                    }
                }
                return View(users);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
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

        [HttpPost]
        public ActionResult Edit(string id, ApplicationUser user)
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

        public ActionResult CourseCategory()
        {
            var coursecategories = Db.CourseCategories
                .OrderBy(e => e.Id)
                .ToList();
            return View(coursecategories);
        }

        public ActionResult CourseCategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CourseCategoryCreate(CourseCategory category)
        {
            Db.CourseCategories.Add(category);
            Db.SaveChanges();
            return RedirectToAction("CourseCategory");
        }

        public ActionResult Course()
        {
            List<Course> courses = new List<Course> { };
            var courses1 = Db.Courses
                .OrderBy(e => e.Id)
                .ToList();
            foreach (var item in courses1)
            {
                var category = Db.CourseCategories
                    .Where(c => c.Id == item.CategoryId)
                    .FirstOrDefault();
                item.CourseCategory = category;
                courses.Add(item);
            }
            return View(courses);
        }

        public ActionResult CourseCreate()
        {
            ViewBag.CourseCategory = new SelectList(Db.CourseCategories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult CourseCreate(Course course)
        {
            if (!ModelState.IsValid)
            {
                return CourseCreate();
            }

            var coursecategory = Db.CourseCategories
                .Where(c => c.Id == course.CategoryId)
                .FirstOrDefault();

            var createdcourse = new Course
            {
                Name = course.Name,
                CategoryId = course.CategoryId,
                CourseCategory = coursecategory,
                Description = course.Description
            };
            Db.Courses.Add(createdcourse);
            Db.SaveChanges();
            return RedirectToAction("Course");
        }

        [HttpGet]
        public ActionResult AssignTrainerToCourse()
        {
            dynamic dy = new ExpandoObject();
            dy.userlist = gettrainers();
            ViewBag.Courses = new SelectList(Db.Courses.ToList(), "Id", "Name");
            return View(dy);
        }

        public List<ApplicationUser> gettrainers()
        {
            var role = Db.Roles
                .Where(r => r.Name == "Trainer")
                .FirstOrDefault();
            List<ApplicationUser> users = Db.Users
                .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                .ToList();
            return users;
        }

        public List<Course> getcourses()
        {
            List<Course> courses = Db.Courses.ToList();
            return courses;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTrainerToCourse(string[] user , int courseid)
        {
            if (!ModelState.IsValid)
            {
                return Trainer();
            }
            foreach (var u in user )
            {
                var x = Db.CoursesAssigned
                    .Where(a => a.UserId == u && a.CourseId == courseid)
                    .FirstOrDefault();

                if (x != null)
                {
                    TempData["error"] = "Some users is already assign to this course";
                    return RedirectToAction("AssignTrainerToCourse");
                }
                else
                {
                    var assigned = new CoursesAssigned
                    {
                        UserId = u,
                        CourseId = courseid,

                    };
                    Db.CoursesAssigned.Add(assigned);
                    Db.SaveChanges();
                }
            }
            return RedirectToAction("Trainer");
        }

        [HttpGet]
        public ActionResult AssignTraineeToCourse()
        {
            dynamic dy = new ExpandoObject();
            dy.userlist = gettrainees();
            ViewBag.Courses = new SelectList(Db.Courses.ToList(), "Id", "Name");
            return View(dy);
        }

        public List<ApplicationUser> gettrainees()
        {
            var role = Db.Roles
                .Where(r => r.Name == "Trainee")
                .FirstOrDefault();
            List<ApplicationUser> users = Db.Users
                .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                .ToList();
            return users;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTraineeToCourse(string[] user, int courseid)
        {
            if (!ModelState.IsValid)
            {
                return Trainee();
            }
            foreach (var u in user)
            {
                var x = Db.CoursesAssigned
                    .Where(a => a.UserId == u && a.CourseId == courseid)
                    .FirstOrDefault();

                if (x != null)
                {
                    TempData["error"] = "Some users is already assign to this course";
                    return RedirectToAction("AssignTraineeToCourse");
                }
                else
                {
                    var assigned = new CoursesAssigned
                    {
                        UserId = u,
                        CourseId = courseid,

                    };
                    Db.CoursesAssigned.Add(assigned);
                    Db.SaveChanges();
                }
            }
            return RedirectToAction("Trainee");
        }
    }
}