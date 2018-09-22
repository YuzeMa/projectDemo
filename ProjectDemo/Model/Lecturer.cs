using System;
using System.Collections.Generic;

namespace ProjectDemo.Model
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
       
        public ICollection<LecturersCourses> LecturersCourses { get; set; }
    }
}
