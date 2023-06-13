using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;

namespace long_term_care.Controllers
{
    public class CarPicksController : Controller
    {
        private readonly longtermcareContext _context;

        public CarPicksController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CarPicks
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CarPicks.Include(c => c.CaseCont).Include(c => c.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: CarPicks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPick = await _context.CarPicks
                .Include(c => c.CaseCont)
                .Include(c => c.MemS)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (carPick == null)
            {
                return NotFound();
            }

            return View(carPick);
        }

        // GET: CarPicks/Create
        public IActionResult Create()
        {
            ViewData["CaseContId"] = new SelectList(_context.CaseDailyRegistrations, "CaseContId", "CaseContId");
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");
            return View();
        }

        // POST: CarPicks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,MemSid,CarDate,CarType,CarNum,CarMonth,CarCaseAdr,CarL,CarKm,CarPrice,CaseContId")] CarPick carPick)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carPick);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseContId"] = new SelectList(_context.CaseDailyRegistrations, "CaseContId", "CaseContId", carPick.CaseContId);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);
            return View(carPick);
        }

        // GET: CarPicks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPick = await _context.CarPicks.FindAsync(id);
            if (carPick == null)
            {
                return NotFound();
            }
            ViewData["CaseContId"] = new SelectList(_context.CaseDailyRegistrations, "CaseContId", "CaseContId", carPick.CaseContId);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);
            return View(carPick);
        }

        // POST: CarPicks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CarId,MemSid,CarDate,CarType,CarNum,CarMonth,CarCaseAdr,CarL,CarKm,CarPrice,CaseContId")] CarPick carPick)
        {
            if (id != carPick.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carPick);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarPickExists(carPick.CarId))
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
            ViewData["CaseContId"] = new SelectList(_context.CaseDailyRegistrations, "CaseContId", "CaseContId", carPick.CaseContId);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);
            return View(carPick);
        }

        // GET: CarPicks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPick = await _context.CarPicks
                .Include(c => c.CaseCont)
                .Include(c => c.MemS)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (carPick == null)
            {
                return NotFound();
            }

            return View(carPick);
        }

        // POST: CarPicks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var carPick = await _context.CarPicks.FindAsync(id);
            _context.CarPicks.Remove(carPick);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarPickExists(string id)
        {
            return _context.CarPicks.Any(e => e.CarId == id);
        }
    }
}
