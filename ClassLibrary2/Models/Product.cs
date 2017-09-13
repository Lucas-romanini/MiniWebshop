using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MWRepo.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool Active { get; set; }
        public int CategoryID { get; set; }
        public DateTime DateAdded { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }


        public double GetPrice()
        {
            if (SalePrice < Price)
            {
                return SalePrice;
            }
            return Price;
        }
    }
}
