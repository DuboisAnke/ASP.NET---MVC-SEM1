using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.Models;

namespace ProjectCWeb_DuboisAnke_2ProA.ViewModels
{
    public class OverzichtGebruikerViewModel
    {
        
        public List<Gebruiker> AlleStudenten { get; set; }
        public List<Gebruiker> AlleLectoren { get; set; }

        public static PXLAppDbContext _context;

        public OverzichtGebruikerViewModel()
        {
                
        }

        public OverzichtGebruikerViewModel(PXLAppDbContext pxlAppDbContext,
            List<Gebruiker> alleLectoren,
            List<Gebruiker> allStudents)
        {
            _context = pxlAppDbContext;
            AlleStudenten = allStudents;
            AlleLectoren = alleLectoren;
        }

        
    }

    
}
