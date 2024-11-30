using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using tesztek_feleveshez_3.Data;
using tesztek_feleveshez_3.Entities.EntityModels;
using tesztek_feleveshez_3.Entities.Help;
using static System.Net.Mime.MediaTypeNames;

namespace tesztek_feleveshez_3.Repository
{
    public class EmployeeLogic
    {
        EmployeeDbContext context;
        public EmployeeLogic(EmployeeDbContext context) 
        {
            this.context = context;
        }
        public void EmployeeImport(string xmlFilePath)
        {
            XDocument xmlDoc = XDocument.Load(xmlFilePath);

            foreach (var employee in xmlDoc.Descendants("Employee"))
            {
                string employeeId = employee.Attribute("employeeid").Value;
                string name = employee.Element("Name").Value;
                int birthYear = int.Parse(employee.Element("BirthYear").Value);
                int startYear = int.Parse(employee.Element("StartYear").Value);
                int completedProjects = int.Parse(employee.Element("CompletedProjects").Value);
                bool active = bool.Parse(employee.Element("Active").Value);
                bool retired = bool.Parse(employee.Element("Retired").Value);
                string email = employee.Element("Email").Value;
                string phone = employee.Element("Phone").Value;
                string job = employee.Element("Job").Value;
                string level = employee.Element("Level").Value;
                decimal salary = decimal.Parse(employee.Element("Salary").Value);
                Commission commission = ParseCommission(employee.Element("Commission"));

                
                var existingEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                ;
                if (existingEmployee == null)
                {
                    existingEmployee = new Employee
                    {
                        EmployeeId = employeeId,
                        Name = name,
                        BirthYear = birthYear,
                        StartYear = startYear,
                        CompletedProjects = completedProjects,
                        Active = active,
                        Retired = retired,
                        Email = email,
                        Phone = phone,
                        Job = job,
                        Level = level,
                        Salary = salary,
                        Commission = commission
                    };
                    if (existingEmployee.Commission != null && existingEmployee.Commission.Currency == "eur")
                    {
                        existingEmployee.Commission.Value = existingEmployee.Commission.Value * 400;
                        existingEmployee.Commission.Currency = "huf";
                    }
                    context.Employees.Add(existingEmployee);
                    context.SaveChanges(); 
                }
                ;
                foreach (var department in employee.Element("Departments").Elements("Department"))
                {
                    string departmentCode = department.Element("DepartmentCode").Value;
                    string departmentName = department.Element("Name").Value;
                    string headOfDepartment = department.Element("HeadOfDepartment").Value;

                    // Add or find the department in the database
                    var existingDepartment = context.Departments.FirstOrDefault(d => d.DepartmentCode == departmentCode);
                    if (existingDepartment == null)
                    {
                        existingDepartment = new Department
                        {
                            DepartmentCode = departmentCode,
                            DepartmentName = departmentName,
                            HeadOfDepartment = headOfDepartment
                        };
                        context.Departments.Add(existingDepartment);
                        context.SaveChanges(); 
                    }

                    context.EmployeeDepartments.Add(new EmployeeDepartment
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = existingEmployee.EmployeeId,
                        DepartmentCode = existingDepartment.DepartmentCode
                    });
                }
                ;
              
                context.SaveChanges();
            }
        }
    private Commission ParseCommission(XElement commissionElement)
        {
            if (commissionElement == null)
                return null;
            var value = decimal.Parse(commissionElement.Value);
            var currencyAttr = commissionElement.Attribute("currency");
            var currency = currencyAttr != null ? currencyAttr.Value.ToLower() : "huf";
            return new Commission { Value = value, Currency = currency };
        }
    }
    
}
