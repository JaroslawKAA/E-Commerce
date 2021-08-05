using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models
{
    public class CreditCard
    {
        [Required]
        [DisplayName("Card number")]
        [StringLength(16, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        [DisplayName("Owner name")]
        [StringLength(100)]
        public string CardHolder { get; set; }
        [Required]
        [DisplayName("Expiry date (format mm/YY)")]
        [RegularExpression( @"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Wrong date format.")]
        public string ExpiryDate { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CVV { get; set; }
    }
}