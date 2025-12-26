using TimeSheets.Models.Enums;

namespace TimeSheets.Dto
{
    public class SubjectImportDto
    {
        public string Name { get; set; } = null!;
        public SubjectType Type { get; set; }
    }
}
