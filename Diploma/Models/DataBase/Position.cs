using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models.DataBase
{
    public class Position
    {
        public int Id { get; set; }
        public string Post { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}