using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class Teacher:_BaseEntity
    {
        public int Id { get; set; }
    
        public string Name { get; set; }

        public List<Student> Students { get; set; }

    }
}
