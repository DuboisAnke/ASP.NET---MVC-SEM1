using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using ProjectCWeb_DuboisAnke_2ProA.Models;

namespace ProjectCWeb_DuboisAnke_2ProA.ViewModels
{
    public class InschrijvingNaamVakViewModel
    {
        public int VakId { get; set; }
        public int VakLectorId { get; set; }
        public int? LectorId { get; set; }
        public Vak Vak { get; set; }
        public Lector Lector { get; set; }

        public string LectorVak => $"{Lector.Gebruiker.Naam} ({Vak.VakNaam})";

        public List<InschrijvingNaamVakViewModel> LectorNaamList = new List<InschrijvingNaamVakViewModel>();

        public InschrijvingNaamVakViewModel(IIncludableQueryable<VakLector, Vak> context)
        {
            foreach (var vakLector in context)
            {
                LectorNaamList.Add(new InschrijvingNaamVakViewModel
                {
                    VakId = vakLector.VakId,
                    VakLectorId = vakLector.VakLectorId,
                    LectorId = vakLector.LectorId,
                    Vak = vakLector.Vak,
                    Lector = vakLector.Lector
                });
            }
        }

        public InschrijvingNaamVakViewModel()
        {
            
        }
    }
}
