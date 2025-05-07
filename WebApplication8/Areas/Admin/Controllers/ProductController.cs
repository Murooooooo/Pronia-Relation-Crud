using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.DAL;
using WebApplication8.Models;

namespace WebApplication8.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.Include(x=>x.Category).Include(x=>x.Photo).ToList();
            return View(products);
        }
        
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile file)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (file != null)
            {
                string filename = Guid.NewGuid() + file.FileName;
                var path = Path.Combine(_env.WebRootPath, "Upload", filename);

                await using FileStream fileStream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(fileStream);

                
                var photo = new Photo
                {
                    PhotoUrl = "/Upload/" + filename,
                    Product = product
                };

                
                await _context.Photos.AddAsync(photo);
            }

            
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
