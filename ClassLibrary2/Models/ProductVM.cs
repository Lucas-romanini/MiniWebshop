﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWRepo.Models
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public List<Image> Images { get; set; }
        public Category Category { get; set; }
    }
}
