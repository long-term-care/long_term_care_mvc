using DinkToPdf;
using long_term_care.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using IronPdf;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Text;

namespace long_term_care.Controllers

{
    public class PDFController : Controller
    {
        
        [HttpPost]
        public IActionResult ExportToPdf([FromBody] List<List<string>> tableData)
        {
            try
            {
                // 根据需要处理接收到的表格数据

                // 创建一个 StringBuilder 来构建 HTML 表格内容
                var htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<table>");

                // 遍历传递的表格数据，构建表格的行和列
                foreach (var row in tableData)
                {
                    htmlBuilder.AppendLine("<tr>");

                    foreach (var cell in row)
                    {
                        htmlBuilder.AppendLine($"<td>{cell}</td>");
                    }

                    htmlBuilder.AppendLine("</tr>");
                }

                htmlBuilder.AppendLine("</table>");

                var htmlContent = htmlBuilder.ToString();

                // 使用 DinkToPdf 或其他 PDF 库将 HTML 转换为 PDF
                // 这里使用的是 DinkToPdf，确保已正确设置和配置 DinkToPdf

                var converter = new BasicConverter(new PdfTools());
                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings =
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
                    Objects =
            {
                new ObjectSettings()
                {
                    HtmlContent = htmlContent
                }
            }
                };

                var pdfBytes = converter.Convert(doc);

                // 将生成的 PDF 字节数组返回给客户端进行下载
                return File(pdfBytes, "application/pdf", "TableExport.pdf");
            }
            catch (Exception ex)
            {
                // 处理异常
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }


}
