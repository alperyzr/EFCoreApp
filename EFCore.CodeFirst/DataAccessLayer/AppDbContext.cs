using EFCore.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EFCore.CodeFirst.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }


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
            //Bir kategorinin birden fazla productı, bir productın bir categorysi olur anlamında kullanılıp product içerisindeki
            //CategoryId foreing key olarak belirtildi           
            //OnDelete methodu içerisinde DeleteBahavior Enumını alır. Bu method herhangi bir kategori silindiği zaman child tablolarında nasıl aksiyon
            //alacağına karar verir. Cascade default davranıştır. 1 id li kategori silindiği zaman product tablosundaki bütün 1 kategori İd li kayıtları siler.
            //Restrict enumı 1 id li kategoride product varsa kategoriyi silmeye izin vermez
            //SetNull enumu product tablosundaki kategori Idleri null a çeker
            //NoAction enum ı ise hiç bir şey yapmamasını, sql tarafında kullanıcının halledeceğini belirtir
            modelBuilder.Entity<Category>().HasMany(x => x.Products).WithOne(x=>x.Category).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Restrict);

            //ProductFeature tablosundaki Id alanı hem Primary Key, hemde Foreign Key olarak ayarladık.
            //Böylece İlgili Product İd si olmayan bir kayıt eklenemez ve fazla kolon kullanımından kurtulmuş olduk
            modelBuilder.Entity<Product>().HasOne(x => x.ProductFeature).WithOne(x => x.Product).HasForeignKey<ProductFeature>(x => x.Id);

            //Bir öğrencinin birden fazla öğretmeni, bir öğretmenin de birden fazla öğrencisi olabilir.
            //Bu sebepten dolayı çoka-çok ilişki kullanıyoruz.
            //EFCore 5.0 versiyonununda FluentAPI ile bu kısımları oluşturmadan ilişki kurulduğunda
            //EFCore otomatik olarak iki tablonun birleşim tablosunu oluşturur
            modelBuilder.Entity<Student>().HasMany(x => x.Teachers).WithMany(x => x.Students).UsingEntity<Dictionary<string, object>>(
                "StudentTeacher",
                x => x.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId").HasConstraintName("FK_TeacherId"),
                x => x.HasOne<Student>().WithMany().HasForeignKey("StudentId").HasConstraintName("FK_StudentId")
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
