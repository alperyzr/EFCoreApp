using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Models.DTO
{
    public class ProductDto2
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductNames { get; set; }
        public decimal TotalPrice{ get; set; }       
    }
}
