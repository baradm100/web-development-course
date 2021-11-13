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
    public class BranchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index(string name)
        {
            if (_context.Branch.Count() > 0)
            {
                ViewData["ErrorTitle"] = "Sorry, we dont have any physical store match to your search";
                ViewData["ErrorParagraph"] = "But hey! look for other one";
            }
            else
            {
                ViewData["ErrorTitle"] = "Sorry, we dont have any physical store yet";
                ViewData["ErrorParagraph"] = "But dont worry! we are working on it";
            }
            if (name != null)
            {
                var q = await _context.User.FirstOrDefaultAsync(c => (c.FirstName + " " + c.LastName) == User.Identity.Name);
                var SearchedBranches = _context.Branch.Include(b => b.Address).Include(d => d.OpeningHours)
                    .Where(p => p.Name.ToLower().Contains(name.ToLower()) ||
                    p.Address.City.Contains(name.ToLower()) || p.Address.Street.ToLower().Contains(name.ToLower()));
                return View("Index", await SearchedBranches.ToListAsync());
            }
            var applicationDbContext = _context.Branch.Include(b => b.Address).Include(d=>d.OpeningHours);
            return View("Index", await applicationDbContext.ToListAsync());
        }

        // GET: Branches/GetJson
        public async Task<IActionResult> GetData()
        {
            var applicationDbContext = _context.Branch.Include(b => b.Address);
            return Json(await applicationDbContext.ToListAsync());
        }

        // GET: Branches/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Address, "Id,Name,City,Street,BuildingNumber,Longitude,Latitude");
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([Bind("Id,City,Street,BuildingNumber,Longitude,Latitude")] Address address, [Bind("Id,Name")] string name, List<OpeningHour> openingHours)
        {
            var ExistBranch = await _context.Branch.FirstOrDefaultAsync(p => p.Address == address || p.Name == name.ToLower());
            if (ExistBranch != null)
                return Json(new { success = false, error = "this branch already exist" });
            if (ModelState.IsValid)
            {
                _context.Add(address);
                Branch branch = new Branch();
                branch.AddressId = address.Id;
                branch.Address = address;
                branch.Name = name;
                foreach (var openHour in openingHours)
                {
                    openHour.BranchId = branch.Id;
                    //openHour.Branch = branch;
                    _context.Add(openHour);
                }
                branch.OpeningHours = openingHours;
                _context.Add(branch);
                await _context.SaveChangesAsync();
                Console.WriteLine("Success");
                return Json(new { success = true });
            }
            Console.WriteLine("error");
            return Json(new { success = false });
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            ViewData["Address"] = await _context.Address.FindAsync(branch.AddressId);
             var temp = (from open in _context.OpeningHour
                                              where open.BranchId == branch.Id
                                              select open).ToList();
            // sort the day of week
            ViewData["OpeningHours"] = temp.OrderBy(o => o.DayOfWeek).ToList();
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Street,BuildingNumber,Longitude,Latitude")] Address address, [Bind("Id,Name")] string name, List<OpeningHour> openingHours)
        {
            var branchToEdit = (from branch in _context.Branch
                         where branch.Id == id
                         select branch).ToList();

            if (branchToEdit == null || branchToEdit.Count > 1 || branchToEdit.Count == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    branchToEdit[0].Address = address;
                    branchToEdit[0].AddressId = address.Id;
                    branchToEdit[0].Name = name;
                    branchToEdit[0].OpeningHours = openingHours;
                    _context.Update(branchToEdit[0]);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branchToEdit[0].Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            var branch = await _context.Branch.FindAsync(id);
            var address = await _context.Address.FindAsync(branch.AddressId);
            _context.Address.Remove(address);

            var days = (from day in _context.OpeningHour
                        where day.BranchId == branch.Id
                        select day).ToList();
            foreach (var day in days)
            {
                _context.OpeningHour.Remove(day);
            }

            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }
    }
}
