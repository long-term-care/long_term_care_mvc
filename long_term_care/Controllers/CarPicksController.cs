using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using long_term_care.ViewModels;
using System.Runtime.ConstrainedExecution;

namespace long_term_care.Controllers
{
    public class CarPicksController : Controller
    {
        private readonly longtermcareContext _context;

        public CarPicksController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CarPicks
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CarPicks.Include(c => c.MemS);
            return View(await longtermcareContext.ToListAsync());
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string CaseNo, DateTime CarSearch)
        {
            if (string.IsNullOrEmpty(CaseNo))
            {
                return Content("請填入長者編號!");
            }
            var no1 = from ci in _context.CarPicks
                      join ccr in _context.MemberInformation on ci.MemSid equals ccr.MemSid
                      where ccr.MemSid == MemSid
                      select new CarPickViewModel
                      {
                          MemSid = ci.MemSid,
                          CarSearchY = ci.CarSearchY,
                          CarSearchM = ci.CarSearchM,
                          CarType = ci.CarType,
                          CarNum = ci.CarNum,
                          CarCaseAdr = ci.CarCaseAdr,
                          CarMonth = ci.CarMonth,
                          CarL = ci.CarL,
                          CarKm = ci.CarKm,
                          CarPrice = ci.CarPrice,
                        
                      };
            var no2 = await no1.ToListAsync();
            if (no2 == null)
            {
                return NotFound();
            }

            return View("SearchResult", carPicks);

        }



        // GET: CarPicks/Create
        public IActionResult Create()
        {
            string nextFormNumber = "";

            var lastForm = _context.CarPicks.OrderByDescending(f => f.CarId).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber = int.Parse(lastForm.CarId);
                int nextFormNumberInt = lastFormNumber + 1;
                nextFormNumber = nextFormNumberInt.ToString("0000");
            }
            else
            {
                nextFormNumber = "0001";
            }

            ViewData["CarId"] = nextFormNumber;
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid");

            return View();
        }

        // POST: CarPicks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,MemSid,CaseNo,CarSearch,CarAgencyLoc,CarType,CarNum,CarMonth,CarCaseAdr,CarL,CarKm,CarPrice")] CarPick carPick)
        {
            if (ModelState.IsValid)
            {
                // Check if CarSearch and CarMonth are not equal
                if (carPick.CarSearch.Year != carPick.CarMonth.Year || carPick.CarSearch.Month != carPick.CarMonth.Month)
                {
                    ModelState.AddModelError("CarMonth", "The CarMonth must be equal to CarSearch.");

                    ViewData["CarId"] = carPick.CarId;
                    ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);

                    return View(carPick);
                }

                _context.Add(carPick);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarId"] = carPick.CarId;
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);

            return View(carPick);
        }


        // GET: CarPicks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPick = await _context.CarPicks.FindAsync(id);
            if (carPick == null)
            {
                return NotFound();
            }
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);
            return View(carPick);
        }

        // POST: CarPicks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CarId,MemSid,CaseNo,CarAgencyLoc,CarSearch,CarType,CarNum,CarMonth,CarCaseAdr,CarL,CarKm,CarPrice")] CarPick carPick)
        {
            if (id != carPick.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carPick);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarPickExists(carPick.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemSid"] = new SelectList(_context.MemberInformations, "MemSid", "MemSid", carPick.MemSid);
            return View(carPick);
        }

        // GET: CarPicks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPick = await _context.CarPicks
                .Include(c => c.MemS)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (carPick == null)
            {
                return NotFound();
            }

            return View(carPick);
        }

        // POST: CarPicks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var carPick = await _context.CarPicks.FindAsync(id);
            _context.CarPicks.Remove(carPick);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarPickExists(string id)
        {
            return _context.CarPicks.Any(e => e.CarId == id);
        }
    }
}