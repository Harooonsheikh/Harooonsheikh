using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpComment
    {
        public ErpComment()
        {

        }

        public long? RecordId { get; set; }
        public string AuthorStaffId { get; set; }
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
