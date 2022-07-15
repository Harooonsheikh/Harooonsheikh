using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.DTO
{
    public class ApplicationSettingDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int StoreId { get; set; }
        public bool IsActive { get; set; }
    }
}
