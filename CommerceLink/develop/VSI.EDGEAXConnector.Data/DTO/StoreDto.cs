using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.DTO
{
    public class StoreDto
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string StoreKey { get; set; }
        public string CreatedBy { get; set; }
        public string RetailChannelId { get; set; }
    }
}
