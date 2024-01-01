using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class ReviewViewModel
    {
        public List<review> reviewList { get; set; }
        public List<product> productList { get; set; }
        public List<User_account> userList { get; set; }
    }
}