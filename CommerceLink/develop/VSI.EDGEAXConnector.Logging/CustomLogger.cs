using Common.Logging;
using NLog;
using System;
using System.Globalization;
using System.Text;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Logging
{
    public class CustomLogger
    {
        #region Logger Initialization
        private static string _loggerName = "VSICommerceLink";
        private static ILog _commonLogger = null;
        private static ILog _externalTimeLogger = null;
        private static ILog _traceLogger = null;
        private static string _requestResponseLoggerName = "RequestResponseLogger";
        private static ILog _requestResponseLogger = null;
        public int StoreId;
        public string UserId;
        public static ILog CommonLogger
        {
            get
            {
                if (_commonLogger == null) _commonLogger = GetLogger();
                return _commonLogger;

            }
            set { _commonLogger = value; }
        }
        public static ILog TraceLogger
        {
            get
            {
                if (_traceLogger == null) _traceLogger = GetTraceLogger();
                return _traceLogger;

            }
            set { _traceLogger = value; }
        }
        public static ILog ExternalTimeLogger
        {
            get
            {
                if (_externalTimeLogger == null) _externalTimeLogger = GetExternalTimeLogger();
                return _externalTimeLogger;

            }
            set { _externalTimeLogger = value; }
        }
        public CustomLogger(LoggerContext context)
        {
            this.StoreId = context.StoreId;
            this.UserId = context.UserId;
        }
        public CustomLogger()
        {
        }
        private static Common.Logging.ILog GetLogger()
        {
            return Common.Logging.LogManager.GetLogger(_loggerName);
        }
        private static Common.Logging.ILog GetTraceLogger()
        {
            return Common.Logging.LogManager.GetLogger("TraceLogger");
        }
        private static Common.Logging.ILog GetExternalTimeLogger()
        {
            return Common.Logging.LogManager.GetLogger("ExternalTimeLogger");
        }
        public static ILog RequestResponseLogger
        {
            get
            {
                if (_requestResponseLogger == null) _requestResponseLogger = GetRequestResponseLogger();
                return _requestResponseLogger;

            }
            set { _requestResponseLogger = value; }
        }
        private static Common.Logging.ILog GetRequestResponseLogger()
        {
            return Common.Logging.LogManager.GetLogger(_requestResponseLoggerName);
        }
        #endregion Logger Initialization

        #region Logger Methods
        public static void LogException(Exception ex, int storeId, string createdBy, string callerController = "")
        {
            if (CommonLogger.IsFatalEnabled)
            {
                string exInfo = string.Format("Controller: {0}, {1}", callerController, GetExceptionInfo(ex));
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Fatal, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = callerController;
                theEvent.Level = NLog.LogLevel.Fatal;
                theEvent.Message = exInfo;
                CommonLogger.Fatal(theEvent);
            }
        }
        public static void LogException(Exception ex, int storeId, string createdBy, string label, string identityId = "")
        {
            if (CommonLogger.IsFatalEnabled)
            {
                string exInfo = GetExceptionInfo(ex);
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Fatal, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Level = NLog.LogLevel.Fatal;
                theEvent.Message = exInfo;
                CommonLogger.Fatal(theEvent);
            }
        }
        public static void LogTraceInfoCSV(string message, string requestId, string actionName, DateTime dateTime)
        {
            if (CommonLogger.IsTraceEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Trace, _loggerName, "");
                theEvent.Properties["Message"] = message;
                theEvent.Properties["RequestId"] = requestId;
                theEvent.Properties["ActionName"] = actionName;
                theEvent.Properties["Date"] = dateTime;
                theEvent.Properties["Time"] = dateTime;
                theEvent.Level = NLog.LogLevel.Trace;
                CommonLogger.Trace(theEvent);

            }
        }
        public static void LogTraceTimeInfoCSV(string message, string requestId, string actionName, double timeTaken)
        {
            if (ExternalTimeLogger.IsTraceEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Debug, "ExternalTimeLogger", "");
                theEvent.Properties["Message"] = message;
                theEvent.Properties["RequestId"] = requestId;
                theEvent.Properties["ActionName"] = actionName;
                theEvent.Properties["Date"] = DateTime.UtcNow;
                theEvent.Properties["Time"] = timeTaken.ToString();
                theEvent.Level = NLog.LogLevel.Trace;
                ExternalTimeLogger.Trace(theEvent);

            }
        }
        public static void LogSyncTrace(int StoreId, long JobId, string jobName, string action, string details)
        {
            if (TraceLogger.IsTraceEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Trace, _loggerName, "");
                theEvent.Properties["StoreId"] = StoreId;
                theEvent.Properties["JobId"] = JobId;
                theEvent.Properties["JobName"] = jobName;
                theEvent.Properties["Action"] = action;
                theEvent.Properties["Details"] = details;
                theEvent.Properties["Time"] = DateTime.UtcNow;
                theEvent.Level = NLog.LogLevel.Trace;
                TraceLogger.Trace(theEvent);

            }
        }
        public static void LogException(string exInfo, int storeId, string createdBy, string identityId = "", string errorMessage = "", string errorInnerMessage = "")
        {
            if (CommonLogger.IsFatalEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Fatal, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Properties["error-message"] = errorMessage;
                theEvent.Properties["inner-error-message"] = errorInnerMessage;

                theEvent.Level = NLog.LogLevel.Fatal;
                theEvent.Message = exInfo;
                CommonLogger.Fatal(theEvent);
            }
        }
        
        public static void LogException(Exception ex, int storeId, string createdBy, string identityId, string errorMessage, string errorInnerMessage)
        {
            if (CommonLogger.IsFatalEnabled)
            {
                string exInfo = GetExceptionInfo(ex);
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Fatal, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Properties["error-message"] = errorMessage;
                theEvent.Properties["inner-error-message"] = errorInnerMessage;

                theEvent.Level = NLog.LogLevel.Fatal;
                theEvent.Message = exInfo;
                CommonLogger.Fatal(theEvent);
            }
        }

        public static void LogDebugInfo(string traceInfo, int storeId, string createdBy, string identityId = "")
        {
            if (CommonLogger.IsDebugEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Debug, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Level = NLog.LogLevel.Debug;
                theEvent.Message = string.Format("{0} - Debug", traceInfo);
                CommonLogger.Debug(theEvent);

            }

        }
        public static void LogDebugInfo(object traceObject, int storeId, string createdBy, string identityId = "")
        {
            if (CommonLogger.IsDebugEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Debug, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Level = NLog.LogLevel.Debug;
                theEvent.Message = string.Format("{0} - Debug", traceObject);
                CommonLogger.Debug(theEvent);
            }
        }
        public static void LogTraceInfo(string traceInfo, int storeId, string createdBy, string identityId = "")
        {
            //if (CommonLogger.IsTraceEnabled)
            //{
            //    LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Trace, _loggerName, "");
            //    theEvent.Properties["StoreId"] = storeId;
            //    theEvent.Properties["CreatedBy"] = createdBy;
            //    theEvent.Properties["IdentityId"] = identityId;
            //    theEvent.Level = NLog.LogLevel.Trace;
            //    theEvent.Message = string.Format("{0} - Trace", traceInfo);
            //    CommonLogger.Trace(theEvent);

            //}
        }
        public static void LogWarn(string traceInfo, int storeId, string createdBy, string identityId = "")
        {
            if (CommonLogger.IsFatalEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Warn, _loggerName, "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Level = NLog.LogLevel.Warn;
                theEvent.Message = string.Format("{0} - Warn", traceInfo);
                CommonLogger.Fatal(theEvent);
            }
        }
        public static void LogFatal(string traceInfo, int storeId, string createdBy, string identityId = "")
        {
            if (CommonLogger.IsFatalEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Warn, "", "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["IdentityId"] = identityId;
                theEvent.Level = NLog.LogLevel.Warn;
                theEvent.Message = string.Format("{0} - Warn", traceInfo);
                CommonLogger.Fatal(theEvent);
            }
        }
        public static void LogRequestResponse(string methodName, DataDirectionType dataDirectionId,  string dataPacket,DateTime createdOn, int storeId, string createdBy, string description , 
            string ecomTransactionId , string requestInitiatedIP,string outputPacket , DateTime outputSentAt,string identifierKey, 
            string identifierValue , int isSuccess, decimal totalProcessingDuration )
        {
            if (RequestResponseLogger.IsInfoEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Info, "", "");
                theEvent.Properties["StoreId"] = storeId;
                theEvent.Properties["DataDirectionId"] = (int)dataDirectionId;
                theEvent.Properties["EcomTransactionId"] = ecomTransactionId;
                theEvent.Properties["MethodName"] = methodName;
                theEvent.Properties["Description"] = description;
                theEvent.Properties["DataPacket"] = dataPacket;
                theEvent.Properties["CreatedOn"] = createdOn.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff", CultureInfo.InvariantCulture);
                theEvent.Properties["CreatedBy"] = createdBy;
                theEvent.Properties["RequestInitiatedIP"] = requestInitiatedIP;
                theEvent.Properties["OutputPacket"] = outputPacket;
                theEvent.Properties["OutputSentAt"] = outputSentAt.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff", CultureInfo.InvariantCulture);
                theEvent.Properties["IdentifierKey"] = identifierKey;
                theEvent.Properties["IdentifierValue"] = identifierValue;
                theEvent.Properties["IsSuccess"] = isSuccess;
                theEvent.Properties["TotalProcessingDuration"] = totalProcessingDuration;
                theEvent.Level = NLog.LogLevel.Info;
                RequestResponseLogger.Info(theEvent);
            }
        }


        public static void LogRequestResponse(LoggingContext loggingContext)
        {
            LogRequestResponse(loggingContext.MethodName, loggingContext.DataDirectionId, loggingContext.DataPacket, loggingContext.CreatedOn, loggingContext.StoreId, loggingContext.CreatedBy, loggingContext.Description,
            loggingContext.EcomTransactionId, loggingContext.RequestInitiatedIP, loggingContext.OutputPacket, loggingContext.OutputSentAt, loggingContext.IdentifierKey,
            loggingContext.IdentifierValue, loggingContext.IsSuccess, loggingContext.TotalProcessingDuration);
        }

        private static string GetExceptionInfo(Exception ex)
        {
            Exception e = ex;
            StringBuilder s = new StringBuilder();
            int level = 0;
            s.AppendLine("...................EXCEPTION STARTS.............");
            string tab = "";
            while (e != null)
            {
                if (level > 0)
                {
                    for (int i = 0; i < level; i++)
                    {
                        tab += "    ";
                    }
                    s.AppendLine(tab + "[Inner Exception: " + e.GetType().FullName + "] [Level :" + level.ToString() + "]");

                }
                else
                    s.AppendLine("[Exception: " + e.GetType().FullName + "] [Level :" + level.ToString() + "]");
                s.AppendLine(tab + "[Message: " + e.Message + "]");
                s.AppendLine(tab + "[Source: " + e.Source + "]");
                s.AppendLine(tab + "[HelpLink: " + e.HelpLink + "]");
                s.AppendLine(tab + "[Stacktrace:" + e.StackTrace + "]");
                s.AppendLine(tab + "[TargetSite:" + e.TargetSite + "]");
                level++;
                e = e.InnerException;
            }
            s.AppendLine("...................EXCEPTION ENDS .............");
            return s.ToString();
        }
        #endregion Logger Methods
        public static void LogSyncTraceCSV(string message)
        {
            if (ExternalTimeLogger.IsTraceEnabled)
            {
                LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Debug, "ExternalTimeLogger", message);
                theEvent.Level = NLog.LogLevel.Trace;
                ExternalTimeLogger.Trace(theEvent);

            }
        }
    }
}
