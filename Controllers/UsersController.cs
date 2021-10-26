using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_development_course.Data;
using web_development_course.Models;

namespace web_development_course.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Password,Email,DateOfBirth")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Email.ToLower() == user.Email.ToLower()
                        select u;

                if (q.Count() > 0)
                {
                    ViewData["Error"] = "Email is already exist";
                    return View(user);
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        [Route("users/json")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getUsersJson()
        { 
            var q = from user in _context.User 
                    select new { user.Id, user.FirstName, user.LastName, userType = user.UserType.ToString(), user.Email };

            var users = await q.ToListAsync();
            return Json(new 
            { 
                success = true, 
                users
            });
        }

        [HttpPost]
        [Route("users/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id, string role)
        {
            var q = await _context.User.FirstOrDefaultAsync(a => a.Id == id);
            if (q.Email == "admin@admin.com")
            {
                return Json(new
                {
                    success = false,
                    error = "cannot delete this user",
                });
            }
            if (q == null)
            {
                return Json(new
                {
                    success = false,
                    error = "user not found"
                });
            }
            _context.User.Remove(q);
            _context.SaveChanges();
            return Json(new
            {
                success = true,
            });
        }


        [HttpPost]
        [Route("users/ChangeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(int id , string role)
        {
            var q = await _context.User.FirstOrDefaultAsync(a => a.Id == id);
            if (q == null)
            {
                return Json(new
                {
                    success = false,
                    error = "user not found"
                });
            }
            if (q.Email == "admin@admin.com")
            {
                return Json(new
                {
                    success = false,
                    error = "cannot change this user role",
                });
            }
            q.UserType = Enum.Parse<UserLevel>(role);
            _context.Update(q);
            _context.SaveChanges();
            return Json(new
            {
                success = true,
            });
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] User user)
        {
            var q = from u in _context.User
                    where u.Email.ToLower() == user.Email.ToLower() && 
                          u.Password == user.Password
                    select u;

            if (q.Count() > 0)
            {
                var currentUser = q.First();
                loginUser(currentUser.FirstName + " " + currentUser.LastName, currentUser.Email, currentUser.UserType);
                return RedirectToAction("Index","home");
            } else
            {
                ViewData["Error"] = "Email / Password is incorrect";
                return View(user);
            }
        }
        
        //create the relevant cookie and connect the user for 30 min
        private async void loginUser(string fullName, string email, UserLevel type)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, type.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Password,Email,UserType,DateOfBirth")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
