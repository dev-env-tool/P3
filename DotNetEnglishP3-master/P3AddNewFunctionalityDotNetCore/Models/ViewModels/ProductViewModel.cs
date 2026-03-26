using Microsoft.AspNetCore.Mvc.ModelBinding;
//Adding the Data Anotations library
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingStock")]
        [RegularExpression("^((?!^Stock$)[0-9])+$", ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "StockNotAnInteger")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "StockNotGreaterThanZero")]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PriceNotDouble]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingPrice")]
        [Range(0.01, 10000000000, ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PriceGreaterThan0]
        [P3AddNewFunctionalityDotNetCore.Resources.Models.Services.PriceNotDouble]
        public string Price { get; set; }
    }


    public class GetErrorMessage
    {
        public string errorMessage = P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService.MissingPrice;
    }
}
