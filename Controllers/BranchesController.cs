using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {


            var applicationDbContext = _context.Branch.Include(b => b.Address).Include(d=>d.OpeningHours);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .Include(b => b.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Address, "Id,Name,City,Street,BuildingNumber,Longitude,Latitude");
            return View();
        }

        // GET: Branches/GetJson
         public async Task<IActionResult> GetJson()
        {
            var applicationDbContext = _context.Branch.Include(b => b.Address);
            return Json(await applicationDbContext.ToListAsync());
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Street,BuildingNumber,Longitude,Latitude")] Address address, [Bind("Id,Name")] string name)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                Branch branch = new Branch();
                branch.AddressId = address.Id;
                branch.Address = address;
                branch.Name = name;
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Branches/Edit/5
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
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Street,BuildingNumber,Longitude,Latitude")] Address address, [Bind("Id,Name")] string name)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Name,City,Street,BuildingNumber,Longitude,Latitude", branchToEdit[0].AddressId);
            return View(branchToEdit[0]);
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .Include(b => b.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
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
