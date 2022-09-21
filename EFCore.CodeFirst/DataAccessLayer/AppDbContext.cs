using EFCore.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }


        //Db yolunu appsettingsten okuyabilmek için;
        //Microsoft.Extensions.Configurations
        //Microsoft.Extensions.Configurations.FileExtensions
        //Microsoft.Extensions.Configurations.Json
        //Nugget paketleri yüklenmelidir ve aşağıdaki yol izlenmelidir.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
        }
    }
}
