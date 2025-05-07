using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.DAL;

namespace WebApplication8.Areas.Admin.Controllers
{
    [Area("Index")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.Include(x=>x.Category).Include(x=>x.Photo).ToList();
            return View(products);
        }
    }
}
