using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.Helpers
{
    public class RoleHelper
    {
        public const string AdminRole = "Admin";
        public const string StudentRole = "Student";
        public const string LectorRole = "Lector";
        public static IEnumerable<string> Roles => GetRoles();
        private static List<string> GetRoles()
        {
            List<string> lst = new List<string>();
            lst.Add(StudentRole);
            lst.Add(LectorRole);
            lst.Add(AdminRole);
            return lst;
        }
    }
}
