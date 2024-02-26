using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Day1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? Address { get; set; }
        public string Image { get; set; }

        [ForeignKey(nameof(Department))]
        public int DeptId { get; set; }
      //  [JsonIgnore]
        public virtual Department? Department { get; set; }

    }
}
