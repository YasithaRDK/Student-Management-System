using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using StudentManagement.Api.Data.Dtos.ResponseDtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/allocate-subject")]
    public class TeacherSubjectController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public TeacherSubjectController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Get all allocateSubject
        [HttpGet]
        public async Task<IActionResult> GetAllAllocateSubject()
        {
            var allocateSubjects = await _dataContext.TeacherSubjects
            .GroupBy(tc => new { tc.TeacherId, tc.Teacher.FirstName, tc.Teacher.LastName })
            .Select(group => new TeacherSubjectResponseDto
            {
                TeacherId = group.Key.TeacherId,
                TeacherName = group.Key.FirstName + " " + group.Key.LastName,
                Subjects = group.Select(tc => new SubjectResponseDto
                {
                    SubjectId = tc.Subject.SubjectId,
                    SubjectName = tc.Subject.SubjectName
                }).ToList()
            })
            .ToListAsync();
            return Ok(allocateSubjects);
        }

        // Get a allocateSubject by ID
        [HttpGet("{teacherId}")]
        public async Task<IActionResult> GetAllocateSubjectById([FromRoute] int teacherId)
        {
            var allocateSubject = await _dataContext.TeacherSubjects
            .GroupBy(tc => new { tc.TeacherId, tc.Teacher.FirstName, tc.Teacher.LastName })
            .Select(group => new TeacherSubjectResponseDto
            {
                TeacherId = group.Key.TeacherId,
                TeacherName = group.Key.FirstName + " " + group.Key.LastName,
                Subjects = group.Select(tc => new SubjectResponseDto
                {
                    SubjectId = tc.Subject.SubjectId,
                    SubjectName = tc.Subject.SubjectName
                }).ToList()
            })
            .FirstOrDefaultAsync(ts => ts.TeacherId == teacherId);

            if (allocateSubject == null)
            {
                return NotFound();
            }

            return Ok(allocateSubject);
        }

        // Create a new allocateSubject
        [HttpPost]
        public async Task<IActionResult> CreateAllocateSubject(TeacherSubjectRequestDto teacherSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeacherSubject req = new TeacherSubject
            {
                TeacherId = teacherSubject.TeacherId,
                SubjectId = teacherSubject.SubjectId,
            };

            await _dataContext.TeacherSubjects.AddAsync(req);

            await _dataContext.SaveChangesAsync();

            return StatusCode(201, new { message = "The subject has been successfully assigned to the teacher" });
        }

        // Delete a allocateSubject
        [HttpDelete("{teacherId}/{subjectId}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] int teacherId, int subjectId)
        {
            var teacherSubjectDetails = await _dataContext.TeacherSubjects.FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);

            if (teacherSubjectDetails == null)
            {
                return NotFound();
            }

            _dataContext.TeacherSubjects.Remove(teacherSubjectDetails);

            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}