using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using long_term_care.Models;

namespace long_term_care.Controllers
{
    public class MemberInformationsController : Controller
    {
        private readonly longtermcareContext _context;

        public MemberInformationsController(longtermcareContext context)
        {
            _context = context;
        }

        // GET: MemberInformations
        public async Task<IActionResult> Index()
        {
            return View(await _context.MemberInformations.ToListAsync());
        }

        // GET: MemberInformations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberInformation = await _context.MemberInformations
                .FirstOrDefaultAsync(m => m.MemSid == id);
            if (memberInformation == null)
            {
                return NotFound();
            }

            return View(memberInformation);
        }

        // GET: MemberInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemSid,MemUnitName,MemUnitNum,MemName,MemBd,MemUid,MemPassword,MemGender,MemTphone,MemMphone,MemAddress,MemSite,MemProf,MemCert,MemTrans,MemExpr,MemMovt,MemPserv,MemIdent,MemSerRec,MemEdu")] MemberInformation memberInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberInformation);
        }

        // GET: MemberInformations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberInformation = await _context.MemberInformations.FindAsync(id);
            if (memberInformation == null)
            {
                return NotFound();
            }
            return View(memberInformation);
        }

        // POST: MemberInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemSid,MemUnitName,MemUnitNum,MemName,MemBd,MemUid,MemPassword,MemGender,MemTphone,MemMphone,MemAddress,MemSite,MemProf,MemCert,MemTrans,MemExpr,MemMovt,MemPserv,MemIdent,MemSerRec,MemEdu")] MemberInformation memberInformation)
        {
            if (id != memberInformation.MemSid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberInformationExists(memberInformation.MemSid))
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
            return View(memberInformation);
        }

        // GET: MemberInformations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberInformation = await _context.MemberInformations
                .FirstOrDefaultAsync(m => m.MemSid == id);
            if (memberInformation == null)
            {
                return NotFound();
            }

            return View(memberInformation);
        }

        // POST: MemberInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var memberInformation = await _context.MemberInformations.FindAsync(id);
            _context.MemberInformations.Remove(memberInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberInformationExists(string id)
        {
            return _context.MemberInformations.Any(e => e.MemSid == id);
        }
    }
}
