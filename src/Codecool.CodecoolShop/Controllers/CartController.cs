using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;

namespace Codecool.CodecoolShop.Controllers
{
    public class CartController : Controller
    {
        private const string ORDERS_PATH = "orders";
        private ProductController _productController;
        private readonly ILogger<CartController> _logger;
        private ICompositeViewEngine _viewEngine;
        private readonly ProductService _productService;
        private UserManager<User> _userManager;
        private SignInManager<User> _signManager;

        public CartController(ICompositeViewEngine viewEngine,
            UserManager<User> userManager,
            SignInManager<User> signManager,
            ILogger<CartController> logger = null)
        {
            _productController = new ProductController();
            _logger = logger;
            _productService = new ProductService();
            _viewEngine = viewEngine;
            _userManager = userManager;
            _signManager = signManager;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            ViewBag.cart = cart?.ConvertToProductEntityView();
            if (cart != null)
                ViewBag.total = cart
                    .Sum(item => _productController
                        .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
            return View();
        }


        [Route("Cart/AddToCart/{id?}/{amount?}")]
        public IActionResult AddToCart(int id, int amount = 1)
        {
            if (HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart") == null)
            {
                List<ProductEntityCart> cart = new List<ProductEntityCart>();
                cart.Add(new ProductEntityCart {ProductId = id, Quantity = amount});
                HttpContext.Session.SetObjectAsJson("cart", cart);
            }
            else
            {
                List<ProductEntityCart> cart =
                    HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
                int index = IfExists(id);
                if (index != -1)
                {
                    cart[index].Quantity += amount;
                }
                else
                {
                    cart.Add(new ProductEntityCart {ProductId = id, Quantity = amount});
                }

                HttpContext.Session.SetObjectAsJson("cart", cart);
            }

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Remove(int id)
        {
            List<ProductEntityCart> cart =
                HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            int index = IfExists(id);
            if (index != -1)
            {
                cart.RemoveAt(index);
                HttpContext.Session.SetObjectAsJson("cart", cart);
            }

            return RedirectToAction("Index");
        }

        [Route("Cart/changeAmount/{id?}/{amount?}")]
        public IActionResult ChangeAmount(int id, int amount)
        {
            List<ProductEntityCart> cart =
                HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            int index = IfExists(id);
            if (index != -1)
            {
                // increasing amount by 1 or -1 and checking if amount of items equals 0 - if yes, then remove from cart
                if ((cart[index].Quantity += amount) < 1)
                {
                    return RedirectToAction("Remove", new {id = id});
                }
            }

            HttpContext.Session.SetObjectAsJson("cart", cart);

            return RedirectToAction("Index");
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> BillingAddress()
        {
            BillingAddressModel address;
            if (_signManager.IsSignedIn(User))
            {
                var userIdentity = await SessionHelper.GetCurrentUserAsync(_userManager, HttpContext);
                address = _productService.GetBillingAddress(userIdentity.Id);
            }
            else
            {
                address = HttpContext.Session.GetObjectFromJson<BillingAddressModel>("client");
            }


            return View("BillingAddress", address);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> BillingAddress(BillingAddressModel billingAddressModel)
        {
            // If form is filled by valid data
            if (ModelState.IsValid)
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
                ViewBag.cart = cart.ConvertToProductEntityView();
                if (cart != null)
                    ViewBag.total = cart.Sum(item => _productController
                        .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
                HttpContext.Session.SetObjectAsJson("client", billingAddressModel);

                // If user is logged i can save current billing address
                if (_signManager.IsSignedIn(User))
                {
                    var userIdentity = await SessionHelper.GetCurrentUserAsync(_userManager, HttpContext);
                    var user = _productService.GetUser(userIdentity.Id);
                    var addressOnDb = _productService.GetBillingAddress(user.Id);
                    // Update billing address
                    _productService.UpdateBillingAddress(addressOnDb.Id, billingAddressModel);
                }

                return View("OrderSummary", billingAddressModel);
            }

            // If form is not filled by valid data, redirect to form again
            return View("BillingAddress", billingAddressModel);
        }

        [HttpGet]
        public IActionResult Payment()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            ViewBag.total = cart.Sum(item => _productController
                .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
            return View("Payment");
        }

        [HttpPost]
        public IActionResult Payment(CreditCard card)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            var total = cart.Sum(item => _productController
                .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
            ViewBag.total = total;

            if (ModelState.IsValid)
            {
                MockupBank bankAccount = new MockupBank();
                if (bankAccount.IsPaymentSuccesfull(total))
                {
                    return RedirectToAction("OrderConfirmation");
                }

                return RedirectToAction("OrderError");
            }

            return View("Payment");
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            ViewBag.cart = cart.ConvertToProductEntityView();
            ViewBag.total = cart.Sum(item => _productController
                .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
            var client = HttpContext.Session.GetObjectFromJson<BillingAddressModel>("client");
            ViewBag.client = client;

            User? user = HttpContext.Session.GetObjectFromJson<User>("user");
            Order order = new Order
            {
                Date = DateTime.Now,
                OrderedItems = cart,
                BillingAddressModel = client,
                UserId = user?.Id ?? null
            };
            SaveOrderToFile(order);
            _productService.AddOrder(order);
            if (user != null)
            {
                _productService.RemoveCartByUserId(user.Id);
            }


            ViewData["cart"] = cart.ConvertToProductEntityView();
            ViewData["total"] = cart.Sum(item => _productController
                .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
            ViewData["client"] = client;

            var emailBody =
                await EmailHelper.RenderPartialViewToString(
                    "_OrderConfirmationEmail",
                    new object(),
                    ViewData,
                    this,
                    _viewEngine
                );

            EmailHelper.SendEmailConfirmation(order.BillingAddressModel.Email, emailBody);
            EmptyCart();
            return View("OrderConfirmation");
        }

        [HttpGet]
        public async Task<IActionResult> OrderError()
        {
            var client = HttpContext.Session.GetObjectFromJson<BillingAddressModel>("client");
            ViewBag.client = client;
            Order order = new Order
            {
                BillingAddressModel = client
            };

            var emailBody =
                await EmailHelper.RenderPartialViewToString(
                    "_OrderErrorEmail",
                    new object(),
                    ViewData,
                    this,
                    _viewEngine
                );

            EmailHelper.SendEmailConfirmation(order.BillingAddressModel.Email, emailBody);
            return View("OrderError");
        }

        private int GetOrderId()
        {
            if (!Directory.Exists(ORDERS_PATH))
            {
                return 0;
            }

            var ordersFiles = Directory.GetFiles(ORDERS_PATH);
            return ordersFiles.Length;
        }

        private void SaveOrderToFile(Order order)
        {
            var orderDataJson = JsonSerializer.Serialize(order);
            if (!Directory.Exists(ORDERS_PATH))
            {
                Directory.CreateDirectory(ORDERS_PATH);
            }

            string path = Path.Combine(ORDERS_PATH, $"order_{order.Id}" + ".json");
            System.IO.File.WriteAllText(path, orderDataJson);
        }

        private int IfExists(int id)
        {
            List<ProductEntityCart> cart =
                HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }

        private void EmptyCart()
        {
            HttpContext.Session.SetObjectAsJson("cart", null);
        }


        public IActionResult SaveCart(User user)
        {
            User currentUser = user;
            Cart cart = new Cart()
            {
                ItemsInCart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart"),
                UserId = user.Id
            };
            currentUser.Cart.Add(cart);
            _productService.UpdateCart(cart);

            return RedirectToAction("Index");
        }


        public RedirectToActionResult GetCartState(string id)
        {
            var cartState = _productService.GetCartByUserId(id);
            // Make previous cart null to display only saved items
            EmptyCart();
            if (cartState != null && cartState.ItemsInCart != null)
            {
                foreach (var cartEntity in cartState.ItemsInCart)
                {
                    AddToCart(cartEntity.ProductId, cartEntity.Quantity);
                }
            }

            return RedirectToAction("Index");
        }
    }
}