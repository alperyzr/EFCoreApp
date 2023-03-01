using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Models
{
    [Keyless]
    public class ProductWithFeatureView
    {
        
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Color { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
