using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models.DataBase
{
    public class Building
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Floors { get; set; }
        
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<Rooms> Rooms { get; set; }
    }
}