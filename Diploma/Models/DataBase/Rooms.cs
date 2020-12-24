using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models.DataBase
{
    public class Rooms
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public ICollection<Admit> Admits { get; set; }
    }
}