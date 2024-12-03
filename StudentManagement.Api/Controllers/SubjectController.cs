using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Data.Dtos.RequestDtos;
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
            var subjects = await _dataContext.Subjects.ToListAsync();
            return Ok(subjects);
        }

        // Get a subject by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById([FromRoute] int id)
        {
            var subject = await _dataContext.Subjects.FirstOrDefaultAsync(i => i.SubjectId == id);

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

            return CreatedAtAction(nameof(GetSubjectById), new { id = req.SubjectId }, subject);
        }

        // Update a subject
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject([FromRoute] int id, SubjectRequestDto subjectRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = await _dataContext.Subjects.FirstOrDefaultAsync(i => i.SubjectId == id);

            if (subject == null)
            {
                return NotFound(new { message = $"Subject with ID {id} not found." });
            }


            subject.SubjectName = subjectRequest.SubjectName;

            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubjectById), new { id = subject.SubjectId }, subject);
        }

        // Delete a subject
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] int id)
        {
            var subject = await _dataContext.Subjects.FirstOrDefaultAsync(i => i.SubjectId == id);

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