using System;
using System.Collections.Generic;

namespace ProjectDemo.Model
{
    public interface ICourseDataStore
    {
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int courseId);
        string AddCourse(Course course);
        Course UpdateCourse(int courseId, Course course);
        bool DeleteCourse(int courseId);
        void AddLecturer(Lecturer lecturer);
    }

    public interface IStudentDataStore
    {
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int studentId);
        void AddStudent(Student student);
        Student UpdateStudent(int studentId, Student student);
        bool DeleteStudent(int studentId);
    }

    public interface IStudentsCoursesDataStore
    {
        bool AddStudentToCourse(int studentid, int courseid);
        bool DeleteStudentFromCourse(int studentId, int courseId);
        IEnumerable<Course> GetCoursesByStudentId(int studentId);
        IEnumerable<Student> GetStudentsByCourseId(int courseId);
    }

    public interface ILecturerDataStore
    {
        IEnumerable<Lecturer> GetAllLecturers();
        Lecturer GetLecturerById(int lecturerId);
        void AddLecturer (Lecturer lecturer);
        string AddLecturerToCourse(int courseId, int lecturerId);
        bool DeleteLecturer(int lecturerId);
        bool RemoveLecturerFromCourse(int courseId, int lecturerId);

    }

}
