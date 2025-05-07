using Microsoft.AspNetCore.Mvc;
using WebApplication8.DAL;
using WebApplication8.Models;

namespace WebApplication8.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {
            var category = _context.Categories.ToList();    
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var teachers = _context.Categories.FirstOrDefault(x => x.Id == id);
            return View(teachers);
        }
        [HttpPost]
        public IActionResult Update(int id,Category category)
        {
            var oldteacher = _context.Categories.FirstOrDefault(x => x.Id == id);

            oldteacher.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var teacher = _context.Categories.FirstOrDefault(x => x.Id == id);
            _context.Remove(teacher);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
       
    }
}
