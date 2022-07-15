using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class XMLMap
    {
        public string SourceEntity { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string XML { get; set; }
        public bool isActive { get; set; }
    }
}
