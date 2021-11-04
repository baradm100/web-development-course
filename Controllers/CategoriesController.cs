using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_development_course.Data;
using web_development_course.Models;

namespace web_development_course.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Categories/json
        [HttpGet]
        [Route("Categories/json")]
        public async Task<IActionResult> GetCategories()
        {
            Category[] Categories = await _context.Category.ToArrayAsync();
            return Json(new { success = true, Categories });
        }

        [HttpPost]
        [Route("Categories/ProductCategories")]
        public async Task<IActionResult> GetProductCategories([FromForm]string name)
        {
            try
            {
                var NametoId = await _context.Product.FirstAsync(p => p.Name == name);
                int id = NametoId.Id;
                var Categories = from q in _context.ProductCategory
                                 join CategoryName in _context.Category on q.CategoryId equals CategoryName.Id
                                 where q.ProductId == id
                                 select CategoryName.Name;
                return Json(new { success = true, categories = Categories.ToList(), Name = NametoId.Name });

            } catch
            {
                return Json(new { success = false });
            }
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: categoriesTree/json
        [Route("categoriesTree/json")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> getCategoriesTreeJson()
        {
            var categories = from category in _context.Category
                             select new { id = category.Id, name = category.Name, parent = category.ParentCategory.Name };
            return Json(new { success = true, categories = await categories.ToListAsync() });
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        [Route("Categories")]
        public async Task<IActionResult> AddCategory([Bind("Id,Name")] Category category, string parent)
        {
            try
            {
                Category c = await _context.Category.FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower());
                if (c != null)
                {
                    return Json(new { fail = true, success = false, textStatus = "Category is already exist!" });
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                if (parent == null)
                {
                    return Json(new { success = true });

                }
                Category p = await _context.Category.FirstOrDefaultAsync(c => c.Name == parent);
                Category n = await _context.Category.FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower());
                n.ParentCategoryId = p.Id;
                n.ParentCategory = p;
                _context.Update(n);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            } catch
            {
                return Json( new { success = false });
            }
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
