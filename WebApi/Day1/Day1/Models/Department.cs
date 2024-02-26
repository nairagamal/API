using System.Text.Json.Serialization;

namespace Day1.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Location { get; set; }
        public string? MngName { get; set; }
       // [JsonIgnore]
        public virtual ICollection<Student>? Students { get; set; }  
        // Corrected property name to pluralize


    }
}
