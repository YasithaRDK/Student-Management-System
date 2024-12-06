using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Data.Dtos.ResponseDtos
{
    public class TeacherSpecResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Subjects { get; set; }
    }
}