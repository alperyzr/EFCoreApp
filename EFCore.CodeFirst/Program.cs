﻿
//#define ile bir değişken tanımlamamıza ve aşağıdaki if komutları ile hangi using in çalışması gerektiğinin kontrolü yapılr
#define ManyToManyDataAdded

using EFCore.CodeFirst.DataAccessLayer;
using EFCore.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;

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
#endif
