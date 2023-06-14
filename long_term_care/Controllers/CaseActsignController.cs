using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CaseActsignViewModel model)
        {
            return View();
        }

    }
}
