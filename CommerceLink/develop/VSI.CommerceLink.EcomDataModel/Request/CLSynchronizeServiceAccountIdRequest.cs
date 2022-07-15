using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class CLSynchronizeServiceAccountIdRequest
    {
        public bool SynchronizeAll { get; set; }
        public List<int> StoreIds { get; set; }
    }
}
