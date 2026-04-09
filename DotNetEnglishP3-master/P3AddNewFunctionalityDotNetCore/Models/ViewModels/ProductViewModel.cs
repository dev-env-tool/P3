using Microsoft.AspNetCore.Mvc.ModelBinding;
//Adding the Data Anotations library
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        //<summary>
        //Use of Required to spot on an empty field
        //</summary>
        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "MissingStock")]
        

        //<summary>
        //This "regex" should always be sat before the "range" so that it can detect for any <0 values
        //Use of a regular expression
        //"^(all which is diferent from)
        //((?!^Stock$) the field stock, only filled once per field.
        //-? the string may begin with one "-" character. Only one "-" character is allowed.
        //[0-9]) a number only composed of digits from 0 to 9 (inclued)
        //+$" end of the string
        //</summary>
        [RegularExpression("^((?!^Stock$)-?[0-9])+$", ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "StockNotAnInteger")]
        

        //<summary>
        //Use of a "Range" attribute to check wether the field is an integer between 1 and Signed 2 bytes int = 2,147,483,647.
        //</summary>
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "StockNotGreaterThanZero")]
        
        
        public string Stock { get; set; }




        //<summary>
        //Use of Required to spot on an empty field
        //</summary>
        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "MissingPrice")]
        

        //<summary>
        //Use of a regular expression
        //This "regex" should always be sat before the "range" so that it can detect for any <0 values
        //"^(all which is diferent from)
        //((?!^Stock$) the field stock, only filled once per field.
        //-? the string may begin with one "-" character. Only one "-" character is allowed.
        //(\\d+\\.?\\d+|\\d) A number composed of 1 or more digits [0-9]
        //followed by a "." character which is only allowed once
        //followed by a number of one or more digits.
        //+$" end of the string
        //</summary>
        [RegularExpression("^((?!^Price$)-?(\\d+\\.?\\d+|\\d))+$", ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "PriceNotANumber")]
        

        //<summary>
        //Use of a "Range" attribute to check wether the field is a double between 1 and signed 4 bytes double = 1.7976931348623157*E+308.
        //</summary>
        [Range(0.0001, double.MaxValue, ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        
        public string Price { get; set; }
    }



}
