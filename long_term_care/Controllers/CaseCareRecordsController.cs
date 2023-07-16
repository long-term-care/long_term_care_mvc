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
            return View();/*await longtermcareContext.ToListAsync()*/
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
        public async Task<IActionResult> Details(string CaseCardID)
        {
            if (string.IsNullOrEmpty(CaseCardID))
            {
                return Content("請提供個案身分證...");
            }


            var no1 = from ccr in _context.CaseCareRecords
                      join ci in _context.CaseInfors on ccr.CaseNo equals ci.CaseNo
                      where ci.CaseIdcard == CaseCardID
                      select new CareSearchResultViewModel
                      {
                          CaseName = ci.CaseName,
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

        private static string NullToEmptyString(string input)
        {
            return input ?? string.Empty;
        }

        [HttpPost]
        public IActionResult ExportLatestCaseCareRecord(string exportType, string CaseIDcard)
        {
            using (var dbContext = new longtermcareContext())
            {
                var latestRecord = dbContext.CaseInfors
                    .Where(ci => ci.CaseIdcard == CaseIDcard)
                    .Join(dbContext.CaseCareRecords,
                        ci => ci.CaseNo,
                        ccr => ccr.CaseNo,
                        (ci, ccr) => new CareSearchResultViewModel
                        {
                            CaseName = ci.CaseName,
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
                        })
                    .OrderByDescending(ccr => ccr.CaseQaid)
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
                    headerRow.CreateCell(1).SetCellValue("性別");
                    headerRow.CreateCell(2).SetCellValue("身分別");
                    headerRow.CreateCell(3).SetCellValue("語言");
                    headerRow.CreateCell(4).SetCellValue("婚姻");
                    headerRow.CreateCell(5).SetCellValue("家庭");
                    headerRow.CreateCell(6).SetCellValue("聯絡人");
                    headerRow.CreateCell(7).SetCellValue("關係");
                    headerRow.CreateCell(8).SetCellValue("聯絡人電話");


                    var personalInfoRow = sheet.CreateRow(1);
                    personalInfoRow.CreateCell(0).SetCellValue(latestRecord.CaseName);
                    personalInfoRow.CreateCell(1).SetCellValue(latestRecord.CaseGender);
                    personalInfoRow.CreateCell(2).SetCellValue(latestRecord.CaseIdent);
                    personalInfoRow.CreateCell(3).SetCellValue(latestRecord.CaseLang);
                    personalInfoRow.CreateCell(4).SetCellValue(latestRecord.CaseMari);
                    personalInfoRow.CreateCell(5).SetCellValue(latestRecord.CaseFami);
                    personalInfoRow.CreateCell(6).SetCellValue(latestRecord.CaseCnta);
                    personalInfoRow.CreateCell(7).SetCellValue(latestRecord.CaseCntRel);
                    personalInfoRow.CreateCell(8).SetCellValue(latestRecord.CaseCntTel);

                    var dailyHeaderRow2 = sheet.CreateRow(2);

                    dailyHeaderRow2.CreateCell(0).SetCellValue("問題一");
                    dailyHeaderRow2.CreateCell(1).SetCellValue(latestRecord.CaseQ1);
                    dailyHeaderRow2.CreateCell(2).SetCellValue("問題一其他");
                    dailyHeaderRow2.CreateCell(3).SetCellValue(latestRecord.CaseQ1Other);


                    var dailyHeaderRow3 = sheet.CreateRow(3);
                    dailyHeaderRow3.CreateCell(0).SetCellValue("問題二");
                    dailyHeaderRow3.CreateCell(1).SetCellValue(latestRecord.CaseQ2);
                    dailyHeaderRow3.CreateCell(2).SetCellValue("問題二其他");
                    dailyHeaderRow3.CreateCell(3).SetCellValue(latestRecord.CaseQ2Other);


                    var dailyHeaderRow4 = sheet.CreateRow(4);
                    dailyHeaderRow4.CreateCell(0).SetCellValue("問題三");
                    dailyHeaderRow4.CreateCell(1).SetCellValue(latestRecord.CaseQ3);
                    dailyHeaderRow4.CreateCell(2).SetCellValue("問題三其他");
                    dailyHeaderRow4.CreateCell(3).SetCellValue(latestRecord.CaseQ3Other);


                    var dailyHeaderRow5 = sheet.CreateRow(5);
                    dailyHeaderRow5.CreateCell(0).SetCellValue("問題四");
                    dailyHeaderRow5.CreateCell(1).SetCellValue(latestRecord.CaseQ4);
                    dailyHeaderRow5.CreateCell(2).SetCellValue("問題四其他");
                    dailyHeaderRow5.CreateCell(3).SetCellValue(latestRecord.CaseQ4Other);

                    var dailyHeaderRow6 = sheet.CreateRow(6);
                    dailyHeaderRow6.CreateCell(0).SetCellValue("填寫志工");
                    dailyHeaderRow6.CreateCell(1).SetCellValue(latestRecord.MemSid);



                    using (var memoryStream = new MemoryStream())
                    {
                        workbook.Write(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LatestCaseCareRecord.xlsx");
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
                        paragraph.Add(new iText.Layout.Element.Text("姓名: " + latestRecord.CaseName).SetFont(font));
                        //paragraph.Add(new iText.Layout.Element.Text("\n出生: " + latestRecord.CaseBd).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n性別: " + latestRecord.CaseGender).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n身份別: " + latestRecord.CaseIdent).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n語言: " + latestRecord.CaseLang).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n婚姻: " + latestRecord.CaseMari).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n家庭: " + latestRecord.CaseFami).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n聯絡人: " + latestRecord.CaseCnta).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n聯絡人關係: " + latestRecord.CaseCntRel).SetFont(font));
                        paragraph.Add(new iText.Layout.Element.Text("\n聯絡人地址: " + latestRecord.CaseCntTel).SetFont(font));

                        document.Add(paragraph);


                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(9);
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("問題一").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("其它").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("問題二").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("其它").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("問題三").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("其它").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("問題四").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("其它").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("填寫志工").SetFont(font)));


                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ1)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ1Other)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ2)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ2Other)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ3)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ3Other)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ4)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.CaseQ4Other)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(NullToEmptyString(latestRecord.MemSid)).SetFont(font)));


                        document.Add(table);

                        document.Close();

                        return File(mem.ToArray(), "application/pdf", "data.pdf");
                    }
                }


                return BadRequest("格式錯誤");
            }
        }



    }
}