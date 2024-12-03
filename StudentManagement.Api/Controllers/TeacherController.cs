using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public TeacherController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Get all teachers
        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _dataContext.Teachers.ToListAsync();
            return Ok(teachers);
        }

        // Get a teacher by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById([FromRoute] int id)
        {
            var teacher = await _dataContext.Teachers.FirstOrDefaultAsync(i => i.TeacherId == id);

            if (teacher == null)
            {
                return NotFound(new { message = $"Teacher with ID {id} not found." });
            }

            return Ok(teacher);
        }

        // Create a new teacher
        [HttpPost]
        public async Task<IActionResult> CreateTeacher(TeacherRequestDto teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Teacher req = new Teacher
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ContactNo = teacher.ContactNo,
                EmailAddress = teacher.EmailAddress,
            };

            await _dataContext.Teachers.AddAsync(req);

            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacherById), new { id = req.TeacherId }, teacher);
        }

        // Update a teacher
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher([FromRoute] int id, TeacherRequestDto teacherRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacher = await _dataContext.Teachers.FirstOrDefaultAsync(i => i.TeacherId == id);

            if (teacher == null)
            {
                return NotFound(new { message = $"Teacher with ID {id} not found." });
            }


            teacher.FirstName = teacherRequest.FirstName;
            teacher.LastName = teacherRequest.LastName;
            teacher.ContactNo = teacherRequest.ContactNo;
            teacher.EmailAddress = teacherRequest.EmailAddress;

            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
        }

        // Delete a teacher
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] int id)
        {
            var teacher = await _dataContext.Teachers.FirstOrDefaultAsync(i => i.TeacherId == id);

            if (teacher == null)
            {
                return NotFound(new { message = $"Teacher with ID {id} not found." });
            }

            _dataContext.Teachers.Remove(teacher);

            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}