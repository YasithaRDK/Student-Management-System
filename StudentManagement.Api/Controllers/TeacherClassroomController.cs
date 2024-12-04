using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/allocate-classroom")]
    public class TeacherClassroomController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public TeacherClassroomController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Get all allocateClassroom
        [HttpGet]
        public async Task<IActionResult> GetAllAllocateClassroom()
        {
            var allocateClassrooms = await _dataContext.TeacherClassrooms.ToListAsync();
            return Ok(allocateClassrooms);
        }

        // Get a allocateClassroom by ID
        [HttpGet("{teacherId}/{classroomId}")]
        public async Task<IActionResult> GetAllocateClassroomById([FromRoute] int teacherId, int classroomId)
        {
            var allocateClassroom = await _dataContext.TeacherClassrooms.FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.ClassroomId == classroomId);

            if (allocateClassroom == null)
            {
                return NotFound();
            }

            return Ok(allocateClassroom);
        }

        // Create a new allocateClassroom
        [HttpPost]
        public async Task<IActionResult> CreateAllocateClassroom(TeacherClassroomRequestDto teacherClassroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeacherClassroom req = new TeacherClassroom
            {
                TeacherId = teacherClassroom.TeacherId,
                ClassroomId = teacherClassroom.ClassroomId,
            };

            await _dataContext.TeacherClassrooms.AddAsync(req);

            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllocateClassroomById), new { teacherId = req.TeacherId, classroomId = req.ClassroomId }, teacherClassroom);
        }

        // Delete a allocateClassroom
        [HttpDelete("{teacherId}/{classroomId}")]
        public async Task<IActionResult> DeleteClassroom([FromRoute] int teacherId, int classroomId)
        {
            var teacherClassroomDetails = await _dataContext.TeacherClassrooms.FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.ClassroomId == classroomId);

            if (teacherClassroomDetails == null)
            {
                return NotFound();
            }

            _dataContext.TeacherClassrooms.Remove(teacherClassroomDetails);

            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}