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
    
    public partial class WorkFlowState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkFlowState()
        {
            this.WorkFlowTransition = new HashSet<WorkFlowTransition>();
        }
    
        public int WorkFlowStateID { get; set; }
        public int WorkFlowID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrderSequence { get; set; }
    
        public virtual WorkFlow WorkFlow { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkFlowTransition> WorkFlowTransition { get; set; }
    }
}
