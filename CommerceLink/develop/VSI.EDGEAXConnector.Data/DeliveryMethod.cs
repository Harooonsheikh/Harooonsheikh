//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VSI.EDGEAXConnector.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ItemId { get; set; }
        public long ErpKey { get; set; }
        public int StoreId { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
