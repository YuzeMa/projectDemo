using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDemo.Model;
using Microsoft.AspNetCore.Mvc;

namespace ProjectDemo.Controllers
{
    [Route("api/Lecturer")]
    public class LecturersController : Controller
    {
        private ILecturerDataStore _lecturerDataStore;

        public LecturersController(ILecturerDataStore lecturerDataStore)
        {
            _lecturerDataStore = lecturerDataStore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _lecturerDataStore.GetAllLecturers();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _lecturerDataStore.GetLecturerById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddLecturer(Lecturer lecturer)
        {
            _lecturerDataStore.AddLecturer(lecturer);
            return Ok("success");
        }

        [HttpPost("{courseId}/{lecturerId}")]
        public IActionResult AddLecturerToCourse(int courseId, int lecturerId)
        {
            var result = _lecturerDataStore.AddLecturerToCourse(courseId, lecturerId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // DELETE api/values/5
        [HttpDelete("{lecturerid}")]
        public IActionResult DeleteLecturer(int lecturerid)
        {
            bool result = _lecturerDataStore.DeleteLecturer(lecturerid);
            return result ? NoContent() : (IActionResult)NotFound();
        }

        [HttpDelete("{courseId}/{lecturerId}")]
        public IActionResult RemoveLecturerFromCourse(int courseId, int lecturerId)
        {
            bool result = _lecturerDataStore.RemoveLecturerFromCourse(courseId, lecturerId);
            return result ? NoContent() : (IActionResult)NotFound();
        }
    }
}
