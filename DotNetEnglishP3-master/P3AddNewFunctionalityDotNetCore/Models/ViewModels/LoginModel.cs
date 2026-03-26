using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Login), ErrorMessageResourceName = "ErrorMissingLoginName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(P3AddNewFunctionalityDotNetCore.Resources.Models.Login), ErrorMessageResourceName = "ErrorMissingLoginPassword")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}