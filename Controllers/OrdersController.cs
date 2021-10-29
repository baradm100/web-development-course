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
using Newtonsoft.Json;
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

            int dbUser = await IsValidUserAsync();

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
            int dbUser = await IsValidUserAsync();

            if (dbUser > 0)
            {
                var order = await (_context.OrderItem.Include(o=>o.Order).
                                    Include(o=>o.ProductType).
                                    Include(o=>o.ProductType.Product).
                                    Where(o=>o.Id == orderId && o.Order.IsCart && o.Order.UserId == dbUser).FirstAsync());
           
                var amount =  order.Amount;
                var price = order.ProductType.Product.Price;
                var discount = order.ProductType.Product.DiscountPercentage;
                discount = discount > 0 ? ((100 - discount) / 100) : 1;
                var totalPrice = price * discount * amount;

                order.TotalPrice = (double) totalPrice;
                _context.OrderItem.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true, data = new { totalPrice = totalPrice, currecnyIndex = 0 }});
            }

            return Json(new { success = false });
        }

        /// <summary>
        /// This Method return the orderItemId product data
        /// It use for get the data after user edit it
        /// </summary>
        public async Task<IActionResult> GetOrderItemData(int orderItemId)
        {
            var dbUser = await IsValidUserAsync();

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
            int dbUser = await IsValidUserAsync();

            if (dbUser > 0)
            {
                var orders = await (_context.OrderItem.Include(o => o.Order).
                            Include(o => o.ProductType).
                            Include(o => o.ProductType.Product).
                            Where(o => (o.Order.UserId == dbUser && o.Order.IsCart))).ToListAsync();

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

        //Post: Orders/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int? orderId, double totalPrice, string? deliveryOption, string phone ,[Bind("Id,City,Street,BuildingNumber")] Address? address)
        {
            int dbUser = await IsValidUserAsync();
            DeliveryOptions option = ((DeliveryOptions)DeliveryExtractor(deliveryOption));
            Address tempAdr = new Address();
            tempAdr.City = address.City;
            tempAdr.Street = address.Street;
            tempAdr.BuildingNumber = address.BuildingNumber;

            if (dbUser > 0)
            {
                if (orderId != null)
                {
                    var order = await _context.Order.FindAsync(orderId);
                    
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
                                return Json(new { success = false, textStatus = "WE have qutatiy problem" });
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
                                return Json(new { success = false, textStatus = "WE have qutatiy problem" });
                            }

                            return Json(new
                            {
                                success = true,
                                data = new
                                {
                                    orderId = order.Id,
                                    price = totalPrice,
                                }
                            });
                        }
                    }
                }

                return Json(new { success = false, textStatus = "didnt find order" });
            }

            return Json(new { success = false, textStatus = "didnt find user"});
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

            var dbUser = IsValidUserAsync();

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

                    var isLastItem = false;

                    // Check is this is the last orderItem in the order
                    if (orderItem[0].Order.OrderItems.ToList().Count() <= 1)
                    {
                        _context.Order.Remove(orderItem[0].Order);
                        isLastItem = true;
                    }
                    _context.OrderItem.Remove(orderItem[0]);
                    await _context.SaveChangesAsync();

                    return Json(new {success = true, isLastItem = isLastItem });
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

        private async Task<int> IsValidUserAsync()
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

            foreach(var orderItem in orderItems)
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
