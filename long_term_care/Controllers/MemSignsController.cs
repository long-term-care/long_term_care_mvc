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
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.XSSF.UserModel;
using System.IO;
using iText.Layout.Properties;

namespace long_term_care.Controllers
{
    public class MemSignsController : Controller
    {
        private readonly longtermcareContext _context;

        public MemSignsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: MemSigns
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.MemSigns.Include(m => m.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: MemSigns/Details/5
        public IActionResult Sign()
        {
            string userName = User.Identity.Name;
            var member = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);

            string nextFormNumber = "";


            var lastForm = _context.MemSigns.OrderByDescending(f => f.MemSignQaid).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.MemSignQaid);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }
            ViewData["MemSignQaid"] = nextFormNumber;
            return View(member);
        }
        [HttpPost]
        public IActionResult Sign([FromBody] MemsignViewModel model)
        {
            string userName = User.Identity.Name;
            var member = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);

            var today = _context.MemSigns.Where(x=>x.MemSid==userName).FirstOrDefault(x=>x.MemSignDate == model.MemTelTime1);
            if(today != null)
            {
                string message = "今天已簽到過了!";
                return Content(message);

            }
            else
            {
                var data = new MemSign()
                {
                    MemSignQaid = model.MemSignQaid,
                    MemSid = member.MemSid,
                    MemTelTime1 = model.MemTelTime1,
                    MemTelTime2 = null,
                    MemSignDate = model.MemTelTime1
                };
                _context.MemSigns.Add(data);
                _context.SaveChanges();
                return View();
            }
            
        }


        public IActionResult Signout()
        {
            string userName = User.Identity.Name;
            var member = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);


            string nextFormNumber = "";


            var lastForm = _context.MemSigns.OrderByDescending(f => f.MemSignQaid).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.MemSignQaid);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }
            ViewData["MemSignQaid"] = nextFormNumber;

            return View(member);
        }
        [HttpPost]
        public IActionResult Signout([FromBody] MemsignViewModel model)
        {
            string userName = User.Identity.Name;
            var member = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);
            DateTime today = DateTime.Today;
            var data = _context.MemSigns.Where(x => x.MemSignDate == today).FirstOrDefault(x => x.MemSid == userName);

            if (data == null)
            {
                var signdata = new MemSign
                {
                    MemSignQaid = model.MemSignQaid,
                    MemSid = member.MemSid,
                    MemTelTime1 = null,
                    MemTelTime2 = model.MemTelTime2,
                    MemRecord = model.MemRecord,
                    MemSignDate = today,
                };
                _context.MemSigns.Add(signdata);
                _context.SaveChanges();
            }
            else
            {
                data.MemTelTime2 = model.MemTelTime2;
                data.MemRecord = model.MemRecord;
                _context.SaveChanges();
            }
            return View();
        }


        public IActionResult Details()
        {
            string userName = User.Identity.Name;
            var member = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(string Name, DateTime MemTelTime1)
        {
            if (MemTelTime1 == DateTime.MinValue)
            {
                return Content("必须填入年月!");
            }

         var no1 = _context.MemSigns
        .Include(t1 => t1.MemS)
        .Where(x => x.MemSid == Name && x.MemSignDate.Month == MemTelTime1.Month && x.MemSignDate.Year == MemTelTime1.Year)
        .Select(x => new MemSignSearchResultViewModel
        {
            MemYM = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
            MemSignQaid = x.MemSignQaid,
            MemSid = x.MemSid,
            MemName = x.MemS.MemName,
            MemDate = x.MemSignDate,
            MemTelTime1 = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
            MemTelTime2 = x.MemTelTime2.HasValue ? x.MemTelTime2.Value : DateTime.Today,
            MemRecord = x.MemRecord ?? string.Empty
        }).ToList();
            return View("SearchResult", no1);
        }

        [HttpPost]
        public IActionResult Checksign(string Qid, DateTime time, string Name, DateTime CheckDate)
        {
            var data = _context.MemSigns.FirstOrDefault(x => x.MemSignQaid == Qid);

            DateTime currentDate = data.MemSignDate.Date;

            // 建立新的日期時間物件，將日期部分設置為指定的日期，時間部分保持不變
            DateTime newDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, time.Hour, time.Minute, time.Second);
            data.MemTelTime1 = newDate;
            _context.SaveChanges();


         var no1 = _context.MemSigns
        .Include(t1 => t1.MemS)
        .Where(x => x.MemSid == Name && x.MemSignDate.Month == CheckDate.Month && x.MemSignDate.Year == CheckDate.Year)
        .Select(x => new MemSignSearchResultViewModel
        {
            MemYM = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
            MemSignQaid = x.MemSignQaid,
            MemSid = x.MemSid,
            MemName = x.MemS.MemName,
            MemDate = x.MemSignDate,
            MemTelTime1 = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
            MemTelTime2 = x.MemTelTime2.HasValue ? x.MemTelTime2.Value : DateTime.Today,
            MemRecord = x.MemRecord ?? string.Empty
        }).ToList();
            return View("SearchResult", no1);
        }
        [HttpPost]
        public IActionResult Checksignout(string Qid, DateTime time, string Name, DateTime CheckDate)
        {
            var data = _context.MemSigns.FirstOrDefault(x => x.MemSignQaid == Qid);

            DateTime currentDate = data.MemSignDate.Date;

            // 建立新的日期時間物件，將日期部分設置為指定的日期，時間部分保持不變
            DateTime newDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, time.Hour, time.Minute, time.Second);
            data.MemTelTime2 = newDate;
            _context.SaveChanges();

         var no1 = _context.MemSigns
       .Include(t1 => t1.MemS)
       .Where(x => x.MemSid == Name && x.MemSignDate.Month == CheckDate.Month && x.MemSignDate.Year == CheckDate.Year)
       .Select(x => new MemSignSearchResultViewModel
       {
           MemYM = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
           MemSignQaid = x.MemSignQaid,
           MemSid = x.MemSid,
           MemName = x.MemS.MemName,
           MemDate = x.MemSignDate,
           MemTelTime1 = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
           MemTelTime2 = x.MemTelTime2.HasValue ? x.MemTelTime2.Value : DateTime.Today,
           MemRecord = x.MemRecord ?? string.Empty
       }).ToList();
            return View("SearchResult", no1);
        }



        private IEnumerable<MemSignSearchResultViewModel> GetDataForDateRange( String id,DateTime startDate, DateTime endDate)
        {
            using (var dbContext = new longtermcareContext())
            {
                
                var no1 = _context.MemSigns
              .Include(t1 => t1.MemS)
              .Where(x => x.MemSid == id && x.MemSignDate >= startDate && x.MemSignDate <= endDate)
              .OrderBy(x=>x.MemSignDate)
              .Select(x => new MemSignSearchResultViewModel
              {
                  MemYM = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
                  MemSignQaid = x.MemSignQaid,
                  MemSid = x.MemSid,
                  MemName = x.MemS.MemName,
                  MemDate = x.MemSignDate,
                  MemTelTime1 = x.MemTelTime1.HasValue ? x.MemTelTime1.Value : DateTime.Today,
                  MemTelTime2 = x.MemTelTime2.HasValue ? x.MemTelTime2.Value : DateTime.Today,
                  MemRecord = x.MemRecord ?? string.Empty
              }).ToList();

                return no1;
            }
            
        }


        [HttpPost]
        public IActionResult ExportData(string exportType, String id, DateTime startDate, DateTime endDate)
        {
            var data = GetDataForDateRange(id,startDate, endDate);

            if (exportType == "excel")
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("編號");
                headerRow.CreateCell(1).SetCellValue("姓名");
                var personalInfoRow = sheet.CreateRow(1);
                personalInfoRow.CreateCell(0).SetCellValue(data.First().MemSid);
                personalInfoRow.CreateCell(1).SetCellValue(data.First().MemName);

                var SigntitleRow = sheet.CreateRow(2);
                SigntitleRow.CreateCell(0).SetCellValue("簽到日期");
                SigntitleRow.CreateCell(1).SetCellValue("簽到時間");
                SigntitleRow.CreateCell(2).SetCellValue("簽退時間");
                SigntitleRow.CreateCell(3).SetCellValue("工作日誌");

                int rowIndex = 3;
                foreach (var item in data)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(item.MemDate.ToString("yyyy/MM/dd"));
                    row.CreateCell(1).SetCellValue(item.MemTelTime1.ToString("t"));
                    row.CreateCell(2).SetCellValue(item.MemTelTime2.ToString("t"));
                    row.CreateCell(3).SetCellValue(item.MemRecord);
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
                    paragraph.Add(new iText.Layout.Element.Text("編號: " + data.First().MemSid).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\t姓名: " + data.First().MemName).SetFont(font));
                    document.Add(paragraph);

                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(4);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("簽到日期").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("簽到時間").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("謙退時間").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("工作日誌").SetFont(font)));


                    foreach (var item in data)
                    {
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.MemDate.ToString("yyyy/MM/dd")).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.MemTelTime1.ToString("t")).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.MemTelTime2.ToString("t"))));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.MemRecord).SetFont(font)));
                    }

                    iText.Layout.Element.Paragraph tableParagraph = new iText.Layout.Element.Paragraph().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableParagraph.Add(table);

                    document.Add(tableParagraph);

                    document.Close();

                    return File(mem.ToArray(), "application/pdf", "data.pdf");
                }
            }


            return BadRequest("格式錯誤");
        }
    }
}
