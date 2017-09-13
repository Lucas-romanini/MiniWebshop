using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MWRepo.Models;
using MWRepo.Factories;

namespace MiniWebshop.Areas.Admin.Controllers
{
    public class CMSController : Controller
    {
        private ProductFac productFac = new ProductFac();
        private CategoryFac categoryFac = new CategoryFac();
        private ImageFac imageFac = new ImageFac();

        // GET: Admin/CMS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products(int? id)
        {
            List<ProductVM> products = new List<ProductVM>();
            if (TempData["searchResult"] != null)
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
                foreach (Product item in productFac.GetBy("CategoryID", id))
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
        #region Products
        public ActionResult AddProduct()
        {
            ViewBag.Catagories = categoryFac.GetAll();
            ViewBag.Images = imageFac.GetBy("ProductID",0);

            return View();
        }

        [HttpPost]
        public ActionResult AddproductSubmit(Product product, List<int> imageIDs)
        {
            int productID = productFac.Insert(product);

            for(int i = 0; i<imageIDs.Count; i++)
            {
                Image img = imageFac.Get(imageIDs[i]);
                img.ProductID = productID;
                imageFac.Update(img);
            }

            TempData["MSG"] = "A product has been added.";

            return RedirectToAction("Products");
        }
        #endregion
    }
}