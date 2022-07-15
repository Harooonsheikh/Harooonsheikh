using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Common
{

    public static class CommerceLinkLogger
    {
        public static string GetMessage(CommerceLinkLoggerMessages messageCode)
        {
            ResourceManager resourceManager = new ResourceManager("VSI.EDGEAXConnector.Common.CLMessages", Assembly.GetExecutingAssembly());
            string message = resourceManager.GetString(messageCode.ToString());
            return message;
        }
        [Trace]
        public static string LogTrace(CommerceLinkLoggerMessages messageCode, StoreDto storeDto, params object[] args)
        {
            string message;
            message = string.Format(CultureInfo.InvariantCulture, CommerceLinkLogger.GetMessage(messageCode), args);
            CustomLogger.LogTraceInfo("[" + messageCode.ToString() + "]: " + message, storeDto.StoreId, storeDto.CreatedBy);
            return message;
        }
        [Trace]
        public static string LogWarning(CommerceLinkLoggerMessages messageCode, StoreDto storeDto, params object[] args)
        {
            string message;
            message = string.Format(CultureInfo.InvariantCulture, CommerceLinkLogger.GetMessage(messageCode), args);
            CustomLogger.LogWarn("[" + messageCode.ToString() + "]: " + message, storeDto.StoreId, storeDto.CreatedBy);
            return message;
        }
        [Trace]
        public static string LogFatal(CommerceLinkLoggerMessages messageCode, StoreDto storeDto, params object[] args)
        {
            string message;
            message = string.Format(CultureInfo.InvariantCulture, CommerceLinkLogger.GetMessage(messageCode), args);
            CustomLogger.LogFatal("[" + messageCode.ToString() + "]: " + message, storeDto.StoreId, storeDto.CreatedBy);
            return message;
        }
        [Trace]
        public static string LogException(StoreDto storeDto, Exception ex, string methodName, string guid, string innerExceptionMessage = "")
        {
            string message = "";
            if (string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
            }
            bool.TryParse(Convert.ToString(ConfigurationManager.AppSettings["DisableCustomMessage"]),
                out bool disableCustomMessage);

            if (disableCustomMessage)
            {
                message = string.Format(GetMessage(CommerceLinkLoggerMessages.VSICL40000), methodName,
                    ex.Message + " | " + GetInnerMostErrorMessage(ex), guid);
            }
            else
            {
                message = string.Format(CultureInfo.InvariantCulture, GetMessage(CommerceLinkLoggerMessages.VSICL400017), guid);
            }

            innerExceptionMessage = GetInnerMostErrorMessage(ex);
            CustomLogger.LogException(ex, storeDto.StoreId, string.Empty, guid, ex.Message, innerExceptionMessage);
            return message;
        }
        [Trace]
        public static string LogException(int storeId, string createdBy, string exception, string guid = "")
        {
            if (string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
            }

            string message = string.Format(CultureInfo.InvariantCulture,
                CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL400017), guid);
            CustomLogger.LogException(exception, storeId, createdBy, guid);
            return message;
        }
        [Trace]
        public static string LogDebug(CommerceLinkLoggerMessages messageCode, StoreDto storeDto, params object[] args)
        {
            string message;
            message = string.Format(CultureInfo.InvariantCulture, CommerceLinkLogger.GetMessage(messageCode), args);
            CustomLogger.LogDebugInfo("[" + messageCode.ToString() + "]: " + message, storeDto.StoreId, storeDto.CreatedBy);
            return message;
        }
        [Trace]
        public static string LogTraceCSV(CommerceLinkLoggerMessages messageCode, params object[] args)
        {
            string message = GetMessage(messageCode);
            string requestId = args[0].ToString();
            string actionName = args[1].ToString();
            DateTime dateTime = DateTime.UtcNow;
            if (args.Count() > 2)
            {
                dateTime = (DateTime)args[2];
            }
            CustomLogger.LogTraceInfoCSV(message, requestId, actionName, dateTime);
            return message;
        }
        [Trace]
        public static string LogTraceCSV(CommerceLinkLoggerMessages messageCode, StoreDto storeDto, params object[] args)
        {
            string message = CommerceLinkLogger.GetMessage(messageCode);
            string requestId = args[0].ToString();
            string actionName = args[1].ToString();
            DateTime dateTime = DateTime.UtcNow;
            if (args.Count() > 2)
            {
                dateTime = (DateTime)args[2];
            }
            CustomLogger.LogTraceInfoCSV(message, requestId, actionName, dateTime);
            return message;
        }
        [Trace]
        public static string LogSyncTrace(string message)
        {
            CustomLogger.LogSyncTraceCSV(message);
            return message;
        }
        public static RequestDetailsVM GetErrorDetails(string errorId)
        {
            var loggingDal = new LoggingDAL();
            return loggingDal.GetErrorDetails(errorId);
        }
        [Trace]
        public static void LogTimeTraceCSV(CommerceLinkLoggerMessages messageCode, params object[] args)
        {
            string message = CommerceLinkLogger.GetMessage(messageCode);
            string requestId = args[0].ToString();
            string actionName = args[1].ToString();
            double timeTaken = 0;
            if (args.Count() > 2)
            {
                timeTaken = Convert.ToDouble(args[2]);
            }

            CustomLogger.LogTraceTimeInfoCSV(message, requestId, actionName, timeTaken);
        }
        [Trace]
        public static void LogTimeTraceCSV(CommerceLinkLoggerMessages messageCode, StoreDto storeDto, params object[] args)
        {
            string message = CommerceLinkLogger.GetMessage(messageCode);
            string requestId = args[0].ToString();
            string actionName = args[1].ToString();
            double timeTaken = 0;
            if (args.Count() > 2)
            {
                timeTaken = Convert.ToDouble(args[2]);
            }
            CustomLogger.LogTraceTimeInfoCSV(message, requestId, actionName, timeTaken);

        }
        [Trace]
        public static string GetInnerMostErrorMessage(Exception e)
        {
            Exception exp = e;
            string innerError = e.Message;
            while (exp != null)
            {
                innerError = exp.Message;
                exp = exp.InnerException;
            }

            return innerError;
        }
    }
}
