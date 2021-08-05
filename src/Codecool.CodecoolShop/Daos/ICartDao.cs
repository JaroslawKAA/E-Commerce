using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using Data.Models;

namespace Codecool.CodecoolShop.Daos
{
    public interface ICartDao : IDao<Cart>
    {
        void Update(Cart cart);
        Cart GetByUserId(string id);
        void RemoveByUserId(string id);
    }
}