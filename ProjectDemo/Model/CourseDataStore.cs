using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;

//namespace ProjectDemo.Model
//{
//    public class CourseDataStore : ICourseDataStore
//    {
//        private SchoolDBContext _schoolDBContext;

//        public CourseDataStore(SchoolDBContext schoolDBContext)
//        {
//            _schoolDBContext = schoolDBContext;
//        }

//        IEnumerable<Course> ICourseDataStore.GetAllCourses()
//        {
//            return _schoolDBContext.Courses.ToList();
//        }

//        public Course GetCourseById(int courseId)
//        {
//            return _schoolDBContext.Courses.Find(courseId);
//        }

//        public void AddCourse(Course course)
//        {
//            _schoolDBContext.Courses.Add(course);
//            _schoolDBContext.SaveChanges();
//        }

//        public Course UpdateCourse(int courseId, Course course)
//        {
//            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
//            selectedCourse.CourseName = course.CourseName;
//            _schoolDBContext.SaveChanges();
//            return _schoolDBContext.Courses.Find(courseId);
//        }

//        bool ICourseDataStore.DeleteCourse(int courseId)
//        {
//            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
//            bool result = false;
//            if (selectedCourse != null)
//            {
//                _schoolDBContext.Courses.Remove(selectedCourse);
//                _schoolDBContext.SaveChanges();
//                result = true;
//            }
//            return result;
//        }
//    }
//}

