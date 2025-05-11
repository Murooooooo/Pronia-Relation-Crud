using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var products = _context.Products.Include(x => x.Category).Include(x => x.Photo).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Category = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile file)
        {
            ViewBag.Category = _context.Categories.ToList();
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(_env.WebRootPath, "Upload", filename);
            using FileStream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);

            Photo photo = new()
            {
                PhotoUrl = "/Upload/" + filename,

            };
            product.Photo = new List<Photo>();
            product.Photo.Add(photo);


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            ViewBag.Category = _context.Categories.ToList();
            var product = _context.Products.Include(x => x.Category).Include(x => x.Photo).FirstOrDefault(x => x.Id == id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product, IFormFile file, int id)
        {
            ViewBag.Category = _context.Categories.ToList();
            var oldproduct = _context.Products.Include(x => x.Category).Include(x => x.Photo).FirstOrDefault(x => x.Id == id);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            oldproduct.Name= product.Name;
            oldproduct.Description= product.Description;
            oldproduct.CategoryId = product.CategoryId;
            oldproduct.Price= product.Price;

            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(_env.WebRootPath, "Upload", filename);
            using FileStream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);

            Photo photo = new()
            {
                PhotoUrl = "/Upload/" + filename,

            };
            product.Photo = new List<Photo>();
            product.Photo.Add(photo);

            oldproduct.Photo=product.Photo;
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            _context.Remove(_context.Products.FirstOrDefault(x => x.Id == id));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
