using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class ProductList
    {
        public List<product> productList { get; set; }
        public List<product_variant_value> productvariantList { get; set; }
        public List<category> categoryList { get; set; }
        public List<sub_category> subcategoryList { get; set; }

        public List<variant> variantList { get; set; }
        public List<variant_value> variantvalueList { get; set; }

        public List<image> ImageList { get; set; }



        public int category_id { get; set; }
        public int sub_category_id { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public int Isactive { get; set; }
        public long price_factor_id { get; set; }
        public long admin_id { get; set; }
        public double shipping_rate { get; set; }
        public string sku { get; set; }

        public string variant_value { get; set; }
        public string variant_value_description { get; set; }

        public string variantsku { get; set; }
        public long product_id { get; set; }
        public long variant_id { get; set; }
        public Nullable<long> variant_value_id { get; set; }
        public double MRP { get; set; }
        public double Selling_price { get; set; }
        public double discount { get; set; }
        public int discount_min_quantity { get; set; }
        public int discount_max_quantity { get; set; }
        public int weight { get; set; }
        public int unit_in_stock { get; set; }
        public int max_purchase_quantity { get; set; }
        public int quantity_per_unit { get; set; }
        public string product_variant_title { get; set; }
        public long product_variant_id { get; set; }
        public string main_image { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }

        public string category_name { get; set; }
        public string sub_category_name { get; set; }


        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int total_page { get; set; }
    }
}