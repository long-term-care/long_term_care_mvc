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
    public class CaseNeedsController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseNeedsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CaseNeeds
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CaseNeeds.Include(c => c.CaseNoNavigation);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: CaseNeeds/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNeed = await _context.CaseNeeds
                .Include(c => c.CaseNoNavigation)
                .FirstOrDefaultAsync(m => m.CaseNeedId == id);
            if (caseNeed == null)
            {
                return NotFound();
            }

            return View(caseNeed);
        }

        // GET: CaseNeeds/Create
        public IActionResult Create()
        {
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            return View();
        }

        // POST: CaseNeeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseNeedId,CaseNo,CaseRead,CaseFami,CaseCons,CaseSpeak,CaseAct,CaseMed,CaseSee,CaseHear,CaseEat,CaseCare,CaseView1,CaseView2,CaseView3,CaseView4,CaseView5,CaseView6,CaseView7,CaseView8,CaseView9")] CaseNeed caseNeed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseNeed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseNeed.CaseNo);
            return View(caseNeed);
        }

        // GET: CaseNeeds/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNeed = await _context.CaseNeeds.FindAsync(id);
            if (caseNeed == null)
            {
                return NotFound();
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseNeed.CaseNo);
            return View(caseNeed);
        }

        // POST: CaseNeeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseNeedId,CaseNo,CaseRead,CaseFami,CaseCons,CaseSpeak,CaseAct,CaseMed,CaseSee,CaseHear,CaseEat,CaseCare,CaseView1,CaseView2,CaseView3,CaseView4,CaseView5,CaseView6,CaseView7,CaseView8,CaseView9")] CaseNeed caseNeed)
        {
            if (id != caseNeed.CaseNeedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseNeed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseNeedExists(caseNeed.CaseNeedId))
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseNeed.CaseNo);
            return View(caseNeed);
        }

        // GET: CaseNeeds/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNeed = await _context.CaseNeeds
                .Include(c => c.CaseNoNavigation)
                .FirstOrDefaultAsync(m => m.CaseNeedId == id);
            if (caseNeed == null)
            {
                return NotFound();
            }

            return View(caseNeed);
        }

        // POST: CaseNeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var caseNeed = await _context.CaseNeeds.FindAsync(id);
            _context.CaseNeeds.Remove(caseNeed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseNeedExists(string id)
        {
            return _context.CaseNeeds.Any(e => e.CaseNeedId == id);
        }
    }
}
