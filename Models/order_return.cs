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
    
    public partial class order_return
    {
        public long order_return_id { get; set; }
        public long order_id { get; set; }
        public long return_status_id { get; set; }
        public long return_reason_id { get; set; }
        public Nullable<System.DateTime> return_pickup_date { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual return_reason return_reason { get; set; }
        public virtual return_status return_status { get; set; }
    }
}
