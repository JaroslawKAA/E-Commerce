using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.ImplementationsDB;
using Codecool.CodecoolShop.Models;
using Data;

namespace Codecool.CodecoolShop
{
    public class ShopSeeder
    {
        private readonly ShopContext _dbContext;

        public ShopSeeder(ShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Products.Any())
                {
                    var products = GetProducts();
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>();
            
            // SUPPPLIERS ------------------
            Supplier amazon = new Supplier {Name = "Amazon", Description = "Digital content and services"};
            Supplier lenovo = new Supplier {Name = "Lenovo", Description = "Computers"};
            Supplier microsoft = new Supplier {Name = "Microsoft", Description = "Computers"};

            // CATEGORIES -----------------
            ProductCategory tablet = new ProductCategory
            {
                Name = "Tablet", Department = "Hardware",
                Description = "A tablet computer, commonly shortened to tablet, " +
                              "is a thin, flat mobile computer with a touchscreen display."
            };
            ProductCategory laptop = new ProductCategory
            {
                Name = "Laptop", Department = "Hardware",
                Description = "A computer, which you can take with you everywhere you need"
            };
            ProductCategory computer = new ProductCategory
            {
                Name = "Computer", Department = "Hardware",
                Description = "A computer, which you can take with you everywhere you need"
            };

            // PRODUCTS ------------------
            products.Add(new Product
            {
                Name = "Amazon Fire", DefaultPrice = 49.9m, Currency = "USD",
                Description =
                    "Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.",
                ProductCategory = tablet, Supplier = amazon
            });
            products.Add(new Product
            {
                Name = "Lenovo IdeaPad Miix 700", DefaultPrice = 479.0m, Currency = "USD",
                Description =
                    "Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.",
                ProductCategory = tablet, Supplier = lenovo
            });
            products.Add(new Product
            {
                Name = "Amazon Fire HD 8", DefaultPrice = 89.0m, Currency = "USD",
                Description = "Amazon's latest Fire HD 8 tablet is a great value for media consumption.",
                ProductCategory = tablet, Supplier = amazon
            });
            products.Add(new Product
            {
                Name = "Default laptop", DefaultPrice = 89.0m, Currency = "USD",
                Description = "Amazon's latest Fire HD 8 tablet is a great value for media consumption.",
                ProductCategory = laptop, Supplier = amazon
            });
            products.Add(new Product
            {
                Name = "Surface Pro 7", DefaultPrice = 399.0m, Currency = "USD",
                Description = "Computer from microsoft.", ProductCategory = laptop, Supplier = microsoft
            });
            products.Add(new Product
            {
                Name = "Surface Studio", DefaultPrice = 1999.0m, Currency = "USD",
                Description = "Super computer from microsoft.", ProductCategory = computer, Supplier = microsoft
            });

            return products;
        }
    }
}