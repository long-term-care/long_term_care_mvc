using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace long_term_care.Controllers
{
    [Authorize]
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
    }
}
