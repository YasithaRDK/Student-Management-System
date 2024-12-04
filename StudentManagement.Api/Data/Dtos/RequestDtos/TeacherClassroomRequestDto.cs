using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Api.Data.Dtos.RequestDtos
{
    public class TeacherClassroomRequestDto
    {
        [Required(ErrorMessage = "Teacher Id is required")]
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Classroom Id is required")]
        [ForeignKey("Classroom")]
        public int ClassroomId { get; set; }
    }
}