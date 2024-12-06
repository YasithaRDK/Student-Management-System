namespace StudentManagement.Api.Data.Dtos.ResponseDtos
{
    public class TeacherClassroomResponseDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public List<ClassroomResponseDto> Classrooms { get; set; }
    }
}