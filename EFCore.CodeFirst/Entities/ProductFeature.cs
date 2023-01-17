using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class ProductFeature:_BaseEntity
    {
        public int Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Color { get; set; }

        //Product Parent property
        //LazyLoading için virtual eklenmelidir
        //public virtual Product Product { get; set; }
        public virtual Product Product { get; set; }

    }
}
