using Day1.DTOs;
using Day1.Entity;
using Day1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly CompanyContext std;

        public DepartmentController(CompanyContext _std)
        {
            std = _std;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var departments = std.Department.ToList();
            if (departments is null)
            {
                return NotFound();
            }
            return Ok(departments);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
           // var department = std.Department.Find(id);
            var department = std.Department.Include(e=> e.Students).FirstOrDefault(d=>d.Id == id);
            if (department is null)
            {
                return NotFound(new { msg = $"Department with id {id} is not found" });
            }
          //  DeptWithStdsName deptDto = new DeptWithStdsName();
            DeptWithStdsName deptDTO = new DeptWithStdsName();
            deptDTO.Department_Number = department.Id;
            deptDTO.Department_Name = department.Name;
           // deptDTO.Department_Manger = department.MgrName;
            foreach (var e in department.Students)
            {
                deptDTO.Students_Names.Add(e.Name);
            }
            return Ok(new { msg = $"Department with id {id} is found", department = deptDTO });
        }

        [HttpGet]
        [Route("ByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var department = std.Department.Include(e => e.Students).FirstOrDefault(d => d.Name == name);
            if (department is null)
            {
                return NotFound(new { msg = $"Department with name {name} is not found" });
            }

            var deptDTO = new DeptWithStdsName
            {
                Department_Number = department.Id,
                Department_Name = department.Name
            };

            foreach (var e in department.Students)
            {
                deptDTO.Students_Names.Add(e.Name);
            }

            return Ok(new { msg = $"Department with name {name} is found", department = deptDTO });
        }

        [HttpPost]
        public IActionResult Add([FromBody] Department department)
        {
            std.Department.Add(department);
            std.SaveChanges();

            return Ok(new { msg = "Department added successfully", department });
        }


        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int id, [FromBody] Department updatedDepartment)
        {
            var department = std.Department.Find(id);
            if (department is null)
            {
                return NotFound(new { msg = $"Department with id {id} is not found" });
            }

            department.Name = updatedDepartment.Name;
            department.Location = updatedDepartment.Location;
            department.MngName = updatedDepartment.MngName;
            // Update other properties as needed

            std.SaveChanges();

            return Ok(new { msg = $"Department with id {id} updated successfully", department });
        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var department = std.Department.Find(id);
            if (department is null)
            {
                return NotFound(new { msg = $"Department with id {id} is not found" });
            }

            std.Department.Remove(department);
            std.SaveChanges();

            return Ok(new { msg = $"Department with id {id} deleted successfully" });
        }

    }
}
