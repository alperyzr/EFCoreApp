using EFCore.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Models.DTO
{
    public class ProductDtoWithAutoMapper
    {
        public int Id { get; set; }


        public string Name { get; set; }

        
        public string Url { get; set; }

       
        public decimal Price { get; set; }

      
        public decimal DiscountPrice { get; set; }

        public int Stock { get; set; }

        public Category Category { get; set; }

        public ProductFeature? ProductFeature { get; set; }
    }
}
