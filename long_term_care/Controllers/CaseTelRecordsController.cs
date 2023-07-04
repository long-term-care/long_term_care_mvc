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
        public IActionResult StoreInTempData(string caseNo, string caseName, string caseIDcard)
        {
            TempData["CaseNo"] = caseNo;
            //TempData["CaseName"] = caseName;
            //TempData["CaseIDCard"] = caseIDcard;

            return Json(new { status = "success" });
        }

        // GET: CaseTelRecords/Details/5
        public IActionResult Details()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(DateTime CaseYM, string CaseCardID)
        {
            if (string.IsNullOrEmpty(CaseCardID))
            {
                return Content("請填入個案案號 ...");
            }
            if (CaseYM == DateTime.MinValue)
            {
                return Content("請填入搜索年月!");
            }
            var no1 = from ctr in _context.CaseTelRecords
                      join ci in _context.CaseInfors on ctr.CaseNo equals ci.CaseNo
                      where ci.CaseIdcard == CaseCardID && ctr.CaseRegTime.Month == CaseYM.Month && ctr.CaseRegTime.Year == CaseYM.Year

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
            var viewModel = new CaseTelRecord();

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


            var lastForm = _context.CaseTelRecords.OrderByDescending(f => f.CaseTelQaid).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.CaseTelQaid);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }


            ViewData["CaseTelQaid"] = nextFormNumber;
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");
            return View(viewModel);
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
