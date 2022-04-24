using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMTaste.Models.Decanat
{
    internal class Group
    {
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
        public string Description { get; set; }
    }
}
