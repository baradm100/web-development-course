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

        private readonly UserManager<User> _userManager;


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

        public void MyMethod(Microsoft.AspNetCore.Http.HttpContext context)
        {
           

            // Other code
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

            string user = HttpContext.User.Identity.Name;
            var authanticated = HttpContext.User.Identity.IsAuthenticated;
            var userAuthLevel = HttpContext.User.Identity.AuthenticationType;
             
            if (user != null) {
                var dbUser = await _context.User.FirstOrDefaultAsync(v => (v.FirstName + " " + v.LastName) == user);

                if (dbUser != null)
                {
                    var a = await (from order in _context.Order
                                   join item in _context.OrderItem on order.Id equals item.OrderId
                                   select order).ToListAsync();

                    var b = await (from item in _context.OrderItem
                            join productType in _context.ProductType on item.ProductTypeID equals productType.Id
                            select item).ToListAsync();

                    // suppose to get all the product that the loggon user add to cart
                    var model = await (from order in _context.Order
                                       join item in _context.OrderItem on order.Id equals item.OrderId
                                       join productType in _context.ProductType on item.ProductTypeID equals productType.Id
                                       join product in _context.Product on productType.ProductId equals product.Id
                                       where (order.UserId == dbUser.Id && order.IsCart == true)
                                       select order).ToListAsync();

                    return View(model);
                }

                return NotFound();
            }
            else
            {
                return View(await _context.Order.ToListAsync());
                // dont know
            }
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
    }
}
