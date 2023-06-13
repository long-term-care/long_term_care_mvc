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
    public class MemSignsController : Controller
    {
        private readonly longtermcareContext _context;

        public MemSignsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: MemSigns
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.MemSigns.Include(m => m.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: MemSigns/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memSign = await _context.MemSigns
                .Include(m => m.MemS)
                .FirstOrDefaultAsync(m => m.MemSignQaid == id);
            if (memSign == null)
            {
                return NotFound();
            }

            return View(memSign);
        }

        // GET: MemSigns/Create
        public IActionResult Create()
        {
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");
            return View();
        }

        // POST: MemSigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemSignQaid,MemSid,MemTelTime1,MemTelTime2,MemRecord")] MemSign memSign)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memSign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", memSign.MemSid);
            return View(memSign);
        }

        // GET: MemSigns/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memSign = await _context.MemSigns.FindAsync(id);
            if (memSign == null)
            {
                return NotFound();
            }
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", memSign.MemSid);
            return View(memSign);
        }

        // POST: MemSigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemSignQaid,MemSid,MemTelTime1,MemTelTime2,MemRecord")] MemSign memSign)
        {
            if (id != memSign.MemSignQaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memSign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemSignExists(memSign.MemSignQaid))
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
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", memSign.MemSid);
            return View(memSign);
        }

        // GET: MemSigns/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memSign = await _context.MemSigns
                .Include(m => m.MemS)
                .FirstOrDefaultAsync(m => m.MemSignQaid == id);
            if (memSign == null)
            {
                return NotFound();
            }

            return View(memSign);
        }

        // POST: MemSigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var memSign = await _context.MemSigns.FindAsync(id);
            _context.MemSigns.Remove(memSign);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemSignExists(string id)
        {
            return _context.MemSigns.Any(e => e.MemSignQaid == id);
        }
    }
}
