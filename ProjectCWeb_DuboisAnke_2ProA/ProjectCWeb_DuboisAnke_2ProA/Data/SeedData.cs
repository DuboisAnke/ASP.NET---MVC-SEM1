using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProjectCWeb_DuboisAnke_2ProA.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectCWeb_DuboisAnke_2ProA.Helpers;

namespace ProjectCWeb_DuboisAnke_2ProA.Data
{
    public class SeedData
    {

        public static async void EnsurePopulatedAsync(IApplicationBuilder app)
        {
            PXLAppDbContext context = app.ApplicationServices.CreateScope()
                .ServiceProvider
                .GetRequiredService<PXLAppDbContext>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<CustomIdentityUser> userManager = app.ApplicationServices.CreateScope()
                .ServiceProvider
                .GetRequiredService<UserManager<CustomIdentityUser>>();

            if (!context.Roles.Any())
            {
                var role = new IdentityRole(RoleHelper.AdminRole);
                await roleManager.CreateAsync(role);
                role = new IdentityRole(RoleHelper.StudentRole);
                await roleManager.CreateAsync(role);
                role = new IdentityRole(RoleHelper.LectorRole);
                await roleManager.CreateAsync(role);
                var adminUser = new CustomIdentityUser();
                adminUser.Email = "admin@pxl.be";
                adminUser.UserName = adminUser.Email;
                adminUser.RoleName = RoleHelper.AdminRole;
                var result = await userManager.CreateAsync(adminUser, "Admin456!");
                if (result.Succeeded)
                {
                    role = await roleManager.FindByNameAsync(RoleHelper.AdminRole);
                    if (role != null)
                    {
                        await userManager.AddToRoleAsync(adminUser, role.Name);
                    }
                }

                var studentUser = new CustomIdentityUser();
                studentUser.Email = "student@pxl.be";
                studentUser.UserName = studentUser.Email;
                studentUser.RoleName = RoleHelper.StudentRole;
                var student = await userManager.CreateAsync(studentUser, "Student123!");
                if (student.Succeeded)
                {
                    role = await roleManager.FindByNameAsync(RoleHelper.StudentRole);
                    if (role != null)
                    {
                        await userManager.AddToRoleAsync(studentUser, role.Name);
                    }
                }
            }

            if (!context.Gebruikers.Any())
            {
                context.Gebruikers.AddRange(
                new Gebruiker()
                {
                    Naam = "Dubois",
                    Voornaam = "Anke",
                    Email = "duboisanke@pxl.be"
                },
                new Gebruiker()
                {
                    Naam = "Palmaers",
                    Voornaam = "Kristof",
                    Email = "KPalmaers@pxl.be"
                }
                );
                context.SaveChanges();
            }
            if (!context.Handboeken.Any())
            {
                context.Handboeken.AddRange(
                new Handboek()
                {
                    Titel = "C# Web 1",
                    Kostprijs = 30,
                    UitgifteDatum = new DateTime(2021,7,21),
                    Afbeelding = "https://media.s-bol.com/xO0xBWD9vN9/550x788.jpg"
                }
                );
                context.SaveChanges();
            }
            if (!context.Studenten.Any())
            {
                context.Studenten.AddRange(
                    new Student()
                    {
                        GebruikerId = context.Gebruikers.Where(g => g.Naam == "Dubois").FirstOrDefault().GebruikerId
                    }
                );
                context.SaveChanges();
            }
            if (!context.Lectors.Any())
            {
                context.Lectors.AddRange(
                new Lector()
                {
                    GebruikerId = context.Gebruikers.FirstOrDefault(x => x.Naam == "Palmaers").GebruikerId,
                }
                );
                context.SaveChanges();
            }
            if (!context.Vakken.Any())
            {
                context.Vakken.AddRange(
                new Vak()
                {
                    VakNaam = "C# Web 1",
                    StudiePunten = 6,
                    HandBoekId = context.Handboeken.FirstOrDefault( x => x.Titel == "C# Web 1").HandboekId
                }
                );
                context.SaveChanges();
            }
            if (!context.VakLectoren.Any())
            {
                /*var gbPalmaers = context.Gebruikers.FirstOrDefault(x => x.Naam == "Palmaers");*/

                var gbPalmaers = context.Gebruikers.FirstOrDefault(x => x.Naam == "Palmaers" );
                var lectPalmaers = context.Lectors.Include(x => x.Gebruiker)
                    .FirstOrDefault(x => x.GebruikerId == gbPalmaers.GebruikerId);


                var cweb = context.Vakken.FirstOrDefault(x => x.VakNaam == "C# Web 1");

                if (context.Lectors.Any(x => x.LectorId == lectPalmaers.LectorId) &&
                    context.Vakken.Any(x => x.VakId == cweb.VakId))
                {
                    context.VakLectoren.AddRange(
                        new VakLector
                        {
                            LectorId = lectPalmaers.LectorId,
                            VakId = cweb.VakId

                        }
                    );
                }

                context.SaveChanges();
            }
            
            if (!context.AcademieJaar.Any())
            {
                context.AcademieJaar.AddRange(
                new Academiejaar()
                {
                    StartDatum = DateTime.ParseExact("20-09-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture)
                }
                );
                context.SaveChanges();
            }

            if (!context.Inschrijvingen.Any())
            {
                var datum = context.AcademieJaar.FirstOrDefault(x => x.StartDatum ==
                                                                     DateTime.ParseExact("20-09-2021", "dd-MM-yyyy",
                                                                         CultureInfo.InvariantCulture));
                var dubois = context.Studenten.FirstOrDefault(x => x.Gebruiker.Naam == "Dubois");


                var vakPalmaers = context.VakLectoren.Include(x => x.Lector)
                    .Include(x => x.Lector.Gebruiker)
                    .FirstOrDefault(x => x.Lector.Gebruiker.Naam == "Palmaers");

                    

                if (context.Studenten.Any(x => x.StudentId == dubois.StudentId) &&
                    context.VakLectoren.Any(x => x.VakLectorId == vakPalmaers.VakLectorId) &&
                    context.AcademieJaar.Any(x => x.AcademieJaarId == datum.AcademieJaarId))
                {
                    context.Inschrijvingen.AddRange(
                        new Inschrijving
                        {
                            StudentID = dubois.StudentId,
                            VakLectorId = vakPalmaers.VakLectorId,
                            AcademieJaarID = datum.AcademieJaarId
                        }
                        );
                    
                }
                context.SaveChanges();
            }

        }

    }
}
