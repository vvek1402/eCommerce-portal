using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace E_Commerce.Models
{
    public class UserDetails
    {
        public User_account user_account { get; set; }
        public List<User_address> addresslist { get; set; }

        public List<address_type> addtypeList { get; set; }
        public List<city> cityList { get; set; }
        public List<Country> countryList { get; set; }
        public string address { get; set; }
        public int pincode { get; set; }
        public long city_id { get; set; }
        public long country_id { get; set; }
        public long address_type_id { get; set; }
        public int isdefault { get; set; }

        public string name { get; set; }
        public string email_id { get; set; }
        public string password { get; set; }
        public string mobile_no { get; set; }
        public long role_id { get; set; }

        public List<User_account> UserList { get; set; }
    }
}