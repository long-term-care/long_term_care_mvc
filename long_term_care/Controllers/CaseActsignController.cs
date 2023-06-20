using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.CaseActs.ToListAsync());
        }

        public IActionResult CheckAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = _context.CaseActs
                .Include(t1=>t1.CaseActContents)
                .ThenInclude(t2=>t2.CaseNoNavigation)
                .Where(x=>x.ActId == id)
                .SelectMany(t1 => t1.CaseActContents,(t1,t2) =>new CaseActsignViewModel
                {
                 ActId = t1.ActId,
                 ActCourse = t1.ActCourse,
                 ActDate = t1.ActDate,
                 ActLec = t1.ActLec,
                 ActLoc = t1.ActLoc,
                 ActSer = t2.ActSer,
                 CaseNo = t2.CaseNoNavigation.CaseNo,
                 CaseName = t2.CaseNoNavigation.CaseName
                }).ToList();
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
           
        }
        public Task<IActionResult> Create()
        {
            return Task.FromResult<IActionResult>(View());
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
            var data = _context.CaseActs.FirstOrDefault(x => x.ActId == id);

            return View(data);
        }
        [HttpPost]
        public IActionResult CreateCaseNo([FromBody] CaseActsignViewModel model)
        {
            var Actcase = new CaseActContent
            {
               ActId = model.ActId,
               CaseNo = model.CaseNo,
               ActSer = model.ActSer
            };
            _context.CaseActContents.Add(Actcase);
            _context.SaveChangesAsync();

            return View();
        }
    }
}
