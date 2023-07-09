using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;
using System.Data;
using System.IO;
using DocumentFormat.OpenXml;
using NPOI.XSSF.UserModel;





namespace long_term_care.Controllers
{
    public class CaseDailyRegistrationsController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseDailyRegistrationsController(longtermcareContext context)
        {
            _context = context;
        }



        // GET: CaseDailyRegistrations
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CaseDailyRegistrations.Include(c => c.CaseNoNavigation);
            return View(await longtermcareContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> IsDateExist(DateTime caseDate)
        {
            bool isExist = await _context.CaseDailyRegistrations.AnyAsync(x => x.Casedate == caseDate);
            return Json(new { exists = isExist });
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


        public IActionResult Details()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string CaseCardID, DateTime Casedate)
        {
            if (string.IsNullOrEmpty(CaseCardID))
            {
                return Content("請填入個案身分證");
            }
            if (Casedate == DateTime.MinValue)
            {
                return Content("請填入搜索年月!");
            }
            var no1 = from ccr in _context.CaseDailyRegistrations
                      join ci in _context.CaseInfors on ccr.CaseNo equals ci.CaseNo
                      where ci.CaseIdcard == CaseCardID &&
                            ccr.Casedate.Year == Casedate.Year &&
                            ccr.Casedate.Month == Casedate.Month
                      select new DailyResultViewModel
                      {
                          CaseName = ci.CaseName,
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
                          CaseNo = ci.CaseNo,
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
            var viewModel = new CaseDailyRegistration();

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
            return View(viewModel);
        }

        // POST: CaseDailyRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseContId,CaseNo,CaseDiastolic,CaseSystolic,Casedate,CaseTemp,CasePluse,CaseBlood,CasePick")] CaseDailyRegistration caseDailyRegistration)
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
        public async Task<IActionResult> Edit(string id, [Bind("CaseContId,CaseDiastolic,CaseSystolic,CaseNo,Casedate,CaseTemp,CasePluse,CaseBlood,CasePick")] CaseDailyRegistration caseDailyRegistration)
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

        private IEnumerable<DailyResultViewModel> GetDataForDateRange(DateTime startDate, DateTime endDate)
        {
            using (var dbContext = new longtermcareContext())
            {
                var data = dbContext.CaseDailyRegistrations
                    .Where(x => x.Casedate >= startDate && x.Casedate <= endDate)
                    .Include(c => c.CaseNoNavigation)
                    .Join(dbContext.CaseInfors,
                        ccr => ccr.CaseNo,
                        ci => ci.CaseNo,
                        (ccr, ci) => new DailyResultViewModel
                        {
                            Casedate = ccr.Casedate,
                            CaseTemp = ccr.CaseTemp,
                            CasePluse = ccr.CasePluse,
                            CaseBlood = ccr.CaseBlood,
                            CasePick = ccr.CasePick,
                            CaseSystolic = ccr.CaseSystolic,
                            CaseDiastolic = ccr.CaseDiastolic,
                            CaseContId = ccr.CaseContId,
                            CaseNo = ccr.CaseNo,
                            CaseName = ci.CaseName,
                            CaseBd = ci.CaseBd,
                            CaseGender = ci.CaseGender,
                            CaseIdent = ci.CaseIdent,
                            CaseLang = ci.CaseLang,
                            CaseMari = ci.CaseMari,
                            CaseFami = ci.CaseFami,
                            CaseAddr = ci.CaseAddr,
                            CaseCnta = ci.CaseCnta,
                            CaseCntTel = ci.CaseCntTel,
                            CaseCntRel = ci.CaseCntRel
                        })
                    .ToList();

                return data;
            }
        }


        [HttpPost]
        public IActionResult ExportData(string exportType, DateTime startDate, DateTime endDate)
        {
            var data = GetDataForDateRange(startDate, endDate);

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
                headerRow.CreateCell(5).SetCellValue("婚姻");
                headerRow.CreateCell(6).SetCellValue("家庭");
                headerRow.CreateCell(7).SetCellValue("聯絡人");
                headerRow.CreateCell(8).SetCellValue("關係");
                headerRow.CreateCell(9).SetCellValue("聯絡人電話");

                var personalInfoRow = sheet.CreateRow(1);
                personalInfoRow.CreateCell(0).SetCellValue(data.First().CaseName);
                personalInfoRow.CreateCell(1).SetCellValue(data.First().CaseBd);
                personalInfoRow.CreateCell(2).SetCellValue(data.First().CaseGender);
                personalInfoRow.CreateCell(3).SetCellValue(data.First().CaseIdent);
                personalInfoRow.CreateCell(4).SetCellValue(data.First().CaseLang);
                personalInfoRow.CreateCell(5).SetCellValue(data.First().CaseMari);
                personalInfoRow.CreateCell(6).SetCellValue(data.First().CaseFami);
                personalInfoRow.CreateCell(7).SetCellValue(data.First().CaseCnta);
                personalInfoRow.CreateCell(8).SetCellValue(data.First().CaseCntRel);
                personalInfoRow.CreateCell(9).SetCellValue(data.First().CaseCntTel);

                var dailyHeaderRow = sheet.CreateRow(2);
                dailyHeaderRow.CreateCell(0).SetCellValue("日期");
                dailyHeaderRow.CreateCell(1).SetCellValue("體溫");
                dailyHeaderRow.CreateCell(2).SetCellValue("脈搏");
                dailyHeaderRow.CreateCell(3).SetCellValue("舒張壓");
                dailyHeaderRow.CreateCell(4).SetCellValue("收縮壓");
                dailyHeaderRow.CreateCell(5).SetCellValue("血壓狀態");
                dailyHeaderRow.CreateCell(6).SetCellValue("交通接送");

                int rowIndex = 3;
                foreach (var item in data.Reverse())
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(item.Casedate.ToString("yyyy-MM-dd"));
                    row.CreateCell(1).SetCellValue(item.CaseTemp);
                    row.CreateCell(2).SetCellValue(item.CasePluse);
                    row.CreateCell(3).SetCellValue(item.CaseSystolic);
                    row.CreateCell(4).SetCellValue(item.CaseDiastolic);
                    row.CreateCell(5).SetCellValue(item.CaseBlood);
                    row.CreateCell(6).SetCellValue(item.CasePick);
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.Write(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
                }
            }
            else if (exportType == "pdf")
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfWriter(mem));
                    iText.Layout.Document document = new iText.Layout.Document(pdfDoc);

                    // Load the NotoSansHK-Regular.otf font
                    var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "Font", "NotoSansHK-Regular.otf");
                    var font = iText.Kernel.Font.PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H);

                    // Set the font to the document
                    document.SetFont(font);

                    iText.Layout.Element.Paragraph paragraph = new iText.Layout.Element.Paragraph("\n\n基本資料\n").SetFont(font);
                    paragraph.Add(new iText.Layout.Element.Text("姓名: " + data.First().CaseName).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n出生: " + data.First().CaseBd).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n性別: " + data.First().CaseGender).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n身份別: " + data.First().CaseIdent).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n語言: " + data.First().CaseLang).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n婚姻: " + data.First().CaseMari).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n家庭: " + data.First().CaseFami).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n聯絡人: " + data.First().CaseCnta).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n聯絡人關係: " + data.First().CaseCntRel).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\n聯絡人地址: " + data.First().CaseCntTel).SetFont(font));

                    document.Add(paragraph);


                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(7);

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("日期").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("體溫").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("脈搏").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("舒張壓").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("收縮壓").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("血壓狀態").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("交通接送").SetFont(font)));

                    foreach (var item in data)
                    {
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.Casedate.ToString("yyyy/MM/dd")).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CaseTemp).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CasePluse).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CaseDiastolic).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CaseSystolic).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CaseBlood).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CasePick).SetFont(font)));
                    }

                    document.Add(table);

                    document.Close();

                    return File(mem.ToArray(), "application/pdf", "data.pdf");
                }
            }


            return BadRequest("格式錯誤");
        }



        


    }
}