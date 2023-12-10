using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.DTOs
{
    public class AddDiscountDTO
    {
        public string DiscountCode { get; set; }
        public int CompletedRequestsThreshold { get; set; }
        public DiscountTypeEnum DiscountType { get; set; }
        public decimal Amount { get; set; }
    }
}
