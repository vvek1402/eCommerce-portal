using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class Categoryvm
    {
        public category category { get; set;}
        public List<sub_category> subcategoryList { get; set; }

        public sub_category subcategory { get; set; }
    }
}