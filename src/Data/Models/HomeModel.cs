using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models
{
    public class HomeModel : BaseModel
    {
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
}