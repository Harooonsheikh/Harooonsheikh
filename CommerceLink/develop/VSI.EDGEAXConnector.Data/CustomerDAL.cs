using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class CustomerDAL : BaseClass
    {
        public CustomerDAL(string storeKey) : base(storeKey)
        {

        }

        public static string IntegDBConnString = ConfigurationManager.ConnectionStrings["IntegrationDBEntities"].ConnectionString;
        public DataTable GetAllAddressIdsByActiveAddressIdVSI(long Id)
        {
            DataTable Ids = new DataTable();

            using (SqlConnection connection = new SqlConnection(IntegDBConnString))
            {
                String sql = "[dbo].[GetAllAddressIdsByActiveAddressIdVSI]";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Add the input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@Id";
                    parameter.SqlDbType = SqlDbType.BigInt;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = Id;
                    command.Parameters.Add(parameter);
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

        public DataTable GetUpdatedCustomers(DateTime StartDateTime, string CustGroup)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                String sql = "[dbo].[GetUpdatedCustomersVSI]";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Add the input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@Timestamp";
                    parameter.SqlDbType = SqlDbType.DateTime;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = StartDateTime;
                    command.Parameters.Add(parameter);

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@CustGroup";
                    parameter2.SqlDbType = SqlDbType.NVarChar;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter2.Value = CustGroup;
                    // Add the parameter to the Parameters collection. 
                    command.Parameters.Add(parameter2);

                    try
                    {
                        connection.Open();
                        SqlDataReader dr = command.ExecuteReader();

                        dt.Load(dr);
                        // process or return dt or dr

                    }

                    catch (Exception ex)
                    {
                        CustomLogger.LogException(ex, StoreId, UserId);
                    }

                }


            }

            return dt;
        }

        public DataTable GetUpdatedAddress(DateTime StartDateTime, string CustGroup)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                String sql = "[dbo].[GetCustomersByUpdatedAddressVSI]";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Add the input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@Timestamp";
                    parameter.SqlDbType = SqlDbType.DateTime;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = StartDateTime;
                    command.Parameters.Add(parameter);

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@CustGroup";
                    parameter2.SqlDbType = SqlDbType.NVarChar;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter2.Value = CustGroup;
                    // Add the parameter to the Parameters collection. 
                    command.Parameters.Add(parameter2);

                    try
                    {
                        connection.Open();
                        SqlDataReader dr = command.ExecuteReader();

                        dt.Load(dr);
                        // process or return dt or dr

                    }

                    catch (Exception ex)
                    {
                        CustomLogger.LogException(ex, StoreId, UserId);
                    }

                }


            }

            return dt;
        }
        public DataTable GetAddressKeys(DataTable dt)
        {
            DataTable Keys = new DataTable();

            using (SqlConnection connection = new SqlConnection(IntegDBConnString))
            {
                String sql = "[dbo].[GetAddressKey]";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@AddressIds", SqlDbType.Structured);
                    parameter.Value = dt;
                    parameter.TypeName = "dbo.TVP_AddressIds";
                    command.Parameters.Add(parameter);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        Keys.Load(reader);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return Keys;
        }

        public DataTable GetAddressKeys(long erpKey)
        {
            DataTable Keys = new DataTable();
            if (erpKey > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("AddressId");
                DataRow dr = dt.NewRow();
                dr[0] = erpKey;
                dt.Rows.Add(dr);
                Keys = GetAddressKeys(dt);
            }


            return Keys;
        }

        public DataTable GetState(string code, string countryCode = "USA")
        {
            DataTable State = new DataTable();

            using (SqlConnection connection = new SqlConnection(IntegDBConnString))
            {
                String sql = "[dbo].[GetState]";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameterCode = new SqlParameter("@Code", SqlDbType.NVarChar);
                    parameterCode.Value = code;
                    command.Parameters.Add(parameterCode);
                    var parameterCountryCode = new SqlParameter("@CountryCode", SqlDbType.NVarChar);
                    parameterCountryCode.Value = countryCode;
                    command.Parameters.Add(parameterCountryCode);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        State.Load(reader);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return State;
        }
    }
}
