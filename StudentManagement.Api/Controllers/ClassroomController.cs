using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using StudentManagement.Api.Data.Dtos.ResponseDtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/classroom")]
    public class ClassroomController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public ClassroomController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Get all classrooms
        [HttpGet]
        public async Task<IActionResult> GetAllClassrooms()
        {
            var classrooms = await _dataContext.Classrooms
            .Select(c => new ClassroomResponseDto
            {
                ClassroomId = c.ClassroomId,
                ClassroomName = c.ClassroomName
            }).ToListAsync();
            return Ok(classrooms);
        }

        // Get a classroom by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassroomById([FromRoute] int id)
        {
            var classroom = await _dataContext.Classrooms
            .Where(c => c.ClassroomId == id)
            .Select(c => new ClassroomResponseDto
            {
                ClassroomId = c.ClassroomId,
                ClassroomName = c.ClassroomName
            }).FirstOrDefaultAsync();

            if (classroom == null)
            {
                return NotFound(new { message = $"Classroom with ID {id} not found." });
            }

            return Ok(classroom);
        }

        // Create a new classroom
        [HttpPost]
        public async Task<IActionResult> CreateClassroom(ClassroomRequestDto classroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Classroom req = new Classroom
            {
                ClassroomName = classroom.ClassroomName,
            };

            await _dataContext.Classrooms.AddAsync(req);

            await _dataContext.SaveChangesAsync();

            return StatusCode(201, new { message = "Classroom created successfully" });
        }

        // Update a classroom
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassroom([FromRoute] int id, ClassroomRequestDto classroomRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classroom = await _dataContext.Classrooms.FirstOrDefaultAsync(c => c.ClassroomId == id);

            if (classroom == null)
            {
                return NotFound(new { message = $"Classroom with ID {id} not found." });
            }


            classroom.ClassroomName = classroomRequest.ClassroomName;

            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "Classroom updated successfully" });
        }

        // Delete a classroom
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom([FromRoute] int id)
        {
            var classroom = await _dataContext.Classrooms.FirstOrDefaultAsync(i => i.ClassroomId == id);

            if (classroom == null)
            {
                return NotFound(new { message = $"Classroom with ID {id} not found." });
            }

            _dataContext.Classrooms.Remove(classroom);

            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}