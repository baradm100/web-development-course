using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_development_course.Common;
using web_development_course.Data;
using web_development_course.Models;
using web_development_course.Models.ProductModels;

namespace web_development_course.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Admin") || User.IsInRole("Edtior"))
                return RedirectToAction("EditorIndex", await _context.Product.Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes).Include(product => product.ProductCategories)
                    .ToListAsync());

            return View(await _context.Product.Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes).Include(product => product.ProductCategories)
                    .ToListAsync());



        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(List<IFormFile> files, [Bind("Id,Price,Name,DiscountPercentage,Categories")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    var pro = _context.Product.FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                    if (pro != null)
                    {
                        ViewBag.ProductExistError = Consts.ProductExsistError;
                        return View();
                    }
                    if (product.ProductImages == null)
                        product.ProductImages = new List<ProductImage>();
                    if (product.ProductTypes == null)
                        product.ProductTypes = new List<ProductType>();
                    foreach (var file in files)
                    {
                    ProductImage img = UploadImageToDb(file);
                    product.ProductImages.Append(img);
                    img.Product = product;
                    img.ProductId = product.Id;
                    _context.ProductImage.Add(img);
                    }
                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AddGoods", "ProductTypes", new {product.Id});
                }
                ViewBag.ImageError = Consts.ProductImageMissingError;
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddProduct([Bind("Id,Price,Name,DiscountPercentage")] Product product, List<string> categories)
        {
            {
                var pro = _context.Product.FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                if (pro != null)
                {
                    return Json(new { success = false });
                }
                if (product.ProductImages == null)
                    product.ProductImages = new List<ProductImage>();
                if (product.ProductTypes == null)
                    product.ProductTypes = new List<ProductType>();
                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                Product p = _context.Product.First(p => p.Name.ToLower() == product.Name.ToLower());
                foreach (var cat in categories)
                {
                    Category category = _context.Category.FirstOrDefault(c => c.Name == cat);
                    if (category != null)
                    {
                        ProductCategory bind = new ProductCategory();
                        bind.CategoryId = category.Id;
                        bind.Categories.Append(category);
                        bind.ProductId = p.Id;
                        bind.Products.Append(p);
                        category.ProductCategories.Append(bind);
                        product.ProductCategories.Append(bind);
                        _context.Category.Update(category);
                        _context.ProductCategory.Add(bind);
                    }
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true, productId = product.Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddProductImage(int id)
        {
            try
            {
                var product = _context.Product.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id);
                foreach (var image in this.Request.Form.Files)
                {
                    ProductImage img = UploadImageToDb(image);
                    product.ProductImages.Append(img);
                    img.Product = product;
                    img.ProductId = product.Id;
                    _context.ProductImage.Add(img);
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            } catch
            {
            return Json(new { success = false });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> deleteProduct(int id)
        {
            try
            {
                Product product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public ProductImage UploadImageToDb(IFormFile files)
        {
            var fileName = Path.GetFileName(files.FileName);
            ProductImage img = new ProductImage();
            img.Name = fileName;
            var ms = new MemoryStream();
            files.CopyTo(ms);
            img.ImageData = ms.ToArray();
            return img;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddGoods(List<IFormFile> files, [Bind("Id,Price,Name,DiscountPercentage")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    var pro = _context.Product.FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                    if (pro != null)
                    {
                        ViewBag.ProductExistError = Consts.ProductExsistError;
                        return View();
                    }
                    if (product.ProductImages == null)
                        product.ProductImages = new List<ProductImage>();
                    if (product.ProductTypes == null)
                        product.ProductTypes = new List<ProductType>();
                    foreach (var file in files)
                    {
                        ProductImage img = UploadImageToDb(file);
                        product.ProductImages.Append(img);
                        img.Product = product;
                        img.ProductId = product.Id;
                        _context.ProductImage.Add(img);
                    }
                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();
                    return Json(new { Result = "success" });
                }
                ViewBag.ImageError = Consts.ProductImageMissingError;
                return View();
            }
            return View();
        }

       

        public ActionResult GetLastProductId()
        {
            int lastId = _context.Product.OrderByDescending(p => p.Id).First().Id;
            return Json(new { Id = lastId });
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Name,DiscountPercentage")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
