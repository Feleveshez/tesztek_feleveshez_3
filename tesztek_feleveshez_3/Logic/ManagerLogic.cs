using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using tesztek_feleveshez_3.Data;
using tesztek_feleveshez_3.Entities.EntityModels;

namespace tesztek_feleveshez_3.Logic
{
    public class ManagerLogic
    {
        public List<Manager> ProcessManagerJson(string url)
        {
            var jsondata = new WebClient().DownloadString(url);
            List<Manager> managers = JsonConvert.DeserializeObject<List<Manager>>(jsondata);
            return managers;
        }
        public void SaveJsonToDatabase(List<Manager> managers)
        {
            using (var context = new ManagerDbContext())
            {
                foreach (var manager in managers)
                {
                    context.Managers.Add(manager);
                }
                context.SaveChanges();
            }
        }
        public void JsonMainProcess(string url)
        {
            // 1. XML feldolgozása és alkalmazottak betöltése
            var managers = ProcessManagerJson(url);

            // 2. Alkalmazottak mentése az adatbázisba
            SaveJsonToDatabase(managers);
        }
    }
}
