using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Data.Dtos.ResponseDtos
{
    public class TeacherResponseDto
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public List<SubjectResponseDto> Subjects { get; set; }
    }
}