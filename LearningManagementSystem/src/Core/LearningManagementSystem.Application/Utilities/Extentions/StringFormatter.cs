using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Utilities.Extentions
{
    public static class StringFormatter
    {
        public static bool isDigit(this string name)
        {
            return (name.Any(char.IsDigit));
        }
        public static string Capitalize(this string name)
        {
            name = name.Trim();
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
            return name;
        }

        public static bool CheckEmail(this string email)
        {        
            if (string.IsNullOrEmpty(email)) return false;
            string emailregex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(emailregex);
            return regex.IsMatch(email);

        }
        public static bool ValidateAge(this DateTime? birthday,int limit)
        {
            if (birthday.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - birthday.Value.Year;
                if (birthday > today.AddYears(-age))
                {
                    age--;
                }
                if (age < limit)
                { 
                    return false;
                }
            }
            return true;
        }
    }
}
