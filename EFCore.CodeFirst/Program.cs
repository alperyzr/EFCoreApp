
//#define ile bir değişken tanımlamamıza ve aşağıdaki if komutları ile hangi using in çalışması gerektiğinin kontrolü yapılr
#define RawSQLQuery

using EFCore.CodeFirst.DataAccessLayer;
using EFCore.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

//DbContext in appsettingsten okunması için classın Build methodu çağrılır.
DbContextInitializer.Build();



#if EntryState
//using bir işlem dispose olduktan sonra otomatik olarak connection bağlantısının kapatılması ve memory i kastırmaması için kullanılır.
//Appsettings dosyasındaki SqlConnection ı kullanmak için yazılan classlar
using (var _context = new AppDbContext())
{
    
    var products = await _context.Products.ToListAsync();
    products.ForEach(p =>
    {
        //Entitynin statelerini çağırır.
        //ToList methodundan sonra çağrıldığsa Unchanged(Veri tabanından ilk data çağrıldığı anda),
        //Add methodundan sonra Added
        //Update methodundan sonra modifield olarak döner
        var state = _context.Entry(p).State;
        Console.WriteLine($"{p.Id} : {p.Name}, Price:{p.Price}, Stok:{p.Stock} State:{state}");
    });

    
    var newProduct = new Product
    {
        Name = "Kalem 2",
        Barcode = 333,
        Price = (decimal)15.99,
        Stock = 100   
    };
    //State Deteched olarak çıkar
    var newproductState = _context.Entry(newProduct).State;
    Console.WriteLine($"{newProduct.Id} : {newProduct.Name}, Price:{newProduct.Price}, Stok:{newProduct.Stock} State:{newproductState} Product Nesnesi Oluşturuldu");

    await _context.AddAsync(newProduct);
    //State Added olarak çıkar
    var newproductAddState =  _context.Entry(newProduct).State;
    Console.WriteLine($"{newProduct.Id} : {newProduct.Name}, Price:{newProduct.Price}, Stok:{newProduct.Stock} State:{newproductAddState} ADD methodu çalıştırldı");


    await _context.SaveChangesAsync();
    //State UnChanged olarak çıkar
    var newproductSaveState = _context.Entry(newProduct).State;
    Console.WriteLine($"{newProduct.Id} : {newProduct.Name}, Price:{newProduct.Price}, Stok:{newProduct.Stock} State:{newproductSaveState} SaveChanges Methodu çalıştırıldı");

    Console.ReadKey();
}


#elif ChangeTraker

using (var _context = new AppDbContext())
{
    _context.Products.Add(new() { Name = "Kalem3" });
    _context.Products.Add(new() { Name = "Kalem4" });
    _context.Products.Add(new() { Name = "Kalem5" });

    //Birden fazla context yapısı olduğu durumlarda birbirinden ayırmak için Id olarak kullanılır ve loglama için kullanılabilir.
    Console.WriteLine($"Context Id: {_context.ContextId}");

    //ChangeTracker methodu Entitiler arasında istediğimizi yakalayabilme imkanı sunar
    _context.ChangeTracker.Entries().ToList().ForEach(e =>
    {
        //is keyword u herhangi bir nesnenin bir nesneye dönüştürüp dönüştürülemeyeceğini kontrol eder
        //ToList methodunda AsNoTraching methodu da çağrıldıysa bu if bloğuna girmez.
        if (e.Entity is Product product)
        {
            if (e.State == EntityState.Added)
            {
                product.CreatedDate = DateTime.Now;
                product.Stock += 50;
                product.Price = 50;
            }
            
            Console.WriteLine($"{product.Id} : {product.Name}, Price:{product.Price}, Stok:{product.Stock}");
        }
    });

    _context.SaveChanges();
    Console.ReadKey();
}

