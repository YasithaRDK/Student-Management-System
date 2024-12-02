using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact No must be a 10-digit number")]
        public string ContactNo { get; set; } // Contact No *

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public ICollection<TeacherClassroom> TeacherClassrooms { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}