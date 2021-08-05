using Data.Models;

namespace Codecool.CodecoolShop.Daos
{
    public interface IUserDao : IDao<User>
    {
        public void Remove(string id);
        public User Get(string id);
    }
}