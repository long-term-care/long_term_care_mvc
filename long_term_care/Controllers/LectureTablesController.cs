using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;

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
        public IActionResult Create(LectureTableViewModel model)
        {

            return View();
        }

        public IActionResult Check()
        {
            var lectureTable = _context.LectureTables.FirstOrDefault(x => x.LecId == "0001");

            // 传递单个模型对象
            return View(lectureTable);


        }
    }
}
