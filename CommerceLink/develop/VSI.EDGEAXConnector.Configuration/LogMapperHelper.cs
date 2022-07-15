using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.Configuration
{
    public sealed class LogMapperHelper
    {
        private static List<LogMapper> LogMapperSettings = new List<LogMapper>();

        public static bool IsLogginEnabled { get; private set; }

        public static LogMapper GetLogSettings(string methodName)
        {
            return LogMapperSettings.FirstOrDefault(x => x.MethodName.Equals(methodName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static void LoadLogMappers()
        {
            LoggingDAL loggingDAL = new LoggingDAL();
            LogMapperSettings = loggingDAL.GetAllLogMappers();
        }

        public static void SetIsLogginEnabled(string isLogginEnabled)
        {
            if (isLogginEnabled != null && isLogginEnabled.Trim().ToUpper().Equals("TRUE"))
            {
                IsLogginEnabled = true;
            }
            else
            {
                IsLogginEnabled = false;
            }
        }
    }
}
