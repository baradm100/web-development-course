using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_development_course.Common;
using web_development_course.Data;
using web_development_course.Models;
using web_development_course.Models.OrderModels;
using web_development_course.WebServices;
using static System.Net.WebRequestMethods;

namespace web_development_course.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<int, string> Currencies;

        public OrdersController(ApplicationDbContext context, CartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            Currencies = new Dictionary<int, string>();
            Currencies.Add(1, "$");
            Currencies.Add(2, "₪");
            Currencies.Add(3, "€");
            Currencies.Add(4, "£");
        }

        // GET: Orders/Cart
        //<summary> the context use for know the current loged in user
        public async Task<IActionResult> Cart()
        {
            int dbUser = await isValidUserAsync();

            if (dbUser > 0)
            {
                var model = await (_context.OrderItem.Include(o => o.Order).
                            Include(o => o.ProductType).
                            Include(o => o.ProductType.Product).
                            Include(o => o.ProductType.Product.ProductImages).
                            Include(o => o.ProductType.Color).
                            Where(o => (o.Order.UserId == dbUser && o.Order.IsCart))).ToListAsync();

                return View(model);
            }

            return NotFound();

        }

        // GET: Orders/GetCart
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            int dbUser = await isValidUserAsync();
            if (dbUser <= 0)
            {
                return Unauthorized();
            }

            List<Object> cartItems = await _cartService.GetUpdatedCartByUserId(dbUser);
            return Json(cartItems);
        }

        // DELETE: Orders/DeleteOrderItem/5
        [HttpDelete]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            int dbUser = await isValidUserAsync();
            if (dbUser <= 0)
            {
                return Unauthorized();
            }
            OrderItem ItemToDelete = await _context.OrderItem
                .Include(oi => oi.Order)
                .Where(oi => oi.Id == id && oi.Order.UserId == dbUser)
                .FirstOrDefaultAsync();

            if (ItemToDelete == null)
            {
                return NotFound();
            }

            _context.OrderItem.Remove(ItemToDelete);
            await _context.SaveChangesAsync();

            return Json(new {success = true });
        }

        // GET: Orders/GetItemFinalPrice
        public async Task<IActionResult> GetItemFinalPrice(int orderId)
        {
            int dbUser = await isValidUserAsync();

            if (dbUser > 0)
            {
                var order = await (_context.OrderItem.Include(o => o.Order).
                                    Include(o => o.ProductType).
                                    Include(o => o.ProductType.Product).
                                    Where(o => o.Id == orderId && o.Order.IsCart && o.Order.UserId == dbUser).FirstAsync());

                float currency = 1;
                if (_httpContextAccessor.HttpContext.Request.Cookies["currency"] != null)
                    currency = float.Parse(_httpContextAccessor.HttpContext.Request.Cookies["currency"]);
                var currencySign = "$";
                if (_httpContextAccessor.HttpContext.Request.Cookies["currencySign"] != null)
                {
                    currencySign = Currencies[int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["CurrencySign"])];
                }

                var amount = order.Amount;
                var price = order.ProductType.Product.Price * currency;

                var discount = order.ProductType.Product.DiscountPercentage;
                discount = discount > 0 ? ((100 - discount) / 100) : 1;
                var totalPrice = price * discount * amount;

                order.TotalPrice = (double)totalPrice;
                _context.OrderItem.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true, data = new { totalPrice = totalPrice, sign = currencySign } });
            }

            return Json(new { success = false });
        }

        /// <summary>
        /// This Method return the orderItemId product data
        /// It use for get the data after user edit it
        /// </summary>
        public async Task<IActionResult> GetOrderItemData(int orderItemId)
        {
            var dbUser = await isValidUserAsync();

            if (dbUser > 0)
            {
                var order = await (_context.OrderItem.Include(o => o.Order).
                                    Include(o => o.ProductType).
                                    Include(o => o.ProductType.Product).
                                    Include(o => o.ProductType.Color).
                                    Where(o => o.Id == orderItemId && o.Order.IsCart && o.Order.UserId == dbUser).FirstAsync());

                var color = order.ProductType.Color.Color;
                var amount = order.Amount;
                var size = order.ProductType.Size;

                return Json(new { success = true, data = new { color = color, amount = amount, size = size } });
            }

            return Json(new { success = false });

        }

        // GET: Orders/GetSummary
        public async Task<IActionResult> GetSummary()
        {
            int dbUser = await isValidUserAsync();

            if (dbUser > 0)
            {
                var orders = await (_context.OrderItem.Include(o => o.Order).
                            Include(o => o.ProductType).
                            Include(o => o.ProductType.Product).
                            Where(o => (o.Order.UserId == dbUser && o.Order.IsCart))).ToListAsync();

                double midPrice = 0;
                double totalDiscount = 0;
                double totalPrice = 0;

                float currency = 1;
                if (_httpContextAccessor.HttpContext.Request.Cookies["currency"] != null)
                    currency = float.Parse(_httpContextAccessor.HttpContext.Request.Cookies["currency"]);
                var currencySign = "$";
                if (_httpContextAccessor.HttpContext.Request.Cookies["currencySign"] != null)
                {
                    currencySign = Currencies[int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["CurrencySign"])];
                }

                foreach (var data in orders)
                {
                    var price = data.Amount * data.ProductType.Product.Price * currency;
                    var discountNum = (data.ProductType.Product.DiscountPercentage / 100);

                    // if there is no discount the amount will be 0 
                    var discountAmount = price * discountNum;

                    totalPrice += (price - discountAmount);
                    midPrice += price;
                    totalDiscount += discountAmount;
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalPrice = totalPrice.ToString("#.##"),
                        midPrice = midPrice.ToString("#.##"),
                        saving = totalDiscount,
                        sign = currencySign,
                    }
                });
            }

            return Json(new { success = false });
        }

        // GET: Orders/json
        [HttpGet]
        [Route("orders/json")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> getOrdersJsonAsync(string product, string category, string username)
        {
            if (product == null)
                product = "";
            if (category == null || category == "Select")
                category = "";
            if (username == null)
                username = "";
            float currency = 1;
            if (_httpContextAccessor.HttpContext.Request.Cookies["currency"] != null)
                currency = float.Parse(_httpContextAccessor.HttpContext.Request.Cookies["currency"]);
            var currencySign = "$";
            if (_httpContextAccessor.HttpContext.Request.Cookies["currencySign"] != null)
            {
                currencySign = Currencies[int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["CurrencySign"])];
            }

            var orders = from order in _context.Order
                         join item in _context.OrderItem on order.Id equals item.OrderId
                         join user in _context.User on order.UserId equals user.Id
                         join productType in _context.ProductType on item.ProductTypeID equals productType.Id
                         join pCategory in _context.ProductCategory on productType.ProductId equals pCategory.ProductId
                         join c in _context.Category on pCategory.CategoryId equals c.Id
                         join p in _context.Product on productType.ProductId equals p.Id
                         where p.Name.Contains(product) && c.Name.Contains(category) && (user.FirstName + " " + user.LastName).Contains(username)
                         select new { 
                             order.Id, 
                             user.FirstName, 
                             user.LastName ,
                             date = order.Date.ToShortDateString(),
                             order.IsCart,
                             item.Amount,
                             totalPrice = (item.TotalPrice * currency).ToString("#.##") + currencySign,
                             p.Name,
                         };

            var o = await orders.ToListAsync();
            if (orders != null)
            {
                return Json(new
                {
                    success = true,
                    orders = o,
                }) ;
            }
            return Json(new { success = false });
        }

        // GET: Orders/json
        [HttpGet]
        [Route("orders/monthsummery/json")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> getOrdersByMonthJsonAsync()
        {
            var orders = from order in _context.Order
                         where order.IsCart == false
                         join item in _context.OrderItem on order.Id equals item.OrderId
                         join productType in _context.ProductType on item.ProductTypeID equals productType.Id
                         join pCategory in _context.ProductCategory on productType.ProductId equals pCategory.ProductId
                         join category in _context.Category on pCategory.CategoryId equals category.Id
                         group new { item.Amount } by order.Date.Month into sum
                         select new { sum.Key, amount = sum.Select(item => item.Amount).Sum() };

            return Json(new { success = true, orders = await orders.ToListAsync() });
        }



        // GET: Orders/Details/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        //Post: Orders/UpdateAmount
        [HttpPost]
        public async Task<IActionResult> UpdateAmount(int id, int amount)
        {
            OrderItem order = await _context.OrderItem.Include(oi => oi.ProductType).Where(oi => oi.Id == id).FirstOrDefaultAsync();

            if (order != null)
            {
                // Checks if we still have in stock the requested amount
                if (amount > order.ProductType.Quantity)
                {
                    return BadRequest(new { errorMessage = Consts.NOT_IN_STOCK });
                }

                order.Amount = amount;

                _context.OrderItem.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, textStatus = "didnt find order" });
        }

        //Post: Orders/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(double totalPrice, string? deliveryOption, string branchName, string phone, [Bind("Id,City,Street,BuildingNumber")] Address? address)
        {
            int dbUser = await isValidUserAsync();
            DeliveryOptions option = ((DeliveryOptions)DeliveryExtractor(deliveryOption));
            Address tempAdr = new Address();
            tempAdr.City = address.City;
            tempAdr.Street = address.Street;
            tempAdr.BuildingNumber = address.BuildingNumber;

            if (dbUser > 0)
            {
                 var order = await GetOrderByUser(dbUser);

                if (order != null)
                {
                    // check that this is the user order id
                    if (order.UserId == dbUser)
                    {
                        order.Delivery = option;
                        order.Date = DateTime.Now;
                        order.IsCart = false;
                        var user = await _context.User.FindAsync(dbUser);

                        // Check if the Address already exsit in the Db
                        Address dbAdr = await IsAddressinDbAsync(address);

                        if (dbAdr == null)
                        {
                            await _context.Address.AddAsync(tempAdr);
                            _context.SaveChanges();
                            // Call dbAdr again for getting it's correct id in the database 
                            dbAdr = await IsAddressinDbAsync(address);
                        }

                        if (dbAdr == null)
                        {
                            return Json(new { success = false, textStatus = "WE have Quantity problem" });
                        }

                        // check is the address is in the users adresses
                        if (user.Addresses == null)
                        {
                            user.Addresses = new List<Address> { dbAdr };
                            _context.User.Update(user);
                        }
                        else if (!user.Addresses.Contains(dbAdr))
                        {
                            user.Addresses.Append(dbAdr);
                            _context.User.Update(user);
                        }

                        // Check if the user phone numbr changed
                        if (user.Phone != phone)
                        {
                            user.Phone = phone;
                            _context.User.Update(user);
                        }

                        // check if the user is in the address users list 
                        if (address.Users == null)
                        {
                            address.Users = new List<User> { user };
                            _context.Address.Update(dbAdr);
                        }
                        else if (!address.Users.Contains(user))
                        {
                            address.Users.Append(user);
                            _context.Address.Update(address);
                        }

                        // Check there is enough items before commit the order
                        if (await UpdateQuantity(order))
                        {
                            _context.Order.Update(order);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return Json(new { success = false, textStatus = "WE have Quantity problem" });
                        }

                        return Json(new
                        {
                            success = true,
                            data = new
                            {
                                orderId = order.Id,
                                price = totalPrice,
                                branchName = branchName,
                                phone = phone
                            }
                        });
                    }
                }

                return Json(new { success = false, textStatus = "didnt find order" });
            }

            return Json(new { success = false, textStatus = "didnt find user" });
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([Bind("Id,Date")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int colorId, ProductSize size, int quantity)
        {
            int dbUser = await isValidUserAsync();
            if (dbUser <= 0)
            {
                return Unauthorized();
            }
            Order userCart = await _cartService.GetOrCreateCartForUser(dbUser);

            ProductType releventProductType = await _context.ProductType
                .Include(pd => pd.Product)
                .Where(pd => pd.ProductId == productId && pd.ColorId == colorId)
                .FirstOrDefaultAsync();

            if (releventProductType == null)
            {
                return NotFound(new { errorMessage = Consts.ITEM_NOT_FOUND });
            }

            if (quantity > releventProductType.Quantity)
            {
                return BadRequest(new { errorMessage = Consts.NOT_IN_STOCK });
            }

            OrderItem releventOrderItem = await GetOrCreateOrderItem(userCart.Id, releventProductType.Id);

            releventOrderItem.Amount = quantity;
            releventOrderItem.TotalPrice = releventProductType.Product.TotalPrice() * quantity;
            _context.OrderItem.Update(releventOrderItem);
            await _context.SaveChangesAsync();

            // TODO: Send the updated order deatiles
            return Json(new { });
        }

        private async Task<OrderItem> GetOrCreateOrderItem(int OrderId, int ProductTypeID)
        {
            OrderItem releventOrderItem = await _context.OrderItem
                .Where(oi => oi.OrderId == OrderId && oi.ProductTypeID == ProductTypeID)
                .FirstOrDefaultAsync();

            if (releventOrderItem == null)
            {
                releventOrderItem = new OrderItem { OrderId = OrderId, Amount = 0, TotalPrice = 0, ProductTypeID = ProductTypeID };
                _context.Add(releventOrderItem);
                await _context.SaveChangesAsync();
            }

            return releventOrderItem;
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        //GET: Orders/DeleteByUser
        public async Task<IActionResult> DeleteByUser(int? orderItemId)
        {
            if (orderItemId == null)
            {
                return NotFound();
            }

            var dbUser = isValidUserAsync();

            if (dbUser.Result > 0)
            {
                var orderItem = await (_context.OrderItem.Include(o => o.Order).
                                    Include(o => o.Order.OrderItems).
                                    Include(o => o.ProductType).
                                    Include(o => o.ProductType.Product).
                                    Where(o => o.Id == orderItemId && o.Order.IsCart).ToListAsync());

                // Makes sure the login user ask for the data
                if (orderItem != null && orderItem[0].Order.UserId == dbUser.Result)
                {
                    orderItem[0].Order.OrderItems.ToList().Remove(orderItem[0]);

                    var isLastItem = false;

                    // Check is this is the last orderItem in the order
                    if (orderItem[0].Order.OrderItems.ToList().Count() <= 1)
                    {
                        _context.Order.Remove(orderItem[0].Order);
                        isLastItem = true;
                    }

                    _context.OrderItem.Remove(orderItem[0]);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, isLastItem = isLastItem });
                }
            }

            return Json(new { success = false });
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        private async Task<int> isValidUserAsync()
        {
            string user = HttpContext.User.Identity.Name;

            if (user != null)
            {
                var dbUser = await _context.User.FirstOrDefaultAsync(v => (v.FirstName + " " + v.LastName) == user);

                if (dbUser != null)
                {
                    return dbUser.Id;
                }
                else
                {
                    // should be gust
                }
            }
            else
            {
                return -1;
            }

            return -1;
        }

        private async Task<Order> GetOrderByUser(int userid)
        {
            var q = await _context.Order.Where(o => o.UserId == userid && o.IsCart).FirstOrDefaultAsync();
            return q;
        }

        private int DeliveryExtractor(string delivery)
        {
            var helper = delivery.Split("_");
            return Int16.Parse(helper[1]);
        }

        private async Task<Address> IsAddressinDbAsync(Address address)
        {
            var adr = await _context.Address.FirstOrDefaultAsync(c => (c.City == address.City &&
                                                                    c.Street == address.Street &&
                                                                    c.BuildingNumber == address.BuildingNumber));

            return adr;
        }
        private async Task<bool> UpdateQuantity(Order order)
        {
            // should return a list of all order items that in the order and there products
            var orderItems = await (_context.OrderItem.Include(o => o.Order).
                                        Include(o => o.Order.OrderItems).
                                        Include(o => o.ProductType).
                                        Include(o => o.ProductType.Product).
                                        Where(o => o.Order.Id == order.Id).ToListAsync());

            foreach (var orderItem in orderItems)
            {
                var productType = await _context.ProductType.FindAsync(orderItem.ProductType.Id);

                if (productType.Quantity < orderItem.Amount)
                {
                    return false;
                }
                else
                {
                    productType.Quantity -= orderItem.Amount;
                }
            }
            return true;
        }

    }
}

