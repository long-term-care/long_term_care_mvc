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
    public class CaseCareRecordsController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseCareRecordsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CaseCareRecords
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CaseCareRecords.Include(c => c.CaseNoNavigation).Include(c => c.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: CaseCareRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseCareRecord = await _context.CaseCareRecords
                .Include(c => c.CaseNoNavigation)
                .Include(c => c.MemS)
                .FirstOrDefaultAsync(m => m.CaseQaid == id);
            if (caseCareRecord == null)
            {
                return NotFound();
            }

            return View(caseCareRecord);
        }

        // GET: CaseCareRecords/Create
        public IActionResult Create()
        {
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");
            return View();
        }

        // POST: CaseCareRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseQaid,CaseNo,CaseTel,CaseHealth,CaseHome,CaseTime1,CaseQ1,CaseQ1Other,CaseQ2,CaseQ2Other,CaseQ3,CaseQ3Other,CaseQ4,CaseQ4Other,MemSid")] CaseCareRecord caseCareRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseCareRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseCareRecord.CaseNo);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", caseCareRecord.MemSid);
            return View(caseCareRecord);
        }

        // GET: CaseCareRecords/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseCareRecord = await _context.CaseCareRecords.FindAsync(id);
            if (caseCareRecord == null)
            {
                return NotFound();
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseCareRecord.CaseNo);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", caseCareRecord.MemSid);
            return View(caseCareRecord);
        }

        // POST: CaseCareRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseQaid,CaseNo,CaseTel,CaseHealth,CaseHome,CaseTime1,CaseQ1,CaseQ1Other,CaseQ2,CaseQ2Other,CaseQ3,CaseQ3Other,CaseQ4,CaseQ4Other,MemSid")] CaseCareRecord caseCareRecord)
        {
            if (id != caseCareRecord.CaseQaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseCareRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseCareRecordExists(caseCareRecord.CaseQaid))
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseCareRecord.CaseNo);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", caseCareRecord.MemSid);
            return View(caseCareRecord);
        }

        // GET: CaseCareRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseCareRecord = await _context.CaseCareRecords
                .Include(c => c.CaseNoNavigation)
                .Include(c => c.MemS)
                .FirstOrDefaultAsync(m => m.CaseQaid == id);
            if (caseCareRecord == null)
            {
                return NotFound();
            }

            return View(caseCareRecord);
        }

        // POST: CaseCareRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var caseCareRecord = await _context.CaseCareRecords.FindAsync(id);
            _context.CaseCareRecords.Remove(caseCareRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseCareRecordExists(string id)
        {
            return _context.CaseCareRecords.Any(e => e.CaseQaid == id);
        }
    }
}
