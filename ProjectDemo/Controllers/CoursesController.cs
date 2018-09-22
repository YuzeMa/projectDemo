using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDemo.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectDemo.Controllers
{
    [Route("api/Courses")]
    public class CoursesController : Controller
    {
        private ICourseDataStore _courseDataStore;

        public CoursesController(ICourseDataStore courseDataStore)
        {
            _courseDataStore = courseDataStore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _courseDataStore.GetAllCourses();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _courseDataStore.GetCourseById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Course value)
        {
            string result = _courseDataStore.AddCourse(value);
            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Course value)
        {
            Course resultCourse = _courseDataStore.UpdateCourse(id, value);
            if(resultCourse != null)
            {
                return Accepted(resultCourse);
            }
            else
            {
                return NotFound();
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _courseDataStore.DeleteCourse(id);

            return result ? NoContent() : (IActionResult)NotFound();
        }
    }
}
