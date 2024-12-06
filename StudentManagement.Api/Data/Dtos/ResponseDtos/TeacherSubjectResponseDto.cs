using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Data.Dtos.ResponseDtos
{
    public class TeacherSubjectResponseDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public List<SubjectResponseDto> Subjects { get; set; }
    }
}