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
    public class OpeningHoursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OpeningHoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OpeningHours
        public async Task<IActionResult> Index()
        {
            return View(await _context.OpeningHour.ToListAsync());
        }

        // GET: OpeningHours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingHour = await _context.OpeningHour
                .FirstOrDefaultAsync(m => m.Id == id);
            if (openingHour == null)
            {
                return NotFound();
            }

            return View(openingHour);
        }

        // GET: OpeningHours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OpeningHours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Open,Close,DayOfWeek")] OpeningHour openingHour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(openingHour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(openingHour);
        }

        // GET: OpeningHours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingHour = await _context.OpeningHour.FindAsync(id);
            if (openingHour == null)
            {
                return NotFound();
            }
            return View(openingHour);
        }

        // POST: OpeningHours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Open,Close,DayOfWeek")] OpeningHour openingHour)
        {
            if (id != openingHour.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(openingHour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpeningHourExists(openingHour.Id))
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
            return View(openingHour);
        }

        // GET: OpeningHours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingHour = await _context.OpeningHour
                .FirstOrDefaultAsync(m => m.Id == id);
            if (openingHour == null)
            {
                return NotFound();
            }

            return View(openingHour);
        }

        // POST: OpeningHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var openingHour = await _context.OpeningHour.FindAsync(id);
            _context.OpeningHour.Remove(openingHour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpeningHourExists(int id)
        {
            return _context.OpeningHour.Any(e => e.Id == id);
        }
    }
}
