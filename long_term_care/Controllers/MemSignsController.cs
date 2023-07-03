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
    }
}
