using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_development_course.Common;
using web_development_course.Data;
using web_development_course.Models;
using web_development_course.Models.OrderModels;
using web_development_course.WebServices;

namespace web_development_course.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Cart
        //<summary> the context use for know the current loged in user
        public async Task<IActionResult> Cart()
        {
            // getting the update value of the currency
            CurrencyConverter converter = new CurrencyConverter();
            ViewData["ILS"] = converter.Ils;
            ViewData["GBP"] = converter.Gbp;
            ViewData["EUR"] = converter.Eur;

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

                var amount = order.Amount;
                var price = order.ProductType.Product.Price;
                var discount = order.ProductType.Product.DiscountPercentage;
                discount = discount > 0 ? ((100 - discount) / 100) : 1;
                var totalPrice = price * discount * amount;

                order.TotalPrice = (double)totalPrice;
                _context.OrderItem.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true, data = new { totalPrice = totalPrice, currecnyIndex = 0 } });
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

                foreach (var data in orders)
                {
                    var price = data.Amount * data.ProductType.Product.Price;
                    var discountNum = (data.ProductType.Product.DiscountPercentage / 100);

                    // if there is no discount the amount will be 0 
                    var discountAmount = price * discountNum;

                    // add logic in case we in a diffrent currency
                    totalPrice += (price - discountAmount);
                    midPrice += price;
                    totalDiscount += discountAmount;
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalPrice = totalPrice,
                        midPrice = midPrice,
                        saving = totalDiscount,
                        //later change this!
                        currencyIndex = 0
                    }
                });
            }

            return Json(new { success = false });
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
            var order = await _context.OrderItem.FindAsync(id);

            if (order != null)
            {
                order.Amount = (int)amount;
                _context.OrderItem.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, textStatus = "didnt find order" });
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
            Order userCart = await GetOrCreateCartForUser(dbUser);

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

        private async Task<Order> GetOrCreateCartForUser(int UserId)
        {
            Order userCart = await _context.Order
                .Where(o => o.UserId == UserId && o.IsCart)
                .FirstOrDefaultAsync();

            if (userCart == null)
            {
                // The user does not have a cart, creating a new one
                userCart = new Order { UserId = UserId, IsCart = true, OrderItems = new List<OrderItem>() };
                _context.Add(userCart);
                await _context.SaveChangesAsync();
            }

            return userCart;
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
    }

}
