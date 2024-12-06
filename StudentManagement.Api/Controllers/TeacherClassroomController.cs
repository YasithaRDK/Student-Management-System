using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using StudentManagement.Api.Data.Dtos.ResponseDtos;
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
            var teacherClassrooms = await _dataContext.TeacherClassrooms
            .GroupBy(tc => new { tc.TeacherId, tc.Teacher.FirstName, tc.Teacher.LastName })
            .Select(group => new TeacherClassroomResponseDto
            {
                TeacherId = group.Key.TeacherId,
                TeacherName = group.Key.FirstName + " " + group.Key.LastName,
                Classrooms = group.Select(tc => new ClassroomResponseDto
                {
                    ClassroomId = tc.Classroom.ClassroomId,
                    ClassroomName = tc.Classroom.ClassroomName
                }).ToList()
            })
            .ToListAsync();

            return Ok(teacherClassrooms);
        }


        // Get a allocateClassroom by ID
        [HttpGet("{teacherId}")]
        public async Task<IActionResult> GetAllocateClassroomById([FromRoute] int teacherId)
        {
            var allocateClassroom = await _dataContext.TeacherClassrooms
            .Where(ts => ts.TeacherId == teacherId)
            .GroupBy(tc => new { tc.TeacherId, tc.Teacher.FirstName, tc.Teacher.LastName })
            .Select(group => new TeacherClassroomResponseDto
            {
                TeacherId = group.Key.TeacherId,
                TeacherName = group.Key.FirstName + " " + group.Key.LastName,
                Classrooms = group.Select(tc => new ClassroomResponseDto
                {
                    ClassroomId = tc.Classroom.ClassroomId,
                    ClassroomName = tc.Classroom.ClassroomName
                }).ToList()
            })
            .FirstOrDefaultAsync();

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

            return StatusCode(201, new { message = "The classroom has been successfully assigned to the teacher" });
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