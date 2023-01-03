using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class Product:_BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int Barcode { get; set; }

        //Her products ın bir kategorisi olacak
        public int? CategoryId { get; set; }

        //Navigation property
        //Category Parent property
        public Category Category { get; set; }

        //ProductFeature bire-bir ilişki kurmak için kullanılır
        public ProductFeature ProductFeature { get; set; }
    }
}
