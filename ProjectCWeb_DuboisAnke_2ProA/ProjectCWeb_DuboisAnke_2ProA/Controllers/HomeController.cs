using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectCWeb_DuboisAnke_2ProA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    public class HomeController : Controller
    {
       /* private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        private readonly PXLAppDbContext _context;

        public HomeController(PXLAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var data = _context;

            // list met gbids in tabel studenten
            List<int> gbidsStudenten = new List<int>();
            List<int> gbidsLectoren = new List<int>();
            List<Gebruiker> AlleStudenten = new List<Gebruiker>();
            List<Gebruiker> AlleLectoren = new List<Gebruiker>();
            foreach (var item in _context.Studenten)
            {
                gbidsStudenten.Add(item.GebruikerId);
            }

            foreach (var item in gbidsStudenten)
            {
                AlleStudenten.Add(await data.Gebruikers.FirstOrDefaultAsync(x => x.GebruikerId == item));
            }

            foreach (var item in data.Lectors)
            {
                gbidsLectoren.Add(item.GebruikerId);
            }

            foreach (var item in gbidsLectoren)
            {
                AlleLectoren.Add(await data.Gebruikers.FirstOrDefaultAsync(x => x.GebruikerId == item));
            }


            var overzicht = new OverzichtGebruikerViewModel(data, AlleLectoren, AlleStudenten);
            return View(overzicht);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
