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
    
    public partial class JobLog
    {
        public long JobLogId { get; set; }
        public long JobId { get; set; }
        public Nullable<System.DateTime> LastExecutionTimeStamp { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual Job Job { get; set; }
        public virtual JobLogStatus JobLogStatus { get; set; }
    }
}
