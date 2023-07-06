using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;
using NPOI.XSSF.UserModel;
using System.IO;

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


        [HttpPost]
        public IActionResult ExportLatestCaseNeed(string exportType, string CaseIDcard)
        {
            using (var dbContext = new longtermcareContext())
            {
                var latestRecord = dbContext.CaseInfors
                    .Where(ci => ci.CaseIdcard == CaseIDcard)
                    .Join(dbContext.CaseNeeds,
                        ci => ci.CaseNo,
                        ctr => ctr.CaseNo,
                        (ci, ctr) => new NeedSearchResultViewModal
                        {
                            CaseName = ci.CaseName,
                            CaseGender = ci.CaseGender,
                            CaseBd = ci.CaseBd,
                            CaseIdent = ci.CaseIdent,
                            CaseLang = ci.CaseLang,
                            CaseNeedId = ctr.CaseNeedId,
                            CaseNo = ctr.CaseNo,
                            CaseRead = ctr.CaseRead,
                            CaseFami = ctr.CaseFami,
                            CaseCons = ctr.CaseCons,
                            CaseSpeak = ctr.CaseSpeak,
                            CaseAct = ctr.CaseAct,
                            CaseMed = ctr.CaseMed,
                            CaseSee = ctr.CaseSee,
                            CaseHear = ctr.CaseHear,
                            CaseEat = ctr.CaseEat,
                            CaseCare = ctr.CaseCare,
                            CaseView1 = ctr.CaseView1,
                            CaseView2 = ctr.CaseView2,
                            CaseView3 = ctr.CaseView3,
                            CaseView4 = ctr.CaseView4,
                            CaseView5 = ctr.CaseView5,
                            CaseView6 = ctr.CaseView6,
                            CaseView7 = ctr.CaseView7,
                            CaseView8 = ctr.CaseView8,
                            CaseView9 = ctr.CaseView9,
                        })
                    .OrderByDescending(ctr => ctr.CaseNeedId)
                    .FirstOrDefault();

                if (latestRecord == null)
                {
                    return NotFound("查無此資料...");
                }
                if (exportType == "excel")
                {
                    var workbook = new XSSFWorkbook();
                    var sheet = workbook.CreateSheet("Sheet1");


                    var headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("姓名");
                    headerRow.CreateCell(1).SetCellValue("出生");
                    headerRow.CreateCell(2).SetCellValue("性別");
                    headerRow.CreateCell(3).SetCellValue("身分別");
                    headerRow.CreateCell(4).SetCellValue("語言");

                    var personalInfoRow = sheet.CreateRow(1);
                    personalInfoRow.CreateCell(0).SetCellValue(latestRecord.CaseName);
                    personalInfoRow.CreateCell(1).SetCellValue(latestRecord.CaseBd.ToString());
                    personalInfoRow.CreateCell(2).SetCellValue(latestRecord.CaseGender);
                    personalInfoRow.CreateCell(3).SetCellValue(latestRecord.CaseIdent);
                    personalInfoRow.CreateCell(4).SetCellValue(latestRecord.CaseLang);


                    var dailyHeaderRow2 = sheet.CreateRow(2);

                    dailyHeaderRow2.CreateCell(0).SetCellValue("自我照顧");
                    dailyHeaderRow2.CreateCell(1).SetCellValue("識字");
                    dailyHeaderRow2.CreateCell(2).SetCellValue("居住情形");
                    dailyHeaderRow2.CreateCell(3).SetCellValue("意識清楚");
                    dailyHeaderRow2.CreateCell(4).SetCellValue("說話清楚");
                    dailyHeaderRow2.CreateCell(5).SetCellValue("協助行動");
                    dailyHeaderRow2.CreateCell(6).SetCellValue("服藥情形");
                    dailyHeaderRow2.CreateCell(7).SetCellValue("視力");
                    dailyHeaderRow2.CreateCell(8).SetCellValue("聽力");
                    dailyHeaderRow2.CreateCell(9).SetCellValue("進食");

                    var dailyHeaderRow3 = sheet.CreateRow(3);

                    dailyHeaderRow3.CreateCell(0).SetCellValue(latestRecord.CaseCare);
                    dailyHeaderRow3.CreateCell(1).SetCellValue(latestRecord.CaseRead);
                    dailyHeaderRow3.CreateCell(2).SetCellValue(latestRecord.CaseFami);
                    dailyHeaderRow3.CreateCell(3).SetCellValue(latestRecord.CaseCons);
                    dailyHeaderRow3.CreateCell(4).SetCellValue(latestRecord.CaseSpeak);
                    dailyHeaderRow3.CreateCell(5).SetCellValue(latestRecord.CaseAct);
                    dailyHeaderRow3.CreateCell(6).SetCellValue(latestRecord.CaseMed);
                    dailyHeaderRow3.CreateCell(7).SetCellValue(latestRecord.CaseSee);
                    dailyHeaderRow3.CreateCell(8).SetCellValue(latestRecord.CaseHear);
                    dailyHeaderRow3.CreateCell(9).SetCellValue(latestRecord.CaseEat);

                    var dailyHeaderRow4 = sheet.CreateRow(4);

                    dailyHeaderRow4.CreateCell(0).SetCellValue("如果○○C單位巷弄長照站，您贊成嗎");
                    dailyHeaderRow4.CreateCell(1).SetCellValue("是否喜歡與其他老人一起聊天、活動");
                    dailyHeaderRow4.CreateCell(2).SetCellValue("每天在家中生活情形");
                    dailyHeaderRow4.CreateCell(3).SetCellValue("在家裡如何吃中餐");
                    dailyHeaderRow4.CreateCell(4).SetCellValue("您可以自行前往活動地點嗎");
                    dailyHeaderRow4.CreateCell(5).SetCellValue("您喜歡的活動");
                    dailyHeaderRow4.CreateCell(6).SetCellValue("您個人會的傳統技藝有");
                    dailyHeaderRow4.CreateCell(7).SetCellValue("如果需繳交活動費、伙食費（每週2個上午含點心或午餐），您認為每個月多少錢可接受");
                    dailyHeaderRow4.CreateCell(8).SetCellValue("您本人會來參加C單位巷弄長照站的活動嗎");

                    var dailyHeaderRow5 = sheet.CreateRow(5);

                    dailyHeaderRow5.CreateCell(0).SetCellValue(latestRecord.CaseView1);
                    dailyHeaderRow5.CreateCell(1).SetCellValue(latestRecord.CaseView2);
                    dailyHeaderRow5.CreateCell(2).SetCellValue(latestRecord.CaseView3);
                    dailyHeaderRow5.CreateCell(3).SetCellValue(latestRecord.CaseView4);
                    dailyHeaderRow5.CreateCell(4).SetCellValue(latestRecord.CaseView5);
                    dailyHeaderRow5.CreateCell(5).SetCellValue(latestRecord.CaseView6);
                    dailyHeaderRow5.CreateCell(6).SetCellValue(latestRecord.CaseView7);
                    dailyHeaderRow5.CreateCell(7).SetCellValue(latestRecord.CaseView8);
                    dailyHeaderRow5.CreateCell(8).SetCellValue(latestRecord.CaseView9);


                    using (var memoryStream = new MemoryStream())
                    {
                        workbook.Write(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LatestCaseCareRecord.xlsx");
                    }
                }


                return BadRequest("Invalid exportType specified.");
            }
        }
    }
}
