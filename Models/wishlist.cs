//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Commerce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class wishlist
    {
        public long wishlist_id { get; set; }
        public long user_id { get; set; }
        public long product_variant_id { get; set; }
    
        public virtual product_variant_value product_variant_value { get; set; }
        public virtual User_account User_account { get; set; }
    }
}