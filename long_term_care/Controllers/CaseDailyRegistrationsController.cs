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
using System.Data;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OpenXmlWordprocessing = DocumentFormat.OpenXml.Wordprocessing;
using iTextSharpWordprocessing = iTextSharp.text;
using NPOIWordprocessing = NPOI.SS.UserModel;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;

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
        public async Task<IActionResult> Details(string CaseNo, DateTime Casedate)
        {
            if (string.IsNullOrEmpty(CaseNo))
            {
                return Content("必須填入個案案號 !");
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




        /*

        private IEnumerable<DailyResultViewModel> GetDataForDateRange(DateTime startDate, DateTime endDate)
        {
            
            var dbContext = new longtermcareContext();
            var data = dbContext.CaseDailyRegistrations
                .Where(x => x.Casedate >= startDate && x.Casedate <= endDate)
                .Select(x => new DailyResultViewModel
                {
                    Casedate = x.Casedate,
                    CaseName = x.CaseName,
                    CaseTemp = x.CaseTemp,
                    CasePluse = x.CasePluse,
                    CaseBlood = x.CaseBlood,
                    CasePick = x.CasePick,
                    CaseSystolic = x.CaseSystolic,
                    CaseDiastolic = x.CaseDiastolic,
                    CaseContId = x.CaseContId,
                    CaseNo = x.CaseNo,
                    CaseIDcard = x.CaseIDcard,

                   
                    CaseBd = "...", 
                    CaseGender = "...",
                    CaseIdent = "...",
                    CaseLang = "...",
                    CaseMari = "...",
                    CaseFami = "...",
                    CaseAddr = "...",
                    CaseCnta = "...",
                    CaseCntTel = "...",
                    CaseCntRel = "..."
                }).ToList();

            return data;
        }




        [HttpPost]
        public IActionResult ExportData(string exportType, DateTime startDate, DateTime endDate)
        {
            // Retrieve data from the database
            var data = GetDataForDateRange(startDate, endDate); // It should return IEnumerable<DailyResultViewModel>

            if (exportType == "excel")
            {
                // Implement Excel export logic here using NPOI
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");

                // Create header for personal information
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("CaseName");
                headerRow.CreateCell(1).SetCellValue("CaseBd");
                headerRow.CreateCell(2).SetCellValue("CaseGender");
                // ... Other personal information headers

                // Create header for daily registrations
                var dailyHeaderRow = sheet.CreateRow(1);
                dailyHeaderRow.CreateCell(0).SetCellValue("日期");
                dailyHeaderRow.CreateCell(1).SetCellValue("體溫");
                dailyHeaderRow.CreateCell(2).SetCellValue("脈搏");
                dailyHeaderRow.CreateCell(3).SetCellValue("舒張壓");
                dailyHeaderRow.CreateCell(4).SetCellValue("收縮壓");
                dailyHeaderRow.CreateCell(5).SetCellValue("血壓狀態");
                dailyHeaderRow.CreateCell(6).SetCellValue("交通接送");

                // Fill personal information
                var personalInfoRow = sheet.CreateRow(2);
                personalInfoRow.CreateCell(0).SetCellValue(data.First().CaseName);
                personalInfoRow.CreateCell(1).SetCellValue(data.First().CaseBd);
                personalInfoRow.CreateCell(2).SetCellValue(data.First().CaseGender);
                // ... Other personal information values

                // Fill daily registrations
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

                // Save and return file
                using (var memoryStream = new MemoryStream())
                {
                    workbook.Write(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
                }
            }

            else if (exportType == "doc")
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                    {
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new OpenXmlWordprocessing.Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        Table table = new Table();
                        TableRow headerRow = new TableRow();
                        table.Append(headerRow);

                        // Add header row cells
                        headerRow.Append(
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("姓名")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("日期")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("體溫")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("脈搏")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("血壓狀態")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("交通接送")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("舒張壓")))),
                            new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text("收縮壓"))))
                        );

                        // Add data rows
                        foreach (var item in data)
                        {
                            TableRow dataRow = new TableRow();
                            dataRow.Append(
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CaseName)))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.Casedate.ToString("yyyy/MM/dd"))))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CaseTemp)))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CasePluse)))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CaseBlood)))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CasePick)))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CaseSystolic)))),
                                new TableCell(new OpenXmlWordprocessing.Paragraph(new Run(new Text(item.CaseDiastolic))))
                            );
                            table.Append(dataRow);
                        }

                        body.Append(table);
                    }

                    return File(mem.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "data.docx");
                }
            }

            else if (exportType == "pdf")
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document();
                    PdfWriter.GetInstance(pdfDoc, mem);
                    pdfDoc.Open();

                    PdfPTable table = new PdfPTable(8); // 8 columns for the daily registration data

                    // Add column headers
                    table.AddCell("日期");
                    table.AddCell("體溫");
                    table.AddCell("脈搏");
                    table.AddCell("舒張壓");
                    table.AddCell("收縮壓");
                    table.AddCell("血壓狀態");
                    table.AddCell("交通接送");
                    table.AddCell("個案編號");

                    // Add data
                    foreach (var item in data)
                    {
                        table.AddCell(item.Casedate.ToString("yyyy/MM/dd"));
                        table.AddCell(item.CaseTemp);
                        table.AddCell(item.CasePluse);
                        table.AddCell(item.CaseDiastolic);
                        table.AddCell(item.CaseSystolic);
                        table.AddCell(item.CaseBlood);
                        table.AddCell(item.CasePick);
                        table.AddCell(item.CaseContId);
                    }

                    pdfDoc.Add(table);

                    iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("\n\nAdditional Information\n");
                    paragraph.Add(new iTextSharp.text.Chunk("姓名: " + data.First().CaseName));
                    paragraph.Add(new iTextSharp.text.Chunk("\n出生: " + data.First().CaseBd));
                    paragraph.Add(new iTextSharp.text.Chunk("\n性別: " + data.First().CaseGender));
                    // pass

                    pdfDoc.Add(paragraph);

                    pdfDoc.Close();

                    return File(mem.ToArray(), "application/pdf", "data.pdf");
                }
            }


            return BadRequest("Invalid export type");
        }
        */


    }
}