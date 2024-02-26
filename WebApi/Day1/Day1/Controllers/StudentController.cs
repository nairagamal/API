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
    public class StudentController : ControllerBase
    {
        private readonly CompanyContext std;
        public StudentController(CompanyContext _std)
        {
            std = _std;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = std.Students.Include(e => e.Department).ToList();

            if (students == null || students.Count == 0)
            {
                return NotFound();
            }

            List<StdWithDept> studentsWithDeptDTO = students.Select(student => new StdWithDept
            {
                Student_Age = student.Age.ToString(),
                Student_Name = student.Name,
                Student_Address = student.Address,
                Student_Department = student.Department?.Name
            }).ToList();

            return Ok(studentsWithDeptDTO);

        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetbyId(int id)
        {
            //var Student = std.Students.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            var Student = std.Students.Include(e => e.Department).FirstOrDefault(e =>e.Id== id);
            StdWithDept stdDTO = new StdWithDept();
            stdDTO.Student_Department = Student.Department.Name;
            stdDTO.Student_Name = Student.Name;
            stdDTO.Student_Address = Student.Address;
            stdDTO.Student_Age = Student.Age;
            return Ok(stdDTO);
        }


        [HttpGet]
        [Route("/GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var student = std.Students.Include(e => e.Department).FirstOrDefault(s => s.Name == name);

            if (student == null)
            {
                return NotFound(new { msg = $"Student with name {name} is not found" });
            }

            // You can map the result to a DTO if needed
            // For simplicity, returning the student with department info
            return Ok(new
            {
                msg = $"Student with name {name} is found",
                student = new
                {
                    student.Id,
                    student.Name,
                    student.Address,
                    student.Age,
                    Department = student.Department.Name
                }
            });
        }


        
        [HttpPost]
        public IActionResult Add(Student mm)
        {
            if (ModelState.IsValid)
            {
                // Create a new student with only the necessary properties
                Student newStudent = new Student
                {
                    Name = mm.Name,
                    Address = mm.Address,
                    Age = mm.Age,
                    DeptId = mm.DeptId // Assuming DepartmentId is the property you want to use
                };

                std.Students.Add(newStudent);
                std.SaveChanges();

                // You may need to reload the student with the department after save
                var addedStudent = std.Students.Include(e => e.Department).FirstOrDefault(e => e.Id == newStudent.Id);

                return CreatedAtAction(actionName: "GetbyId", routeValues: new { id = newStudent.Id }, addedStudent);
            }

            return BadRequest(new { msg = "Invalid student data" });

        }

        [HttpPut]
      
        public IActionResult Edit(Student mm)
        {
            if (ModelState.IsValid)
            {
                // Find the existing student by Id
                var existingStudent = std.Students.Include(e => e.Department).FirstOrDefault(e => e.Id == mm.Id);

                if (existingStudent == null)
                {
                    return NotFound(new { msg = $"Student with ID {mm.Id} not found" });
                }

                // Update only the necessary properties
                existingStudent.Name = mm.Name;
                existingStudent.Address = mm.Address;
                existingStudent.Age = mm.Age;
                existingStudent.DeptId = mm.DeptId; // Assuming DepartmentId is the property you want to use

                std.Students.Update(existingStudent);
                std.SaveChanges();

                // You may need to reload the student with the department after save
                var editedStudent = std.Students.Include(e => e.Department).FirstOrDefault(e => e.Id == mm.Id);

                // Shape the response using StdWithDept DTO
                StdWithDept stdDTO = new StdWithDept
                {
                    Student_Age = editedStudent.Age.ToString(),
                    Student_Name = editedStudent.Name,
                    Student_Address = editedStudent.Address,
                    Student_Department = editedStudent.Department?.Name
                };

                return Ok(stdDTO);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
     
        public IActionResult Delete(int id)
        {
            var studentToDelete = std.Students.Include(e => e.Department).FirstOrDefault(e => e.Id == id);

            if (studentToDelete == null)
            {
                return NotFound(new { msg = $"Student with ID {id} not found" });
            }

            std.Students.Remove(studentToDelete);
            std.SaveChanges();

            StdWithDept stdDTO = new StdWithDept
            {
                Student_Age = studentToDelete.Age.ToString(),
                Student_Name = studentToDelete.Name,
                Student_Address = studentToDelete.Address,
                Student_Department = studentToDelete.Department?.Name
            };

            return Ok(new { msg = $"Student with ID {id} deleted successfully", deletedStudent = stdDTO });




        }


    }
}
