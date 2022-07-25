using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCWeb_DuboisAnke_2ProA.CustomModelValidation
{
    public class CustomDateValidation : Attribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {

            var dtm = DateTime.Now;
            var lst = new List<ModelValidationResult>();
            if (DateTime.TryParse(context.Model.ToString(), out dtm))
            {
                if (dtm > new DateTime(dtm.Year, 10, 1))
                {
                    lst.Add(new ModelValidationResult("", $"De datum van uitgave is niet mogelijk na 1/10/{dtm.Year}"));

                }
                else if (dtm < new DateTime(1980, 1, 1))
                {
                    lst.Add(new ModelValidationResult("", "De datum van uitgave is niet mogelijk voor 1/1/1980"));
                }

            }

            else
            {
                lst.Add(new ModelValidationResult("", "Geen geldige datum!"));
            }
            return lst;
        }
    }
}
