using EFCore.CodeFirst.Entities;
using EFCore.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly int Barcode;
        public AppDbContext(int barcode)
        {
            Barcode= barcode;
        }

        public AppDbContext()
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<_BasePerson> BasePeople { get; set; }
        public DbSet<ProductFull> ProductFulls { get; set; }
        public DbSet<ProdutsEssential> ProdutsEssentials { get; set; }
        public DbSet<ProductWithFeature> ProductWithFeatures { get; set; }
        public DbSet<ProductWithFeatureView> ProductWithFeatureViews { get; set; }


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
            //LogTo methodu da lazyLoading ile beraber yapılan işlemlerde Console log bilgilerini yazdırmak için kullanılır
            //UseLazyLoadingProxies methodu kurulan nugget paketten sonra appsettingse eklenerek lazyLoading yapmamıza imkan tanır
           
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).UseLazyLoadingProxies().UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
            //optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));

        }


        //FluentAPI kullanma yöntemi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //PrimaryKey olduğu belirtili
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            //Name alanının required olduğu belirtilitr
            //100 karakter sabit bir değer alması için İsFİxed Methodu kullanılır
            modelBuilder.Entity<Product>().Property(x => x.Name).IsRequired()/*.HasMaxLength(100).IsFixedLength()*/;
            

            //decimal değerin toplam kaç karakter olup, virgülden sonra kaç karakter alacağını belirten fluentAPI tarafındaki gösterimi
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);

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

            //TPT Table-Per-Type
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Manager>().ToTable("Managers");
            modelBuilder.Entity<_BasePerson>().ToTable("BasePeople");

            //[Keyless] validationun FluentAPI tarafındaki yazımı
            modelBuilder.Entity<ProductFull>().HasNoKey();

            //[NotMapped] attribute ünün FluentAPI tarafında kullanımı Barcode propertysinin db de oluşmamasını sağlar
            modelBuilder.Entity<Product>().Ignore(x => x.Test);

            //[Unicode(false)] attributeünün FluentApıu tarafındaki karşığı, ASCII karakterleri için kullanılır. Türkçe karakter kabul etmez
            //Tip olarak artık varchar sayılır ve 2 Byte verine 1 bytelık yer kaplar           
            modelBuilder.Entity<Product>().Property(x=>x.Test).IsUnicode(false);

            //[Column(TypeName="varchar(200)")] attribüteünün FluenTAPI tarafında kullanımı
            modelBuilder.Entity<Product>().Property(x => x.Url).HasColumnType("nvarchar(MAX)");

            //Product classı üzerinde yer alan [Index(nameof(Name))] attribüte ünün FluentAPI tarafındaki karşılığıdır.
            //Product tablosundaki Name alanına göre indexleme yaparak Sql tarafında daha hızlı sorgular ve sonuçlar almamızı sağlar
            //IncluedeProperties methodu ise bu index tablosuna sorgu atarken price, stock, ve url propertylerininde hızlı bir şekilde gelmesini istiyorsam 
            //Bu şekilde ekliyoruz ve IndeTablosunda Where(x=>x.Name =="Alper").Select(x=>new{x.Price,x.Stock}) dersem hızlı bir şekilde bu kolonları alırım
            modelBuilder.Entity<Product>().HasIndex(x => x.Name).IncludeProperties(x => new { x.Price, x.Stock, x.Url});
            modelBuilder.Entity<Product>().HasIndex(x => x.Price).IncludeProperties(x => new {x.Name,x.Stock,x.Url});

            //ComposedIndex (tek Where şartında birden fazla property kullandığımız zamanda hızlı sonuç gelmesini istiyorsak) FluentAPI tarafındaki şekli
            modelBuilder.Entity<Product>().HasIndex(x => new{ x.Name , x.Price }).IncludeProperties(x => new {x.Stock,x.Url});

            //HasCheckConstraint methodu sayesinde bir kural belirtiriz. Bu kurala ilk olarak isim veririz, ardından ilgili kuralı veririz
            //Bu kuralda Her zaman fiyatın indirimli fiyattan büyük olması gerektiğini belirttik
            modelBuilder.Entity<Product>().HasCheckConstraint("PriceDiscountCheck" , "[Price]>[DiscountPrice]");


            //ToSqlQeury custom sorgular için kullanılır. _context.ToList eddiğimiz gibi bu sorgu arkada çalışır ve tek bir yerden kontrol etmiş oluruz
            modelBuilder.Entity<ProdutsEssential>().HasNoKey().ToSqlQuery("select p.Id, p.Name, p.Price, pf.Color, pf.Width from Products p inner join ProductFeatures pf on p.Id = pf.Id");
            modelBuilder.Entity<ProductWithFeature>().HasNoKey();

            //Sql Tarafında View dan okuman için ToView methodu kullanılır
            modelBuilder.Entity<ProductWithFeatureView>().ToView("productWithFeature");

            modelBuilder.Entity<Product>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Product>().Property(x=>x.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Product>().Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now);

            //Product nesnesi için Daima IsActive true çekmek istersek global olarak bruada böyle tanımlıyoruz ve çağırdığımız yerde ToList dediğimiz zaman otomatik olarak
            //bu koşula uyanlar gelecektir
            modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsActive == true);

            if (Barcode != default(int))
            {
                modelBuilder.Entity<Product>().HasQueryFilter(x => x.Barcode == Barcode);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
