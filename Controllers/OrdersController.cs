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

            var dbUser = isValidUserAsync();

            if (dbUser.Result > 0)
            {
                var model = await (_context.OrderItem.Include(o => o.Order).
                            Include(o => o.ProductType).
                            Include(o => o.ProductType.Product).
                            Include(o => o.ProductType.Product.ProductImages).
                            Include(o => o.ProductType.Color).
                            Where(o => (o.Order.UserId == dbUser.Result && o.Order.IsCart))).ToListAsync();

                return View(model);
            }

            return NotFound();

        }

        // GET: Orders/GetItemFinalPrice
        public async Task<IActionResult> GetItemFinalPrice(int orderId)
        {
            var dbUser = isValidUserAsync();

            if (dbUser.Result > 0)
            {
                var order = await (_context.OrderItem.Include(o=>o.Order).
                                    Include(o=>o.ProductType).
                                    Include(o=>o.ProductType.Product).
                                    Where(o=>o.Id == orderId && o.Order.IsCart).ToListAsync());

                // Makes sure the login user ask for the data
                if(order != null && order[0].Order.UserId == dbUser.Result)
                {
                    var amount =  order[0].Amount;
                    var price = order[0].ProductType.Product.Price;
                    var discount = order[0].ProductType.Product.DiscountPercentage;
                    discount = discount > 0 ? ((100 - discount) / 100) : 1;
                    var totalPrice = price * discount * amount;

                    order[0].TotalPrice = (double) totalPrice;
                    _context.OrderItem.Update(order[0]);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, data = new { totalPrice = totalPrice, currecnyIndex = 0 }});
                }
            }

            return Json(new { success = false });
        }

        public async Task<IActionResult> GetOrderItemData(int orderItemId)
        {
            var dbUser = isValidUserAsync();

            if (dbUser.Result > 0)
            {
                var order = await (_context.OrderItem.Include(o => o.Order).
                                    Include(o => o.ProductType).
                                    Include(o => o.ProductType.Product).
                                    Include(o => o.ProductType.Color).
                                    Where(o => o.Id == orderItemId && o.Order.IsCart).ToListAsync());

                // Makes sure the login user ask for the data
                if (order != null && order[0].Order.UserId == dbUser.Result)
                {
                    var color = order[0].ProductType.Color.Color;
                    var amount = order[0].Amount;
                    var size = order[0].ProductType.Size;

                    return Json(new { success = true, data = new { color = color, amount = amount, size = size } });
                }
            }

            return Json(new { success = false });

        }

        // GET: Orders/GetSummary
        public async Task<IActionResult> GetSummary()
        {
            var dbUser = isValidUserAsync();

            if (dbUser.Result > 0)
            {
                var orders = await (_context.OrderItem.Include(o => o.Order).
                            Include(o => o.ProductType).
                            Include(o => o.ProductType.Product).
                            Where(o => (o.Order.UserId == dbUser.Result && o.Order.IsCart))).ToListAsync();

                double midPrice = 0;
                double totalDiscount = 0;
                double totalPrice = 0;

                foreach (var data in orders) {
                    var price = data.Amount * data.ProductType.Product.Price;
                    var discountNum = (data.ProductType.Product.DiscountPercentage / 100);

                    // if there is no discount the amount will be 0 
                    var discountAmount = price * discountNum;

                    // add logic in case we in a diffrent currency
                    totalPrice += (price - discountAmount); 
                    midPrice += price;
                    totalDiscount += discountAmount;
                }

                return Json(new { success = true, data = new { totalPrice = totalPrice,
                                                               midPrice = midPrice,
                                                               saving = totalDiscount,
                                                               //later change this!
                                                               currencyIndex = 0 } });
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

            if(order != null)
            {
                order.Amount = (int)amount;
                _context.OrderItem.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, textStatus = "didnt find order" }) ;
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
            if (orderItemId == null )
            {
                return NotFound();
            }

            var dbUser = isValidUserAsync();

            if (dbUser.Result > 0)
            {
                var orderItem = await (_context.OrderItem.Include(o => o.Order).
                                    Include(o=>o.Order.OrderItems).
                                    Include(o => o.ProductType).
                                    Include(o => o.ProductType.Product).
                                    Where(o => o.Id == orderItemId && o.Order.IsCart).ToListAsync());

                // Makes sure the login user ask for the data
                if (orderItem != null && orderItem[0].Order.UserId == dbUser.Result)
                {
                    orderItem[0].Order.OrderItems.ToList().Remove(orderItem[0]);
                    
                    // Check is this is the last orderItem in the order
                    if (orderItem[0].Order.OrderItems.ToList().Count() <= 1)
                    {
                        _context.Order.Remove(orderItem[0].Order);
                    }

                    _context.OrderItem.Remove(orderItem[0]);
                    await _context.SaveChangesAsync();

                    return Json(new {success = true });
                }
            }

            return Json( new {success = false });
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
