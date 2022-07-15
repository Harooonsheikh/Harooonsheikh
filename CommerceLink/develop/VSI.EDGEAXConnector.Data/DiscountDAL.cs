using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data
{
 public class DiscountDAL : BaseClass
    {
        

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["IntegrationDBEntities"].ConnectionString;//public static string ConnectionString = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.OnlineStoreDBConnectionString;
        public static string IntegDBConnString = ConfigurationManager.ConnectionStrings["IntegrationDBEntities"].ConnectionString; //public static string IntegDBConnString = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.ConnectorDBConnectionString;
        public DiscountDAL(string storeKey) : base(storeKey)
        {

        }
        public DataTable GetItemsAgaintOfferID()
         {
             DataTable Ids = new DataTable();
             using (SqlConnection connection = new SqlConnection(ConnectionString))
             {
                 String sql = "[dbo].[GETITEMSAGAINSTOFFERID]";
                 using (SqlCommand command = new SqlCommand(sql, connection))
                 {
                     command.CommandType = CommandType.StoredProcedure;
                     try
                     {
                         connection.Open();
                         SqlDataReader reader = command.ExecuteReader();
                         Ids.Load(reader);
                     }
                     catch (Exception)
                     {
                         throw;
                     }
                 }
             }
             return Ids;
         }
    }
}
