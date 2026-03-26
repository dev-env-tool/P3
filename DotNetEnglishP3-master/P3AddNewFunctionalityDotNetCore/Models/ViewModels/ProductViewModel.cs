using Microsoft.AspNetCore.Mvc.ModelBinding;
//Adding the Data Anotations library
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        //[Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        //[Required]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PriceNotDouble]
        public string Stock { get; set; }

        //[Required]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PriceGreaterThan0]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PriceNotDouble]
        public string Price { get; set; }
    }


    public class GetErrorMessage
    {
        public string errorMessage { get; set; }
        public string PpropretyName { get; set; }
    }
}
