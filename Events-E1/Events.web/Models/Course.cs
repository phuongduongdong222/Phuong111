using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public CourseCategory CourseCategory { get; set; }

        /*public IEnumerable<ApplicationUser> Users { get; set; }*/
    }
}