#elif EntityFrameWorkCoreMethods
using (var _context = new AppDbContext())
{
    //Product tablosundaki bütün kayıtları çeker
    var products = _context.Products.ToList();
    products.ForEach(p =>
    {
        Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Stock: {p.Stock}, Barcode: {p.Barcode}");
    });

    //First methodu, bestpracties için direkt olarak Id ile işlem yapmak için kullanılır. Yalnızca bir kayıt döndürür. İlgili Id li kayıt yoksa exception fırlatır
    var productFirst = _context.Products.First(x=>x.Id == 1);
    Console.WriteLine($"Id: {productFirst.Id}, Name:{productFirst.Name}, Stok:{productFirst.Stock}, Barcode: {productFirst.Barcode}");

    //FirstOrDefault methodu Yalnızca bir kayıt döndürür. İlgili parametreli kayıt yoksa null döner
    //null dönem durumunda null yerine ilk kayıdı getirmesini istersek virgül ile ilgili koşulu belirtebilirz
    var productFirstOrDefault = _context.Products.FirstOrDefault(x => x.Id == 2);
    Console.WriteLine($"Id: {productFirstOrDefault.Id}, Name:{productFirstOrDefault.Name}, Stok:{productFirstOrDefault.Stock}, Barcode: {productFirstOrDefault.Barcode}");

    //SingleAsync methodu asenkron şekilde tek bir kayıt getirir. Db de ilgili parametreyi karşılayan birden fazla kayıt varsa exceptin fırlatır.
    //Şuan Id si 1den büyük birden fazla kayıt olduğu için exception fırlatacaktır
    var productSingleAsync = await _context.Products.SingleAsync(x => x.Id == 3);
    Console.WriteLine($"Id: {productSingleAsync.Id}, Name:{productSingleAsync.Name}, Stok:{productSingleAsync.Stock}, Barcode: {productSingleAsync.Barcode}");

    //SingleOrDefault tek bir data getirmek için kullanılır. İlgili parametre şartını karşılayan birden fazla data varsa exception fırlatır. Sonuç yoksa null döner
    //Bu haliyle Id si 1 den büyük birden fazla kayıt olduğu için exception fırlatacaktır.
    var productSingleOrDefault =  _context.Products.SingleOrDefault(x => x.Id == 4);
    Console.WriteLine($"Id: {productSingleOrDefault.Id}, Name:{productSingleOrDefault.Name}, Stok:{productSingleOrDefault.Stock}, Barcode: {productSingleOrDefault.Barcode}");

    //Single tek bir data getirmek için kullanılır. İlgili parametre şartını karşılayan birden fazla data varsa exception fırlatır.
    var productSingle = _context.Products.Single(x => x.Id == 5);
    Console.WriteLine($"Id: {productSingle.Id}, Name:{productSingle.Name}, Stok:{productSingle.Stock}, Barcode: {productSingle.Barcode}");

    //Where ilgili parametre şartını karşılayan bir veya birden fazla sonuç döndürür. Kayıt yoksa null döner
    var produtsWhere = _context.Products.Where(x => x.Id >= 1).ToListAsync();
    produtsWhere.Result.ForEach(p =>
    {
        Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Stock: {p.Stock}, Barcode: {p.Barcode}");
    });

    //Find methodu primaryKey ler içerisinde otomatik olarak sorgu yapmak için kullanılır. Birden fazla PrimaryKey durumunda virgül ile parametre sayısı arttırılabilir.
    //Sonuç bulunmazsa null döner
    var produtFind = await _context.Products.FindAsync(1);
    Console.WriteLine($"Id: {produtFind.Id}, Name: {produtFind.Name}");
}
#elif OneToManyDataAdded //Bire Çok İlişki
using (var _context = new AppDbContext())
{
    //Kategori eklemek için
    var category = new Category()
    {
        Name = "Kalemler",
        CreatedDate= DateTime.Now,
        IsActive= true, 
        IsDeleted= false,
        
    };

    //Product Eklemek için
    var product = new Product()
    {
        Name = "Kalem 1",
        Price = 100,
        Stock = 200,
        Barcode = 123,
        Category= category,
        CreatedDate= DateTime.Now,
        IsActive= true,
        IsDeleted= false,
        
    };

    //ProductFeature Eklemek İçin
    var productFeature = new ProductFeature()
    {
        Width = 200,
        Height = 200,
        IsDeleted = false,
        CreatedDate = DateTime.Now,
        IsActive = true,
        Color = "red",
        Product = product,
    };

    //db ye yansıtmak için
    //EfCore un yeni süürmlerinde child e eklersek otomatik olarak ilgili parentlerı da ekler. Örnek product ve category gibi
    _context.Add(productFeature);
    _context.SaveChanges();
    Console.WriteLine("Kaydedildi");
    Console.ReadKey();
}
#elif ManyToManyDataAdded //Çoka Çok ilişki
using (var _context = new AppDbContext())
{
    //Öğrenciye öğretmenler Ekleme
    var student = new Student()
    {
        Name = "Alper",
        Age = 25,
        CreatedDate = DateTime.Now,
        IsActive = true,
        IsDeleted = false,
        Teachers = new List<Teacher>()
        {
            new Teacher()
            {
                Name = "Alper Öğretmen",
                CreatedDate= DateTime.Now,
                IsActive= true,
                IsDeleted= false,
            },
            new Teacher()
            {
                Name = "Mehmet Öğretmen",
                CreatedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
            },
            new Teacher()
            {
                Name = "Özlem Öğretmen",
                CreatedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
            }
        }

    };

    //Öğretmene Öğrenciler Ekleme
    var teacher = new Teacher()
    {
        Name = "Hasan Öğretmen",
        CreatedDate = DateTime.Now,
        IsActive = true,
        IsDeleted = false,
        Students = new List<Student>()
        {
           new Student()
           {
               IsDeleted= false,
               IsActive= true,
               CreatedDate= DateTime.Now,
               Name = "Miray",
               Age= 19,
           },
           new Student ()
           {
               IsDeleted= false,
               IsActive= true,
               CreatedDate= DateTime.Now,
               Name = "Arzu",
               Age= 20,
           },
           new Student ()
           {
               IsDeleted= false,
               IsActive= true,
               CreatedDate= DateTime.Now,
               Name = "Hasan",
               Age= 21,
           }
        }
    };

    //var olan öğretmene öğrenci ekleme
    //Alper Öğretmeni çağırıp memory de track edildiği için alt kısımda sadece SaceCganges methodunu çağırıyoruz.
    //Çünkü zaten memory de track edilmiş bir datayı tekrar eklemeye çalışırsak hata alırız
    var getTeacher = _context.Teachers.Where(x => x.Name.Contains("Alper Öğretmen")).FirstOrDefault();
    getTeacher.Students = new List<Student>()
    {
        new Student()
        {
            Name = "Fatma",
            CreatedDate = DateTime.Now,
            IsActive = true,
            Age= 19,
            IsDeleted= false,
        },
        new Student()
        {
            Name = "Ayşe",
            CreatedDate = DateTime.Now,
            IsActive = true,
            Age= 19,
            IsDeleted= false,
        }
    };

    //_context.Add(student);
    //_context.Add(teacher);
   
    _context.SaveChanges();
    Console.WriteLine("Kaydedildi");
}
#elif EagerLoading //Örneğin bir categoriye bağlı category ve productları çekmek istediğimizde kullanılır
using (var _context = new AppDbContext())
{
    //Include method category çekerken, bağlı olduğu pğroductlarıda çeker. Böylece EagerLoading yapılmış olur
    //ThenInclude ise bir product a bağlı ProductFeatureları getirir.
    //Böylece parent category den en alt child tablosuna kadar tek bir sorgu ile çekebiliriz
    //Önce Include methodunu kullandıktan sonra child tablolarda ne kadar child entity varsa onlara ulaşmak için istediğimiz kadar ThenInclude kullanabilirz.
    //ThenInclude dan sonra tekrar Include yazarsak, artık parente döner ve Category nin altında başka child aramaya başlar.
    var categoryWithProducts = _context.Categories.Include(x => x.Products).ThenInclude(x => x.ProductFeature).First();
    Console.WriteLine("Kategori: "+categoryWithProducts.Name);
    categoryWithProducts.Products.ForEach(x => Console.WriteLine("Product Name: "+x.Name));
    categoryWithProducts.Products.ForEach(x => Console.WriteLine("Product Feature: "+x.ProductFeature.Width + " " + x.ProductFeature.Height));



    //Tamtersi işlem Childtan Parent tabloya
    var productFeature = _context.ProductFeatures.Include(x=>x.Product).ThenInclude(x=>x.Category).First();
    Console.WriteLine("ProductFeature: " + productFeature.Color + " Product: " + productFeature.Product.Name + " Category: " + productFeature.Product.Category.Name);


    //İçerisinde iki navigation property si bulunan product için hem parent e hem child entitysine erişme
    var product = _context.Products.Include(x=>x.ProductFeature).Include(x=>x.Category).First();
    Console.WriteLine("Product: " + product.Name + " ProductFeature: " + product.ProductFeature.Color + " Category: " + product.Category.Name);
    Console.ReadKey();
}
#elif ExplicitLoading //Sonraan Yüklenme ihtimali olabilecek mavigation propertyler için kullanılır
using (var _context = new AppDbContext())
{
    var category = _context.Categories.First();
    if (true)
    {
        //Kategorileri çektikten sonra belli bir yerde productlara ihtiyaç duymaya başladıysak
        //Entry methoduna ilgili parent entitiyi veriyoruz
        //Ardından Collection methodu ile ilgili categorinin productlarını çekip Load methodu ile yüklemesini sağlıyoruz
        //Collection methodunu category içerisinde birden fazla product olabileceği için kullanıyoruz
        _context.Entry(category).Collection(x => x.Products).Load();
        category.Products.ForEach(x =>
        {
            Console.WriteLine("ProductName: " + x.Name);
        });
    }

    var product = _context.Products.First();
    if (true)
    {
        //Product ve ProductFeature Bire bir ilişki olduğu için Collection methodu yerine Referance methodu kullanılır.
        _context.Entry(product).Reference(x => x.ProductFeature).Load();
        Console.WriteLine("Product Feature: "+product.ProductFeature.Color);
    }
}
#elif LazyLoading //ExplicitLoading loading gibi çalışır. Sonradan ihtiyaç duyulabilecek navigation propertyleri yüklemek için kullanılır.
//Ancak 2 defa db ye sorgu atar. Eksi yön olarak bu açıdan maliyetlidir
//Bu özellik EFCore da default olarak kapalı gelir, açık hale getirmek öncelikle için Microsoft.EntityFrameworkCore.Proxies kütüphanesi nuggettan kurulmalıdır
//Ardındann AppDbContext te UseSqlServer methodundan önce modelBuilder.UseLazyLoadingProxies() methodu çaürılmaşıdır
using (var _context = new AppDbContext())
{
    //Bu işlemler program.cs e eklenen Console a yazdırma log komutu ile incelendiğinde category çekerken sorgu attığı,
    //Product ı çekerken ayrı sorgu yaptığı
    //Foreach içerisindeki her bir ProductFeature Itemı için bile db yte sorgu attığı görülüyor.
    //Performans açısından aslında her işlemde db ye sorgu atar ve db yi yorar
    var category = await _context.Categories.FirstAsync();
    Console.Write("Category Çekildi");

    var products = category.Products;
    Console.WriteLine("Categoriinin Productları Çekildi");

    foreach (var item in products)
    {
        var productFeatures = item.ProductFeature;
        Console.WriteLine("ProductFeature Çekildi");
    }
    Console.ReadKey();   
}
#elif TPH //Table-Per-Hierarchy
//BaseEntity den miras almış bir entity, ApDbContext e DbSet<BaseEntity> şeklinde yazılmadıysa EfCore un default davranışı olarak davranır ve Baseden gelen
//propertyler ile her entity için bir tablo oluşturur
//Ancak DbSet<BaseEntity> şeklinde belirtildiyse bu sefer BaseEntityden miras almış bütün entityleri tek bir db de birleştirir
using (var _context = new AppDbContext())
{
    await _context.Managers.AddAsync(new Manager()
    {
        Grade = 1,
        Age = 20,
        FirstName = "Alper",
        LastName = "Yazır"
    });
    await _context.Employees.AddAsync(new Employee()
    {
        FirstName = "Employee",
        LastName = "Employee",
        Age = 20,
        Salary = 20,
    });

    await _context.BasePeople.AddAsync(new Manager()
    {
        Age = 20,
        FirstName = "manager1",
        LastName = "manager1",
        Grade = 1
    });
    await _context.BasePeople.AddAsync(new Employee()
    {
        Age = 20,
        FirstName = "manager1",
        LastName = "manager1",
        Salary = 20
    });
    _context.SaveChanges();
    Console.WriteLine("Manager And Employteee Addeed");

    
    var person = _context.BasePeople.ToList();
    person.ForEach(x =>
    {
        switch (x)
        {
            case Manager m:
                Console.WriteLine("Manager: "+ m.Grade);
                break;
            case Employee e:
                Console.WriteLine("Employee: " + e.Salary);
                break;
            default:
                break;
        }
    });


}
#elif TPT //Table-Per-Type
//BaseEntity den gelen propertyleri başka tabloda, ilgili entityden gelen propertyleride başka tabloda oluşturur
using (var _context = new AppDbContext())
{
    await _context.Managers.AddAsync(new Manager()
    {
        Grade = 2,
        Age = 20,
        FirstName = "Manager3",
        LastName = "Manager3"
    });
    await _context.Employees.AddAsync(new Employee()
    {
        FirstName = "Employee3",
        LastName = "Employee3",
        Age = 20,
        Salary = 20,
    });
    _context.SaveChanges();
    var employee = await _context.Employees.ToListAsync();
    var manager = await _context.Managers.ToListAsync();
    var basePerson = await _context.BasePeople.ToListAsync();
    basePerson.ForEach(x =>
    {
        switch (x)
        {
            case Manager m:
                Console.WriteLine($"Manager: "+ m.Grade);
                break;
            case Employee e:
                Console.WriteLine($"Employee: " + e.Salary);
                break;
            default:
                break;
        }
    });

}
#elif KeylessEntityType //Bir Sql Sorgusu sonucunda dönen ve içerisinde PrimaryKey olmayan tablolar için kullanılır.
//Onu belirtmek için EfCore otomatik olarak primaryKey olarak algılamasın diye Category_Id ve Product_Id olarak belirttik.
//CategoryId veya ProductId olarak belirtseydik EfCore otomatik olarak primary key olarak algılardı ve keyless attributüne gerek kalmazdı
//Keyless entitylerde içerisinde PrimaryKey olmadığı için ekleme-silme-güncelleme işlemleri yapılmaz sadece get işlemleri olur ve okuma işlemi sağlanır
using (var _context = new AppDbContext())
{
    //FromSqlRaw methodu içerisinde direct olarak ham sql cümleciği yazmamıza olanak sağlar
    var productsFull = _context.ProductFulls.FromSqlRaw(@"select c.Id 'Category_Id', c.Name 'CategoryName', p.Id 'Product_Id', p.Name 'ProductName', p.Price, pf.Id 'ProductFeature_Id', pf.Color from Products p join ProductFeatures pf on p.Id = pf.Id join Categories c on p.CategoryId = c.Id
        ").ToList();

    productsFull.ForEach(x =>
    {
        Console.WriteLine($"CategoryId: {x.Category_Id} CategoryName: {x.CategoryName} ProductId: {x.Product_Id} ProductName: {x.ProductName} Price: {x.Price} Product Color: {x.Color}");
    });
}
#elif Index //EfCore tarafındaki SqlIndex
using (var _context = new AppDbContext())
{
    //Indexli Sorgu Sadece Select kısmında IncludeColumn a eklenen propertyler çekildiğinde
    //Performans açısından debugsuz ayağa kaldırıldığı zaman iki sorgununda ekrana yazdırma hızının arasındaki farktan indexin farkı bariz şekilde görülüyor
    var productIndex = _context.Products.Where(x => x.Name.Contains("Kalem 1")).Select(x => new { Name = x.Name, Price = x.Price, Stock = x.Stock, URL = x.Url}).ToList();
    productIndex.ForEach(x =>
    {
        Console.WriteLine($"Name: {x.Name}, Price: {x.Price}, Stock: {x.Stock}, URL: {x.URL}");
    });

    //Index olmayan Sorgu, IncludeColumn kısmına Barcode alanı eklenmediği için Barcode kolonunu çekmek için tekrar ana tabloya gider ve zaman kaybı olur
    var productIndex2 = _context.Products.Where(x => x.Name.Contains("Kalem 1")).Select(x => new { Name = x.Name, Price = x.Price, Stock = x.Stock, URL = x.Url, Barcode = x.Barcode }).ToList();
    productIndex2.ForEach(x =>
    {
        Console.WriteLine($"Name: {x.Name}, Price: {x.Price}, Stock: {x.Stock}, URL: {x.URL}, Barcode: {x.Barcode}");
    });

    //DiscountPrice daima Pricedan küçük olacak diye kural belirttiğimiz için bu kayıdı eklemez ve hata fırlatır
   var product = _context.Products.Add(new Product()
    {
        Name = "Kalem 2",
        Barcode = 123,
        Price = 100,
        CreatedDate = DateTime.Now,
        DiscountPrice = 101,
        IsActive = true,
        IsDeleted = false,
        Stock = 100,
        CategoryId = 3,
        Url = "123"
    });
    
    _context.SaveChanges();
}
#elif LINQ
using (var _context = new AppDbContext())
{
    //örneğin GetFormatPhone local bir method üzerinden telefon formatlayarak karşılaştırma yapmak istediğimizi düşünelim.
    //Aşağıdaki örnekteki gibi where koşulu içerisine direkt olarak o methodu yazamayız. Çünkü Sql tarafı o methodu çözümleyemez.
    //Bizde formatlı şekilde telefon numarası almak istiyorsak ilk önce bütün datayı çekip, ardından where şartı içerisine formatlama methoduna
    //yönlendirip,  belirttiğimiz formata göre toList diyebiliriz
    //var teacher = _context.Teachers.Where(x => x.Phone == GetFormatPhone(x.Phone)).ToList();
    var teacher2 = _context.Teachers.ToList().Select(x => new {Phone = GetFormatPhone(x.Phone) }).ToList();
    teacher2.ForEach(x =>
    {
        Console.WriteLine($"PhoneNumber:{x.Phone}");
    });

       
}
string GetFormatPhone(string phone)
{
    return phone.Substring(1, phone.Length - 1);
}
#elif Joins //NavigationProperty olmadığı senaryolarda join kullanılır.
//NavigationProperty varsa EagerLoadding, ExplicientLoading veya LazzyLoading kullanırız ve joine gerek kalmaz
using (var _context = new AppDbContext())
{
    //Method Syntax olarak Adnalndırılır
    //EFCore Tarafına join işlemi için Join methodundan sonra _context üzerindeki joinlecenek tablo verilir.
    //Ardından İlk tablomuzun Id si, joinlenecek tablodaki karşılık gelecek Id ye eşitlenir.
    //Burada x Category tablosunu, y ise Product tablosuna karşılık gelir ve categorynin Id si ile Productın CategoryId si birbirine eşitlenir.
    //Aerdından (c,p) dediğimiz yerde isim vermiş oluruz ve new anahtar sözcüğü ile artık c ve p de ki propertylere ulaşabiliriz.
    //CategoryName, ProductName ve ProductPrice alanları aslınada entityde yok. Bir nevi sql tarafın c.CategoryName as CategoryName
    //diyerek isimlendirme gibi kullanılıyor
    //2 li Join
    var result = _context.Categories
        .Join(_context.Products, x => x.Id, y => y.CategoryId, (c, p) => new
        {
            CategoryName = c.Name,
            ProductName = p.Name,
            ProductPrice = p.Price,
        }).ToList();

    result.ForEach(x =>
    {
        Console.WriteLine($"CategoryName: {x.CategoryName}, ProductName: {x.ProductName}, ProductPrice: {x.ProductPrice}");
    });

    //Method Syntax olarak Adnalndırılır
    //3lü Join
    var result2 = _context.Categories
        .Join(_context.Products, x => x.Id, y => y.CategoryId, (c, p) => new { c, p })
        .Join(_context.ProductFeatures, x => x.p.Id, y => y.Id, (c, pf) => new
        {
            CategoryName = c.c.Name,
            ProductName = c.p.Name,
            ProductPrice = c.p.Price,
            Color = pf.Color,
        }).ToList();
    
    result2.ForEach(x =>
    {
        Console.WriteLine($"CategoryName: {x.CategoryName}, ProductName: {x.ProductName}, ProductPrice: {x.ProductPrice}, Color: {x.Color}");
    });


    //QuerySyntax olarak adlanrırılır
    //LINQ kullanmak yerine direkt sql cümleciği gibi de kullanılabilir aynı işlemi yapar
    //2 li Join
    var result3 = (from category in _context.Categories
                   join product in _context.Products on category.Id equals product.CategoryId
                   select new
                   {
                       CategoryName = category.Name,
                       ProductName = product.Name,
                       ProductPrice = product.Price,
                   }).ToList();


    result3.ForEach(x =>
    {
        Console.WriteLine($"CategoryName: {x.CategoryName}, ProductName: {x.ProductName}, ProductPrice: {x.ProductPrice}");
    });

    //QuerySyntax olarak adlanrırılır
    //3lü Join
    var result4 = (from category in _context.Categories 
                   join product in _context.Products on category.Id equals product.CategoryId
                   join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id select new
                   {
                       CategoryName = category.Name,
                       ProductName =  product.Name,
                       ProductPrice = product.Price,
                       Color = productFeature.Color
                   }).ToList();

    result4.ForEach(x =>
    {
        Console.WriteLine($"CategoryName: {x.CategoryName}, ProductName: {x.ProductName}, ProductPrice: {x.ProductPrice}, Color: {x.Color}");
    });
}
#elif Left_RightJoins 
//Lef Join hem keşisen tablolarladaki dataları, hemde kesişmese de sol tarafta belirtilen tablonun tamamını almak için kullanılır.
//Right join ise yine tam tersi hem ortada kesişen, hemde hiç kesişmeyebn sağdaki tablonun datalarını alır
using (var _context = new AppDbContext())
{

    //Method Syntax olarak Adnalndırılır
    //Left Join
    //pfList.DefaultEmpty methodu ile Productta olup, ProductFeature da olmayan kayıtlar null olarak gelecek
    //Böylece LeftJoin yapılmış olacak
    var result = (from p in _context.Products
                  join pf in _context.ProductFeatures on p.Id equals pf.Id into pfList
                  from pf in pfList.DefaultIfEmpty()
                  select new 
                  {
                      ProductName = p.Name,
                      ProductColor = pf.Color,
                      //Her product a ait her zaman bir productFeature nesnesi olmayabilir. Bu durumda sayısal değerleri nullable türde cast etmek gerekir
                      ProductWidth = (int?)pf.Width,
                  }).ToList();

    result.ForEach(x =>
    {
        Console.WriteLine($"ProductName: {x.ProductName}, ProductColor: {x.ProductColor}, ProductWidth: {x.ProductWidth}");
    });

    //RightJoin
    var result2 = (from pf in _context.ProductFeatures
                  join p in _context.Products on pf.Id equals p.Id into pList
                  from p in pList.DefaultIfEmpty()
                  select new
                  {
                      ProductName = p.Name,
                      ProductColor = pf.Color,
                      //Burada da ProductFeature da olan kayıtlar Productta olmayabilir. Bu yüzden ProductPrice alanını cast etmem gerekir.
                      ProductPrice =  (decimal?)p.Price,
                      ProductWidth = pf.Width,
                  }).ToList();

    result2.ForEach(x =>
    {
        Console.WriteLine($"ProductName: {x.ProductName}, ProductColor: {x.ProductColor}, ProductWidth: {x.ProductWidth}");
    });
}

#elif FullOutherjoin 
using (var _context = new AppDbContext())
{
    //QuerySyntax olarak adlanrırılır
    var left = await (from p in _context.Products
                      join pf in _context.ProductFeatures on p.Id equals pf.Id into pfList
                      from pf in pfList.DefaultIfEmpty()
                      select new
                      {
                          ProductId = p.Id,
                          ProductName = p.Name,
                          Color = pf.Color, 
                          
                      }).ToListAsync();

    //QuerySyntax olarak adlanrırılır
    var right = await (from pf in _context.ProductFeatures
                      join p in _context.Products on pf.Id equals p.Id into pList
                      from p in pList.DefaultIfEmpty()
                      select new
                      {
                          ProductId = p.Id,
                          ProductName = p.Name,
                          Color = pf.Color,                        
                      }).ToListAsync();

    //Union methodu iki listeyi birleştirmek için kullanılır.
    //Öncelikle left join yazıyoruz. Ardından tam tersi olan rightJoini yuazdıktan sonra union ile iki listeyi birleştiriyoruz
    //Böylece FullOutherJoin olmuş oluyor
    var outherjoin = left.Union(right).ToList();
    outherjoin.ForEach(x =>
    {
        Console.WriteLine($"ProductId: {x.ProductId}, ProductName:  {x.ProductName}, Color: {x.Color}");
    });
}
#elif RawSQLQuery 
using (var _context = new AppDbContext())
{
    var Id = 1;
    var Price = 70;
    //Where şartı string format gibi kullanılır. Süslü parantez içerisindeki sıfır, virgülden sonra gelecek ilk paramatreye karşılık gelir. 
    //{1} yazarsak virgülden sonra iki parametre göndermemiz gerekir
    var products = await _context.Products.FromSqlRaw("select * from Products where Id={0}",Id).ToListAsync();
    products.ForEach(x =>
    {
        Console.WriteLine($"Name: {x.Name}, Price: {x.Price}");
    });

    //İkinci kullanımı
    var product2 = await _context.Products.FromSqlInterpolated($"select * from Products where Price >= {Price}").ToListAsync();
    product2.ForEach(x =>
    {
        Console.WriteLine($"Name: {x.Name}, Price: {x.Price}");
    });

    //Sadece name ve Price gibi custom dönecek sorgular için Models klasörü altında yeni bir ProductsEsentials nesnesi oluşturup DbSet e ekledik.
    var productsEsentials = await _context.ProdutsEssentials.FromSqlRaw("Select p.Id, p.Name, p.Price, pf.Color, pf.Width from Products p join ProductFeatures pf on p.Id = pf.Id").ToListAsync();
    productsEsentials.ForEach(x =>
    {
        Console.WriteLine($"Id:{x.Id}, Name: {x.Name}, Price: {x.Price}, Color: {x.Color}, Width: {x.Width}");
    });

}
#endif

