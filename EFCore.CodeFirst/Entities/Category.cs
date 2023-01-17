using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class Category: _BaseEntity
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

        //LazyLoading için virtual kullanılmalıır
        //public virtual List<Product> Products { get; set; }
        public virtual List<Product> Products { get; set; }

        public Category()
        {
            Products= new List<Product>();
        }


    }
}
