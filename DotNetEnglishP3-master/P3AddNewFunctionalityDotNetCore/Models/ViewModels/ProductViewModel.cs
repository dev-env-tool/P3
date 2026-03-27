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

        //<summary>
        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingStock")]
        [RegularExpression("^((?!^Stock$)-?[0-9])+$", ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "StockNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "StockNotGreaterThanZero")]
        //</summary>
        
        public string Stock { get; set; }


        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingPrice")]
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        [RegularExpression("^((?!^Price$)-?^(\\d+\\.?\\d+|\\d))+$", ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService), ErrorMessageResourceName = "PriceNotANumber")]

        public string Price { get; set; }
    }



}
