using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class VakLector
    {
        [Required]
        public int VakLectorId { get; set; }
        public int? LectorId { get; set; }
        [Required]
        public int VakId { get; set; }
        public Lector Lector { get; set; }
        public Vak Vak { get; set; }

        public ICollection<Inschrijving> Inschrijvingen { get; set; }
        
    }
}
