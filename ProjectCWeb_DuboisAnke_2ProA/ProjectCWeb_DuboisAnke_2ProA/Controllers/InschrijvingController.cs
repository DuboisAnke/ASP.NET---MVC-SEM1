using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.Helpers;
using ProjectCWeb_DuboisAnke_2ProA.Models;
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    [Authorize(Roles = RoleHelper.AdminRole)]
    public class InschrijvingController : Controller
    {
        private readonly PXLAppDbContext _context;
        private InschrijvingNaamVakViewModel _lectorNaamVakViewModel;

        public InschrijvingController(PXLAppDbContext context)
        {
            _context = context;
            _lectorNaamVakViewModel = new InschrijvingNaamVakViewModel(_context.VakLectoren.Include(x => x.Lector)
                .Include(x => x.Lector.Gebruiker)
                .Include(x => x.Vak));
        }


        // GET: Inschrijving
        public async Task<IActionResult> Index()
        {
            var pXLAppDbContext = _context.Inschrijvingen.Include(i => i.Academiejaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Lector.Gebruiker)
                .Include(x => x.VakLector.Vak)
                ;
            return View(await pXLAppDbContext.ToListAsync());
        }

        // GET: Inschrijving/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijving = await _context.Inschrijvingen
                .Include(i => i.Academiejaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Lector.Gebruiker)
                .FirstOrDefaultAsync(m => m.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            return View(inschrijving);
        }

        // GET: Inschrijving/Create
        public IActionResult Create()
        {
            var studenten = _context.Studenten.Include(x => x.Gebruiker);
            var vaklectoren = _context.VakLectoren.Include(x => x.Lector.Gebruiker);

            ViewData["AcademieJaarID"] = new SelectList(_context.AcademieJaar, "AcademieJaarId", "StartDatum");
            ViewData["StudentID"] = new SelectList(studenten, "StudentId", "Gebruiker.Naam");
            ViewData["VaklectorID"] = new SelectList(_lectorNaamVakViewModel.LectorNaamList, "VakLectorId", "LectorVak");

            if (!_context.Studenten.Any() && !_context.Vakken.Any())
            {
                ModelState.AddModelError(string.Empty, "Er moeten zowel studenten als vakken zijn om een inschrijving aan te maken");
            }
            return View();
            
        }

        // POST: Inschrijving/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InschrijvingId,StudentID,AcademieJaarID,VakLectorId")] Inschrijving inschrijving)
        {
            var studenten = _context.Studenten.Include(x => x.Gebruiker);

            if (ModelState.IsValid)
            {
                bool comboExists = false;
                foreach (var item in _context.Inschrijvingen)
                {
                    
                    if (item.StudentID == inschrijving.StudentID && item.VakLectorId == inschrijving.VakLectorId)
                    {
                        comboExists = true;
                        ModelState.AddModelError(string.Empty, "Je hebt deze lector al aan dit vak toegekend.");

                        ViewData["AcademieJaarID"] = new SelectList(_context.AcademieJaar, "AcademieJaarId", "StartDatum");
                        ViewData["StudentID"] = new SelectList(studenten, "StudentId", "Gebruiker.Naam");
                        ViewData["VaklectorID"] = new SelectList(_lectorNaamVakViewModel.LectorNaamList, "VakLectorId", "LectorVak");
                        return View();
                    }
                }

                if (!comboExists)
                {
                    _context.Add(inschrijving);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["AcademieJaarID"] = new SelectList(_context.AcademieJaar, "AcademieJaarId", "StartDatum", inschrijving.AcademieJaarID);
            ViewData["StudentID"] = new SelectList(studenten, "StudentId", "StudentId", inschrijving.StudentID);
            ViewData["VaklectorID"] = new SelectList(_lectorNaamVakViewModel.LectorNaamList, "VakLectorId", "LectorVak", inschrijving.VakLectorId);
            return View(inschrijving);
        }


        // GET: Inschrijving/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijving = await _context.Inschrijvingen.FindAsync(id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            var studenten = _context.Studenten.Include(x => x.Gebruiker);
            var vaklectoren = _context.VakLectoren.Include(x => x.Lector.Gebruiker);


            ViewData["AcademieJaarID"] = new SelectList(_context.AcademieJaar, "AcademieJaarId", "StartDatum");
            ViewData["StudentID"] = new SelectList(studenten, "StudentId", "Gebruiker.Naam");
            ViewData["VaklectorID"] = new SelectList(_lectorNaamVakViewModel.LectorNaamList, "VakLectorId", "LectorVak");
            return View(inschrijving);
        }

        // POST: Inschrijving/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InschrijvingId,StudentID,AcademieJaarID,VakLectorId")] Inschrijving inschrijving)
        {
            if (id != inschrijving.InschrijvingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inschrijving);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingExists(inschrijving.InschrijvingId))
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
            ViewData["AcademieJaarID"] = new SelectList(_context.AcademieJaar, "AcademieJaarId", "StartDatum");
            ViewData["StudentID"] = new SelectList(_context.Studenten, "StudentId", "Gebruiker.Naam");
            ViewData["VaklectorID"] = new SelectList(_lectorNaamVakViewModel.LectorNaamList, "VakLectorId", "LectorVak");
            return View(inschrijving);
        }

        // GET: Inschrijving/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijving = await _context.Inschrijvingen
                .Include(i => i.Academiejaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Lector.Gebruiker)
                .FirstOrDefaultAsync(m => m.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            return View(inschrijving);
        }

        // POST: Inschrijving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inschrijving = await _context.Inschrijvingen.FindAsync(id);
            _context.Inschrijvingen.Remove(inschrijving);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingExists(int id)
        {
            return _context.Inschrijvingen.Any(e => e.InschrijvingId == id);
        }
    }
}
