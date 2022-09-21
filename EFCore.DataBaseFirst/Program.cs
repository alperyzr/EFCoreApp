using EFCore.DataBaseFirst.DataAccessLayer;
using Microsoft.EntityFrameworkCore;


//Console uygulaması ayakta olduğu sürece, ilk seferde initialize edilsin ve daha sonrasında işlemleri devam etsin amacında kullanılır
DbContextInitializer.Build();


//using bir işlem dispose olduktan sonra otomatik olarak connection bağlantısının kapatılması ve memory i kastırmaması için kullanılır.
//Appsettings dosyasındaki SqlConnection ı kullanmak için yazılan classlar
using (var _context = new AppDbContext(DbContextInitializer.OptionsBuilder.Options))
{
    var products = await _context.Products.ToListAsync();
    products.ForEach(p =>
    {
        Console.WriteLine($"{p.Id} : {p.Name}, Price:{p.Price}, Stok:{p.Stock}");
    });
    
    Console.ReadKey();
}
