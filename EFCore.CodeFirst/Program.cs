using EFCore.CodeFirst.DataAccessLayer;
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
        Console.WriteLine($"{p.Id} : {p.Name}, Price:{p.Price}, Stok:{p.Stock}");
    });

    Console.ReadKey();
}