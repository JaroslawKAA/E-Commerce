using System;
using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class CartDaoDb : ICartDao
    {
        private ShopContext context = new ShopContext();
        private static CartDaoDb instance;
        
        public static CartDaoDb GetInstance()
        {
            if (instance == null)
            {
                instance = new CartDaoDb();
            }

            return instance;
        }
        public void Add(Cart item)
        {
            context.Add(item);
            context.SaveChanges();
        }

        // Update Cart for user to avoid restoring many Cart Data for one user
        public void Update(Cart item)
        {
            var currentUserCart = context.Carts.FirstOrDefault(x => x.UserId == item.UserId);
            if (currentUserCart != null)
            {
                currentUserCart.ItemsInCart = item.ItemsInCart;
                context.SaveChanges();
            }
            else
            {
                Add(item);
            }
        }

        public void Remove(int id)
        {
            if (context.Carts.Any(x => x.Id == id) )
            {
                context.Carts.Remove(context.Carts.First(x => x.Id == id));
                context.SaveChanges();
            }
            else
            {
                throw new IndexOutOfRangeException($"Don't find cart with id == {id}");
            }
        }

        public Cart Get(int id)
        {
            return context.Carts.FirstOrDefault(x => x.Id == id);
        }

        public Cart GetByUserId(string id)
        {
            return context.Carts.Include(a => a.ItemsInCart).FirstOrDefault(x => x.UserId == id);

        }
        
        
        public void RemoveByUserId(string id)
        {
            if (context.Carts.Any(x => x.UserId == id) )
            {
                context.Carts.Remove(context.Carts.First(x => x.UserId == id));
                context.SaveChanges();
            }
        }

        public IEnumerable<Cart> GetAll()
        {
            return context.Carts;
        }
        
    }
}