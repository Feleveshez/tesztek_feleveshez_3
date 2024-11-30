using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using tesztek_feleveshez_3.Data;
using tesztek_feleveshez_3.Entities.EntityModels;
using tesztek_feleveshez_3.Entities.Help;
using tesztek_feleveshez_3.Logic;
using tesztek_feleveshez_3.Repository;

namespace tesztek_feleveshez_3
{
    internal class Program  
    {
        static void Main(string[] args)
        {
            //string xmlFilePath = "employees-departments.xml";
            //using (var context = new EmployeeDbContext())
            //{
            //    EmployeeLogic employeeLogic = new EmployeeLogic(context);
            //    employeeLogic.EmployeeImport(xmlFilePath);
            //}
            //ManagerLogic managerLogic = new ManagerLogic();
            //var jsonFilePath = "https://nik.siposm.hu/db/managers.json";
            //managerLogic.JsonMainProcess(jsonFilePath);
            EmployeeDbContext ctx = new EmployeeDbContext();
            var employeeRepository = new EmployeeRepository<EmployeeDepartment>(ctx);
            //employeeRepository.Grapich();
            ManagerDbContext managerctx = new ManagerDbContext();
            var managerRepository = new ManagerRepository<Manager>(managerctx);
            //DelayedPrintObject(managerRepository.ManagerQ1());
            //DelayedPrintObject(managerRepository.ManagerQ2());
            //DelayedPrintObject(managerRepository.ManagerQ3());
            //DelayedPrintObject(managerRepository.ManagerQ4());
            DelayedPrintObject(managerRepository.ManagerQ5());// Még nem jó
            //DelayedPrintObject(employeeRepository.EmployeeQ1());
            //DelayedPrintObject(employeeRepository.EmployeeQ2);
            //DelayedPrintObject(employeeRepository.EmployeeQ3);
            //DelayedPrintObject(employeeRepository.EmployeeQ4);
            //DelayedPrintObject(employeeRepository.EmployeeQ5);
            //DelayedPrintObject(employeeRepository.EmployeeQ6());
            //DelayedPrintObject(employeeRepository.EmployeeQ7());
            //DelayedPrintObject(employeeRepository.EmployeeQ8);
            //DelayedPrintObject(employeeRepository.EmployeeQ9);
            //DelayedPrintObject(employeeRepository.EmployeeQ10);
            //DelayedPrintObject(employeeRepository.EmployeeQ11);
            //DelayedPrintObject(employeeRepository.EmployeeQ12);
            //DelayedPrintObject(employeeRepository.EmployeeQ13);
            //DelayedPrintObject(employeeRepository.EmployeeQ14);
            //DelayedPrintObject(employeeRepository.EmployeeQ15);
            //DelayedPrintObject(employeeRepository.EmployeeQ16);

        }
        
        static void PrintObject(object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("null");
                return;
            }
            Type type = obj.GetType();
            if (type.IsPrimitive || obj is string || obj is decimal || obj is DateTime)
            {
                Console.WriteLine(obj);
                return;
            }
            if (obj is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    PrintObject(item);
                }
                return;
            }
            Console.WriteLine();
            foreach (var property in type.GetProperties())
            {
                object value = property.GetValue(obj);
                Console.Write($"{property.Name}: ");
                PrintObject(value);
            }
        }
        static void DelayedPrintObject(object obj)
        {
            PrintObject(obj);
            Console.WriteLine("Nyomj meg egy gombot a folytatáshoz");
            Console.ReadKey();
        }
        static void ExportXML(Object objectClass)
        {
            Type t = objectClass.GetType();
            MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .ToArray();


            if (t.IsDefined(typeof(ToExportAttribute)))
            {
               PropertyInfo[] properties = t.GetProperties();
               XElement exportDatas = new XElement("entities",
               new XElement("entity",
               new XAttribute("hash", Guid.NewGuid()),
               new XElement("type", t.Name),
               new XElement("Namespace", t.Namespace),

               new XElement("Properites",
               new XAttribute("count", t.GetProperties().Count(x => x.IsDefined(typeof(HideFromExportAttribute)) == false)),
               from property in properties
               where property.IsDefined(typeof(HideFromExportAttribute)) == false
               select new XElement("property", $"{property.PropertyType.Name} {property.Name}")

               ),
               new XElement("Methods",
               new XAttribute("count", methods.Count(m => !m.IsDefined(typeof(HideFromExportAttribute), false))),
               from method in methods
               where !method.IsDefined(typeof(HideFromExportAttribute), false)
               select new XElement("method",
                    new XElement("returnType", method.ReturnType.Name),
                    new XElement("name", method.Name),
                    new XElement("parameters",
                        from param in method.GetParameters()
                        select new XElement("param", $"{param.ParameterType.Name} {param.Name}")
                    )))
               ));
                exportDatas.Save("saves.xml");
            }

        }

    }
   
}
