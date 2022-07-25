using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.Helpers;
using ProjectCWeb_DuboisAnke_2ProA.Models;

namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    public class AcademiejaarController : Controller
    {
        private readonly PXLAppDbContext _context;

        public AcademiejaarController(PXLAppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole)]
        // GET: Academiejaar
        public async Task<IActionResult> Index()
        {
            return View(await _context.AcademieJaar.ToListAsync());
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole)]
        // GET: Academiejaar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academiejaar = await _context.AcademieJaar
                .FirstOrDefaultAsync(m => m.AcademieJaarId == id);
            if (academiejaar == null)
            {
                return NotFound();
            }

            return View(academiejaar);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Academiejaar/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Academiejaar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademieJaarId,StartDatum")] Academiejaar academiejaar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academiejaar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academiejaar);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Academiejaar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academiejaar = await _context.AcademieJaar.FindAsync(id);
            if (academiejaar == null)
            {
                return NotFound();
            }
            return View(academiejaar);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Academiejaar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcademieJaarId,StartDatum")] Academiejaar academiejaar)
        {
            if (id != academiejaar.AcademieJaarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academiejaar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademiejaarExists(academiejaar.AcademieJaarId))
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
            return View(academiejaar);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Academiejaar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academiejaar = await _context.AcademieJaar
                .FirstOrDefaultAsync(m => m.AcademieJaarId == id);
            if (academiejaar == null)
            {
                return NotFound();
            }

            return View(academiejaar);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Academiejaar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academiejaar = await _context.AcademieJaar.FindAsync(id);
            _context.AcademieJaar.Remove(academiejaar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademiejaarExists(int id)
        {
            return _context.AcademieJaar.Any(e => e.AcademieJaarId == id);
        }
    }
}
