using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using VSI.EDGEAXConnector.Logging;
using System.Data.SqlClient;

namespace VSI.EDGEAXConnector.Data
{
    public class CSVFileManager : BaseClass
    {

        public CSVFileManager(string storeKey) : base(storeKey)
        {

        }
        public static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDBEntities"].ConnectionString;
        public static int CreateCSVFile(string fileName, int totalRecords)
        {
            int Id = 0;
            try
            {
               
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    String sp = "dbo.CreateCSVFile";
                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Add the input parameter and set its properties.
                        SqlParameter parameter = new SqlParameter();
                        parameter.ParameterName = "@FileName";
                        parameter.SqlDbType = SqlDbType.NVarChar;
                        parameter.Direction = ParameterDirection.Input;
                        parameter.Value = fileName;
                        command.Parameters.Add(parameter);

                        SqlParameter parameter2 = new SqlParameter();
                        parameter2.ParameterName = "@TotalRecords";
                        parameter2.SqlDbType = SqlDbType.Int;
                        parameter2.Direction = ParameterDirection.Input;
                        parameter2.Value = totalRecords;
                        // Add the parameter to the Parameters collection. 
                        command.Parameters.Add(parameter2);

                        try
                        {
                            connection.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            if (dr.Read()) // you only have one row so you can use "if" instead of "while"
                            {
                                Id = dr.GetInt32(0);
                            }
                        }

                        catch (Exception ex)
                        {
                           // customLogger.LogException(ex);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Id;
        }
        public static int CreateCSVFileData(int fileId, string[] lines)
        {
            int records = 0;
            try
            {
                string timestamp = DateTime.UtcNow.ToString(@"yyyy-MM-dd hh:mm:ss");
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("CSVFileId");
                dt.Columns.Add("Record");
                dt.Columns.Add("Status");
                dt.Columns.Add("Timestamp");

                foreach (string item in lines)
                {
                    dt.Rows.Add("", fileId, item, "0", timestamp);
                }
                using (SqlConnection dbConnection = new SqlConnection(ConnectionString))
                {
                    dbConnection.Open();
                    using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                    {
                        s.DestinationTableName = "CSVFileData";
                        foreach (var column in dt.Columns)
                            s.ColumnMappings.Add(column.ToString(), column.ToString());
                        s.WriteToServer(dt);
                    }
                }
                records = lines.Length;
            }
            catch (Exception)
            {

                throw;
            }
            return records;
        }
        public static string ReadCSVFile(string fileId, int pageNo, int pageSize )
        {
            string records = string.Empty;
            try
            {
                DataTable table = new DataTable();
                using (SqlConnection dbConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ReadCSVFile", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@FileId";
                    parameter.SqlDbType = SqlDbType.Int;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = fileId;
                    command.Parameters.Add(parameter);

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@PageNo";
                    parameter2.SqlDbType = SqlDbType.Int;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter2.Value = pageNo;
                    command.Parameters.Add(parameter2);

                    SqlParameter parameter3 = new SqlParameter();
                    parameter3.ParameterName = "@RecordsPerPage";
                    parameter3.SqlDbType = SqlDbType.Int;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter3.Value = pageSize;
                    command.Parameters.Add(parameter3);


                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        dbConnection.Open();
                        adapter.Fill(table);
                    }

                    catch (Exception ex)
                    {
                      //  customLogger.LogException(ex);
                    }
                }
                if(table != null && table.Rows!= null && table.Rows.Count> 0)
                {
                    var lines = table.AsEnumerable().Select(row => String.Join(",", row.ItemArray) + ";");
                    records = String.Join(Environment.NewLine, lines);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            return records;
        }
    }
}
