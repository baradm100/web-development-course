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
using web_development_course.WebServices;


namespace web_development_course.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TwitterApi twitterApi;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
            twitterApi = new TwitterApi();
        }

        // GET: Products?categoryId=5
        public async Task<IActionResult> Index(int? categoryId)
        {

            ViewBag.Colors = await _context.ProductColor.ToListAsync();
            ViewBag.shouldShowEdit = User.IsInRole("Admin") || User.IsInRole("Editor");

            Category[] RelevantCategories;
            if (categoryId == null)
            {
                RelevantCategories = await _context.Category.ToArrayAsync();
            }
            else
            {
                RelevantCategories = await _context.Category.Where(c => c.Id == categoryId || c.ParentCategoryId == categoryId).ToArrayAsync();
            }

            HashSet<int> RelevantCategoryIds = RelevantCategories.Select(c => c.Id).ToHashSet();

            var ProductsQuery = _context.Product
                    .Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes)
                    .ThenInclude(pt => pt.Color)
                    .Include(product => product.ProductCategories)
                    .Where(p => p.ProductCategories.Any(pc => RelevantCategoryIds.Contains(pc.CategoryId)));
            List<Product> ProductsToShow = await ProductsQuery.ToListAsync();
            return View(ProductsToShow);
        }

        public async Task<IActionResult> AdvancedSearch(string? productName, float? maximumPrice, int? categoryId)
        {

            ViewBag.Colors = await _context.ProductColor.ToListAsync();
            ViewBag.shouldShowEdit = User.IsInRole("Admin") || User.IsInRole("Editor");

            Category[] RelevantCategories;
            if (categoryId == null)
            {
                RelevantCategories = await _context.Category.ToArrayAsync();
            }
            else
            {
                RelevantCategories = await _context.Category.Where(c => c.Id == categoryId || c.ParentCategoryId == categoryId).ToArrayAsync();
            }
            float maximumPriceValue;
            if (maximumPrice == null)
            {
                maximumPriceValue = await _context.Product.MaxAsync(p => p.Price);
            } else
            {
                maximumPriceValue = (float)maximumPrice;
            }
            string productNameValue;
            if (productName == null)
            {
                productNameValue = "";
            } else
            {
                productNameValue = productName.ToLower();
            }

            HashSet<int> RelevantCategoryIds = RelevantCategories.Select(c => c.Id).ToHashSet();

            var ProductsQuery = _context.Product
                    .Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes)
                    .ThenInclude(pt => pt.Color)
                    .Include(product => product.ProductCategories)
                    .Where(p => p.ProductCategories.Any(pc => RelevantCategoryIds.Contains(pc.CategoryId)) && 
                    p.Name.ToLower().Contains(productNameValue) && p.Price <= maximumPriceValue);
            List<Product> ProductsToShow = await ProductsQuery.ToListAsync();
            return View("index",ProductsToShow);
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> EditorIndex(int? categoryId)
        {
            if (categoryId != null)
            {
                Category category = await _context.Category.FirstOrDefaultAsync(q => q.Id == categoryId);
                if (category != null)
                {
                    var products = from q in _context.ProductCategory
                                   join CategoryName in _context.Category on q.CategoryId equals CategoryName.Id
                                   where q.CategoryId == category.Id
                                   join p in _context.Product.Include(a => a.ProductImages).Include(a => a.ProductTypes)
                                   on q.ProductId equals p.Id
                                   where q.ProductId == p.Id
                                   orderby p.Id descending
                                   select p;
                    ViewBag.Colors = await _context.ProductColor.ToListAsync();
                    return View(await products.ToListAsync());
                }
            }
            ViewBag.Colors = await _context.ProductColor.ToListAsync();
            return View(await _context.Product.Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes).Include(product => product.ProductCategories).OrderByDescending(p => p.Id).ToListAsync());

        }

        // GET: Products/json
        [Route("products/json")]
        public async Task<IActionResult> getProductsJson(string? color, string? name)
        {

            ProductColor productColor = await _context.ProductColor.FirstOrDefaultAsync(c => color.Contains(c.Color));
            if(productColor != null)
            {
                var productType = from product in _context.Product
                               join type in _context.ProductType on product.Id equals type.Product.Id
                               where type.ColorId == productColor.Id && product.Name.ToLower() == name.ToLower()
                               select type;
                var productTypes = await productType.ToListAsync();
                return Json(new { success = true, types = productTypes });
            }
            return NotFound();

        }

        // GET: Products/json
        [Route("products/MaxPrice/json")]
        public async Task<IActionResult> getMaxPriceJson()
        {
            var maxPrice = await _context.Product.MaxAsync(p => p.Price);
            if (maxPrice != 0)
                return Json(new { success = true, max = maxPrice });
            return Json(new { success = false });
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> EditorIndexSearch(string? product)
        {
            try
            {
                ViewBag.Colors = await _context.ProductColor.ToListAsync();
                if (product != null)
                {
                    var q = _context.Product.Include(product => product.ProductTypes)
                        .Include(product => product.ProductCategories)
                        .Where(q => q.Name.ToLower().Contains(product.ToLower()));
                    return View("EditorIndex",await q.ToListAsync());
                }
                return View("EditorIndex", await _context.Product.Include(product => product.ProductImages)
                        .Include(product => product.ProductTypes).Include(product => product.ProductCategories).ToListAsync());
            } catch
            {
                return NotFound();
            }
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

        // GET: Products/GetProduct/{name}
        [HttpPost]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Product.Include(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            var categories = from q in _context.ProductCategory
                             join CategoryName in _context.Category on q.CategoryId equals CategoryName.Id
                             where q.ProductId == id
                             select CategoryName.Name;
            var imagesNames = product.ProductImages.Select(m => new { m.Name, m.ImageData, m.Id }).ToList();
            if (product == null)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true, name = product.Name, id = id, discount = product.DiscountPercentage, categories = categories.ToList(), price = product.Price, images = imagesNames });
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddProduct([Bind("Id,Price,Name,DiscountPercentage")] Product product, List<string> categories)
        {
            try
            {
                // adding the product to the DB
                var pro = _context.Product.FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                if (pro != null)
                {
                    return Json(new { success = false, errorDetails = "Product name already exist" });
                }
                if (product.ProductImages == null)
                    product.ProductImages = new List<ProductImage>();
                if (product.ProductTypes == null)
                    product.ProductTypes = new List<ProductType>();
                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                foreach (var cat in categories)
                {
                    Category category = _context.Category.FirstOrDefault(c => c.Name == cat);
                    if (category != null)
                    {
                        // adding category and product to the many to many table: ProductCategory.
                        ProductCategory bind = new ProductCategory();
                        bind.CategoryId = category.Id;
                        bind.ProductId = product.Id;
                        category.ProductCategories.Append(bind);
                        product.ProductCategories.Append(bind);
                        _context.Category.Update(category);
                        _context.ProductCategory.Add(bind);
                    }
                }
                
                await _context.SaveChangesAsync();
                float priceAfterDiscount = product.Price * ((100 - product.DiscountPercentage) / 100);
                try
                {
                    await twitterApi.PostTweetAsync("ClothIt has a new Product: '" + product.Name + "' just in " + priceAfterDiscount + " come and check it!");
                } catch
                {}
                return Json(new { success = true, productId = product.Id });
            }
            catch
            {
                return Json(new { success = false, errorDetails = "sorry, we had a problem in server" });
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
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            try
            {
                var productImage = _context.ProductImage.Include(p => p.Product).FirstOrDefault(p => p.Id == id);
                _context.ProductImage.Remove(productImage);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        private ProductImage UploadImageToDb(IFormFile files)
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
        public async Task<IActionResult> deleteProduct(int id)
        {
            try
            {
                Product product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
                var productsCategory = await _context.ProductCategory.Where(a => a.ProductId == product.Id).ToListAsync();
                if (productsCategory != null)
                {
                    foreach (var pc in productsCategory)
                    {
                        _context.ProductCategory.Remove(pc);
                    }
                }
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> EditProduct(int id, int Price, string Name, int DiscountPercentage, List<string> Categories)
        {
            // Getting: all attributes of product and compare it to the exist one. if something had been change Update the exist product.
            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                    return NotFound();
                product.Name = Name;
                product.Price = Price;
                product.DiscountPercentage = DiscountPercentage;
                var pc = _context.ProductCategory.Where(q => q.ProductId == id);
                foreach (var cat in pc)
                {
                    _context.ProductCategory.Remove(cat);
                }
                foreach (var cat in Categories)
                {
                    Category category = _context.Category.FirstOrDefault(c => c.Name == cat);
                    if (category != null)
                    {
                        ProductCategory bind = new ProductCategory();
                        bind.CategoryId = category.Id;
                        bind.ProductId = product.Id;
                        category.ProductCategories.Append(bind);
                        product.ProductCategories.Append(bind);
                        _context.Category.Update(category);
                        _context.ProductCategory.Add(bind);
                    }
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false, errorDetails = "sorry, we had a problem in server" });
            }
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
