using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.ViewModels
{
    public class RegisterViewModel:LoginViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
