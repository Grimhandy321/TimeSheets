namespace TimeSheets.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }        
    }
}
