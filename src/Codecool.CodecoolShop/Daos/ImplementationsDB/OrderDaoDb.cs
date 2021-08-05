using System;
using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;
using Data;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class OrderDaoDb : IOrderDao
    {
        private ShopContext context = new ShopContext();
        private static OrderDaoDb instance;
        
        public static OrderDaoDb GetInstance()
        {
            if (instance == null)
            {
                instance = new OrderDaoDb();
            }

            return instance;
        }
        public void Add(Order item)
        {
            context.Add(item);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            if (context.Orders.Any(x => x.Id == id) )
            {
                context.Orders.Remove(context.Orders.First(x => x.Id == id));
                context.SaveChanges();
            }
            else
            {
                throw new IndexOutOfRangeException($"Didn't found order with id == {id}");
            }
        }

        public Order Get(int id)
        {
            return context.Orders.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return context.Orders;
        }
    }
}