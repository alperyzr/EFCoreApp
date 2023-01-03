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
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }


        //Db yolunu appsettingsten okuyabilmek için;
        //Microsoft.Extensions.Configurations
        //Microsoft.Extensions.Configurations.FileExtensions
        //Microsoft.Extensions.Configurations.Json
        //Nugget paketleri yüklenmelidir ve aşağıdaki yol izlenmelidir.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Console alt yapısı olduğu için bu method çağrılır. MVC ve CORE projelerinde otomatik algılanır
            DbContextInitializer.Build();
            //appsettings ten okumak için kullanılır
            optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
        }


        //FluentAPI kullanma yöntemi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //PrimaryKey olduğu belirtili
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            //Name alanının required olduğu belirtilitr
            //100 karakter sabit bir değer alması için İsFİxed Methodu kullanılır
            modelBuilder.Entity<Product>().Property(x => x.Name).IsRequired().HasMaxLength(100).IsFixedLength();

            //İlişkili tablolarda her zaman has ile başlanacak
            //Bir kategorinin birden fazla productı, bir productın bir categorysi olur anlamında kullanılıp product içerisindeki CAtegoryId foreing key olarak belirtildi
            modelBuilder.Entity<Category>().HasMany(x => x.Products).WithOne(x=>x.Category).HasForeignKey(x=>x.CategoryId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
