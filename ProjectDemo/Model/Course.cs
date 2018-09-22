using System;
using System.Collections.Generic;

namespace ProjectDemo.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }

        public ICollection<LecturersCourses> LecturersCourses { get; set; }
        public ICollection<StudentsCourses> StudentsCourses { get; set; }
       
    }
}
