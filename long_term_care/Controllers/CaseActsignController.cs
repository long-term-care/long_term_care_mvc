using iText.Layout.Properties;
using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace long_term_care.Controllers
{

    public class CaseActsignController : Controller
    {
        private readonly longtermcareContext _context;

        public CaseActsignController(longtermcareContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm)
        {
            var model = YourDataAccessMethod(searchTerm); // 获取所有表单数据的方法，根据您的实际情况修改

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(m => m.LecTheme.Contains(searchTerm)).ToList();
            }

            return View(model);
        }
        public List<LectureTable> YourDataAccessMethod(string searchTerm)
        {
            var dbContext = new longtermcareContext(); // 替换为您自己的 DbContext

            var model = dbContext.LectureTables.ToList(); // 获取所有表单数据

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(m => m.LecTheme.Contains(searchTerm)).ToList(); // 使用关键字进行过滤
            }

            return model;
        }

        public IActionResult CheckAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = _context.LectureTables
            .Include(t1 => t1.CaseActContents)
            .ThenInclude(t2 => t2.CaseNoNavigation)
            .Where(x => x.LecId == id)
            .SelectMany(t1 => t1.CaseActContents.DefaultIfEmpty(), (t1, t2) => new CaseActsignViewModel
            {
            ActId = t1.LecId,
            ActCourse = t1.LecTheme,
            ActDate = t1.LecDate,
            ActLec = t1.LecLeader,
            ActLoc = t1.LecPla,
            ActSer = t2 != null ? t2.ActSer : string.Empty,
            CaseNo = t2 != null && t2.CaseNoNavigation != null ? t2.CaseNoNavigation.CaseNo : string.Empty,
            CaseName = t2 != null && t2.CaseNoNavigation != null ? t2.CaseNoNavigation.CaseName : string.Empty
            })
            .ToList();

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
           
        }
        public IActionResult Create()
        {
            string nextFormNumber = "";


            var lastForm = _context.CaseActs.OrderByDescending(f => f.ActId).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.ActId);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }
            ViewData["ActId"] = nextFormNumber;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CaseActsignViewModel model)
        {
           
            if (model.type == 1)
            {
                var Act = new CaseAct
                {
                    ActId = model.ActId,
                    ActCourse = model.ActCourse,
                    ActDate = model.ActDate,
                    ActLec = model.ActLec,
                    ActLoc = model.ActLoc,
                };
                _context.CaseActs.Add(Act);
                await _context.SaveChangesAsync();
            }

            return View();
        }


        public IActionResult CreateCaseNo(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = _context.LectureTables
            .Include(t1 => t1.CaseActContents)
            .ThenInclude(t2 => t2.CaseNoNavigation)
            .Where(x => x.LecId == id)
            .SelectMany(t1 => t1.CaseActContents.DefaultIfEmpty(), (t1, t2) => new CaseActsignViewModel
            {
            ActId = t1.LecId,
            ActCourse = t1.LecTheme,
            ActDate = t1.LecDate,
            ActLec = t1.LecLeader,
            ActLoc = t1.LecPla,
            ActSer = t2 != null ? t2.ActSer : string.Empty,
            CaseNo = t2 != null && t2.CaseNoNavigation != null ? t2.CaseNoNavigation.CaseNo : string.Empty,
            CaseName = t2 != null && t2.CaseNoNavigation != null ? t2.CaseNoNavigation.CaseName : string.Empty
            })
            .ToList();
            if (data == null)
            {
                return NotFound();
            }           
            ViewBag.CaseNoList = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            return View(data);
        }

        [HttpPost]
        public IActionResult CreateCaseNo([FromBody] CaseActsignViewModel model)
        {
            ViewBag.CaseNoList = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            var Actcase = new CaseActContent
            {
                LecId = model.ActId,
                CaseNo = model.CaseNo,
                ActSer = model.ActSer
            };
            _context.CaseActContents.Add(Actcase);
            _context.SaveChanges(); // 使用SaveChanges()而不是SaveChangesAsync()，以确保操作同步保存到数据库
            return Ok(); // 返回200 OK状态码，表示成功
        }

        [HttpPost]
        public IActionResult DeleteCaseNo([FromBody] CaseActsignViewModel model)
        {
            var existingCase = _context.CaseActContents.FirstOrDefault(c => c.LecId == model.ActId && c.CaseNo == model.CaseNo);

            if (existingCase != null)
            {
                _context.CaseActContents.Remove(existingCase);
                _context.SaveChanges(); // 使用SaveChanges()而不是SaveChangesAsync()，以确保操作同步保存到数据库
                return Ok(); // 返回200 OK状态码，表示成功
            }

            return NotFound(); // 返回404 Not Found状态码，表示未找到要删除的数据
        }
        private IEnumerable<CaseActsignViewModel> GetDataForDateRange(string Id)
        {
            using (var dbContext = new longtermcareContext())
            {
                var data = _context.LectureTables
                .Include(t1 => t1.CaseActContents)
                .ThenInclude(t2 => t2.CaseNoNavigation)
                .Where(x => x.LecId == Id)
                .SelectMany(t1 => t1.CaseActContents.DefaultIfEmpty(), (t1, t2) => new CaseActsignViewModel
                {
                ActId = t1.LecId,
                ActCourse = t1.LecTheme,
                ActDate = t1.LecDate,
                ActLec = t1.LecLeader,
                ActLoc = t1.LecPla,
                ActSer = t2 != null ? t2.ActSer : string.Empty,
                CaseNo = t2 != null && t2.CaseNoNavigation != null ? t2.CaseNoNavigation.CaseNo : string.Empty,
                CaseName = t2 != null && t2.CaseNoNavigation != null ? t2.CaseNoNavigation.CaseName : string.Empty
                })
                .ToList();
                return data;
            }
        }


        [HttpPost]
        public IActionResult ExportData(string exportType,string Id)
        {
            var data = GetDataForDateRange(Id);

            if (exportType == "excel")
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("活動名稱");
                headerRow.CreateCell(1).SetCellValue("活動日期");
                headerRow.CreateCell(2).SetCellValue("講師姓名");
                headerRow.CreateCell(4).SetCellValue("活動地點");

                var personalInfoRow = sheet.CreateRow(1);
                personalInfoRow.CreateCell(0).SetCellValue(data.First().ActCourse);
                personalInfoRow.CreateCell(1).SetCellValue(data.First().ActDate);
                personalInfoRow.CreateCell(2).SetCellValue(data.First().ActLec);
                personalInfoRow.CreateCell(3).SetCellValue(data.First().ActLoc);


                var dailyHeaderRow = sheet.CreateRow(2);
                headerRow.CreateCell(5).SetCellValue("學員姓名");
                headerRow.CreateCell(6).SetCellValue("服務項目");

                int rowIndex = 3;
                foreach (var item in data.Reverse())
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(item.CaseName);
                    row.CreateCell(1).SetCellValue(item.ActSer);
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



                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動名稱").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.First().ActCourse).SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動日期").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.First().ActDate.ToString("yyyy/MM/dd")).SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("講師姓名").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.First().ActLec).SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動地點").SetFont(font)));           
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.First().ActLoc).SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("學員姓名").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("服務項目").SetFont(font)));
                    foreach (var item in data)
                    {
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.CaseName).SetFont(font)));
                        table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.ActSer).SetFont(font)));
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
