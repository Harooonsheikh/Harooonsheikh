using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class RequestDetailsVM
    {
        public string RequestID { get; set; }
        public RequestResponse RequestResponseDetails { get; set; }

        public Log Log { get; set; }

    }
}
