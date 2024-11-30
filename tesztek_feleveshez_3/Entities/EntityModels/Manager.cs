using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesztek_feleveshez_3.Entities.Help;

namespace tesztek_feleveshez_3.Entities.EntityModels
{
    public class Manager : IIdenitiy
    {
        public Manager(string name, string managerId, int birthYear, string startOfEmployment, bool hasMBA)
        {
            Name = name;
            ManagerId = managerId;
            BirthYear = birthYear;
            StartOfEmployment = startOfEmployment;
            HasMBA = hasMBA;
            Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
        public string ManagerId { get; set; }
        [HideFromExport]
        public int BirthYear { get; set; }
        public string StartOfEmployment { get; set; }
        public bool HasMBA { get; set; }
        public string Id { get; set; }
        public int Valamimethod(int szam)
        {
            return szam * 2;
        }
    }
}
