using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MWRepo;
using MWRepo.Models;
using MWRepo.Factories;

namespace MiniWebshop.Controllers
{
    public class HomeController : Controller
    {
        ProductFac productFac = new ProductFac();
        ImageFac imageFac = new ImageFac();
        CategoryFac categoryFac = new CategoryFac();
        

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


    }
}