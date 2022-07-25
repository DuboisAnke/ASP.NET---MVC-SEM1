using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.Models;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectCWeb_DuboisAnke_2ProA.Helpers;


namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    
    public class HandboekController : Controller
    {
        private readonly PXLAppDbContext _context;
        private UserManager<CustomIdentityUser> _userManager;

        public HandboekController(PXLAppDbContext context, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole + "," + RoleHelper.StudentRole)]
        // GET: Handboek
        public async Task<IActionResult> Index()
        {
            var handboeken = _context.Handboeken;
            var user = _userManager.GetUserAsync(User);
            user.Wait();
            if (user.IsCompletedSuccessfully)
            {
                if (user.Result.RoleName != null)
                {
                    if (user.Result.RoleName == RoleHelper.LectorRole)
                    {
                        string lectorEmail = user.Result.Email;
                        var lector = _context.Gebruikers.FirstOrDefault(x => x.Email == lectorEmail);

                        if (lector != null)
                        {
                            List<Handboek> AlleHandboeken = new List<Handboek>();

                            foreach (var item in _context.VakLectoren
                                         .Include(x => x.Vak)
                                         .Include(x => x.Lector.Gebruiker)
                                         .Include(x => x.Vak.Handboek))
                            {
                                if (item.Lector.GebruikerId == lector.GebruikerId)
                                {
                                    AlleHandboeken.Add(item.Vak.Handboek);
                                }
                            }

                            return View(AlleHandboeken);
                        }
                        else
                        {
                            string error = "U heeft nog geen handboeken beschikbaar.";
                            return RedirectToAction("ErrorPage", "Account", new {errorMessage = error});
                        }
                        

                    }
                }
            }
            return View(await handboeken.ToListAsync());
        }

        [Authorize(Roles = RoleHelper.AdminRole + ", " + RoleHelper.LectorRole + "," + RoleHelper.StudentRole)]
        // GET: Handboek/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handboek = await _context.Handboeken
                .FirstOrDefaultAsync(m => m.HandboekId == id);
            if (handboek == null)
            {
                return NotFound();
            }

            return View(handboek);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Handboek/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Handboek/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HandboekId,Titel,Kostprijs,UitgifteDatum,Afbeelding")] Handboek handboek)
        {
            if (ModelState.IsValid)
            {

                _context.Add(handboek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(handboek);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Handboek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handboek = await _context.Handboeken.FindAsync(id);
            if (handboek == null)
            {
                return NotFound();
            }
            return View(handboek);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Handboek/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HandboekId,Titel,Kostprijs,UitgifteDatum,Afbeelding")] Handboek handboek)
        {
            if (id != handboek.HandboekId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(handboek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandboekExists(handboek.HandboekId))
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
            return View(handboek);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // GET: Handboek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handboek = await _context.Handboeken
                .FirstOrDefaultAsync(m => m.HandboekId == id);
            if (handboek == null)
            {
                return NotFound();
            }

            return View(handboek);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        // POST: Handboek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var handboek = await _context.Handboeken
                .FindAsync(id);

            _context.Handboeken.Remove(handboek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandboekExists(int id)
        {
            return _context.Handboeken.Any(e => e.HandboekId == id);
        }



    }
}
