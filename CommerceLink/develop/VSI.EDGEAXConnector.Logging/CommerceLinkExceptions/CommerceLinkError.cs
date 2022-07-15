using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Logging.CommerceLinkExceptions
{
    [Serializable]
    public class CommerceLinkError : Exception
    {
        public CommerceLinkError()
        {
        }

        public CommerceLinkError(string message)
            : base(message)
        {
        }

        public CommerceLinkError(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
