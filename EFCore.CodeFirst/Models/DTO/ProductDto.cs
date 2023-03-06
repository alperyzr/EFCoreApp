using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Models.DTO
{
    public class ProductDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
