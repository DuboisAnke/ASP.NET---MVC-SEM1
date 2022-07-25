using ProjectCWeb_DuboisAnke_2ProA.CustomModelValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Handboek
    {
        [Required]
        public int HandboekId { get; set; }
        [Required]
        public string Titel { get; set; }
        [Required]
        public double Kostprijs { get; set; }
        [Required]
        [CustomDateValidation]
        [DataType(DataType.Date)]
        public DateTime UitgifteDatum { get; set; }
        [Required(ErrorMessage = "Kies een afbeeldingsfile")]
        public string Afbeelding { get; set; }

        public ICollection<Vak> Vakken { get; set; }
    }
}
