using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Codecool.CodecoolShop.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class ProductCategoryDaoDB: IProductCategoryDao
    {
        private ShopContext context = new ShopContext();
        private static ProductCategoryDaoDB instance;
        
        public static ProductCategoryDaoDB GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductCategoryDaoDB();
            }

            return instance;
        }
        public void Add(ProductCategory item)
        {
            context.ProductCategories.Add(item);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public ProductCategory Get(int id)
        {
            return context.ProductCategories.Find(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return context.ProductCategories;
        }
    }
}