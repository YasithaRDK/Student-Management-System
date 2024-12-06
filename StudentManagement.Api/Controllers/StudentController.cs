using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Models;
using StudentManagement.Api.Data.Dtos.ResponseDtos;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/student")]
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
            var students = await _dataContext.Students
            .Select(s => new StudentResponseDto
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                ContactPerson = s.ContactPerson,
                ContactNo = s.ContactNo,
                EmailAddress = s.EmailAddress,
                DateOfBirth = s.DateOfBirth,
                Age = s.Age,
                ClassroomName = s.Classroom.ClassroomName
            }).ToListAsync();
            return Ok(students);
        }

        // Get a student by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            var student = await _dataContext.Students
            .Where(s => s.StudentId == id)
            .Select(s => new StudentSingleResponseDto
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                ContactPerson = s.ContactPerson,
                ContactNo = s.ContactNo,
                EmailAddress = s.EmailAddress,
                DateOfBirth = s.DateOfBirth,
                Age = s.Age,
                ClassroomName = s.Classroom.ClassroomName,
                Teachers = s.Classroom.TeacherClassrooms
                    .Select(tc => new TeacherResponseDto
                    {
                        FirstName = tc.Teacher.FirstName,
                        LastName = tc.Teacher.LastName,
                        Subjects = tc.Teacher.TeacherSubjects
                            .Select(s => new SubjectResponseDto
                            {
                                SubjectName = s.Subject.SubjectName
                            }).ToList()
                    }).ToList()
            })
            .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found." });
            }

            return Ok(student);
        }

        // Create a new student
        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentRequestDto student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            return StatusCode(201, new { message = "Student created successfully" });
        }

        // Update a student
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, StudentRequestDto studentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _dataContext.Students
            .FirstOrDefaultAsync(s => s.StudentId == id);

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

            return Ok(new { message = "Student updated successfully" });
        }

        // Delete a student
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var student = await _dataContext.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found." });
            }

            _dataContext.Students.Remove(student);

            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}