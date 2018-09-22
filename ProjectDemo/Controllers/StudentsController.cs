using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDemo.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectDemo.Controllers
{
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        private IStudentDataStore _studentDataStore;

        public StudentsController(IStudentDataStore studentDataStore)
        {
            _studentDataStore = studentDataStore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _studentDataStore.GetAllStudents();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _studentDataStore.GetStudentById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Student value)
        {
            _studentDataStore.AddStudent(value);
            return Ok("Success");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Student value)
        {
            Student resultStudent = _studentDataStore.UpdateStudent(id, value);
            if (resultStudent != null)
            {
                return Accepted(resultStudent);
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
            bool result = _studentDataStore.DeleteStudent(id);

            return result ? NoContent() : (IActionResult)NotFound();
        }
    }
}