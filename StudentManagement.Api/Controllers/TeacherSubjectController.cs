using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
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
            var allocateSubjects = await _dataContext.TeacherSubjects.ToListAsync();
            return Ok(allocateSubjects);
        }

        // Get a allocateSubject by ID
        [HttpGet("{teacherId}/{subjectId}")]
        public async Task<IActionResult> GetAllocateSubjectById([FromRoute] int teacherId, int subjectId)
        {
            var allocateSubject = await _dataContext.TeacherSubjects.FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);

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

            return CreatedAtAction(nameof(GetAllocateSubjectById), new { teacherId = req.TeacherId, subjectId = req.SubjectId }, teacherSubject);
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