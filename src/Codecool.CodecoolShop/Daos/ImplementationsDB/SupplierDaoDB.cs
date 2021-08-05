using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class SupplierDaoDB: ISupplierDao
    {
        private ShopContext context = new ShopContext();
        private static SupplierDaoDB instance;
        
        
        public static SupplierDaoDB GetInstance()
        {
            if (instance == null)
            {
                instance = new SupplierDaoDB();
            }

            return instance;
        }
        public void Add(Supplier item)
        {
            context.Suppliers.Add(item);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public Supplier Get(int id)
        {
            return context.Suppliers.Find(id);
        }

        public IEnumerable<Supplier> GetAll()
        {
            return context.Suppliers;
        }
    }
}