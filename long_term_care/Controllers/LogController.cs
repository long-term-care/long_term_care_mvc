using long_term_care.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace long_term_care.Controllers
{
    public class LogController : Controller
    {
        private readonly longtermcareContext _context;

        public LogController(longtermcareContext context)
        {
            _context = context;
        }
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var changeLogs = _context.ChangeLogs.OrderByDescending(c => c.ActionDate).ToList();

            return View(changeLogs);
        }
         public IActionResult Create()
    {
        // 執行新增操作

        _logger.LogInformation("New record created.");

        return View();
    }

    public IActionResult Edit()
    {
        // 執行修改操作

        _logger.LogInformation("Record updated.");

        return View();
    }

    public IActionResult Delete()
    {
        // 執行刪除操作

        _logger.LogInformation("Record deleted.");

        return View();
    }
    }
}
