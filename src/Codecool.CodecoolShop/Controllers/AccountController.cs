using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Codecool.CodecoolShop.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        private ICompositeViewEngine _viewEngine;
        private readonly ProductService _productService;

        public AccountController(ICompositeViewEngine viewEngine, UserManager<User> userManager,
            SignInManager<User> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _viewEngine = viewEngine;
            _productService = new ProductService();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {UserName = model.Username, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);

                    ViewData["user"] = user;
                    var emailBody =
                        await EmailHelper.RenderPartialViewToString(
                            "RegisterConfirmation",
                            new object(),
                            ViewData,
                            this,
                            _viewEngine
                        );
                    EmailHelper.SendEmailConfirmation(user.Email, emailBody);
                    
                    HttpContext.Session.SetObjectAsJson("user", user);
                    return RedirectToAction("Index", "Product");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel {ReturnUrl = returnUrl};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Username,
                    model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("AddToCartState");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            HttpContext.Session.SetObjectAsJson("user", null);
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdentity =  await SessionHelper.GetCurrentUserAsync(_userManager, HttpContext);
            var user = _productService.GetUser(userIdentity.Id);
            var address = _productService.GetBillingAddress(userIdentity.Id);
            ViewBag.address = address;
            return View("Index", user);
        }

        [HttpGet]
        public async Task<IActionResult> AddBillingAddress()
        {
            return View("AddBillingAddress");
        }
        
        [HttpPost]
        public async Task<IActionResult> AddBillingAddress(BillingAddressModel billingAddress)
        {
            var user =  await SessionHelper.GetCurrentUserAsync(_userManager, HttpContext);;
            _productService.UpdateUserBillingAddress(user.Id, billingAddress);
            return RedirectToAction("Index", user);
        }


        [HttpGet]
        public async Task<IActionResult> EditBillingAddress()
        {
            var userIdentity =  await SessionHelper.GetCurrentUserAsync(_userManager, HttpContext);
            var address = _productService.GetBillingAddress(userIdentity.Id);

            return View("EditBillingAddress", address);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditBillingAddress(BillingAddressModel billingAddress)
        {
            var userIdentity =  await SessionHelper.GetCurrentUserAsync(_userManager, HttpContext);
            var user = _productService.GetUser(userIdentity.Id);
            var address = _productService.GetBillingAddress(userIdentity.Id);
            _productService.UpdateBillingAddress(address.Id, billingAddress);
            return RedirectToAction("Index", user);
        }
        
        
        public async Task<RedirectToActionResult> SaveUserCart()
        {
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return RedirectToAction("SaveCart", "Cart", currentUser);

        }


        public async Task<RedirectToActionResult> AddToCartState()
        {
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            HttpContext.Session.SetObjectAsJson("user", currentUser);
            return RedirectToAction("GetCartState", "Cart", new {id = currentUser.Id});
        }
    }
}