using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.AuthScheme.PoP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{

    //Ham Sql cümleciği tarafından dönen entityler için Keyless attribute ü kullanılır.
    [Keyless]
    public class ProductFull
    {
        public int Category_Id { get; set; }
       
        public string CategoryName { get; set; }
       
        public int Product_Id { get; set; }
      
        public string ProductName { get; set; }

        [Precision(18,2)]
        public decimal Price { get; set; }

        public int ProductFeature_Id { get; set; }
      
        public string Color { get; set; }
    }
}
