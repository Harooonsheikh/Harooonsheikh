using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace VSI.EDGEAXConnector.DashboardApi.Common
{
    public class StoreHelper
    {
        public static bool InsertStoreInitialData(int storeId)
        {
            try
            {
                string storeInsertQuery = GetStoreSQLScriptFile();
                storeInsertQuery = storeInsertQuery.Replace("{", "{{");
                storeInsertQuery = storeInsertQuery.Replace("}", "}}");
                storeInsertQuery = storeInsertQuery.Replace("{{0}}", "{0}");
                storeInsertQuery = string.Format(storeInsertQuery, storeId);
              
                ExecuteStoreScript(storeInsertQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public static void ExecuteStoreScript(string storeInsertQuery)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = storeInsertQuery;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetStoreSQLScriptFile()
        {
            string storeInsertQuery = "";
            try
            {
                storeInsertQuery = Properties.Resources.Store_Script.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return storeInsertQuery;
        }
        public static string GetConnectionString()
        {
            string connectionString = "";
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["storeConnection"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return connectionString;
        }
    }
}