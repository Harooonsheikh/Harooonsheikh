using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public partial class TransactionLogs
    {
        public long TransactionId { get; set; }
        public Nullable<int> EntityId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public byte[] SupportingFileConents { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedByUser { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<int> ModifiedByUser { get; set; }
    }
}
