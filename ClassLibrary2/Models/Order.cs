﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWRepo.Models
{
    public class Order
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }

        public string Fullname_Delivery { get; set; }
        public string Phone_Delivery { get; set; }
        public string Email_Delivery { get; set; }
        public string Address_Delivery { get; set; }
        public string Postal_Delivery { get; set; }
        public string City_Delivery { get; set; }

        public List<int> ProductIDs { get; set; }
    }
}
