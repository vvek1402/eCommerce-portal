using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class OrderModel
    {
        public Order order { get; set; }
        public Order_detail orderdetail { get; set; }

        public User_address badd { get; set; }
        public User_address sadd { get; set; }
        public User_address adminadd { get; set; }
        public User_account user { get; set; }
        public List<User_account> users { get; set; }
        public List<city> cities { get; set; }
        public List<Country> countries { get; set; } 
        public long order_no { get; set; }
        public System.DateTime order_date { get; set; }
        public System.DateTime shipdate { get; set; }
        public long status_id { get; set; }
        public long billing_add { get; set; }
        public long shipping_add { get; set; }
        public long user_id { get; set; }
        public long payment_mode_id { get; set; }


        public long order_detail_id { get; set; }
        public int quantity { get; set; }
        public int total_amount { get; set; }
        public long product_variant_id { get; set; }
        public long order_id { get; set; }
        public long currency_type_id { get; set; }

        public List<Order_detail> OrderDetailList { get; set; }
        public List<Order> OrderList { get; set; }

        public List<order_status> StatusList { get; set; }

        public List<product_variant_value> ProductVariantList { get; set; }

        public List<payment_mode> paymentmodeList { get; set; }

        public List<image> imageList { get; set; }
        public DateTime pickupDate { get; set; }
        public List<order_return> orderReturnList { get; set; }

        public List<Order> deliveredOrderList { get; set; }
        public List<Order> returnOrderList { get; set; }

        public long return_reason_id { get; set; }
    }
}