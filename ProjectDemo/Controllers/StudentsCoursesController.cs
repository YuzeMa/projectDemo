using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDemo.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectDemo.Controllers
{
    [Route("api/StudentsCourses")]
    public class StudentsCoursesController : Controller
    {
        private IStudentsCoursesDataStore _studentsCoursesDataStore;

        public StudentsCoursesController(IStudentsCoursesDataStore studentsCoursesDataStore)
        {
            _studentsCoursesDataStore = studentsCoursesDataStore;
        }


        [HttpPost("{studentid}/{courseid}")]
        public IActionResult AddStudentToCourse(int studentid, int courseid)
        {
            var result =_studentsCoursesDataStore.AddStudentToCourse(studentid, courseid);
            if (result)
            {
                return Ok("Success Add");
            }

            return NotFound();

        }

        [HttpDelete("{studentid}/{courseid}")]
        public IActionResult DeleteStudentFromCourse(int studentid, int courseid)
        {
            var result = _studentsCoursesDataStore.DeleteStudentFromCourse(studentid, courseid);
            if (result)
            {
                return Ok("Success Delete");
            }

            return NotFound();

        }

        [HttpGet("getCourses/{studentid}")]
        public IActionResult GetCoursesByStudentId(int studentid)
        {
            var result = _studentsCoursesDataStore.GetCoursesByStudentId(studentid);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("getStudents/{courseid}")]
        public IActionResult GetStudentsByCourseId(int courseid)
        {
            var result = _studentsCoursesDataStore.GetStudentsByCourseId(courseid);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
