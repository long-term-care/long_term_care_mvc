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
        // GET: CaseNeeds/Details/5
        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string CaseCardID)
        {
            if (string.IsNullOrEmpty(CaseCardID))
            {
                return Content("請提供個案案號...");
            }


            var no1 = from ci in _context.CaseNeeds
                      join ccr in _context.CaseInfors on ci.CaseNo equals ccr.CaseNo
                      where ccr.CaseIdcard == CaseCardID
                      orderby ci.CaseNeedId
                      select new NeedSearchResultViewModal
                      {
                          CaseNeedId = ci.CaseNeedId,
                          CaseNo = ci.CaseNo,
                          CaseRead = ci.CaseRead,
                          CaseFami = ci.CaseFami,
                          CaseCons = ci.CaseCons,
                          CaseSpeak = ci.CaseSpeak,
                          CaseAct = ci.CaseAct,
                          CaseMed = ci.CaseMed,
                          CaseSee = ci.CaseSee,
                          CaseHear = ci.CaseHear,
                          CaseEat = ci.CaseEat,
                          CaseCare = ci.CaseCare,
                          CaseView1 = ci.CaseView1,
                          CaseView2 = ci.CaseView2,
                          CaseView3 = ci.CaseView3,
                          CaseView4 = ci.CaseView4,
                          CaseView5 = ci.CaseView5,
                          CaseView6 = ci.CaseView6,
                          CaseView7 = ci.CaseView7,
                          CaseView8 = ci.CaseView8,
                          CaseView9 = ci.CaseView9,  

                      };
            var no2 = await no1.LastOrDefaultAsync();
            if (no2 == null)
            {
                return NotFound();
            }

            return View("SearchResult", no2);
        }


        // GET: CaseNeeds/Create
        public IActionResult Create()
        {
            var viewModel = new CaseNeed();

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


            var lastForm = _context.CaseNeeds.OrderByDescending(f => f.CaseNeedId).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.CaseNeedId);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }
            ViewData["CaseNeedId"] = nextFormNumber;

            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            return View(viewModel);
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
