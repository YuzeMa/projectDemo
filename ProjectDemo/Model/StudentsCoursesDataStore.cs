//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;

//namespace ProjectDemo.Model
//{
//    public class StudentsCoursesDataStore : IStudentsCoursesDataStore
//    {
//        private SchoolDBContext _schoolDBContext;

//        public StudentsCoursesDataStore(SchoolDBContext schoolDBContext)
//        {
//            _schoolDBContext = schoolDBContext;
//        }

//        public bool AddStudentToCourse(int studentId, int courseId)
//        {
//            bool result = false;
//            var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
//            foreach (StudentsCourses studentcourse in studentsCourses)
//            {
//                if (studentcourse.StudentId == studentId)
//                {
//                    if (studentcourse.CourseId == courseId)
//                    {
//                        return false;
//                    }
//                }
//            }

//            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
//            if (selectedStudent != null)
//            {
//                if (_schoolDBContext.Courses.Find(courseId) != null)
//                {
//                    _schoolDBContext.StudentsCourses.Add(new StudentsCourses()
//                    {
//                        StudentId = studentId,
//                        CourseId = courseId
//                    });
//                    _schoolDBContext.SaveChanges();
//                    result = true;
//                }
//            }
//            return result;
//        }

//        public bool DeleteStudentFromCourse(int studentId, int courseId)
//        {
//            bool result = false;
//            var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
//            foreach(StudentsCourses studentcourse in studentsCourses)
//            {
//                if (studentcourse.StudentId == studentId)
//                {
//                    if (studentcourse.CourseId == courseId)
//                    {
//                        _schoolDBContext.StudentsCourses.Remove(studentcourse);
//                        result = true;
//                    }
//                }
//            } 
//            _schoolDBContext.SaveChanges();
//            return result;
//        }

//        IEnumerable<Course> IStudentsCoursesDataStore.GetCoursesByStudentId(int studentId)
//        {
//            List<Course> result = new List<Course>();
//            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
//            if (selectedStudent != null)
//            {
//                var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
//                foreach (StudentsCourses studentcourse in studentsCourses)
//                {
//                    if (studentcourse.StudentId == studentId)
//                    {
//                        result.Add(new Course()
//                        {
//                            Id = studentcourse.CourseId,
//                            CourseName = _schoolDBContext.Courses.Find(studentcourse.CourseId).CourseName
//                        });
//                    }
//                }
//                return result.ToList();
//            }
//            return null;
//        }

//        IEnumerable<Student> IStudentsCoursesDataStore.GetStudentsByCourseId(int courseId)
//        {
//            List<Student> result = new List<Student>();
//            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
//            if (selectedCourse != null)
//            {
//                var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
//                foreach (StudentsCourses studentcourse in studentsCourses)
//                {
//                    if (studentcourse.CourseId == courseId)
//                    {
//                        result.Add(new Student()
//                        {
//                            Id = studentcourse.StudentId,
//                            FullName = _schoolDBContext.Students.Find(studentcourse.StudentId).FullName
//                        });
//                    }
//                }
//                return result.ToList();
//            }
//            return null;
//        }
//    }
//}
