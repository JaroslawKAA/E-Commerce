using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Daos.ImplementationsDB;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace Codecool.CodecoolShop.Controllers
{
    public class SharedController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private ICompositeViewEngine _viewEngine;
        private ProductController _productController;
        
        public ProductService ProductService { get; set; }

        public SharedController(ICompositeViewEngine viewEngine, ILogger<ProductController> logger = null)
        {
            _logger = logger;
            _productController = new ProductController();
            ProductService = new ProductService();
            _viewEngine = viewEngine;
        }

        [HttpGet]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel();
            // TODO Specify request id in error view model
            return View("Error", errorViewModel);
        }
        
        public IActionResult CategoryDropmenu()
        {
            ViewData["categories"] = ProductService.GetAllCategories().ToList();
            return new PartialViewResult
            {
                ViewName = "_CategoryPartialView",
                ViewData = ViewData
            };
        }

        public IActionResult SuppliersDropmenu()
        {
            ViewData["suppliers"] = ProductService.GetAllSuppliers().ToList();
            return new PartialViewResult
            {
                ViewName = "_SupplierPartialView",
                ViewData = ViewData
            };
        }

        public IActionResult ModalMenu(int id)
        {
            Product product = ProductService.GetProductById(id);
            ViewData["idOfProduct"] = id;
            ViewData["product"] = product;
            return new PartialViewResult
            {
                ViewName = "_CartModal",
                ViewData = ViewData
            };
        }

        public IActionResult CartItems()
        {
            return Json(HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart"));
        }

        public IActionResult OrderConfirmationEmail()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductEntityCart>>("cart");
            ViewData["total"] = cart.Sum(item => _productController
                .GetProductById(item.ProductId).DefaultPrice * item.Quantity);
            ViewData["cart"] = cart.ConvertToProductEntityView();
            var client = HttpContext.Session.GetObjectFromJson<BillingAddressModel>("client");
            ViewData["client"] = client;

            return new PartialViewResult
            {
                ViewName = "_OrderConfirmationEmail",
                ViewData = ViewData
            };
        }
    }
}