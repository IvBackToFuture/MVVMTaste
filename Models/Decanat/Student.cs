using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMTaste.Models.Decanat
{
    internal class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public double Rating { get; set; }
    }
}
