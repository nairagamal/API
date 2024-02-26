using Day1.Models;
using Microsoft.EntityFrameworkCore;
namespace Day1.Entity
{
    public class CompanyContext :DbContext
    {
        public CompanyContext()
        {
            
        }
        public CompanyContext(DbContextOptions option):base(option) 
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Department { get; set; }
    }
}
