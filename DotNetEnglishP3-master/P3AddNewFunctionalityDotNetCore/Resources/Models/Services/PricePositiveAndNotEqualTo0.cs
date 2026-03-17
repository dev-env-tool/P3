using System;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Resources.Models.Services
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PricePositiveAndNotEqualTo0 : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            if ((double)value == 0)
            {
                result = false;
                return result;
            }
            return result;
        }

    }

}
