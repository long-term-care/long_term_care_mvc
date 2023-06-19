using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;

namespace long_term_care.Controllers
{
    public class CaseTelRecordsController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseTelRecordsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CaseTelRecords
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CaseTelRecords.Include(c => c.CaseNoNavigation).Include(c => c.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: CaseTelRecords/Details/5
        public IActionResult Details()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(DateTime CaseYM, string CaseNo)
        {
            if (string.IsNullOrEmpty(CaseNo))
            {
                return Content("必須填入個案案號 !");
            }
            if (CaseYM == DateTime.MinValue)
            {
                return Content("必须填入年月!");
            }
            var no1 = from ctr in _context.CaseTelRecords
                      join ci in _context.CaseInfors on ctr.CaseNo equals ci.CaseNo
                      where ci.CaseNo == CaseNo && ctr.CaseRegTime.Month == CaseYM.Month && ctr.CaseRegTime.Year == CaseYM.Year

                      select new TelSearchResultViewModel
                      {
                          CaseTelQaid = ctr.CaseTelQaid,
                          CaseNo = ctr.CaseNo,
                          CaseName = ci.CaseName,
                          CaseGender = ci.CaseGender,
                          CaseRegTime = ctr.CaseRegTime,
                          CaseSick = ctr.CaseSick,
                          CaseDay = ctr.CaseDay,
                          CaseTelTime1 = ctr.CaseTelTime1,
                          CaseTelTime2 = ctr.CaseTelTime2,
                          CaseAns = ctr.CaseAns,
                          CaseExp = ctr.CaseExp,
                          CaseHea = ctr.CaseHea,
                          CaseLive = ctr.CaseLive,
                          CaseFam = ctr.CaseFam,
                          CaseMental = ctr.CaseMental,
                          CaseCom = ctr.CaseCom,
                          MemSid = ctr.MemSid,
                      };
            var no2 = await no1.ToListAsync();
            if (no2 == null)
            {
                return NotFound();
            }

            return View("SearchResult", no2);
        }

        // GET: CaseTelRecords/Create
        public IActionResult Create()
        {
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");
            return View();
        }

        // POST: CaseTelRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseTelQaid,CaseNo,CaseRegTime,CaseSick,CaseDay,CaseTelTime1,CaseTelTime2,CaseAns,CaseExp,CaseHea,CaseLive,CaseFam,CaseMental,CaseCom,MemSid")] CaseTelRecord caseTelRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseTelRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseTelRecord.CaseNo);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", caseTelRecord.MemSid);
            return View(caseTelRecord);
        }

        // GET: CaseTelRecords/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTelRecord = await _context.CaseTelRecords.FindAsync(id);
            if (caseTelRecord == null)
            {
                return NotFound();
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseTelRecord.CaseNo);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", caseTelRecord.MemSid);
            return View(caseTelRecord);
        }

        // POST: CaseTelRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseTelQaid,CaseNo,CaseRegTime,CaseSick,CaseDay,CaseTelTime1,CaseTelTime2,CaseAns,CaseExp,CaseHea,CaseLive,CaseFam,CaseMental,CaseCom,MemSid")] CaseTelRecord caseTelRecord)
        {
            if (id != caseTelRecord.CaseTelQaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseTelRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseTelRecordExists(caseTelRecord.CaseTelQaid))
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseTelRecord.CaseNo);
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", caseTelRecord.MemSid);
            return View(caseTelRecord);
        }

        // GET: CaseTelRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTelRecord = await _context.CaseTelRecords
                .Include(c => c.CaseNoNavigation)
                .Include(c => c.MemS)
                .FirstOrDefaultAsync(m => m.CaseTelQaid == id);
            if (caseTelRecord == null)
            {
                return NotFound();
            }

            return View(caseTelRecord);
        }

        // POST: CaseTelRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var caseTelRecord = await _context.CaseTelRecords.FindAsync(id);
            _context.CaseTelRecords.Remove(caseTelRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseTelRecordExists(string id)
        {
            return _context.CaseTelRecords.Any(e => e.CaseTelQaid == id);
        }
    }
}
