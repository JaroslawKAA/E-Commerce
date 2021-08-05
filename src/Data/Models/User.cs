using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    public class User: IdentityUser
    {
        [ForeignKey(nameof(BillingAddressModel))]
        public BillingAddressModel BillingAddress { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        public List<Cart> Cart { get; set; } = new List<Cart>();
    }
}