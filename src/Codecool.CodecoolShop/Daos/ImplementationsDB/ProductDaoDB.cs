using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;
using Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class ProductDaoDB: IProductDao
    {
        private ShopContext context = new ShopContext();
        private static ProductDaoDB instance;
        
        
        public static ProductDaoDB GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductDaoDB();
            }

            return instance;
        }
        public void Add(Product item)
        {
            context.Products.Add(item);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public Product Get(int id)
        {
            return context.Products.Find(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products
                .Include(a => a.Supplier)
                .Include(b => b.ProductCategory);
        }

        public IEnumerable<Product> GetBy(Supplier supplier)
        {
            return context.Products.Where(x => x.Supplier.Id == supplier.Id)
                .Include(a => a.Supplier)
                .Include(b => b.ProductCategory);
                
        }

        public IEnumerable<Product> GetBy(ProductCategory productCategory)
        {
            return context.Products.Where(x => x.ProductCategory.Id == productCategory.Id)
                .Include(a => a.Supplier)
                .Include(b => b.ProductCategory);
        }
    }
}