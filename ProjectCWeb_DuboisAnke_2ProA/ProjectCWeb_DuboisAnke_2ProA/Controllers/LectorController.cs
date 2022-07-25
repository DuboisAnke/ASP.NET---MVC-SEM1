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
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    public class LectorController : Controller
    {
        private readonly PXLAppDbContext _context;

        public LectorController(PXLAppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = RoleHelper.LectorRole + "," + RoleHelper.AdminRole)]
        // GET: Lector
        public async Task<IActionResult> Index(string searchString)
        {
            /* var pXLAppDbContext = _context.Lectors.Include(l => l.Gebruiker);
             return View(await pXLAppDbContext.ToListAsync());*/

            var lectors = from m in _context.Lectors.Include(s => s.Gebruiker)
                select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (lectors.Any(x => x.Gebruiker.Naam.Equals(searchString)))
                {
                    lectors = lectors.Where(s => s.Gebruiker.Naam.Equals(searchString));
                    return View(await lectors.ToListAsync());
                }


                return View(await _context.Lectors.Include(s => s.Gebruiker).ToListAsync());
            }

            return View(await _context.Lectors.Include(s => s.Gebruiker).ToListAsync());
        }

        [Authorize(Roles = RoleHelper.LectorRole + "," + RoleHelper.AdminRole)]
        // GET: Lector/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lector = await _context.Lectors
                .Include(l => l.Gebruiker)
                .FirstOrDefaultAsync(m => m.LectorId == id);

            List<VakLector> VakLectors = new List<VakLector>();
            List<int> vakids = new List<int>();
            foreach (var item in _context.Inschrijvingen.Include(x => x.VakLector))
            {
                if (item.VakLector.LectorId == lector.LectorId)
                {

                    VakLectors.Add(item.VakLector);

                }
            }

            foreach (var item1 in VakLectors)
            {
                vakids.Add(item1.VakId);
            }

            List<Vak> alleVakken = new List<Vak>();
            foreach (var item in vakids)
            {
                alleVakken.Add(await _context.Vakken
                    .Include(x => x.Handboek)
                    .FirstOrDefaultAsync(x => x.VakId == item));
            }

            if (lector == null)
            {
                return NotFound();
            }

            var lectormodel = new InfoLectorViewModel(lector, VakLectors, alleVakken);

            return View(lectormodel);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Lector/Create
        public IActionResult Create()
        {
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email");
            return View();
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Lector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LectorId,Gebruiker,Naam,Voornaam,Email")] Lector lector, Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email", lector.GebruikerId);
            return View(lector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Lector/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lector = await _context.Lectors
                .Include(x => x.Gebruiker)
                .FirstOrDefaultAsync(x => x.LectorId == id);
            if (lector == null)
            {
                return NotFound();
            }
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email", lector.GebruikerId);
            return View(lector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Lector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LectorId,Gebruiker,Naam,Voornaam,Email")] Lector lector, Gebruiker gebruiker)
        {
            if (id != lector.LectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectorExists(lector.LectorId))
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
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email", lector.GebruikerId);
            return View(lector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Lector/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lector = await _context.Lectors
                .Include(l => l.Gebruiker)
                .FirstOrDefaultAsync(m => m.LectorId == id);
            if (lector == null)
            {
                return NotFound();
            }

            return View(lector);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Lector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lector = await _context.Lectors.FindAsync(id);

            foreach (VakLector item in _context.VakLectoren.Where(x => x.LectorId == id))
            {
                _context.VakLectoren.Remove(item);
            }


            _context.Lectors.Remove(lector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectorExists(int id)
        {
            return _context.Lectors.Any(e => e.LectorId == id);
        }
    }
}
