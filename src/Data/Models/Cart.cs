using System.Collections;
using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Data.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<ProductEntityCart> ItemsInCart { get; set; }
        
        public string UserId { get; set; }

    }
}