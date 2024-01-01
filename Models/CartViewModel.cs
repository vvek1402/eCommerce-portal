using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class CartViewModel
    {
        public cart cart { get; set; }
        public List<cart> cartList { get; set; }
        public List<product_variant_value> productVariantList { get; set; }
        public List<image> productImageList { get; set; }

        public double GrandTotal { get; set; }

        public List<wishlist> wishlistList { get; set; }
    }
}