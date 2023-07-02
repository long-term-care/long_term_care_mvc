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
            var data = _context.MemSigns.Where(x=>x.MemSignDate == today).FirstOrDefault(x => x.MemSid == userName);

            if(data == null)
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string MemSid, DateTime MemTelTime1)
        {
            if (string.IsNullOrEmpty(MemSid))
            {
                return Content("必須填入志工id !");
            }
            if (MemTelTime1 == DateTime.MinValue)
            {
                return Content("必须填入年月!");
            }
            var no1 = from ms in _context.MemSigns
                      join mi in _context.MemberInformations on ms.MemSid equals mi.MemSid
                      where ms.MemSid == MemSid && ms.MemSignDate.Month == MemTelTime1.Month && ms.MemSignDate.Year == MemTelTime1.Year

                      select new MemSignSearchResultViewModel
                      {
                          MemYM = (DateTime)ms.MemTelTime1,
                          MemName = mi.MemName,
                          MemDate = (DateTime)ms.MemTelTime1,
                          MemTelTime1 = (DateTime)ms.MemTelTime1,
                          MemTelTime2 = (DateTime)ms.MemTelTime2,
                          MemRecord = ms.MemRecord,
                      };
            var no2 = await no1.ToListAsync();
            if (no2 == null)
            {
                return NotFound();
            }

            return View("SearchResult", no2);
        }
    }

}
