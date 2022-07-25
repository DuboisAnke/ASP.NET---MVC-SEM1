using Microsoft.EntityFrameworkCore;
using ProjectCWeb_DuboisAnke_2ProA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.Data
{
    public class PXLAppDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public PXLAppDbContext(DbContextOptions<PXLAppDbContext> options) : base(options)
        {

        }


        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Vak> Vakken { get; set; }
        public DbSet<Handboek> Handboeken { get; set; }
        public DbSet<Lector> Lectors { get; set; }
        public DbSet<VakLector> VakLectoren { get; set; }
        public DbSet<Student> Studenten { get; set; }
        public DbSet<Inschrijving> Inschrijvingen { get; set; }
        public DbSet<Academiejaar> AcademieJaar { get; set; }
        public DbSet<ProjectCWeb_DuboisAnke_2ProA.ViewModels.RegisterViewModel> RegisterViewModel { get; set; }
        public DbSet<ProjectCWeb_DuboisAnke_2ProA.ViewModels.LoginViewModel> LoginViewModel { get; set; }
    }
}
