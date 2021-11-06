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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<int, string> Currencies;

        public ProductsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            twitterApi = new TwitterApi();
            _httpContextAccessor = httpContextAccessor;
            Currencies = new Dictionary<int, string>();
            Currencies.Add(1, "$");
            Currencies.Add(2, "₪");
            Currencies.Add(3, "€");
            Currencies.Add(4, "£");
        }

        // GET: Products?categoryId=5
        public async Task<IActionResult> Index(int? categoryId, string? categoryName, int? index)
        {
            int pageSize = 10;
            ViewBag.Colors = await _context.ProductColor.ToListAsync();
            ViewBag.shouldShowEdit = User.IsInRole("Admin") || User.IsInRole("Editor");

            if (categoryName != null)
            {
                var cat = await _context.Category.FirstOrDefaultAsync(p => p.Name == categoryName);
                if (cat != null)
                    categoryId = cat.Id;
                    ViewBag.categoryId = categoryId.ToString();
            }
            Category[] RelevantCategories;
            if (categoryId == null)
            {
                RelevantCategories = await _context.Category.ToArrayAsync();
                ViewBag.categoryId = "";
            }
            else
            {
                RelevantCategories = await _context.Category.Where(c => c.Id == categoryId || c.ParentCategoryId == categoryId).ToArrayAsync();
                ViewBag.categoryId = "";
            }
            int i = 1;
            int skipItems = 0;
            if (index > 1 && index != null)
            {
                i = (int)index;
                skipItems = (int)((index -1 ) * pageSize);
            }
            ViewBag.index = i;

            HashSet<int> RelevantCategoryIds = RelevantCategories.Select(c => c.Id).ToHashSet();
            int numOfPages = _context.Product.Count(p => p.ProductCategories.Any(pc => RelevantCategoryIds.Contains(pc.CategoryId))) / pageSize;
            ViewBag.numOfPages = numOfPages;

            var ProductsQuery = _context.Product
                    .Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes)
                    .ThenInclude(pt => pt.Color)
                    .Include(product => product.ProductCategories)
                    .Where(p => p.ProductCategories.Any(pc => RelevantCategoryIds.Contains(pc.CategoryId)))
                    .Skip(skipItems).Take(pageSize);
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
            }
            else
            {
                maximumPriceValue = (float)maximumPrice;
            }
            string productNameValue;
            if (productName == null)
            {
                productNameValue = "";
            }
            else
            {
                productNameValue = productName.ToLower();
            }
            ViewBag.index = -1;
            ViewBag.numOfPages = 0;

            HashSet<int> RelevantCategoryIds = RelevantCategories.Select(c => c.Id).ToHashSet();

            var ProductsQuery = _context.Product
                    .Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes)
                    .ThenInclude(pt => pt.Color)
                    .Include(product => product.ProductCategories)
                    .Where(p => p.ProductCategories.Any(pc => RelevantCategoryIds.Contains(pc.CategoryId)) &&
                    p.Name.ToLower().Contains(productNameValue) && (1 - (p.DiscountPercentage / 100)) * p.Price <= maximumPriceValue);
            List<Product> ProductsToShow = await ProductsQuery.ToListAsync();
            return View("index", ProductsToShow);
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> EditorIndex(int? categoryId, int? index)
        {
            int pageSize = 10;
            int i = 1;
            int skipItems = 0;
            if (index > 1 && index != null)
            {
                i = (int)index;
                skipItems = (int)((index - 1) * pageSize);
            }
            ViewBag.index = i;
            int numOfPages = 1;
            ViewBag.categoryId = "";
            if (categoryId != null)
            {
                ViewBag.index = i;
                Category category = await _context.Category.FirstOrDefaultAsync(q => q.Id == categoryId);
                if (category != null)
                {
                    ViewBag.categoryId = category.Id.ToString();
                    var products = from q in _context.ProductCategory
                                   join CategoryName in _context.Category on q.CategoryId equals CategoryName.Id
                                   where q.CategoryId == category.Id
                                   join p in _context.Product.Include(a => a.ProductImages).Include(a => a.ProductTypes)
                                   on q.ProductId equals p.Id
                                   where q.ProductId == p.Id
                                   orderby p.Id descending
                                   select p;
                    numOfPages = products.Count() / pageSize;
                    ViewBag.numOfPages = numOfPages;
                    ViewBag.Colors = await _context.ProductColor.ToListAsync();
                    return View(await products.Skip(skipItems).Take(pageSize).ToListAsync());
                }
            }
            var defaultProducts = _context.Product.Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes).Include(product => product.ProductCategories).OrderByDescending(p => p.Id);
            ViewBag.numOfPages = defaultProducts.Count() / pageSize;
            ViewBag.Colors = await _context.ProductColor.ToListAsync();
            return View(await defaultProducts.Skip(skipItems).Take(pageSize).ToListAsync());
        }

        // GET: Products/json
        [Route("products/json")]
        public async Task<IActionResult> getProductsJson(string? color, string? name)
        {

            ProductColor productColor = await _context.ProductColor.FirstOrDefaultAsync(c => color.Contains(c.Color));
            if (productColor != null)
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

        [Route("products/MaxPrice/json")]
        public async Task<IActionResult> getMaxPriceJson()
        {
            int numOfProducts = await _context.Product.CountAsync();
            if (numOfProducts == 0)
            {
                // No products in the DB
                return Json(new { success = false });
            }

            var maxPrice = await _context.Product.MaxAsync(p => p.Price);
            float currency = 1;
            if (_httpContextAccessor.HttpContext.Request.Cookies["currency"] != null)
                currency = float.Parse(_httpContextAccessor.HttpContext.Request.Cookies["currency"]);
            var currencySign = "$";
            if (_httpContextAccessor.HttpContext.Request.Cookies["currencySign"] != null)
            {
                currencySign = Currencies[int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["CurrencySign"])];
            }

            if (maxPrice != 0)
                return Json(new { success = true, max = maxPrice * currency, sign = currencySign });
            return Json(new { success = false });
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> EditorIndexSearch(string? product)
        {
            ViewBag.index = 1;
            ViewBag.categoryId = "";
            ViewBag.numOfPages = 1;
            try
            {
                ViewBag.Colors = await _context.ProductColor.ToListAsync();
                if (product != null)
                {
                    var q = _context.Product.Include(product => product.ProductTypes)
                        .Include(product => product.ProductCategories).Include(product => product.ProductImages)
                        .Where(q => q.Name.ToLower().Contains(product.ToLower()));
                    return View("EditorIndex", await q.ToListAsync());
                }
                return View("EditorIndex", await _context.Product.Include(product => product.ProductImages)
                        .Include(product => product.ProductTypes).Include(product => product.ProductCategories).ToListAsync());
            }
            catch
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

            Product product = await _context.Product
                    .Include(product => product.ProductImages)
                    .Include(product => product.ProductTypes)
                    .ThenInclude(pt => pt.Color)
                    .Include(product => product.ProductCategories)
                    .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            Dictionary<ProductSize, int> SizesAndCounts = new Dictionary<ProductSize, int>();
            Dictionary<ProductColor, int> ColorsAndCounts = new Dictionary<ProductColor, int>();
            Dictionary<int, Dictionary<ProductSize, int>> SizesCountByColorIds = new Dictionary<int, Dictionary<ProductSize, int>>();

            foreach (ProductSize size in Enum.GetValues(typeof(ProductSize)))
            {
                SizesAndCounts[size] = 0;
            }

            foreach (ProductType type in product.ProductTypes)
            {
                if (!ColorsAndCounts.ContainsKey(type.Color))
                {
                    ColorsAndCounts[type.Color] = 0;
                }

                if (!SizesCountByColorIds.ContainsKey(type.Color.Id))
                {
                    SizesCountByColorIds[type.Color.Id] = new Dictionary<ProductSize, int>();
                    foreach (ProductSize size in Enum.GetValues(typeof(ProductSize)))
                    {
                        SizesCountByColorIds[type.Color.Id][size] = 0;
                    }
                }

                SizesAndCounts[type.Size] += type.Quantity;
                ColorsAndCounts[type.Color] += type.Quantity;
                SizesCountByColorIds[type.Color.Id][type.Size] += type.Quantity;
            }
        
            ViewBag.SizesAndCounts = SizesAndCounts;
            ViewBag.ColorsAndCounts = ColorsAndCounts;
            ViewBag.SizesCountByColorIds = SizesCountByColorIds;

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
                }
                catch
                { }
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

        // GET: Orders/json
        [HttpGet]
        [Route("products/summery/json")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> getProductSummeryJsonAsync()
        {
            var orders = from order in _context.Order
                         where order.IsCart == false
                         join item in _context.OrderItem on order.Id equals item.OrderId
                         join productType in _context.ProductType on item.ProductTypeID equals productType.Id
                         join pCategory in _context.ProductCategory on productType.ProductId equals pCategory.ProductId
                         join category in _context.Category on pCategory.CategoryId equals category.Id
                         group new { item.Amount } by category.Name into sum
                         select new { sum.Key, amount = sum.Select(item => item.Amount).Sum() };

            
            return Json(new { success = true, orders = await orders.ToListAsync() });
        }

        // GET: Orders/json
        [HttpGet]
        [Route("/products/availablestock/json/")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> getAvialableStockSummeryJsonAsync()
        {
            var products = from product in _context.Product
                         join productType in _context.ProductType on product.Id equals productType.Id
                         where productType.Quantity > 0
                         join pCategory in _context.ProductCategory on product.Id equals pCategory.ProductId
                         join category in _context.Category on pCategory.CategoryId equals category.Id
                         group new { productType.Quantity } by category.Name into sum
                         select new { sum.Key, amount = sum.Select(item => item.Quantity).Sum() };


            return Json(new { success = true, products = await products.ToListAsync() });
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
