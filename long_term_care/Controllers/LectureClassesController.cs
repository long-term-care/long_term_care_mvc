using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace long_term_care.Controllers
{
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
          
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromBody] LectureClassViewModel model)
        {
           
           
            return View();
        }
        public async Task<IActionResult> Check()
        {
            var lectureClasses = await _context.LectureClasses.ToListAsync();
            return View(lectureClasses);
        }
    }
}
