
//========== Aşağıdaki kod bloğunu Package Manager Console ekranına yapıştırdığımız takdirde Dbdeki bütün tablolara karşılık gelecek entitileri belirttiğimiz klasör altında otomatik olarak oluşturur. =============

//Scaffold-DbContext "Data Source=.;Initial Catalog=EFCoreDbFirst;User ID=sa;Password=0123456789aA.;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

using EFCore.DataBaseFirstByScaffold.Models;
using Microsoft.EntityFrameworkCore;

using (var dbContext = new EFCoreDbFirstContext())
{
    var products = await dbContext.Products.ToListAsync();
	products.ForEach (p =>
	{
		Console.WriteLine($"Id:{p.Id}, Name:{p.Name}, Price:{p.Price}, Stock:{p.Stock}");
	});

}
