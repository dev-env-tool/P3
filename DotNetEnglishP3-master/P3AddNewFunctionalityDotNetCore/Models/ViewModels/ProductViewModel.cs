using Microsoft.AspNetCore.Mvc.ModelBinding;
//Adding the Data Anotations library
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required]
        public string Stock { get; set; }

        [Required]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PricePositiveAndNotEqualTo0]
        public string Price { get; set; }
    }
}
