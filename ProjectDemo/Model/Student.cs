using System;
using System.Collections.Generic;

namespace ProjectDemo.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public ICollection<StudentsCourses> StudentsCourses { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public int Age { get; set; }
        //public string Gender { get; set; }
        //public int CreditLeft { get; set; }

    }
}
