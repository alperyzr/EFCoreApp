
using EFCore.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.MVC.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Product> Products{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //FluentAPI tanımlamaları

            //IsRowWersion methodu ilgili property alanının EfCore tarafından otomatik şekilde güncelleneceğini belirtir.
            //Böylece aynı kayıt üzerinde herhangi T zamanlarda farklı kişiler tarafından güncelleme geçilmeye çalışılırsa hata fırlatacaktır
            modelBuilder.Entity<Product>().Property(x => x.RowVersion).IsRowVersion();
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);




            base.OnModelCreating(modelBuilder);
        }
    }
}
