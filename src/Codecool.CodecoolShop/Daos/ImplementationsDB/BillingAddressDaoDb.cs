using System;
using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;
using Data;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class BillingAddressDaoDb : IBillingAddressDao
    {
        private ShopContext context = new ShopContext();
        private static BillingAddressDaoDb instance;
        
        public static BillingAddressDaoDb GetInstance()
        {
            if (instance == null)
            {
                instance = new BillingAddressDaoDb();
            }

            return instance;
        }
        public void Add(BillingAddressModel item)
        {
            context.BillingAddresses.Add(item);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            if (context.BillingAddresses.Any(c => c.Id == id))
            {
                context.BillingAddresses.Remove(context.BillingAddresses.First(x => x.Id == id));
                context.SaveChanges();
            }
            else
            {
                throw new IndexOutOfRangeException($"Not found client with id == {id}");
            }
        }

        public BillingAddressModel Get(int id)
        {
            return context.BillingAddresses.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<BillingAddressModel> GetAll()
        {
            return context.BillingAddresses;
        }
    }
}