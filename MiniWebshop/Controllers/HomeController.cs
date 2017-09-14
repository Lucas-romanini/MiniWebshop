using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MWRepo;
using MWRepo.Models;
using MWRepo.Factories;
using MiniWebshop.ShoppingCart;

namespace MiniWebshop.Controllers
{
    public class HomeController : Controller
    {
        ProductFac productFac = new ProductFac();
        ImageFac imageFac = new ImageFac();
        CategoryFac categoryFac = new CategoryFac();

        //ShoppingCart
        ProductCart cart;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            cart = new ProductCart(HttpContext, "ShoppingCart");
            ViewBag.AmountOfPrpductsInCart = cart.GetShoppingCart().Count;
            ViewBag.ShoppingCartTotal = cart.Total;
            base.OnActionExecuting(filterContext);
        }

        // GET: Home    
        public ActionResult Index()
        {
            return View();
        }
            
        public ActionResult Categories()
        {
            List<CategoryVM> categories = new List<CategoryVM>();

            
            foreach(Category item in categoryFac.GetAll())
            {
                CategoryVM categoryVM = new CategoryVM();
                categoryVM.Category = item;
                categoryVM.Image = imageFac.SqlQuery("SELECT TOP 1 * FROM Image WHERE CategoryID = '" + item.ID + "'");
                categories.Add(categoryVM);

            }

            return View(categories);
        }

        //ID = Category ID
        public ActionResult Products(int? id)
        {
            List<ProductVM> products = new List<ProductVM>();
            if(TempData["searchResult"] != null)
            {
                foreach (Product item in TempData["searchResult"] as List<Product>)
                {
                    ProductVM productVM = new ProductVM();
                    productVM.Product = item;
                    productVM.Images = imageFac.GetBy("ProductID", item.ID);
                    productVM.Category = categoryFac.Get(item.CategoryID);

                    products.Add(productVM);
                }
            }
            else if (id == null)
            {
                foreach (Product item in productFac.GetAll())
                {
                    ProductVM productVM = new ProductVM();
                    productVM.Product = item;
                    productVM.Images = imageFac.GetBy("ProductID", item.ID);
                    productVM.Category = categoryFac.Get(item.CategoryID);

                    products.Add(productVM);
                }
            }
            else
            {
                foreach (Product item in productFac.GetBy("CategoryID",id))
                {
                    ProductVM productVM = new ProductVM();
                    productVM.Product = item;
                    productVM.Images = imageFac.GetBy("ProductID", item.ID);
                    productVM.Category = categoryFac.Get(item.CategoryID);

                    products.Add(productVM);
                }
            }
            return View(products);
        }

        public ActionResult ShowProduct(int id = 0)
        {
            if (id > 0)
            {

                ProductVM vm = new ProductVM();
                vm.Product = productFac.Get(id);
                vm.Images = imageFac.GetBy("ProductID", id);
                vm.Category = categoryFac.Get(vm.Product.CategoryID);

                return View(vm);
            }
            else
            {
                return RedirectToAction("Products");
            }
        }

        [HttpPost]
        public ActionResult SearchSubmit(string searchInput)
        {
            TempData["searchResult"] = productFac.Search(searchInput, "Name", "Description");

            return RedirectToAction("Products");
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        #region ShoppingCart

        [HttpPost]
        public ActionResult AddToCart(int id, int amount)
        {
            ProductVM vm = new ProductVM();
            vm.Product = productFac.Get(id);
            vm.Images = imageFac.GetBy("ProductID", id);
            vm.Category = categoryFac.Get(vm.Product.CategoryID);

            for (int i = 0; i <amount; i++)
            {
                cart.Add(vm);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveFromCart(int id)
        {
            ProductVM vm = cart.GetShoppingCart().Find(x => x.Product.ID == id);

            cart.Remove(vm);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Cart()
        {
            
            return View(cart.GetShoppingCart());
        }

        #endregion

        #region checkout

        public ActionResult Checkout()
        {
            List<ProductVM> cartItems = cart.GetShoppingCart();
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult CheckoutSubmit(Order order)
        {
            EmailClient emailClient = new EmailClient("smtp.gmail.com", 578, "webitumbraco@gmail.com", "FedeAbe200", true);
            emailClient.SendEmail("lucas@romanini.dk");
            return Redirect("OrderConfirmation");
        }

        public ActionResult OrderConfirmation()
        {
            return View();
        }
        #endregion

    }
}   