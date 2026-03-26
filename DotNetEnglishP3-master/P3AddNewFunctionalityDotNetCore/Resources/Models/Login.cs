using System;
using System.Resources;
using System.Reflection;
using System.Globalization;


namespace P3AddNewFunctionalityDotNetCore.Resources.Models
{

    public static class Login
    {
        private static ResourceManager resourceManager = new ResourceManager("P3AddNewFunctionalityDotNetCore.Resources.Models.Login", Assembly.GetExecutingAssembly());
        private static CultureInfo resourceCulture;
        
        public static string ErrorMissingLoginName
        {
            get
            {
                return resourceManager.GetString("ErrorMissingLoginName", resourceCulture);
            }
        }



        public static string ErrorMissingLoginPassword
        {
            get
            {
                return resourceManager.GetString("ErrorMissingLoginPassword", resourceCulture);
            }
        }

        public static string ErrorWrongLoginNameOrPassword
        {
            get
            {
                return resourceManager.GetString("ErrorWrongLoginNameOrPassword", resourceCulture);
            }
        }

        
    }
}
