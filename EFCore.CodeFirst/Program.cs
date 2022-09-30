using EFCore.CodeFirst.DataAccessLayer;
using EFCore.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;

//DbContext in appsettingsten okunması için classın Build methodu çağrılır.
DbContextInitializer.Build();

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