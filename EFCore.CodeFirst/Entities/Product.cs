using Microsoft.EntityFrameworkCore;
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

        //Decimal değerler için kaç karakter olup, virgülden sonra kaç karakter alacağını belirtmek için kullanılır
        //################.## gibi
        [Precision(18,2)]
        public decimal Price { get; set; }
     
        public int Stock { get; set; }
      
        public int Barcode { get; set; }

        //Her products ın bir kategorisi olacak
        public int? CategoryId { get; set; }

        //Navigation property
        //Category Parent property
        //LazyLoading kullanılması durumunda virtual eklenmelidir.
        //public virtual Category Category { get; set; }
        public Category Category { get; set; }

        //ProductFeature bire-bir ilişki kurmak için kullanılır
        //LazyLoading kullanılması durumunda virtual eklenmelidir.
        //public virtual ProductFeature ProductFeature { get; set; }
        public ProductFeature ProductFeature { get; set; }
    }
}
