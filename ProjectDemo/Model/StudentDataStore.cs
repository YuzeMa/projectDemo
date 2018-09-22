//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;

//namespace ProjectDemo.Model
//{
//    public class StudentDataStore:IStudentDataStore
//    {
//        private SchoolDBContext _schoolDBContext;

//        public StudentDataStore(SchoolDBContext schoolDBContext)
//        {
//            _schoolDBContext = schoolDBContext;
//        }

//        IEnumerable<Student> IStudentDataStore.GetAllStudents()
//        {
//            return _schoolDBContext.Students
//                                   //.Include(b=>b.StudentsCourses)
//                                   //.ThenInclude(c=>c.CourseDto)
//                                   .ToList();
//        }

//        public Student GetStudentById(int studentId)
//        {
//            return _schoolDBContext.Students.Find(studentId);
//        }

//        public void AddStudent(Student student)
//        {
//            _schoolDBContext.Students.Add(student);
//            _schoolDBContext.SaveChanges();
//        }

//        public Student UpdateStudent(int studentId,Student student)
//        {
//            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
//            selectedStudent.FullName = student.FullName;
//            _schoolDBContext.SaveChanges();
//            return _schoolDBContext.Students.Find(studentId);
//        }

//        bool IStudentDataStore.DeleteStudent(int studentId)
//        {
//            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
//            bool result = false;
//            if (selectedStudent != null)
//            {
//                _schoolDBContext.Students.Remove(selectedStudent);
//                _schoolDBContext.SaveChanges();
//                result = true;
//            }
//            return result;
//        }
//    }
//}
