using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectCWeb_DuboisAnke_2ProA.Models;

namespace ProjectCWeb_DuboisAnke_2ProA.ViewModels
{
    public class InfoStudentViewModel
    {
        public Student Student { get; set; }
        public Vak Vak { get; set; }

        public List<VakLector> VakLectoren { get; set; }
        public List<Vak> AlleVakken { get; set; }
      /*  public InfoStudentViewModel(Student student)
        {
            Student = student;
           
        }*/

        public InfoStudentViewModel(Student student, List<VakLector> vakLectors, List<Vak> alleVakken)
        {
            Student = student;
            VakLectoren = vakLectors;
            AlleVakken = alleVakken;

        }
    }
}
