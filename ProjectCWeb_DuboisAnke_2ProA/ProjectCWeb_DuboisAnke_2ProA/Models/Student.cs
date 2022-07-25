using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Student
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int GebruikerId { get; set; }
        [Required]
        public Gebruiker Gebruiker { get; set; }
        public ICollection<Inschrijving> Inschrijvingen { get; set; }

    }
}
