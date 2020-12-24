using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models.DataBase
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Building> Buildings { get; set; }
    }
}