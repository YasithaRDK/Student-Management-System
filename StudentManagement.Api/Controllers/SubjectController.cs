using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
using StudentManagement.Api.Data.Dtos.ResponseDtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public SubjectController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Get all subject
        [HttpGet]
        public async Task<IActionResult> GetAllSubject()
        {
            var subjects = await _dataContext.Subjects
            .Select(s => new SubjectResponseDto
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
            })
            .ToListAsync();
            return Ok(subjects);
        }

        // Get a subject by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById([FromRoute] int id)
        {
            var subject = await _dataContext.Subjects
            .Where(s => s.SubjectId == id)
            .Select(s => new SubjectResponseDto
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
            })
            .FirstOrDefaultAsync();

            if (subject == null)
            {
                return NotFound(new { message = $"Subject with ID {id} not found." });
            }

            return Ok(subject);
        }

        // Create a new subject
        [HttpPost]
        public async Task<IActionResult> CreateSubject(SubjectRequestDto subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Subject req = new Subject
            {
                SubjectName = subject.SubjectName,
            };

            await _dataContext.Subjects.AddAsync(req);

            await _dataContext.SaveChangesAsync();

            return StatusCode(201, new { message = "Subject created successfully" });
        }

        // Update a subject
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject([FromRoute] int id, SubjectRequestDto subjectRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = await _dataContext.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null)
            {
                return NotFound(new { message = $"Subject with ID {id} not found." });
            }


            subject.SubjectName = subjectRequest.SubjectName;

            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "Subject updated successfully" });
        }

        // Delete a subject
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] int id)
        {
            var subject = await _dataContext.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null)
            {
                return NotFound(new { message = $"Subject with ID {id} not found." });
            }

            _dataContext.Subjects.Remove(subject);

            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}