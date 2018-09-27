using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ProjectDemo.Model
{
    public class StudentDataStore : IStudentDataStore
    {
        private SchoolDBContext _schoolDBContext;

        public StudentDataStore(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        IEnumerable<Student> IStudentDataStore.GetAllStudents()
        {
            return _schoolDBContext.Students
                                   //.Include(b=>b.StudentsCourses)
                                   //.ThenInclude(c=>c.CourseDto)
                                   .ToList();
        }

        public Student GetStudentById(int studentId)
        {
            return _schoolDBContext.Students.Find(studentId);
        }

        public void AddStudent(Student student)
        {
            _schoolDBContext.Students.Add(student);
            _schoolDBContext.SaveChanges();
        }

        public Student UpdateStudent(int studentId, Student student)
        {
            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
            selectedStudent.FullName = student.FullName;
            _schoolDBContext.SaveChanges();
            return _schoolDBContext.Students.Find(studentId);
        }

        bool IStudentDataStore.DeleteStudent(int studentId)
        {
            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
            bool result = false;
            if (selectedStudent != null)
            {
                _schoolDBContext.Students.Remove(selectedStudent);
                _schoolDBContext.SaveChanges();
                result = true;
            }
            return result;
        }
    }

    public class CourseDataStore : ICourseDataStore
    {
        private SchoolDBContext _schoolDBContext;

        public CourseDataStore(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public void AddLecturer(Lecturer lecturer)
        {
            _schoolDBContext.Lecturers.Add(lecturer);
            _schoolDBContext.SaveChanges();
        }

        IEnumerable<Course> ICourseDataStore.GetAllCourses()
        {
            return _schoolDBContext.Courses.ToList();
        }

        public Course GetCourseById(int courseId)
        {
            return _schoolDBContext.Courses.Find(courseId);
        }

        public string AddCourse(Course course)
        {

            _schoolDBContext.Courses.Add(course);
            _schoolDBContext.SaveChanges();
            return "success";
        }

        public Course UpdateCourse(int courseId, Course course)
        {
            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
            selectedCourse.CourseName = course.CourseName;
            _schoolDBContext.SaveChanges();
            return _schoolDBContext.Courses.Find(courseId);
        }

        bool ICourseDataStore.DeleteCourse(int courseId)
        {
            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
            bool result = false;
            if (selectedCourse != null)
            {
                _schoolDBContext.Courses.Remove(selectedCourse);
                _schoolDBContext.SaveChanges();
                result = true;
            }
            return result;
        }
    }

    public class LecturerDataStore : ILecturerDataStore
    {
        private SchoolDBContext _schoolDBContext;

        public LecturerDataStore(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        IEnumerable<Lecturer> ILecturerDataStore.GetAllLecturers()
        {
            var result = _schoolDBContext.Lecturers.ToList();
            return result;
        }

        public Lecturer GetLecturerById(int lecturerId)
        {
            var result = _schoolDBContext.Lecturers.Find(lecturerId);
            return result;
        }

        public void AddLecturer(Lecturer lecturer)
        {
             _schoolDBContext.Lecturers.Add(lecturer);
            _schoolDBContext.SaveChanges();
        }

        public string AddLecturerToCourse(int courseId, int lecturerId)
        {
       
            var lecturersCourses = _schoolDBContext.LecturersCourses.ToList();
            foreach(LecturersCourses lecturerCourse in lecturersCourses)
            {
                if(lecturerCourse.CourseId == courseId && lecturerCourse.LecturerId == lecturerId)
                {
                    return null;
                }
            }
            Lecturer selectedLecturer = _schoolDBContext.Lecturers.Find(lecturerId);
            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
            if (selectedLecturer == null || selectedCourse == null)
            {
                return null;
            }

            _schoolDBContext.LecturersCourses.Add(new LecturersCourses()
            {
                LecturerId = lecturerId,
                CourseId = courseId
            });
            _schoolDBContext.SaveChanges();
            return "success";
        }

        public bool DeleteLecturer(int lecturerId)
        {
            Lecturer selectedLecturer = _schoolDBContext.Lecturers.Find(lecturerId);
            bool result = false;
            if (selectedLecturer != null)
            {
                _schoolDBContext.Lecturers.Remove(selectedLecturer);
                _schoolDBContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool RemoveLecturerFromCourse(int courseId, int lecturerId)
        {
           var lecturerCourse = _schoolDBContext.LecturersCourses.Find(courseId, lecturerId);
            if (lecturerCourse == null)
            {
                return false;
            }
            _schoolDBContext.LecturersCourses.Remove(lecturerCourse);
            _schoolDBContext.SaveChanges();
            return true;
        }

    }

    public class StudentsCoursesDataStore : IStudentsCoursesDataStore
    {
        private SchoolDBContext _schoolDBContext;

        public StudentsCoursesDataStore(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public bool AddStudentToCourse(int studentId, int courseId)
        {
            bool result = false;
            var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
            foreach (StudentsCourses studentcourse in studentsCourses)
            {
                if (studentcourse.StudentId == studentId)
                {
                    if (studentcourse.CourseId == courseId)
                    {
                        return result;
                    }
                }
            }

            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
            if (selectedStudent != null)
            {
                if (_schoolDBContext.Courses.Find(courseId) != null)
                {
                    _schoolDBContext.StudentsCourses.Add(new StudentsCourses()
                    {
                        StudentId = studentId,
                        CourseId = courseId
                    });
                    _schoolDBContext.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        public bool DeleteStudentFromCourse(int studentId, int courseId)
        {
            bool result = false;
            var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
            foreach (StudentsCourses studentcourse in studentsCourses)
            {
                if (studentcourse.StudentId == studentId)
                {
                    if (studentcourse.CourseId == courseId)
                    {
                        _schoolDBContext.StudentsCourses.Remove(studentcourse);
                        result = true;
                    }
                }
            }
            _schoolDBContext.SaveChanges();
            return result;
        }

        IEnumerable<Course> IStudentsCoursesDataStore.GetCoursesByStudentId(int studentId)
        {
            List<Course> result = new List<Course>();
            Student selectedStudent = _schoolDBContext.Students.Find(studentId);
            if (selectedStudent != null)
            {
                var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
                foreach (StudentsCourses studentcourse in studentsCourses)
                {
                    if (studentcourse.StudentId == studentId)
                    {
                        result.Add(new Course()
                        {
                            Id = studentcourse.CourseId,
                            CourseName = _schoolDBContext.Courses.Find(studentcourse.CourseId).CourseName
                        });
                    }
                }
                return result.ToList();
            }
            return null;
        }

        IEnumerable<Student> IStudentsCoursesDataStore.GetStudentsByCourseId(int courseId)
        {
            List<Student> result = new List<Student>();
            Course selectedCourse = _schoolDBContext.Courses.Find(courseId);
            if (selectedCourse != null)
            {
                var studentsCourses = _schoolDBContext.StudentsCourses.ToList();
                foreach (StudentsCourses studentcourse in studentsCourses)
                {
                    if (studentcourse.CourseId == courseId)
                    {
                        result.Add(new Student()
                        {
                            Id = studentcourse.StudentId,
                            FullName = _schoolDBContext.Students.Find(studentcourse.StudentId).FullName
                        });
                    }
                }
                return result.ToList();
            }
            return null;
        }
    }
}

