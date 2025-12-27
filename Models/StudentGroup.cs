using System.Text.Json.Serialization;

namespace TimeSheets.Models
{
    public class StudentGroup
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int StudentCount { get; set; }
    }
}
