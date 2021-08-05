using System.Collections.Generic;
using Codecool.CodecoolShop.Controllers;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services
{
    public static class ProductEntityHelper
    {
        private static ProductController _productController;

        static ProductEntityHelper()
        {
            _productController = new ProductController();
        }
        
        public static ProductEntityView ConvertToProductEntityView(this ProductEntityCart productEntityCart)
        {
            return new ProductEntityView()
            {
                Id = productEntityCart.Id,
                Product = _productController.GetProductById(productEntityCart.ProductId),
                Quantity = productEntityCart.Quantity
            };
        }
        
        public static IEnumerable<ProductEntityView> ConvertToProductEntityView(this IEnumerable<ProductEntityCart> productEntityCarts)
        {
            var result = new List<ProductEntityView>();
            foreach (var productEntityCart in productEntityCarts)
            {
                result.Add(new ProductEntityView()
                {
                    Id = productEntityCart.Id,
                    Product = _productController.GetProductById(productEntityCart.ProductId),
                    Quantity = productEntityCart.Quantity
                });
            }

            return result;
        }
    }
}