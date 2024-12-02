using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Api.Models
{
    public class Classroom
    {
        [Key]
        public int ClassroomId { get; set; }

        [Required(ErrorMessage = "Classroom Name is required")]
        [MaxLength(50, ErrorMessage = "Classroom Name cannot exceed 50 characters.")]
        public string ClassroomName { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<TeacherClassroom> TeacherClassrooms { get; set; }
    }
}