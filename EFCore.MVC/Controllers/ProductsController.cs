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

        public async  Task<IActionResult> Index()
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
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
