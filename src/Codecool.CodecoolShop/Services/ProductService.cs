using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.ImplementationsDB;
using Codecool.CodecoolShop.Models;
using Data;
using Data.Models;
using Serilog;

namespace Codecool.CodecoolShop.Services
{
    public class ProductService
    {
        private readonly IProductDao productDao;
        private readonly IProductCategoryDao productCategoryDao;
        private readonly ISupplierDao supplierDao;
        private readonly IOrderDao orderDao;
        private readonly IBillingAddressDao _billingAddressDao;
        private readonly IUserDao _userDao;
        private readonly ICartDao cartDao;
        
        private ShopContext context = new ShopContext();

        public ProductService()
        {
            this.productDao = ProductDaoDB.GetInstance();
            this.productCategoryDao = ProductCategoryDaoDB.GetInstance();
            this.supplierDao = SupplierDaoDB.GetInstance();
            this.orderDao = OrderDaoDb.GetInstance();
            this._billingAddressDao = BillingAddressDaoDb.GetInstance();
            this._userDao = new UserDaoDb();
            this.cartDao = CartDaoDb.GetInstance();
        }

        public ProductCategory GetProductCategory(int categoryId)
        {
            return this.productCategoryDao.Get(categoryId);
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            return this.productCategoryDao.GetAll();
        }

        public Supplier GetSupplier(int id)
        {
            return this.supplierDao.Get(id);
        }
        
        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return this.supplierDao.GetAll();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            // ToDO Remove test log below
            Log.Warning("Test log. Get all product metods");
            return this.productDao.GetAll();
        }
        
        public IEnumerable<Product> GetProductsForCategory(int categoryId)
        {
            ProductCategory category = this.productCategoryDao.Get(categoryId);
            return this.productDao.GetBy(category);
        }

        public IEnumerable<Product> GetProductsBySupplier(int supplierId)
        {
            Supplier supplier = this.supplierDao.Get(supplierId);
            return this.productDao.GetBy(supplier);
        }

        public Product GetProductById(int id)
        {
            return productDao.Get(id);
        }

        public void AddProduct(Product product)
        {
            productDao.Add(product);
        }

        public void AddOrder(Order order)
        {
            orderDao.Add(order);
        }

        public void AddCart(Cart cart)
        {
            cartDao.Add(cart);
        }

        public void UpdateCart(Cart cart)
        {
            cartDao.Update(cart);
        }

        public Cart GetCartByUserId(string id)
        {
            return cartDao.GetByUserId(id);
        }

        public void RemoveCartByUserId(string id)
        {
            cartDao.RemoveByUserId(id);
        }

        public void AddClient(BillingAddressModel billingAddressModel)
        {
            _billingAddressDao.Add(billingAddressModel);
        }

        public void UpdateUserBillingAddress(string id, BillingAddressModel billingAddress)
        {
            var user = context.Users.First(u => u.Id == id);
            user.BillingAddress = billingAddress;
            user.BillingAddress.Owner = user;
            context.SaveChanges();
        }

        public User GetUser(string id)
        {
            return _userDao.Get(id);
        }

        public BillingAddressModel GetBillingAddress(string userId)
        {
            return context.BillingAddresses.FirstOrDefault(x => x.Owner.Id == userId);
        }

        public void UpdateBillingAddress(int id, BillingAddressModel billingAddress)
        {
            var address = context.BillingAddresses.First(x => x.Id == id);
            address.City = billingAddress.City;
            address.Email = billingAddress.Email;
            address.Street = billingAddress.Street;
            address.Zipcode = billingAddress.Zipcode;
            address.Country = billingAddress.Country;
            address.LastName = billingAddress.LastName;
            address.FirstName = billingAddress.FirstName;
            address.HomeNumber = billingAddress.HomeNumber;
            address.PhoneNumber = billingAddress.PhoneNumber;
            address.StreetNumber = billingAddress.StreetNumber;
            context.SaveChanges();
        }
    }
}
