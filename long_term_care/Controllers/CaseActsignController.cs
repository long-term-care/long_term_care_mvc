using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                model = model.Where(m => m.ActCourse.Contains(searchTerm)).ToList();
            }

            return View(model);
        }
        public List<CaseAct> YourDataAccessMethod(string searchTerm)
        {
            var dbContext = new longtermcareContext(); // 替换为您自己的 DbContext

            var model = dbContext.CaseActs.ToList(); // 获取所有表单数据

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(m => m.ActCourse.Contains(searchTerm)).ToList(); // 使用关键字进行过滤
            }

            return model;
        }

        public IActionResult CheckAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = _context.CaseActs
    .Include(t1 => t1.CaseActContents)
        .ThenInclude(t2 => t2.CaseNoNavigation)
    .Where(x => x.ActId == id)
    .SelectMany(t1 => t1.CaseActContents.DefaultIfEmpty(), (t1, t2) => new CaseActsignViewModel
    {
        ActId = t1.ActId,
        ActCourse = t1.ActCourse,
        ActDate = t1.ActDate,
        ActLec = t1.ActLec,
        ActLoc = t1.ActLoc,
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
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CaseActsignViewModel model)
        {
            var id = _context.CaseActs.Count(x => x.ActId != null) + 1;
            var actid = "Act" + id;
            if (model.type == 1)
            {
                var Act = new CaseAct
                {
                    ActId = actid,
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
            var data = _context.CaseActs
            .Include(t1 => t1.CaseActContents)
            .ThenInclude(t2 => t2.CaseNoNavigation)
            .Where(x => x.ActId == id)
            .SelectMany(t1 => t1.CaseActContents.DefaultIfEmpty(), (t1, t2) => new CaseActsignViewModel
             {
             ActId = t1.ActId,
             ActCourse = t1.ActCourse,
             ActDate = t1.ActDate,
             ActLec = t1.ActLec,
             ActLoc = t1.ActLoc,
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
                ActId = model.ActId,
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
            var existingCase = _context.CaseActContents.FirstOrDefault(c => c.ActId == model.ActId && c.CaseNo == model.CaseNo);

            if (existingCase != null)
            {
                _context.CaseActContents.Remove(existingCase);
                _context.SaveChanges(); // 使用SaveChanges()而不是SaveChangesAsync()，以确保操作同步保存到数据库
                return Ok(); // 返回200 OK状态码，表示成功
            }

            return NotFound(); // 返回404 Not Found状态码，表示未找到要删除的数据
        }

    }
}
