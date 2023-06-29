using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;
using System.Runtime.ConstrainedExecution;
using System.IO;




namespace long_term_care.Controllers
{
    public class CaseDailyRegistrationsController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseDailyRegistrationsController(longtermcareContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMatchingCases(string caseName, string caseNo, string caseIDcard)
        {
            var matchingCases = _context.CaseInfors
                .Where(ci => ci.CaseName.Contains(caseName) &&
                             ci.CaseNo.Contains(caseNo) &&
                             ci.CaseIdcard.Contains(caseIDcard))
                .Select(ci => new { ci.CaseName, ci.CaseNo, ci.CaseIdcard })
                .ToList();

            return PartialView("_SearchResultsPartial", matchingCases);
        }





        // GET: CaseDailyRegistrations
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CaseDailyRegistrations.Include(c => c.CaseNoNavigation);
            return View(await longtermcareContext.ToListAsync());
        }


        public IActionResult Details()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string CaseNo, DateTime Casedate)
        {
            if (string.IsNullOrEmpty(CaseNo))
            {
                return Content("個案案號!");
            }
            if (Casedate == DateTime.MinValue)
            {
                return Content("請填入搜索年月!");
            }
            var no1 = from ccr in _context.CaseDailyRegistrations
                      join ci in _context.CaseInfors on ccr.CaseNo equals ci.CaseNo
                      where ccr.CaseNo == CaseNo &&
                            ccr.Casedate.Year == Casedate.Year &&
                            ccr.Casedate.Month == Casedate.Month
                      select new DailyResultViewModel
                      {
                          CaseName = ccr.CaseName,
                          CaseBd = ci.CaseBd,
                          CaseGender = ci.CaseGender,
                          CaseIdent = ci.CaseIdent,
                          CaseLang = ci.CaseLang,
                          CaseMari = ci.CaseMari,
                          CaseFami = ci.CaseFami,
                          CaseAddr = ci.CaseAddr,
                          CaseCnta = ci.CaseCnta,
                          CaseCntTel = ci.CaseCntTel,
                          CaseCntRel = ci.CaseCntRel,
                          CaseNo = ccr.CaseNo,
                          CaseContId = ccr.CaseContId,
                          Casedate = ccr.Casedate,
                          CasePluse = ccr.CasePluse,
                          CaseTemp = ccr.CaseTemp,
                          CasePick = ccr.CasePick,
                          CaseBlood = ccr.CaseBlood,
                          CaseDiastolic = ccr.CaseDiastolic,
                          CaseSystolic = ccr.CaseSystolic,
                      };
            var no2 = await no1.ToListAsync();
            if (no2 == null)
            {
                return NotFound();
            }
            return View("SearchResult", no2);

        }

        // GET: CaseDailyRegistrations/Create
        public IActionResult Create()
        {
            string nextFormNumber = "";


            var lastForm = _context.CaseDailyRegistrations.OrderByDescending(f => f.CaseContId).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.CaseContId);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }


            ViewData["CaseContId"] = nextFormNumber;

            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            return View();
        }

        // POST: CaseDailyRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseContId,CaseNo,CaseName,CaseDiastolic,CaseSystolic,Casedate,CaseTemp,CasePluse,CaseBlood,CasePick")] CaseDailyRegistration caseDailyRegistration)
        {

            if (ModelState.IsValid)
            {
                _context.Add(caseDailyRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseDailyRegistration.CaseNo);
            return View(caseDailyRegistration);
        }

        // GET: CaseDailyRegistrations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseDailyRegistration = await _context.CaseDailyRegistrations.FindAsync(id);
            if (caseDailyRegistration == null)
            {
                return NotFound();
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseDailyRegistration.CaseNo);
            return View(caseDailyRegistration);
        }

        // POST: CaseDailyRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseContId,CaseName,CaseDiastolic,CaseSystolic,CaseNo,Casedate,CaseTemp,CasePluse,CaseBlood,CasePick")] CaseDailyRegistration caseDailyRegistration)
        {
            if (id != caseDailyRegistration.CaseContId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseDailyRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseDailyRegistrationExists(caseDailyRegistration.CaseContId))
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", caseDailyRegistration.CaseNo);
            return View(caseDailyRegistration);
        }

        // GET: CaseDailyRegistrations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseDailyRegistration = await _context.CaseDailyRegistrations
                .Include(c => c.CaseNoNavigation)
                .FirstOrDefaultAsync(m => m.CaseContId == id);
            if (caseDailyRegistration == null)
            {
                return NotFound();
            }

            return View(caseDailyRegistration);
        }

        // POST: CaseDailyRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var caseDailyRegistration = await _context.CaseDailyRegistrations.FindAsync(id);
            _context.CaseDailyRegistrations.Remove(caseDailyRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseDailyRegistrationExists(string id)
        {
            return _context.CaseDailyRegistrations.Any(e => e.CaseContId == id);
        }


    }
}