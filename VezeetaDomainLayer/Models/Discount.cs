using System.ComponentModel.DataAnnotations.Schema;
using VezeetaDomainLayer.Enums;

using VezeetaDomainLayer.Models;

namespace VezeetaDomainLayer.Models

{

    public class Discount
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string? CouponCode { get; set; }
        public DiscountTypeEnum Type { get; set; }
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public bool IsDeactivated { get; set; } 
    }

}
