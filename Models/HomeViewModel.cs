using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class HomeViewModel
    {
        public List<product> ProductList { get; set; }
        public List<product_variant_value> ProductVariantList { get; set; }
        public List<image> ProductImageList { get; set; }

        public product Product { get; set; }
        public List<category> categoryList { get; set; }
        public Tuple<HomeViewModel, long, int> data { get; set; }

        public category category { get; set; }
        public sub_category sub_category { get; set; }

        public List<variant> variants { get; set; }

        public List<variant_value> variant_value { get; set; }

        public List<variant_value> variantList { get; set; }
        public List<string> vnameList { get; set; }

        public product_variant_value selectedvariant { get; set; }

        public int qty { get; set; }

        public List<review> reviewList { get; set; }
        public List<User_account> userList { get; set; }
        public User_account user { get; set; }
        public string review { get; set; }
        public int star { get; set; }
    }
}