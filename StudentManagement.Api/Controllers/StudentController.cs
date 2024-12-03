using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Models;
using StudentManagement.Api.Data.Dtos.ResponseDtos;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public StudentController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Get all students
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _dataContext.Students.ToListAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching students.", details = ex.Message });
            }
        }

        // Get a student by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            try
            {
                var student = await _dataContext.Students.FirstOrDefaultAsync(i => i.StudentId == id);

                if (student == null)
                {
                    return NotFound(new { message = $"Student with ID {id} not found." });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the student.", details = ex.Message });
            }
        }

        // Create a new student
        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentRequestDto student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Student req = new Student
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    ContactPerson = student.ContactPerson,
                    ContactNo = student.ContactNo,
                    EmailAddress = student.EmailAddress,
                    DateOfBirth = student.DateOfBirth,
                    ClassroomId = student.ClassroomId
                };

                await _dataContext.Students.AddAsync(req);

                await _dataContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStudentById), new { id = req.StudentId }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the student.", details = ex.Message });
            }
        }

        // Update a student
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, StudentRequestDto studentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var student = await _dataContext.Students.FirstOrDefaultAsync(i => i.StudentId == id);

                if (student == null)
                {
                    return NotFound(new { message = $"Student with ID {id} not found." });
                }


                student.FirstName = studentRequest.FirstName;
                student.LastName = studentRequest.LastName;
                student.ContactPerson = studentRequest.ContactPerson;
                student.ContactNo = studentRequest.ContactNo;
                student.EmailAddress = studentRequest.EmailAddress;
                student.DateOfBirth = studentRequest.DateOfBirth;
                student.ClassroomId = studentRequest.ClassroomId;

                await _dataContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the student.", details = ex.Message });
            }
        }

        // Delete a student
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _dataContext.Students.FirstOrDefaultAsync(i => i.StudentId == id);

                if (student == null)
                {
                    return NotFound(new { message = $"Student with ID {id} not found." });
                }

                _dataContext.Students.Remove(student);

                _dataContext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the student.", details = ex.Message });
            }
        }
    }
}