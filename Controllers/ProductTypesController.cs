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
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductType.ToListAsync());
        }

        // GET: ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // GET: ProductTypes/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([Bind("Id,Size,Quantity,Color")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        // GET: Json Colors
        public IActionResult GetColors()
        {
            var Colors = _context.ProductColor.ToArrayAsync();
            return Json(new { success = true, Colors });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddGoods(int Quantity,ProductSize Size,int ColorId, string productName)
        {
            try
            {
                var product = await _context.Product.Include(p => p.ProductTypes).FirstOrDefaultAsync(p => p.Name.ToLower() == productName.ToLower());
                var Color = await _context.ProductColor.FirstOrDefaultAsync(p => p.Id == ColorId);
                ProductType pt = new ProductType() { ColorId = ColorId, Color = Color, Size = Size, Quantity = Quantity};
                if (product != null)
                {
                    foreach (var p in product.ProductTypes)
                    {
                        if (p.ColorId == Color.Id && p.Size == Size)
                        {
                            p.Quantity = pt.Quantity;
                            _context.Update(p);
                            await _context.SaveChangesAsync();
                            return Json(new { success = true });
                        }
                    }
                    pt.Product = product;
                    if (product.ProductTypes == null)
                        product.ProductTypes = new List<ProductType>();
                    product.ProductTypes.Append(pt);
                    _context.Add(pt);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true});
                }
            }
            catch
            {
                return Json(new { success = false});
            }
            return Json(new { success = false});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteGoods([Bind("id,Quantity,Size,Color")] ProductType pt, string productName)
        {
            try
            {
                var product = await _context.Product.Include(p => p.ProductTypes).FirstOrDefaultAsync(p => p.Name.ToLower() == productName.ToLower());
                if (product != null)
                {
                    foreach (var p in product.ProductTypes)
                    {
                        if (p.Size == pt.Size && p.Color == pt.Color)
                        {
                            _context.ProductType.Remove(p);
                            _context.Product.Update(product);
                            await _context.SaveChangesAsync();
                            return Json(new { success = true });
                        }
                    }
                }
            }
            catch
            {
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }


        // GET: ProductTypes/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Size,Quantity,Color")] ProductType productType)
        {
            if (id != productType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.Id))
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
            return View(productType);
        }

        // GET: ProductTypes/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST: ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductType.FindAsync(id);
            _context.ProductType.Remove(productType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductType.Any(e => e.Id == id);
        }
    }
}
