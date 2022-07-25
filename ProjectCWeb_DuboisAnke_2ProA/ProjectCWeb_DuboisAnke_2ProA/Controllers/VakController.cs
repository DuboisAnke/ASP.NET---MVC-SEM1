using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.Helpers;
using ProjectCWeb_DuboisAnke_2ProA.Models;

namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    public class VakController : Controller
    {
        private readonly PXLAppDbContext _context;
        private UserManager<CustomIdentityUser> _userManager;

        public VakController(PXLAppDbContext context, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole + "," + RoleHelper.StudentRole)]
        // GET: Vak
        public async Task<IActionResult> Index()
        {
            var pXLAppDbContext = _context.Vakken.Include(v => v.Handboek);

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
                        List<Vak> AlleVakken = new List<Vak>();

                        foreach (var item in _context.VakLectoren
                                     .Include(x => x.Vak)
                                     .Include(x => x.Lector)
                                     .Include(x => x.Lector.Gebruiker)
                                     .Include(x => x.Vak.Handboek))
                        {
                            if (item.Lector.GebruikerId == lector.GebruikerId)
                            {
                                AlleVakken.Add(item.Vak);
                            }
                        }

                        return View(AlleVakken);
                    }
                    else
                    {
                        string error = "U heeft nog geen vakken beschikbaar.";
                        return RedirectToAction("ErrorPage", "Account", new { errorMessage = error });
                    }
                   

                }

               
            }
            return View(await pXLAppDbContext.ToListAsync());
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole + "," + RoleHelper.StudentRole)]
        // GET: Vak/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vak = await _context.Vakken
                .Include(v => v.Handboek)
                .FirstOrDefaultAsync(m => m.VakId == id);
            if (vak == null)
            {
                return NotFound();
            }

            return View(vak);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Vak/Create
        public IActionResult Create()
        {
            ViewData["HandBoekId"] = new SelectList(_context.Handboeken, "HandboekId", "Titel");
            

            if (!_context.Handboeken.Any())
            {
                ModelState.AddModelError(string.Empty, "Voeg eerst een handboek toe op de pagina handboeken");
            }

            return View();
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Vak/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VakId,VakNaam,StudiePunten,HandBoekId")] Vak vak)
        {
            if(ModelState.IsValid)
            {
                    _context.Add(vak);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

            }
            ViewData["HandBoekId"] = new SelectList(_context.Handboeken, "HandboekId", "Titel");
            return View(vak);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Vak/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vak = await _context.Vakken.FindAsync(id);
            if (vak == null)
            {
                return NotFound();
            }
            ViewData["HandBoekId"] = new SelectList(_context.Handboeken, "HandboekId", "Afbeelding", vak.HandBoekId);
            return View(vak);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Vak/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VakId,VakNaam,StudiePunten,HandBoekId")] Vak vak)
        {
            if (id != vak.VakId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vak);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VakExists(vak.VakId))
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
            ViewData["HandBoekId"] = new SelectList(_context.Handboeken, "HandboekId", "Afbeelding", vak.HandBoekId);
            return View(vak);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Vak/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vak = await _context.Vakken
                .Include(v => v.Handboek)
                .FirstOrDefaultAsync(m => m.VakId == id);
            if (vak == null)
            {
                return NotFound();
            }

            return View(vak);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Vak/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vak = await _context.Vakken.FindAsync(id);
            _context.Vakken.Remove(vak);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VakExists(int id)
        {
            return _context.Vakken.Any(e => e.VakId == id);
        }
    }
}
