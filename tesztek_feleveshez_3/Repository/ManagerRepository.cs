using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesztek_feleveshez_3.Data;
using tesztek_feleveshez_3.Entities.EntityModels;

namespace tesztek_feleveshez_3.Repository
{
    public class ManagerRepository<T> where T : class, tesztek_feleveshez_3.Entities.Help.IIdenitiy
    {
        ManagerDbContext ctx;
        public ManagerRepository(ManagerDbContext ctx)
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
        public int ManagerQ1()
        {
            return ctx.Managers.Where(x => x.Name.Contains("Dr.")).Count();
        }
        public IEnumerable<string> ManagerQ2()
        {
            return ctx.Managers.Where(x => x.HasMBA == true && x.Name.Contains("Dr.")).Select(x => x.Name);
        }
        public string ManagerQ3()
        {
            return ctx.Managers.Where(x => x.StartOfEmployment == ctx.Managers.Min(x => x.StartOfEmployment)).Select(x => x.Name).First();
        }
        public string ManagerQ4()
        {
            var result = ctx.Managers
             .AsEnumerable() 
             .Select(m => new
             {
                 Ratio = (double)(2024 - m.BirthYear) / (2024 - int.Parse(m.StartOfEmployment.Substring(0, 4))),
                 Name = m.Name
             })
             .OrderBy(x => x.Ratio) 
             .FirstOrDefault();

            return result?.Name;
        }
        public List<string> ManagerQ5()
        {
            var result = from m in ctx.Managers
                         group m by m.HasMBA into h
                         select new string($"Has MBA: {h.Key}, Percentage: {h.Where(x => x.HasMBA == h.Key).Count() * 100 / 10}");
            return result.ToList();
        }
    }
}

