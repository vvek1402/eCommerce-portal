using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class ShopViewModel
    {
        public List<category> categoryList { get; set; }

        public List<sub_category> subcategoryList { get; set; }

        public List<product> productList { get; set; }

        public List<product_variant_value> productvariantList { get; set; }

        public List<image> productimageList { get; set; }

    }
}