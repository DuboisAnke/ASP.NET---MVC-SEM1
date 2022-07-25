using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectCWeb_DuboisAnke_2ProA.Models;

namespace ProjectCWeb_DuboisAnke_2ProA.ViewModels
{
    public class InfoLectorViewModel
    {
        public Lector Lector { get; set; }
        public Vak Vak { get; set; }

        public List<VakLector> VakLectoren { get; set; }
        public List<Vak> Vakken { get; set; }

        public InfoLectorViewModel(Lector lector, List<VakLector> vakLectors, List<Vak> vakken)
        {
            Lector = lector;
            VakLectoren = vakLectors;
            Vakken = vakken;
        }
    }
}
