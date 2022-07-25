using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class Academiejaar
    {
        public int AcademieJaarId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDatum { get; set; }

        public ICollection<Inschrijving> Inschrijvingen { get; set; }
    }
}
