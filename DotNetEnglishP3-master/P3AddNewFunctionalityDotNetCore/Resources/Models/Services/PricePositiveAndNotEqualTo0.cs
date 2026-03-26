using P3AddNewFunctionalityDotNetCore.Models.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Resources.Models.Services
{
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    //public class PricePositiveAndNotEqualTo0 : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        if (value == null)
    //        {
    //            return false;
    //        }

    //        double doubleValue = Convert.ToDouble(value);

    //        if (doubleValue > 0)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}

    //public enum ErrorCode
    //{
    //    NoError,
    //    Error1,
    //    Error2,
    //    Error3,
    //    Error4,
    //    Error5
    //}

    //public class ValidationResult
    //{
    //    public bool IsValid => ErrorCode == ErrorCode.NoError;
    //    public ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;
    //}
    public class PriceGreaterThan0 : ValidationAttribute
    {
        public override bool IsValid(object value)
        { 
            if (value == null)
            {
                return false;
            }
            if (!(decimal.TryParse(value.ToString(), out decimal priceValue)))
            {
                return false;
            }
            if (!(priceValue > 0))
            {
                return false; 
            }
            return true;
        }
    }
    public class PriceNotDouble : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (!(decimal.TryParse(value.ToString(), out decimal priceValue)))
            {
                return false;
            }
            return true;
        }
        
    }

}

