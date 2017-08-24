using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWRepo.Models
{
    public class Image
    {
        public int ID { get; set; }
        public String ImageUrl { get; set; }
        public int ProductID { get; set; }
        public int SubPageID { get; set; }
        public int CategoryID { get; set; }
    }
}
