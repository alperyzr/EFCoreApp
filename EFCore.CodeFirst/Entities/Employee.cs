using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class Employee:_BasePerson
    {
        public int Salary { get; set; }
    }
}
