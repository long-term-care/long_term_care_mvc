using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            
            
            return View(lectureClasses);
        }
        public IActionResult Act()
        {
            // 获取本周的日期范围
            DateTime today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = today.AddDays(-1 * diff);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // 从数据库中获取活动表和课表数据
            List<LectureTable> activityList = _context.LectureTables.ToList();
            List<LectureClass> lectureClasses = _context.LectureClasses.ToList();

            // 遍历活动表的数据，将活动添加到本周的课表中
            // 遍歷活動表的資料，將活動加入到本週的課表中
            foreach (var activity in activityList)
            {
                if (activity.LecDate >= startOfWeek && activity.LecDate <= endOfWeek)
                {
                    // 檢查是否已經存在對應時段的課表項目
                    var existingLectureClass = lectureClasses.FirstOrDefault(l => l.SchWeek == GetDayOfWeek(activity.LecDate));

                    if (existingLectureClass != null)
                    {
                        // 更新對應時段的活動主題
                        switch (GetTimeSlot(activity.LecDate))
                        {
                            case "SchA":
                                existingLectureClass.SchA = activity.LecTheme;
                                ViewData["SchA"] = existingLectureClass.Weeknum;
                                break;
                            case "SchB":
                                existingLectureClass.SchB = activity.LecTheme;
                                ViewData["SchB"] = existingLectureClass.Weeknum;
                                break;
                            case "SchC":
                                existingLectureClass.SchC = activity.LecTheme;
                                ViewData["SchC"] = existingLectureClass.Weeknum;
                                break;
                            case "SchD":
                                existingLectureClass.SchD = activity.LecTheme;
                                ViewData["SchD"] = existingLectureClass.Weeknum;
                                break;
                            case "SchE":
                                existingLectureClass.SchE = activity.LecTheme;
                                ViewData["SchE"] = existingLectureClass.Weeknum;
                                break;
                        }

                    }
                }
            }

            // 将课表数据传递给视图并显示
            return View(lectureClasses);
        }
        public IActionResult NextAct()
        {
            // 获取本周的日期范围
            DateTime today = DateTime.Today;
            DateTime oneWeekLater = today.AddDays(7);
            int diff = (7 + (oneWeekLater.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = oneWeekLater.AddDays(-1 * diff);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // 从数据库中获取活动表和课表数据
            List<LectureTable> activityList = _context.LectureTables.ToList();
            List<LectureClass> lectureClasses = _context.LectureClasses.ToList();

            // 遍历活动表的数据，将活动添加到本周的课表中
            // 遍歷活動表的資料，將活動加入到本週的課表中
            foreach (var activity in activityList)
            {
                if (activity.LecDate >= startOfWeek && activity.LecDate <= endOfWeek)
                {
                    // 檢查是否已經存在對應時段的課表項目
                    var existingLectureClass = lectureClasses.FirstOrDefault(l => l.SchWeek == GetDayOfWeek(activity.LecDate));

                    if (existingLectureClass != null)
                    {
                        // 更新對應時段的活動主題
                        switch (GetTimeSlot(activity.LecDate))
                        {
                            case "SchA":
                                existingLectureClass.SchA = activity.LecTheme;
                                ViewData["SchA"] = existingLectureClass.Weeknum;
                                break;
                            case "SchB":
                                existingLectureClass.SchB = activity.LecTheme;
                                ViewData["SchB"] = existingLectureClass.Weeknum;
                                break;
                            case "SchC":
                                existingLectureClass.SchC = activity.LecTheme;
                                ViewData["SchC"] = existingLectureClass.Weeknum;
                                break;
                            case "SchD":
                                existingLectureClass.SchD = activity.LecTheme;
                                ViewData["SchD"] = existingLectureClass.Weeknum;
                                break;
                            case "SchE":
                                existingLectureClass.SchE = activity.LecTheme;
                                ViewData["SchE"] = existingLectureClass.Weeknum;
                                break;
                        }

                    }
                }
            }

            // 将课表数据传递给视图并显示
            return View(lectureClasses);
        }
        public string GetDayOfWeek(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
                default:
                    return string.Empty;
            }
        }
        public string GetTimeSlot(DateTime date)
        {
            // 使用日期（時間）作為判斷依據，設置相應的課表時段
            if (date.TimeOfDay >= new TimeSpan(7, 30, 0) && date.TimeOfDay < new TimeSpan(8, 30, 0))
            {
                return "SchA";
            }
            else if (date.TimeOfDay >= new TimeSpan(8, 30, 0) && date.TimeOfDay < new TimeSpan(9, 0, 0))
            {
                return "SchB";
            }
            else if (date.TimeOfDay >= new TimeSpan(9, 0, 0) && date.TimeOfDay < new TimeSpan(9, 30, 0))
            {
                return "SchC";
            }
            else if (date.TimeOfDay >= new TimeSpan(9, 30, 0) && date.TimeOfDay < new TimeSpan(12, 0, 0))
            {
                return "SchD";
            }
            else if (date.TimeOfDay >= new TimeSpan(13, 0, 0) && date.TimeOfDay < new TimeSpan(16, 0, 0))
            {
                return "SchE";
            }
            else
            {
                return string.Empty;
            }
        }





    }
}
