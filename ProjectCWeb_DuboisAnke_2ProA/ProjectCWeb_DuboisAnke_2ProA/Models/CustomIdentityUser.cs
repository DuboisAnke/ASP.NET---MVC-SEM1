using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectCWeb_DuboisAnke_2ProA.Models
{
    public class CustomIdentityUser : IdentityUser
    {
        public string RoleName { get; set; }
        public string tempRoleName { get; set; }
    }
}
