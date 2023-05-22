using EFCore.MVC.DAL;
using EFCore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context = null)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productList = _context.Products.ToList();
            return View(productList);
        }

        public async Task<IActionResult> Update(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.First();

                var currentProduct = exceptionEntry.Entity as Product;
                var databaseValue = exceptionEntry.GetDatabaseValues();
                var databaseProduct = databaseValue.ToObject() as Product;
                var clientValues = exceptionEntry.CurrentValues;


                if (databaseValue == null)
                {
                    ModelState.AddModelError(string.Empty, "Bu Ürün Başka Bir Kullanıcı Tarafından Silinmiştir");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bu Ürün Başka Bir Kullanıcı Tarafından Güncellenmiştir");
                    ModelState.AddModelError(string.Empty, $"Güncellenen Değer: Name:{databaseProduct.Name}, Price:{databaseProduct.Price}, Stock: {databaseProduct.Stock}");
                    
                }
                return View(product);
            }

        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = _context.Products.FirstOrDefault(x=>x.Id == Id);
             _context.Products.Remove(product);
            _context.SaveChanges();
            return View(product);
        }
    }
}
