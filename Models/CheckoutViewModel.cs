using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class CheckoutViewModel
    {
        public List<User_address> addressList { get; set; }
        public List<city> cityList { get; set; }
        public List<Country> countryList { get; set; }
        public List<address_type> addtypeList { get; set; }

        public List<cart> cartList { get; set; }
        public List<product_variant_value> productVariantList { get; set; }
        public List<image> productImageList { get; set; }

        public double GrandTotal { get; set; }
    }
}