using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class CoupenViewModel
    {

        public List<coupen> coupenList;

        
        public string name { get; set; }
        public string description { get; set; }
        public DateTime starting_date { get; set; }
        public DateTime ending_date { get; set; }
      
        public double discount_rate { get; set; }
        public double minimun_purchase_price { get; set; }
        public double maximum_purchase_price { get; set; }
      
    }
}