using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Data.Dtos.RequestDtos
{
    public class ClassroomRequestDto
    {
        [Required(ErrorMessage = "Classroom Name is required")]
        [MaxLength(50, ErrorMessage = "Classroom Name cannot exceed 50 characters.")]
        public string ClassroomName { get; set; }
    }
}