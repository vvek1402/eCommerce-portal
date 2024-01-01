using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class RecentOrder
    {
        public string order_no { get; set; }
        public string customername { get; set; }
        public List<string> productlist { get; set; }
        public string orderdate { get; set; }
        public string paymentmode { get; set; }
    }
}