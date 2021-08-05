using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;

namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        public ProductService ProductService { get; set; }
        public ProductController(ILogger<ProductController> logger = null)
        {
            _logger = logger;
            ProductService = new ProductService();
        }

        public IActionResult Index()
        {
            // TODO Remove log, its for testing
            _logger.LogWarning("Test log warning 2");
            
            var products = ProductService.GetAllProducts();
            ViewData["header"] = "All products";
            return View(products.ToList());
        }
        
        public IActionResult ByCategory(int id = 2)
        {
            var products = ProductService.GetProductsForCategory(id);
            ViewData["header"] = ProductService.GetProductCategory(id).Name;
            return View("index", products.ToList());
        }

        public IActionResult BySupplier(int id)
        {
            var products = ProductService.GetProductsBySupplier(id);
            ViewData["header"] = ProductService.GetSupplier(id).Name;
            return View("Index", products.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Example for sending object as json
        public Product GetProductById(int id)
        {
            return ProductService.GetProductById(id);
        }
        

        public IActionResult Product(int id)
        {
            return Json(ProductService.GetProductById(id));
        }

        // POST
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            ProductService.AddProduct(product);
            return RedirectToAction("Index");
        }
        
        // GET
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View("AddProduct");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
