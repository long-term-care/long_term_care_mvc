using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost]
        public IActionResult Index([FromBody] LectureClassViewModel model)
        {
            var record = _context.LectureClasses.Where(l => l.SchWeek == model.Week);
            if (record != null)
            {
                foreach (var item in record)
                {
                    if (model.Time == "SchA")
                    {
                        item.SchA = model.Subject;

                    }
                    if (model.Time == "SchB")
                    {
                        item.SchB = model.Subject;

                    }
                    if (model.Time == "SchC")
                    {
                        item.SchC = model.Subject;

                    }
                    if (model.Time == "SchD")
                    {
                        item.SchD = model.Subject;

                    }

                }
                _context.SaveChanges();
            }
           
            return View("Index");
        }
    }
}
