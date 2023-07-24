using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.XSSF.UserModel;
using OpenXmlWordprocessing = DocumentFormat.OpenXml.Wordprocessing;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using System.IO;

namespace long_term_care.Controllers
{
    public class CasePhysicalMentalsController : Controller
    {
        private readonly longtermcareContext _context;


        public CasePhysicalMentalsController(longtermcareContext context)
        {
            _context = context;

        }

        // GET: CasePhysicalMentals
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CasePhysicalMentals.Include(c => c.CaseNoNavigation);
            return View();
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


        // GET: CasePhysicalMentals/Details/5
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

            var no = from cpm in _context.CasePhysicalMentals
                     join ci in _context.CaseInfors on cpm.CaseNo equals ci.CaseNo
                     where ci.CaseIdcard == CaseCardID
                     orderby cpm.CaseQaid
                     select new PhysicalSearchResultViewModel
                     {
                         CaseName = ci.CaseName,
                         CaseOld = ci.CaseBd,
                         CaseGender = ci.CaseGender,
                         CaseMari = ci.CaseMari,
                         CaseEdu = ci.CaseEdu,
                         CaseLive = cpm.CaseLive,
                         CaseFre = cpm.CaseFre,
                         CaseContent1 = cpm.CaseContent1,
                         CaseContent2 = cpm.CaseContent2,
                         CaseContent3 = cpm.CaseContent3,
                         CaseContent4 = cpm.CaseContent4,
                         CaseContent5 = cpm.CaseContent5,
                         CaseContent6 = cpm.CaseContent6,
                         CaseContent7 = cpm.CaseContent7,
                         CaseContent8 = cpm.CaseContent8,
                         CaseContent9 = cpm.CaseContent9,
                         CaseContent10 = cpm.CaseContent10,
                         CaseContent11 = cpm.CaseContent11,
                         CaseContent12 = cpm.CaseContent12,
                         CaseContent13 = cpm.CaseContent13,
                         CaseQaid = cpm.CaseQaid,
                     };
            var no2 = await no.LastOrDefaultAsync();

            if (no2 == null)
            {
                return NotFound();
            }

            return View("SearchResult", no2);

        }

        // GET: CasePhysicalMentals/Create
        public IActionResult Create()
        {
            var viewModel = new CasePhysicalMental();

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


            var lastForm = _context.CasePhysicalMentals.OrderByDescending(f => f.CaseQaid).FirstOrDefault();
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            return View(viewModel);
        }

