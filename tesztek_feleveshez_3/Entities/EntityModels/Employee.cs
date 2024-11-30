using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using tesztek_feleveshez_3.Entities.Help;

namespace tesztek_feleveshez_3.Entities.EntityModels
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public int StartYear { get; set; }
        public int CompletedProjects { get; set; }
        public bool Active { get; set; }
        public bool Retired { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public string Level { get; set; }
        public decimal Salary { get; set; }
        public Commission Commission { get; set; }


        public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
    }

    public class Department
    {
        [Key]
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string HeadOfDepartment { get; set; }

        public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
    }

    public class EmployeeDepartment : IIdenitiy
    {
        [Key]
        public string Id { get; set; }

        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string DepartmentCode { get; set; }
        public Department Department { get; set; }
    }
    public class Commission
    {
        public decimal Value { get; set; }
        public string Currency { get; set; }
    }

}
