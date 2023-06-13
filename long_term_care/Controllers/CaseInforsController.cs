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
    public class CaseInforsController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseInforsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CaseInfors
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseInfors.ToListAsync());
        }

        // GET: CaseInfors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseInfor = await _context.CaseInfors
                .FirstOrDefaultAsync(m => m.CaseNo == id);
            if (caseInfor == null)
            {
                return NotFound();
            }

            return View(caseInfor);
        }

        // GET: CaseInfors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseInfors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseNo,CaseUnitName,CaseUnitNum,CaseName,CaseIdcard,CasePassword,CaseGender,CaseRelig,CaseBd,CaseLang,CaseSource,CaseWork,CaseProf,CaseEdu,CaseAddr,CaseHouse,CaseIdent,CaseFund,CaseHealth,CaseActv,CaseFactly,CaseMari,CaseCnta,CaseCntTel,CaseCntRel,CaseCntAdd,CaseFami,CaseQues,CaseDesc,CaseRegName,CaseRegTime")] CaseInfor caseInfor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseInfor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseInfor);
        }

        // GET: CaseInfors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseInfor = await _context.CaseInfors.FindAsync(id);
            if (caseInfor == null)
            {
                return NotFound();
            }
            return View(caseInfor);
        }

        // POST: CaseInfors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseNo,CaseUnitName,CaseUnitNum,CaseName,CaseIdcard,CasePassword,CaseGender,CaseRelig,CaseBd,CaseLang,CaseSource,CaseWork,CaseProf,CaseEdu,CaseAddr,CaseHouse,CaseIdent,CaseFund,CaseHealth,CaseActv,CaseFactly,CaseMari,CaseCnta,CaseCntTel,CaseCntRel,CaseCntAdd,CaseFami,CaseQues,CaseDesc,CaseRegName,CaseRegTime")] CaseInfor caseInfor)
        {
            if (id != caseInfor.CaseNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseInfor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseInforExists(caseInfor.CaseNo))
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
            return View(caseInfor);
        }

        // GET: CaseInfors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseInfor = await _context.CaseInfors
                .FirstOrDefaultAsync(m => m.CaseNo == id);
            if (caseInfor == null)
            {
                return NotFound();
            }

            return View(caseInfor);
        }

        // POST: CaseInfors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var caseInfor = await _context.CaseInfors.FindAsync(id);
            _context.CaseInfors.Remove(caseInfor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseInforExists(string id)
        {
            return _context.CaseInfors.Any(e => e.CaseNo == id);
        }
    }
}
