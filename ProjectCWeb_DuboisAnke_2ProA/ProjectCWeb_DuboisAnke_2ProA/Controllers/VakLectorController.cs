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
    public class VakLectorController : Controller
    {
        private readonly PXLAppDbContext _context;

        
        public VakLectorController(PXLAppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole)]
        // GET: VakLector
        public async Task<IActionResult> Index()
        {
            var pXLAppDbContext = _context.VakLectoren.Include(v => v.Lector)
                .ThenInclude(y => y.Gebruiker)
                .Include(v => v.Vak);
            return View(await pXLAppDbContext.ToListAsync());
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole)]
        // GET: VakLector/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLectoren
                .Include(v => v.Lector)
                .Include(v => v.Vak)
                .Include(v => v.Lector.Gebruiker)
                .FirstOrDefaultAsync(m => m.VakLectorId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(vakLector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: VakLector/Create
        public IActionResult Create()
        {

            var lectoren = _context.Lectors.Include(x => x.Gebruiker);
       

            ViewData["LectorId"] = new SelectList(lectoren, "LectorId", "Gebruiker.Naam");
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "VakNaam");
            return View();
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: VakLector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VakLectorId,LectorId,VakId")] VakLector vakLector)
        {
            var lectoren = _context.Lectors.Include(y => y.Gebruiker);

            if (ModelState.IsValid)
            {

                bool comboExists = false;
                foreach (var vl in _context.VakLectoren)
                {
                   
                    if (vl.LectorId == vakLector.LectorId && vl.VakId == vakLector.VakId)
                    {
                        comboExists = true;
                        ModelState.AddModelError(string.Empty, "Je hebt deze lector al aan dit vak toegekend.");

                        ViewData["LectorId"] = new SelectList(lectoren, "LectorId", "Gebruiker.Naam");
                        ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "VakNaam");
                        return View();
                    }

                }

                if (!comboExists)
                {
                    _context.Add(vakLector);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
  
            ViewData["LectorId"] = new SelectList(lectoren, "LectorId", "Gebruiker.Naam", vakLector.LectorId);
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Vaknaam", vakLector.VakId);
            return View(vakLector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: VakLector/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLectoren.FindAsync(id);
            if (vakLector == null)
            {
                return NotFound();
            }

            var lectoren = _context.Lectors.Include(y => y.Gebruiker);

            ViewData["LectorId"] = new SelectList(lectoren, "LectorId", "Gebruiker.Naam");
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "VakNaam", vakLector.VakId);
            return View(vakLector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: VakLector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VakLectorId,LectorId,VakId")] VakLector vakLector)
        {
            if (id != vakLector.VakLectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vakLector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VakLectorExists(vakLector.VakLectorId))
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
            ViewData["LectorId"] = new SelectList(_context.Lectors, "LectorId", "LectorId", vakLector.LectorId);
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "VakNaam", vakLector.VakId);
            return View(vakLector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: VakLector/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLectoren
                .Include(v => v.Lector)
                .Include(v => v.Vak)
                .Include(v => v.Lector.Gebruiker)
                .FirstOrDefaultAsync(m => m.VakLectorId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(vakLector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: VakLector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var vakLector = await _context.VakLectoren.FindAsync(id);

            /*var vakLectorInschrijvingen = await _context.Inschrijvingen.FirstOrDefaultAsync(x => x.VakLectorId == vakLector.VakLectorId);
            _context.Inschrijvingen.Remove(vakLectorInschrijvingen);*/
            _context.VakLectoren.Remove(vakLector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VakLectorExists(int id)
        {
            return _context.VakLectoren.Any(e => e.VakLectorId == id);
        }
    }
}
