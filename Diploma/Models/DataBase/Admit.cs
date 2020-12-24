using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models.DataBase
{
    public class Admit
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomsId { get; set; }
        public Rooms Room { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}