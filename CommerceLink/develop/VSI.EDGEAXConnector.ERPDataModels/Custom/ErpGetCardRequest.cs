using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
   public class ErpGetCardRequest
    {
        public string customerAccount { get; set; }
        public string licenseId { get; set; }
        public List<string> cardProcessors { get; set; }
    }
}
