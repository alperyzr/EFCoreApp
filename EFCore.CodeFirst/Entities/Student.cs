using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class Student:_BaseEntity
    {
        public int Id { get; set; }
      
        public string Name { get; set; }
       
        public int Age { get; set; }

        //LazyLoading yapılacağı zaman virtul kullanılmalıdır
        //public virtual List<Teacher> Teachers { get; set; }
        public List<Teacher> Teachers { get; set; }

        public Student()
        {
            Teachers = new List<Teacher>();
        }
    }
}
