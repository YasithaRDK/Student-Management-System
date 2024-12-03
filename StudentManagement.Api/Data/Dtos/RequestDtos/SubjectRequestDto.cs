using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Data.Dtos.RequestDtos
{
    public class SubjectRequestDto
    {
        [Required(ErrorMessage = "Subject Name is required")]
        [MaxLength(100, ErrorMessage = "Subject Name cannot exceed 100 characters.")]
        public string SubjectName { get; set; }
    }
}