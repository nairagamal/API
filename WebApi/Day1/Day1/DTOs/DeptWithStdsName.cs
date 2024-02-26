namespace Day1.DTOs
{
    public class DeptWithStdsName
    {
        public int Department_Number { get; set; }
        public string Department_Name { get; set; }
        public List<string> Students_Names { get; set; } = new List<string>();
    }
}
