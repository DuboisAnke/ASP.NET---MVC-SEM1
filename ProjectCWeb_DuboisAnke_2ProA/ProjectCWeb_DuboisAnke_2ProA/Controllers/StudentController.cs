using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    
    public class StudentController : Controller
    {

        private readonly PXLAppDbContext _context;
        private UserManager<CustomIdentityUser> _userManager;
        private SignInManager<CustomIdentityUser> _signInManager;

        public StudentController(PXLAppDbContext context, UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = RoleHelper.LectorRole + "," + RoleHelper.AdminRole)]
        // GET: Student
        public async Task<IActionResult> Index(string searchString)
        {
            /*var pXLAppDbContext = _context.Studenten.Include(s => s.Gebruiker);*/

            var inschrijvingen = _context.Inschrijvingen
                .Include(x => x.Student)
                .Include(x => x.Student.Gebruiker)
                ;

           
            if (!String.IsNullOrEmpty(searchString))
            {
                if (inschrijvingen.Any(x => x.Student.Gebruiker.Naam.Equals(searchString)))
                {
                    var result = inschrijvingen.Where(s => s.Student.Gebruiker.Naam.Equals(searchString));
                    return View(await result.ToListAsync());
                }

                
                return View(inschrijvingen);
            }

            var user = _userManager.GetUserAsync(User);
            user.Wait();
            if (user.IsCompletedSuccessfully)
            {
                
                if (user.Result.RoleName == RoleHelper.LectorRole)
                {
                    string lectorEmail = user.Result.Email;
                    var lector = _context.Gebruikers.FirstOrDefault(x => x.Email == lectorEmail);

                    if (lector != null)
                    {
                        var lector2 = _context.Lectors.FirstOrDefault(x => x.GebruikerId == lector.GebruikerId);
                        var vaklector = _context.VakLectoren.FirstOrDefault(x => x.LectorId == lector2.LectorId);
                        var studenten = await _context.Inschrijvingen
                            .Include(x => x.Student)
                            .Include(x => x.Student.Gebruiker)
                            .Where(x => x.VakLectorId == vaklector.VakLectorId)
                            .ToListAsync();

                        return View(studenten);
                    }
                    else
                    {
                        string error = "U heeft nog geen studenten beschikbaar.";
                        return RedirectToAction("ErrorPage", "Account", new { errorMessage = error });
                    }
                }


            }

            return View(inschrijvingen);
        }

        [Authorize(Roles = RoleHelper.LectorRole + "," + RoleHelper.AdminRole)]
        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Studenten
                .Include(s => s.Gebruiker)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            List<VakLector> VakLectors = new List<VakLector>();
            List<int> vakids = new List<int>();
            foreach (var item in _context.Inschrijvingen.Include(x => x.VakLector))
            {
                if (item.StudentID == student.StudentId)
                {

                    VakLectors.Add(item.VakLector);

                }
            }

            /*var vakken = from s in _context.VakLectoren select s.VakId;*/
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

            if (student == null)
            {
                return NotFound();
            }

            var studentmodel = new InfoStudentViewModel(student, VakLectors, alleVakken);
            return View(studentmodel);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Student/Create
        public IActionResult Create()
        {
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email");
            return View();
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Gebruiker,Naam,Voornaam,Email")] Student student, Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           /* ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email", student.GebruikerId);*/
            return View(student);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Studenten
                .Include(x => x.Gebruiker)
                .FirstOrDefaultAsync(x => x.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email", student.GebruikerId);
            return View(student);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Gebruiker,Naam,Voornaam,Email")] Student student, Gebruiker gebruiker)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["GebruikerId"] = new SelectList(_context.Gebruikers, "GebruikerId", "Email", student.GebruikerId);
            return View(student);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Studenten
                .Include(s => s.Gebruiker)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Studenten.FindAsync(id);
            _context.Studenten.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Studenten.Any(e => e.StudentId == id);
        }
    }
}
