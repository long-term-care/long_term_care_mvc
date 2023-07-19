using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using NPOI.XSSF.UserModel;
using iText.Layout.Properties;

namespace long_term_care.Controllers
{
    public class LectureTablesController : Controller
    {
        private readonly longtermcareContext _context;

        public LectureTablesController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: LectureTables
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.LectureTables.Include(l => l.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        public IActionResult Create()
        {
            string nextFormNumber = "";


            var lastForm = _context.LectureTables.OrderByDescending(f => f.LecId).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.LecId);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }


            ViewData["LecId"] = nextFormNumber;
            return View();
        }


        [HttpPost]
        public IActionResult Create([FromBody] LectureTableViewModel model)
        {
            //string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            string userName = User.Identity.Name;

            var data = new LectureTable
            {
                LecId = model.LecId,
                LecTheme = model.LecTheme,
                LecAim = model.LecAim,
                LecClass = model.LecClass,
                LecDate = model.LecDate,
                LecLeader = model.LecLeader,
                LecPla = model.LecPla,
                LecStep = model.LecStep,
                LecTool = model.LecTool,
                MemSid = userName,
            };
            _context.LectureTables.Add(data);
            _context.SaveChanges();

            return View();
        }

        public IActionResult Check(string id)
        {
            var lectureTable = _context.LectureTables.FirstOrDefault(x => x.LecId == id);

            // 传递单个模型对象
            return View(lectureTable);
        }
        public IActionResult Search(string searchTerm)
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


        public IActionResult Revise(string id)
        {
            var lectureTable = _context.LectureTables.FirstOrDefault(x => x.LecId == id);

            // 传递单个模型对象
            return View(lectureTable);
        }
        [HttpPost]
        public IActionResult Revise([FromBody] LectureTableViewModel model)
        {
            var data = _context.LectureTables.FirstOrDefault(x => x.LecId == model.LecId);
            data.LecTool = model.LecTool;
            data.LecTheme = model.LecTheme;
            data.LecLeader = model.LecLeader;
            data.LecPla = model.LecPla;
            data.LecStep = model.LecStep;
            data.LecDate = model.LecDate;
            data.LecAim = model.LecAim;
            data.LecClass = model.LecClass;

            _context.SaveChanges();
            return View();
        }
        [HttpPost]
        public IActionResult ExportData(string exportType,string Id)
        {
            var data = _context.LectureTables.FirstOrDefault(x => x.LecId == Id);

            if (exportType == "excel")
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("問卷編號");
                headerRow.CreateCell(1).SetCellValue("填寫人");
                headerRow.CreateCell(2).SetCellValue("主題");
                headerRow.CreateCell(3).SetCellValue("活動類別");
                headerRow.CreateCell(4).SetCellValue("活動目的");
                headerRow.CreateCell(5).SetCellValue("活動時間");
                headerRow.CreateCell(6).SetCellValue("活動帶領");
                headerRow.CreateCell(7).SetCellValue("活動場地");
                headerRow.CreateCell(8).SetCellValue("用物預備");
                headerRow.CreateCell(9).SetCellValue("步驟");

                var personalInfoRow = sheet.CreateRow(1);
                personalInfoRow.CreateCell(0).SetCellValue(data.LecId);
                personalInfoRow.CreateCell(1).SetCellValue(data.MemSid);
                personalInfoRow.CreateCell(2).SetCellValue(data.LecTheme);
                personalInfoRow.CreateCell(3).SetCellValue(data.LecClass);
                personalInfoRow.CreateCell(4).SetCellValue(data.LecAim);
                personalInfoRow.CreateCell(5).SetCellValue(data.LecDate);
                personalInfoRow.CreateCell(6).SetCellValue(data.LecLeader);
                personalInfoRow.CreateCell(7).SetCellValue(data.LecPla);
                personalInfoRow.CreateCell(8).SetCellValue(data.LecTool);
                personalInfoRow.CreateCell(9).SetCellValue(data.LecStep);


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
                    iText.Layout.Element.Paragraph paragraph = new iText.Layout.Element.Paragraph("\n\n\n").SetFont(font);
                    paragraph.Add(new iText.Layout.Element.Text("問卷編號: " + data.LecId).SetFont(font));
                    paragraph.Add(new iText.Layout.Element.Text("\t填寫人: " + data.MemSid).SetFont(font));
                    document.Add(paragraph);

                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(2);

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("主題:").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecTheme).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動類別").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecClass).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動目的").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecAim).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動時間").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecDate.ToString("yyyy/MM/dd")).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動帶領").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecLeader).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("活動場地").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecPla).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("用物預備").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecTool).SetFont(font)));

                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("步驟").SetFont(font)));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(data.LecStep).SetFont(font)));

                    // Set table properties to allow cell content to wrap to multiple lines
                    table.SetWidth(UnitValue.CreatePercentValue(100));
                    table.SetHorizontalAlignment(HorizontalAlignment.LEFT);

                    document.Add(table);


                    document.Close();

                    return File(mem.ToArray(), "application/pdf", "data.pdf");
                }
            }


            return BadRequest("格式錯誤");
        }

    }
}
