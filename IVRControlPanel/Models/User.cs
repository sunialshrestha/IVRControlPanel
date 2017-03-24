﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace IVRControlPanel.Models
{
      [MetadataType(typeof(TestEntityValidation))]
    public partial class Category{
    }

    public class TestEntityValidation{
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public String CategoryName { get; set; }

      

    }

     [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
    }
     public class UserMetaData
     {
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
         [DateRange("1910/01/01", "2010/01/01")]
         public DateTime BirthDate { get; set; }

         [Required(ErrorMessage = "Please enter your email address")]
         [RegularExpression(".+\\@.+\\..+",
         ErrorMessage = "Please enter a valid email address")]
         public string Email { get; set; }
     }



     public class DateRangeAttribute : ValidationAttribute
     {
         private const string DateFormat = "yyyy/MM/dd";
         private const string DefaultErrorMessage =
      "'{0}' must be a date between {1:d} and {2:d}.";

         public DateTime MinDate { get; set; }
         public DateTime MaxDate { get; set; }

         public DateRangeAttribute(string minDate, string maxDate)
             : base(DefaultErrorMessage)
         {
             MinDate = ParseDate(minDate);
             MaxDate = ParseDate(maxDate);
         }

         public override bool IsValid(object value)
         {
             if (value == null || !(value is DateTime))
             {
                 return true;
             }
             DateTime dateValue = (DateTime)value;
             return MinDate <= dateValue && dateValue <= MaxDate;
         }
         public override string FormatErrorMessage(string name)
         {
             return String.Format(CultureInfo.CurrentCulture,
      ErrorMessageString,
                 name, MinDate, MaxDate);
         }

         private static DateTime ParseDate(string dateValue)
         {
             return DateTime.ParseExact(dateValue, DateFormat,
      CultureInfo.InvariantCulture);
         }
     }







}