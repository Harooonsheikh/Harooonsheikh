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
    
    public partial class ApplicationSetting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicationSetting()
        {
            this.FieldValue = new HashSet<FieldValue>();
        }
    
        public int ApplicationSettingId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public bool IsActive { get; set; }
        public int StoreId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> FieldTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUserForDuplicateStore { get; set; }
    
        public virtual FieldType FieldType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FieldValue> FieldValue { get; set; }
    }
}
