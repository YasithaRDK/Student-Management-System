using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Subject Name is required")]
        [MaxLength(100, ErrorMessage = "Subject Name cannot exceed 100 characters.")]
        public string SubjectName { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}