        // POST: CasePhysicalMentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] selected3, string[] selected4, [Bind("CaseQaid,CaseNo,CaseLive,CaseFre,CaseContent1,CaseContent2,CaseContent3,CaseContent4,CaseContent5,CaseContent6,CaseContent7,CaseContent8,CaseContent9,CaseContent10,CaseContent11,CaseContent12,CaseContent13")] CasePhysicalMental casePhysicalMental)
        {
            if (selected3 != null && selected3.Length > 0)
            {
                // 将选中的复选框值以逗号分隔的字符串形式存储在模型的 content3 属性中
                casePhysicalMental.CaseContent3 = string.Join(",", selected3); ;
            }
            else
            {
                // 没有选中的复选框值时，将 content3 属性设置为 null 或空字符串
                casePhysicalMental.CaseContent3 = null; // 或 model.Content3 = "";
            }
            if (selected4 != null && selected4.Length > 0)
            {
                // 将选中的复选框值以逗号分隔的字符串形式存储在模型的 content3 属性中
                casePhysicalMental.CaseContent4 = string.Join(",", selected4); ;
            }
            else
            {
                // 没有选中的复选框值时，将 content3 属性设置为 null 或空字符串
                casePhysicalMental.CaseContent4 = null; // 或 model.Content3 = "";
            }
            if (ModelState.IsValid)
            {
                _context.Add(casePhysicalMental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", casePhysicalMental.CaseNo);
            return View(casePhysicalMental);
        }

        // GET: CasePhysicalMentals/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePhysicalMental = await _context.CasePhysicalMentals.FindAsync(id);
            if (casePhysicalMental == null)
            {
                return NotFound();
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", casePhysicalMental.CaseNo);
            return View(casePhysicalMental);
        }

        // POST: CasePhysicalMentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseQaid,CaseNo,CaseLive,CaseFre,CaseContent1,CaseContent2,CaseContent3,CaseContent4,CaseContent5,CaseContent6,CaseContent7,CaseContent8,CaseContent9,CaseContent10,CaseContent11,CaseContent12,CaseContent13")] CasePhysicalMental casePhysicalMental)
        {
            if (id != casePhysicalMental.CaseQaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casePhysicalMental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasePhysicalMentalExists(casePhysicalMental.CaseQaid))
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", casePhysicalMental.CaseNo);
            return View(casePhysicalMental);
        }

        // GET: CasePhysicalMentals/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePhysicalMental = await _context.CasePhysicalMentals
                .Include(c => c.CaseNoNavigation)
                .FirstOrDefaultAsync(m => m.CaseQaid == id);
            if (casePhysicalMental == null)
            {
                return NotFound();
            }

            return View(casePhysicalMental);
        }

        // POST: CasePhysicalMentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var casePhysicalMental = await _context.CasePhysicalMentals.FindAsync(id);
            _context.CasePhysicalMentals.Remove(casePhysicalMental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasePhysicalMentalExists(string id)
        {
            return _context.CasePhysicalMentals.Any(e => e.CaseQaid == id);
        }

        [HttpPost]
        public IActionResult ExportLatestCasePhysicalMentals(string exportType, string CaseIDcard)
        {
            using (var dbContext = new longtermcareContext())
            {
                var latestRecord = dbContext.CaseInfors
                    .Where(ci => ci.CaseIdcard == CaseIDcard)
                    .Join(
                        dbContext.CasePhysicalMentals,
                        ci => ci.CaseNo,
                        ccr => ccr.CaseNo,
                        (ci, ccr) => new
                        {
                            CaseInfo = ci,
                            CasePhysicalMental = ccr
                        })
                    .OrderByDescending(c => c.CasePhysicalMental.CaseQaid)
                    .Select(c => new PhysicalSearchResultViewModel
                    {
                        CaseName = c.CaseInfo.CaseName,
                        CaseBd = c.CaseInfo.CaseBd,
                        CaseGender = c.CaseInfo.CaseGender,
                        CaseNo = c.CaseInfo.CaseIdcard,
                        CaseIdent = c.CaseInfo.CaseIdent,
                        CaseLang = c.CaseInfo.CaseLang,
                        CaseMari = c.CaseInfo.CaseMari,
                        CaseFami = c.CaseInfo.CaseFami,
                        CaseAddr = c.CaseInfo.CaseAddr,
                        CaseCnta = c.CaseInfo.CaseCnta,
                        CaseCntTel = c.CaseInfo.CaseCntTel,
                        CaseCntRel = c.CaseInfo.CaseCntRel,
                        CaseLive = c.CasePhysicalMental.CaseLive,
                        CaseFre = c.CasePhysicalMental.CaseFre,
                        CaseContent1 = c.CasePhysicalMental.CaseContent1,
                        CaseContent2 = c.CasePhysicalMental.CaseContent2,
                        CaseContent3 = c.CasePhysicalMental.CaseContent3,
                        CaseContent4 = c.CasePhysicalMental.CaseContent4,
                        CaseContent5 = c.CasePhysicalMental.CaseContent5,
                        CaseContent6 = c.CasePhysicalMental.CaseContent6,
                        CaseContent7 = c.CasePhysicalMental.CaseContent7,
                        CaseContent8 = c.CasePhysicalMental.CaseContent8,
                        CaseContent9 = c.CasePhysicalMental.CaseContent9,
                        CaseContent10 = c.CasePhysicalMental.CaseContent10,
                        CaseContent11 = c.CasePhysicalMental.CaseContent11,
                        CaseContent12 = c.CasePhysicalMental.CaseContent12,
                        CaseContent13 = c.CasePhysicalMental.CaseContent13
                    })
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
                    headerRow.CreateCell(5).SetCellValue("婚姻");
                    headerRow.CreateCell(6).SetCellValue("家庭");
                    headerRow.CreateCell(7).SetCellValue("聯絡人");
                    headerRow.CreateCell(8).SetCellValue("關係");
                    headerRow.CreateCell(9).SetCellValue("聯絡人電話");


                    var personalInfoRow = sheet.CreateRow(1);
                    personalInfoRow.CreateCell(0).SetCellValue(latestRecord.CaseName);
                    personalInfoRow.CreateCell(1).SetCellValue(latestRecord.CaseBd.ToString());
                    personalInfoRow.CreateCell(2).SetCellValue(latestRecord.CaseGender);
                    personalInfoRow.CreateCell(3).SetCellValue(latestRecord.CaseIdent);
                    personalInfoRow.CreateCell(4).SetCellValue(latestRecord.CaseLang);
                    personalInfoRow.CreateCell(5).SetCellValue(latestRecord.CaseMari);
                    personalInfoRow.CreateCell(6).SetCellValue(latestRecord.CaseFami);
                    personalInfoRow.CreateCell(7).SetCellValue(latestRecord.CaseCnta);
                    personalInfoRow.CreateCell(8).SetCellValue(latestRecord.CaseCntRel);
                    personalInfoRow.CreateCell(9).SetCellValue(latestRecord.CaseCntTel);

                    var dailyHeaderRow2 = sheet.CreateRow(2);

                    dailyHeaderRow2.CreateCell(0).SetCellValue("居住情形");
                    dailyHeaderRow2.CreateCell(1).SetCellValue("來關懷站頻率");

                    var dailyHeaderRow3 = sheet.CreateRow(3);
                    dailyHeaderRow3.CreateCell(0).SetCellValue(latestRecord.CaseLive);
                    dailyHeaderRow3.CreateCell(1).SetCellValue(latestRecord.CaseFre);

                    var dailyHeaderRow4 = sheet.CreateRow(4);
                    dailyHeaderRow4.CreateCell(0).SetCellValue("精神是否較好");
                    dailyHeaderRow4.CreateCell(1).SetCellValue(latestRecord.CaseContent1);

                    var dailyHeaderRow5 = sheet.CreateRow(5);
                    dailyHeaderRow5.CreateCell(0).SetCellValue("定期檢測血壓、體溫、體重，是否對您有幫助");
                    dailyHeaderRow5.CreateCell(1).SetCellValue(latestRecord.CaseContent2);

                    var dailyHeaderRow6 = sheet.CreateRow(6);
                    dailyHeaderRow6.CreateCell(0).SetCellValue("經常參與的活動");
                    dailyHeaderRow6.CreateCell(1).SetCellValue(latestRecord.CaseContent3);

                    var dailyHeaderRow7 = sheet.CreateRow(7);
                    dailyHeaderRow7.CreateCell(0).SetCellValue("是否學到新東西");
                    dailyHeaderRow7.CreateCell(1).SetCellValue(latestRecord.CaseContent4);

                    var dailyHeaderRow8 = sheet.CreateRow(8);
                    dailyHeaderRow8.CreateCell(0).SetCellValue("是否有多結交了一些朋友");
                    dailyHeaderRow8.CreateCell(1).SetCellValue(latestRecord.CaseContent5);

                    var dailyHeaderRow9 = sheet.CreateRow(9);
                    dailyHeaderRow9.CreateCell(0).SetCellValue("心情是否改變");
                    dailyHeaderRow9.CreateCell(1).SetCellValue(latestRecord.CaseContent6);

                    var dailyHeaderRow10 = sheet.CreateRow(10);
                    dailyHeaderRow10.CreateCell(0).SetCellValue("場地規劃與設備提供是否滿意");
                    dailyHeaderRow10.CreateCell(1).SetCellValue(latestRecord.CaseContent7);

                    var dailyHeaderRow11 = sheet.CreateRow(11);
                    dailyHeaderRow11.CreateCell(0).SetCellValue("志工的服務態度是否滿意");
                    dailyHeaderRow11.CreateCell(1).SetCellValue(latestRecord.CaseContent8);
                    var dailyHeaderRow12 = sheet.CreateRow(12);
                    dailyHeaderRow12.CreateCell(0).SetCellValue("健康促進活動是否滿意");
                    dailyHeaderRow12.CreateCell(1).SetCellValue(latestRecord.CaseContent9);
                    var dailyHeaderRow13 = sheet.CreateRow(13);
                    dailyHeaderRow13.CreateCell(0).SetCellValue("餐飲服務情形是否滿意");
                    dailyHeaderRow13.CreateCell(1).SetCellValue(latestRecord.CaseContent10);
                    var dailyHeaderRow14 = sheet.CreateRow(14);
                    dailyHeaderRow14.CreateCell(0).SetCellValue("是否喜歡到C單位巷弄長照站活動");
                    dailyHeaderRow14.CreateCell(1).SetCellValue(latestRecord.CaseContent11);
                    var dailyHeaderRow15 = sheet.CreateRow(15);
                    dailyHeaderRow15.CreateCell(0).SetCellValue("辦理的活動是否適合您");
                    dailyHeaderRow15.CreateCell(1).SetCellValue(latestRecord.CaseContent12);
                    var dailyHeaderRow16 = sheet.CreateRow(16);
                    dailyHeaderRow16.CreateCell(0).SetCellValue("此活動之後，對您生活有什麼影響(改變)");
                    dailyHeaderRow16.CreateCell(1).SetCellValue(latestRecord.CaseContent13);

                    using (var memoryStream = new MemoryStream())
                    {
                        workbook.Write(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LatestCasePhysicalMentals.xlsx");
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


                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(15);
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("居住情形").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("來關懷站頻率").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("精神是否較好").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("定期檢測血壓、體溫、體重，是否對您有幫助").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("經常參與的活動").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("是否學到新東西").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("是否有多結交了一些朋友").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("心情是否改變").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("場地規劃與設備提供是否滿意").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("餐飲服務情形是否滿意").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("是否喜歡到C單位巷弄長照站活動").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("辦理的活動是否適合您").SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("此活動之後，對您生活有什麼影響(改變)").SetFont(font)));


                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseLive)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseFre)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent1)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent2)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent3)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent4)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent5)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent6)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent7)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent8)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent9)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent10)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent11)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent12)).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph((latestRecord.CaseContent13)).SetFont(font)));


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
