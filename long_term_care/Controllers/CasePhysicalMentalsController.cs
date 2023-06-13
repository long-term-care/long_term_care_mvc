﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;

namespace long_term_care.Controllers
{
    public class CasePhysicalMentalsController : Controller
    {
        private readonly longtermcareContext _context;

        public CasePhysicalMentalsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: CasePhysicalMentals
        public async Task<IActionResult> Index()
        {
            var longtermcareContext = _context.CasePhysicalMentals.Include(c => c.CaseNoNavigation);
            return View(await longtermcareContext.ToListAsync());
        }

        // GET: CasePhysicalMentals/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePhysicalMental = await _context.CasePhysicalMentals
                .Include(c => c.CaseNoNavigation)
                .FirstOrDefaultAsync(m => m.CaseQaid == id);
            if (casePhysicalMental == null)
            {
                return NotFound();
            }

            return View(casePhysicalMental);
        }

        // GET: CasePhysicalMentals/Create
        public IActionResult Create()
        {
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo");
            return View();
        }

        // POST: CasePhysicalMentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] selected3, string[] selected4, [Bind("CaseQaid,CaseNo,CaseLive,CaseFre,CaseContent1,CaseContent2,CaseContent3,CaseContent4,CaseContent5,CaseContent6,CaseContent7,CaseContent8,CaseContent9,CaseContent10,CaseContent11,CaseContent12,CaseContent13")] CasePhysicalMental casePhysicalMental)
        {
            if (selected3 != null && selected3.Length > 0)
            {
                // 将选中的复选框值以逗号分隔的字符串形式存储在模型的 content3 属性中
                casePhysicalMental.CaseContent3 = string.Join(",", selected3); ;
            }
            else
            {
                // 没有选中的复选框值时，将 content3 属性设置为 null 或空字符串
                casePhysicalMental.CaseContent3 = null; // 或 model.Content3 = "";
            }
            if (selected4 != null && selected4.Length > 0)
            {
                // 将选中的复选框值以逗号分隔的字符串形式存储在模型的 content3 属性中
                casePhysicalMental.CaseContent4 = string.Join(",", selected4); ;
            }
            else
            {
                // 没有选中的复选框值时，将 content3 属性设置为 null 或空字符串
                casePhysicalMental.CaseContent4 = null; // 或 model.Content3 = "";
            }
            if (ModelState.IsValid)
            {
                _context.Add(casePhysicalMental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", casePhysicalMental.CaseNo);
            return View(casePhysicalMental);
        }

        // GET: CasePhysicalMentals/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePhysicalMental = await _context.CasePhysicalMentals.FindAsync(id);
            if (casePhysicalMental == null)
            {
                return NotFound();
            }
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", casePhysicalMental.CaseNo);
            return View(casePhysicalMental);
        }

        // POST: CasePhysicalMentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaseQaid,CaseNo,CaseLive,CaseFre,CaseContent1,CaseContent2,CaseContent3,CaseContent4,CaseContent5,CaseContent6,CaseContent7,CaseContent8,CaseContent9,CaseContent10,CaseContent11,CaseContent12,CaseContent13")] CasePhysicalMental casePhysicalMental)
        {
            if (id != casePhysicalMental.CaseQaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casePhysicalMental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasePhysicalMentalExists(casePhysicalMental.CaseQaid))
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
            ViewData["CaseNo"] = new SelectList(_context.CaseInfors, "CaseNo", "CaseNo", casePhysicalMental.CaseNo);
            return View(casePhysicalMental);
        }

        // GET: CasePhysicalMentals/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePhysicalMental = await _context.CasePhysicalMentals
                .Include(c => c.CaseNoNavigation)
                .FirstOrDefaultAsync(m => m.CaseQaid == id);
            if (casePhysicalMental == null)
            {
                return NotFound();
            }

            return View(casePhysicalMental);
        }

        // POST: CasePhysicalMentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var casePhysicalMental = await _context.CasePhysicalMentals.FindAsync(id);
            _context.CasePhysicalMentals.Remove(casePhysicalMental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasePhysicalMentalExists(string id)
        {
            return _context.CasePhysicalMentals.Any(e => e.CaseQaid == id);
        }
    }
}