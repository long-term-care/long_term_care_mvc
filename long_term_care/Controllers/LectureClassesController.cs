using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace long_term_care.Controllers
{
    [Authorize]
    public class LectureClassesController : Controller
    {
        private readonly longtermcareContext _context;

        public LectureClassesController(longtermcareContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lectureClasses = await _context.LectureClasses.ToListAsync();
            return View(lectureClasses);
        }
        public IActionResult Create()
        {
            // 從資料庫取得內容，假設存放在 dbContent 變數中
           

            // 將內容傳遞給視圖
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromBody] LectureClassViewModel model)
        {
            var data = _context.LectureClasses.FirstOrDefault(x => x.SchWeek == model.Week);
            if(model.Time == "SchA")
            {
                data.SchA = model.Subject;
            }
            if (model.Time == "SchB")
            {
                data.SchB = model.Subject;
            }
            if (model.Time == "SchC")
            {
                data.SchC = model.Subject;
            }
            if (model.Time == "SchD")
            {
                data.SchD = model.Subject;
            }
            if (model.Time == "SchE")
            {
                data.SchE = model.Subject;
            }
            _context.SaveChanges();
            return View();
        }
        public async Task<IActionResult> Check()
        {
            var lectureClasses = await _context.LectureClasses.ToListAsync();

            DateTime today = DateTime.Today;
            var act =  _context.LectureTables.Where(x=>x.LecDate == today);
            
            return View(lectureClasses);
        }
    }
}
