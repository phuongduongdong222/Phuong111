using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class CoursesAssigned
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string UserId { get; set; }
    }
}