using Microsoft.AspNetCore.Mvc;
using AspMongoApi.Models;
using AspMongoApi.Services;

namespace AspMongoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly StudentsService _studentsService;

        public StudentsController(StudentsService studentsService) =>
            _studentsService = studentsService;

        [HttpGet]
        public async Task<List<Student>> Get() =>
            await _studentsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Student>> Get(string id) {
            var student = await _studentsService.GetAsync(id);

            if (student is null) {
                return NotFound();
            }

            return student;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Student newStudent) {
            await _studentsService.CreateAsync(newStudent);

            return CreatedAtAction(nameof(Get), new { id = newStudent.Id }, newStudent);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Student updatedStudent) {
            var student = await _studentsService.GetAsync(id);

            if (student is null) {
                return NotFound();
            }

            updatedStudent.Id = student.Id;

            await _studentsService.UpdateAsync(id, updatedStudent);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id) {
            var student = await _studentsService.GetAsync(id);

            if (student is null) {
                return NotFound();
            }

            await _studentsService.RemoveAsync(student.Id!);

            return NoContent();
        }

    }
}