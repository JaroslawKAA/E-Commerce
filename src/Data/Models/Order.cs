using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Models;
using Newtonsoft.Json;

namespace Codecool.CodecoolShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<ProductEntityCart> OrderedItems { get; set; }
        [Required]
        public BillingAddressModel BillingAddressModel { get; set; }

        public string UserId { get; set; }
    }
}