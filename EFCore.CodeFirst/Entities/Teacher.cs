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

        //LazyLoading yapılacağı zaman virtual kullanılmalıdır
        //public virtual List<Student> Students { get; set; }
        public List<Student> Students { get; set; }

        public string Phone { get; set; }

        public Teacher()
        {
            Students = new List<Student>();
        }

    }
}
