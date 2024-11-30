using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesztek_feleveshez_3.Data;

namespace tesztek_feleveshez_3.Logic
{
    public class EmployeeRepository<T> where T : class, tesztek_feleveshez_3.Entities.Help.IIdenitiy
    {
        EmployeeDbContext ctx;
        public EmployeeRepository(EmployeeDbContext ctx)
        {
            this.ctx = ctx;
        }
        public void Create(T entity)
        {
            ctx.Set<T>().Add(entity);
            ctx.SaveChanges();
        }
        public IQueryable<T> GetAll()
        {
            return ctx.Set<T>();
        }
        public T GetOne(string id)
        {
            return FindById(id);
        }
        public void Update(T entity)
        {
            var old = FindById(entity.Id);
            foreach (var prop in typeof(T).GetProperties())
            {
                prop.SetValue(old, prop.GetValue(entity));
            }
            ctx.Set<T>().Update(old);
            ctx.SaveChanges();
        }
        public void DeleteById(string id)
        {
            var entity = FindById(id);
            ctx.Set<T>().Remove(entity);
            ctx.SaveChanges();
        }

        public void Delete(T entity)
        {
            ctx.Set<T>().Remove(entity);
            ctx.SaveChanges();
        }
        public T FindById(string id)
        {
            return ctx.Set<T>().First(t => t.Id == id);
        }
        public int EmployeeQ1()
        {
            return ctx.Employees.Where(x => x.BirthYear > 1979 && x.BirthYear < 1990).Count();
        }
        public int EmployeeQ2()
        {
            var result = ctx.EmployeeDepartments
                .GroupBy(x => x.EmployeeId)
                .Where(g => g.Count() > 1) 
                .Select(g => g.Key);
            return result.Count();
        }
        public IQueryable EmployeeQ3()
        {
            return ctx.Employees.Where(x => x.Retired == false && x.Active == true).Select(x => x.Name);
            ;
        }
        public int EmployeeQ4()
        {
            return ctx.Employees.Where(x => x.Retired == false && x.Active == false).Count();
        }
        public string EmployeeQ5()
        {
            return new string($"{ctx.Employees.Where(x => x.Retired == false).DefaultIfEmpty().Select(x => x.Salary).DefaultIfEmpty().Average(x => x)} Ft");
        }
        public IQueryable EmployeeQ6()
        {
            return ctx.Employees.OrderByDescending(x => x.Salary + x.Commission.Value).Select(x => x.Name);
        }
        public List<string> EmployeeQ7()
        {
            var help = from e in ctx.Employees
                       group e by e.Level into l
                       select new string($"Level: {l.Key} \n Percentage: {l.Where(x => x.Level == l.Key).Count() * 100 / l.Key.Count()}%");
            return help.ToList();
        }
        public IQueryable EmployeeQ8()
        {
            var departmentCodes = ctx.Departments.Where(xy => xy.HeadOfDepartment.Contains("Dr")).Select(xy => xy.DepartmentCode);
            return ctx.EmployeeDepartments.Where(x => departmentCodes.Contains(x.DepartmentCode)).Select(x => x.Employee.Name);
            ;
        }
        public string EmployeeQ9()
        {
            var avarageSalary = ctx.Employees.Average(x => x.Salary);
            var Lower = ctx.Employees.Count(x => x.Salary < avarageSalary);
            var Higher = ctx.Employees.Count(x => x.Salary > avarageSalary);
            return new string($"Lower salary than avarage: {Lower} \n Higher salary than avarage: {Higher}");
        }
        public string EmployeeQ10()
        {
            string result = "";
            var levels = ctx.Employees.Select(x => x.Level).Distinct().ToList();
            foreach (var level in levels)
            {
                result += ($"Level: {level} Avarage salry: {ctx.Employees.Where(x => x.Level == level).Average(x => x.Salary)}\n");
            }
            return result;
        }
        public string EmployeeQ11()
        {
            var BiggestJuniorSalary = ctx.Employees.Where(x => x.Level == "Junior").OrderByDescending(x => x.Salary).Select(x => x.Salary).FirstOrDefault();
            var AvarageMediorSalary = ctx.Employees.Where(x => x.Level == "Medior").Average(x => x.Salary);
            if (BiggestJuniorSalary == AvarageMediorSalary)
            {
                return new string($"The biggest junior salary({BiggestJuniorSalary} Ft) equals with the avarage medior salary({AvarageMediorSalary} Ft)");
            }
            return BiggestJuniorSalary > AvarageMediorSalary ? new string($"The biggest junior salary({BiggestJuniorSalary} Ft) higher than the avarage medior salary({AvarageMediorSalary} Ft)")
                : new string($"The biggest junior salary({BiggestJuniorSalary} Ft) lower than the avarage medior salary({AvarageMediorSalary} Ft)");
        }
        public string EmployeeQ12()
        {
            List<decimal> commissions = new List<decimal>();
            var levels = ctx.Employees.Select(x => x.Level).Distinct().ToList();
            foreach (var level in levels)
            {
                commissions.Add(ctx.Employees.Where(x => x.Level == level).Sum(x => x.Commission.Value));
            }
            return new string($"The commissions in the {levels[commissions.LastIndexOf(commissions.Max())]} level is the highest({commissions.Max()} Ft)");
        }
        public string EmployeeQ13()
        {
            return new string($"Who worked on the fewest projects: {ctx.Employees.OrderBy(x => x.CompletedProjects).FirstOrDefault().Name}");
        }
        public string EmployeeQ14()
        {
            string result = "";
            var help = ctx.Employees.OrderBy(x => x.BirthYear).Select(p => new { Name = p.Name,Salary = p.Salary });
            foreach (var level in help)
            {
                result += $"Name: {level.Name} Salary: {level.Salary} Ft\n";
            }
            return result;
        }
        public string EmployeeQ15()
        {
            return new string($"Who worked on the fewest projects and has active status: {ctx.Employees.OrderBy(x => x.CompletedProjects).Where(x => x.Active == true).FirstOrDefault().Name}");
        }
        public string EmployeeQ16()
        {
            var result = ctx.Employees.Where(x => x.Commission.Value > ctx.Employees.Min(x => x.Salary)).FirstOrDefault();
            return result == null ? "There is not that kind of case" : $"{result} has higher commison than another employee's salary";
        }
        public void Grapich()
        {
            int left = ctx.Employees.Max(p => p.Name.Length) + 1;
            int top = 0;
            foreach (var p in ctx.Employees)
            {
                decimal entire = p.Salary / 40000;
                bool remain = p.Salary / 200000 != 0;
                Console.Write(p.Name + " ");
                Console.SetCursorPosition(left, top);
                for (int i = 0; i < entire; i++)
                {
                    Console.Write("█|");
                }
                string half = "█";
                if (remain)
                {
                    Console.Write(half);
                }
                Console.WriteLine($" {p.Salary} ft");
                top++;

            }
        }
        
    }
}
