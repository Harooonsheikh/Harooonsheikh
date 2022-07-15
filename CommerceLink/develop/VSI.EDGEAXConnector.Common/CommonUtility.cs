using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EDGEAXConnector.Common
{
    public class CommonUtility
    {
        public static bool StringToBoolean(string val)
        {
            int i = StringToInt(val);
            bool result = false;
            result = i > 0;
            return result;
        }

        public static int StringToInt(string val)
        {
            int result = 0;

            int.TryParse(val, out result);
            return result;
        }
     
        public static string GetExceptionInfo(Exception ex)
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

        public static string GetMonthName(string monthNo)
        {
            string month = string.Empty;
            int no;
            if (int.TryParse(monthNo, out no))
            {
                switch (no)
                {
                    case 1:
                        month = "January";
                        break;
                    case 2:
                        month = "February";
                        break;
                    case 3:
                        month = "March";
                        break;
                    case 4:
                        month = "April";
                        break;
                    case 5:
                        month = "May";
                        break;
                    case 6:
                        month = "June";
                        break;
                    case 7:
                        month = "July";
                        break;
                    case 8:
                        month = "August";
                        break;
                    case 9:
                        month = "September";
                        break;
                    case 10:
                        month = "October";
                        break;
                    case 11:
                        month = "November";
                        break;
                    case 12:
                        month = "December";
                        break;

                }
            }
            else
            {
                month = monthNo;
            }

            return month;
        }
        public static int GetMonthNo(string month)
        {
            int no = 0;
            if (!int.TryParse(month, out no))
            {
                switch (month.ToLower())
                {
                    case "january":
                        no = 1;
                        break;
                    case "february":
                        no = 2;
                        break;
                    case "march":
                        no = 3;
                        break;
                    case "april":
                        no = 4;
                        break;
                    case "may":
                        no = 5;
                        break;
                    case "june":
                        no = 6;
                        break;
                    case "july":
                        no = 7;
                        break;
                    case "august":
                        no = 8;
                        break;
                    case "september":
                        no = 9;
                        break;
                    case "october":
                        no = 10;
                        break;
                    case "november":
                        no = 11;
                        break;
                    case "december":
                        no = 12;
                        break;
                }
            }

            return no;
        }

        public static string CurrentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();

        /// <summary>
        /// Checks if provided string is a valid XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static bool IsValidXML(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Convert object to XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ConvertToXmlString<T>(T request)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, request);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Convert XML to target object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ConvertXMLToObject<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            T targetObject = (T)serializer.Deserialize(memStream);
            return targetObject;
        }

    }
}
