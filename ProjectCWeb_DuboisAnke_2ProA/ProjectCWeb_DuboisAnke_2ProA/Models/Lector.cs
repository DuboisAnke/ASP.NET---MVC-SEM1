using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Lector
    {
        [Required]
        public int LectorId { get; set; }
        [Required]
        public int GebruikerId { get; set; }

        public Gebruiker Gebruiker { get; set; }

        public ICollection<VakLector> VakLectoren { get; set; }
    }
}
