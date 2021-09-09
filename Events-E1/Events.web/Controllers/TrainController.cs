using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;
using Microsoft.AspNet.Identity;

namespace Events.web.Controllers
{
    [MyCustomAuthorize(Roles = "Trainer , Trainee")]
    public class TrainController : BaseController
    {
        
        public ActionResult CourseAssigned()
        {
            List<Course> courses = new List<Course> { };
            var user = this.User.Identity.GetUserId();
            var course = Db.CoursesAssigned
                .Where(c => c.UserId == user)
                .Select(c => c.CourseId)
                .ToList();
            foreach (var id in course)
            {
                var c = Db.Courses
                    .Where(a => a.Id == id)
                    .FirstOrDefault();
                courses.Add(c);
            }
            return View(courses);
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
    }
}