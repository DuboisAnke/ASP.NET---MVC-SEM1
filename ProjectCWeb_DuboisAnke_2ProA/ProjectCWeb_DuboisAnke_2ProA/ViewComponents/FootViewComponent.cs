using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectCWeb_DuboisAnke_2ProA.ViewComponents
{
    public class FootViewComponent : ViewComponent
    {
        /*public FootViewComponent()
        {
                
        }*/

        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
