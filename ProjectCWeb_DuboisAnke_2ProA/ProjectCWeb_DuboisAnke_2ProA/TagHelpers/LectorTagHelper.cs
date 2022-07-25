using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("info-lector")]
    public class LectorTagHelper : TagHelper
    {
        public InfoLectorViewModel InfoLectorViewModel { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
            
                $"<p><b>ID :</b>  {InfoLectorViewModel.Lector.LectorId}</p>\r\n" +

                $"<p><b>NAAM :</b>  {InfoLectorViewModel.Lector.Gebruiker.Naam}</p>\r\n" +

                $"<p><b>VOORNAAM :</b>  {InfoLectorViewModel.Lector.Gebruiker.Voornaam}</p>\r\n" +

                $"<p><b>E-MAIL :</b>  {InfoLectorViewModel.Lector.Gebruiker.Email}</p>\r\n" +

                $"<h6>INGESCHREVEN VAKKEN  :</h4>\r\n");


            foreach (var item in InfoLectorViewModel.VakLectoren)
            {
                stringBuilder.Append(
                    $"<p>Vak: {item.Vak.VakNaam} ------------ Handboek: {item.Vak.Handboek.Titel}</p>\r\n");
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
