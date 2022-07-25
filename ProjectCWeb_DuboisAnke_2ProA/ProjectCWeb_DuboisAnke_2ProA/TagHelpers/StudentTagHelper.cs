using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("info-student")]
    public class StudentTagHelper : TagHelper
    {
        public InfoStudentViewModel InfoStudentViewModel { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                
                        $"<p><b>ID :</b>  {InfoStudentViewModel.Student.StudentId}</p>\r\n" +

                        $"<p><b>NAAM :</b>  {InfoStudentViewModel.Student.Gebruiker.Naam}</p>\r\n" +

                        $"<p><b>VOORNAAM :</b>  {InfoStudentViewModel.Student.Gebruiker.Voornaam}</p>\r\n" +

                        $"<p><b>E-MAIL :</b>  {InfoStudentViewModel.Student.Gebruiker.Email}</p>\r\n"  +

                        $"<h6>INGESCHREVEN VAKKEN  :</h4>\r\n");


            foreach (var item in InfoStudentViewModel.VakLectoren)
            {
                stringBuilder.Append(
                    $"<p>Vak: {item.Vak.VakNaam} ------------- Handboek: {item.Vak.Handboek.Titel}</p>\r\n");
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
