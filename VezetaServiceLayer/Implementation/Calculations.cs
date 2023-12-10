using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Models;
using VezeetaDomainLayer.Enums;
using Microsoft.EntityFrameworkCore;
using VezeetaDomainLayer.DTOs;

namespace VezeetaServiceLayer.Implementation
{
    public static class Calculations
    {
        public static decimal CalculateFinalPrice(decimal basePrice, Discount discount)
        {
            if (discount != null)
            {
                if (discount.Type == DiscountTypeEnum.Percentage)
                {
                    
                    decimal discountAmount = (discount.Amount / 100) * basePrice;
                    return basePrice - discountAmount;
                }
                else
                {
                   
                    return basePrice - discount.Amount;
                }
            }

            
            return basePrice;
        }
        public static bool IsOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
        {
            return start1 < end2 && end1 > start2;
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            DateTime currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateOfBirth.Year;

            if (currentDate.Month < dateOfBirth.Month || (currentDate.Month == dateOfBirth.Month && currentDate.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age;
        }


    }
}
