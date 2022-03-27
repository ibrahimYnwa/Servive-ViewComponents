using LessonMigration.Data;
using LessonMigration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LessonMigration.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(m =>!m.IsDeleted).ToListAsync();
            return View(categories);
        }
        public IActionResult Detail(int id)
        {
            var category = _context.Categories.FirstOrDefault(m => m.Id == id);
            return View(category);
           
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task <IActionResult> Create(Category category )
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExist = _context.Categories.Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu category artiq movcuddur");
                return View();
            }

           await  _context.Categories.AddAsync(category);
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            return Json(new
            {
                action = "Edit",
                Id = id
            });
        }
        public IActionResult Delete(int id)
        {
            return Json(new
            {
                action = "Delete",
                Id = id
            });
        }
    }
}
