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

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchResults(string caseNo, string caseName, string caseIDcard)
        {
            var query = _context.CaseInfors.AsQueryable();

            if (!string.IsNullOrEmpty(caseNo))
            {
                query = query.Where(c => c.CaseNo.Contains(caseNo));
            }

            if (!string.IsNullOrEmpty(caseName))
            {
                query = query.Where(c => c.CaseName.Contains(caseName));
            }

            if (!string.IsNullOrEmpty(caseIDcard))
            {
                query = query.Where(c => c.CaseIdcard.Contains(caseIDcard));
            }

            var results = await query.ToListAsync();

            return PartialView("_SearchResultsPartial", results);
        }

        [HttpPost]
        public IActionResult StoreInTempData(string caseNo)
        {
            TempData["CaseNo"] = caseNo;
            //TempData["CaseName"] = caseName;
            //TempData["CaseIDCard"] = caseIDcard;

            return Json(new { status = "success" });
        }

        // GET: CaseCareRecords/Details/5

        public IActionResult Details()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string CaseNo)
        {
            if (string.IsNullOrEmpty(CaseNo))
            {
                return Content("必須提供個案案號!");
            }


            var no1 = from ccr in _context.CaseCareRecords
                      join ci in _context.CaseInfors on ccr.CaseNo equals ci.CaseNo
                      where ccr.CaseNo == CaseNo
                      select new CareSearchResultViewModel
                     {
                         CaseName = ci.CaseName,
                         CaseBd = ci.CaseBd,
                         CaseGender = ci.CaseGender,
                         CaseNo = ci.CaseIdcard,
                          CasePhn = ccr.CaseTel,
                         CaseHealth = ccr.CaseHealth,
                         CaseIdent = ci.CaseIdent,
                         CaseLang = ci.CaseLang,
                         CaseMari = ci.CaseMari,
                         CaseFami = ci.CaseFami,
                         CaseAddr = ci.CaseAddr,
                         CaseCnta = ci.CaseCnta,
                         CaseCntTel = ci.CaseCntTel,
                         CaseCntRel = ci.CaseCntRel,

                         CaseHome = ccr.CaseHome,
                         CaseTime1 = ccr.CaseTime1,
                         CaseQ1 = ccr.CaseQ1,
                         CaseQ1Other = ccr.CaseQ1Other,
                         CaseQ2 = ccr.CaseQ2,
                         CaseQ2Other = ccr.CaseQ2Other,
                         CaseQ3 = ccr.CaseQ3,
                         CaseQ3Other = ccr.CaseQ3Other,
                         CaseQ4 = ccr.CaseQ4,
                         CaseQ4Other = ccr.CaseQ4Other,
                         MemSid = ccr.MemSid,

                         CaseQaid = ccr.CaseQaid,

                     };
            var no2 = await no1.ToListAsync();
            if (no2 == null)
            {
                return NotFound();
            }

            return View("SearchResult", no2);
        }

        // GET: CaseCareRecords/Create
        public IActionResult Create()
        {

            var viewModel = new CaseCareRecord();

            if (TempData["CaseNo"] != null)
            {
                viewModel.CaseNo = TempData["CaseNo"].ToString();
            }
            /*
            if (TempData["CaseName"] != null)
            {
                viewModel.CaseName = TempData["CaseName"].ToString();
            }

            if (TempData["CaseIDCard"] != null)
            {
                viewModel.CaseIDcard = TempData["CaseIDCard"].ToString();
            }
            */

            string nextFormNumber = "";


            var lastForm = _context.CaseCareRecords.OrderByDescending(f => f.CaseQaid).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.CaseQaid);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }


            ViewData["CaseQaid"] = nextFormNumber;
            //ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");
            return View(viewModel); 
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
