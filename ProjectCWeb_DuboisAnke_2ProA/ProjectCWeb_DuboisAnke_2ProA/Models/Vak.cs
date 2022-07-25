using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Vak
    {
        [Required]
        public int VakId { get; set; }
        [Required]
        public string VakNaam { get; set; }
        [Required]
        public int StudiePunten { get; set; }
        [Required]
        public int HandBoekId { get; set; }
        public Handboek Handboek { get; set; }

        public ICollection<VakLector> VakLectoren { get; set; }
    }
}
