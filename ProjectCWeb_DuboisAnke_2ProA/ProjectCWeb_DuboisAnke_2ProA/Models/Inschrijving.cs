using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Inschrijving
    {
        [Required]
        public int InschrijvingId { get; set; }
        [Required]
        public int StudentID { get; set; }
        [Required]
        public int AcademieJaarID { get; set; }
        [Required]
        public int VakLectorId { get; set; }
        public Student Student { get; set; }
        public VakLector VakLector { get; set; }
        public Academiejaar Academiejaar { get; set; }
    }
}
