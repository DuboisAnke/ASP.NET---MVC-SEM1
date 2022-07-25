using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Gebruiker
    {
        [Required]
        public int GebruikerId { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Voornaam { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Student> Studenten { get; set; }
        public ICollection<Lector> Lectoren { get; set; }
    }
}
