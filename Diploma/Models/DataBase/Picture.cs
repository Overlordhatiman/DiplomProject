using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models.DataBase
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}