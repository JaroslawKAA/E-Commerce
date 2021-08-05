using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Models;

namespace Codecool.CodecoolShop.Daos.ImplementationsDB
{
    public class UserDaoDb : IUserDao
    {
        private ShopContext context = new ShopContext();
        private static UserDaoDb instance;


        public static UserDaoDb GetInstance()
        {
            if (instance == null)
            {
                instance = new UserDaoDb();
            }

            return instance;
        }

        public void Add(User item)
        {
            context.Users.Add(item);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            throw new System.InvalidOperationException("User don't have int id");
        }

        public User Get(int id)
        {
            throw new System.InvalidOperationException("User don't have int id");
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users;
        }

        public void Remove(string id)
        {
            if (context.Users.Any(x => x.Id == id))
            {
                context.Users.Remove(context.Users.First(x => x.Id == id));
                context.SaveChanges();
            }
            else
            {
                throw new IndexOutOfRangeException($"Didn't found user with id == {id}");
            }
        }

        public User Get(string id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}