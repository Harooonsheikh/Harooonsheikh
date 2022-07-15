using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data
{
    public static class SingleCatalogProductDAL
    {
        public static bool Insert(List<SingleCatalogProductStaging> data, int batchSize)
        {
            String dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
            SqlConnection conn = null;

            int pageSize = batchSize; // TODO MAKE THIS CONFIGURABLE
            for (int pageNum = 0; (pageNum * pageSize) < data.Count; pageNum++)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ProductRecId", typeof(string));
                dataTable.Columns.Add("SKU", typeof(string));
                dataTable.Columns.Add("StoreViewCode", typeof(string));
                dataTable.Columns.Add("ProductJson", typeof(string));

                List<SingleCatalogProductStaging> productsToSave = data.Skip(pageNum * pageSize).Take(pageSize).ToList();

                foreach (SingleCatalogProductStaging x in productsToSave)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["ProductRecId"] = x.ProductRecId;
                    dataRow["SKU"] = x.SKU;
                    dataRow["StoreViewCode"] = x.StoreViewCode;
                    dataRow["ProductJson"] = x.ProductJson;
                    dataTable.Rows.Add(dataRow);
                }

                SqlParameter param = new SqlParameter
                {
                    ParameterName = "Products",
                    SqlDbType = SqlDbType.Structured,
                    Value = dataTable,
                    Direction = ParameterDirection.Input
                };

                using (conn = new SqlConnection(dbConnStr))
                {
                    SqlCommand sqlCmd = new SqlCommand("dbo.spSingleCatalogProductStaging_Insert");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandTimeout = 300000;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(param);
                    sqlCmd.ExecuteNonQuery();
                }
            }

            return true;
        }

        public static bool InsertWithBulkCopy(List<SingleCatalogProductStaging> data)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ProductRecId", typeof(string));
            dataTable.Columns.Add("SKU", typeof(string));
            dataTable.Columns.Add("StoreViewCode", typeof(string));
            dataTable.Columns.Add("ProductJson", typeof(string));
            dataTable.Columns.Add("CreatedOn", typeof(DateTime));
            dataTable.Columns.Add("LastModifiedOn", typeof(DateTime));

            foreach (var x in data)
            {
                DataRow dataRow = dataTable.NewRow();

                dataRow["ProductRecId"] = x.ProductRecId;
                dataRow["SKU"] = x.SKU;
                dataRow["StoreViewCode"] = x.StoreViewCode;
                dataRow["ProductJson"] = x.ProductJson;
                dataRow["CreatedOn"] = DateTime.UtcNow;
                dataRow["LastModifiedOn"] = DBNull.Value;
                dataTable.Rows.Add(dataRow);
            }

            SqlParameter param = new SqlParameter();
            param.ParameterName = "Products";
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dataTable;
            param.Direction = ParameterDirection.Input;

            String dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
            SqlConnection conn = null;

            using (conn = new SqlConnection(dbConnStr))
            {
                SqlBulkCopy objbulk = new SqlBulkCopy(conn);
                objbulk.DestinationTableName = "SingleCatalogProductStaging";
                objbulk.ColumnMappings.Add("ProductRecId", "ProductRecId");
                objbulk.ColumnMappings.Add("SKU", "SKU");
                objbulk.ColumnMappings.Add("StoreViewCode", "StoreViewCode");
                objbulk.ColumnMappings.Add("ProductJson", "ProductJson");
                objbulk.ColumnMappings.Add("CreatedOn", "CreatedOn");
                objbulk.ColumnMappings.Add("LastModifiedOn", "LastModifiedOn");
                objbulk.BatchSize = 1000;
                conn.Open();
                objbulk.WriteToServer(dataTable);
            }

            return true;
        }

        public static bool ClearStagingTable()
        {
            String dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
            SqlConnection conn = null;

            using (conn = new SqlConnection(dbConnStr))
            {
                SqlCommand sqlCmd = new SqlCommand();
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "TRUNCATE TABLE [dbo].[SingleCatalogProductStaging];";
                sqlCmd.CommandType = CommandType.Text;

                sqlCmd.ExecuteNonQuery();
            }

            return true;
        }

        public static List<SingleCatalogProductUpdatedData> GetDataFromDeltaTable()
        {
            DataSet updatedProducts = new DataSet();
            String dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
            SqlConnection conn = null;

            using (conn = new SqlConnection(dbConnStr))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT [ProductRecId], [SKU], [StoreViewCode], [RowStatus] FROM [dbo].[SingleCatalogProductDelta]");
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandTimeout = 600000;
                sqlCmd.CommandType = CommandType.Text;

                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(updatedProducts);
            }

            List<SingleCatalogProductUpdatedData> updatedProductList = new List<SingleCatalogProductUpdatedData>();

            if (updatedProducts.Tables.Count > 0)
            {
                foreach (DataRow dr in updatedProducts.Tables[0].Rows)
                {
                    SingleCatalogProductUpdatedData product = new SingleCatalogProductUpdatedData();
                    product.ProductRecId = dr["ProductRecId"].ToString();
                    product.RecordId = Convert.ToInt64(product.ProductRecId);
                    product.SKU = dr["SKU"].ToString();
                    product.StoreViewCode = dr["StoreViewCode"].ToString();
                    product.RowStatus = dr["RowStatus"].ToString();

                    updatedProductList.Add(product);
                }
            }

            return updatedProductList;
        }

        public static bool ProcessAndPopulateDeltaTable()
        {
            DataSet updatedProducts = new DataSet();
            String dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
            SqlConnection conn = null;

            using (conn = new SqlConnection(dbConnStr))
            {
                SqlCommand sqlCmd = new SqlCommand("dbo.spSingleCatalogProduct_CompareAndUpdate");
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandTimeout = 600000;
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.ExecuteNonQuery();
            }

            return true;
        }
    }
    
    public class SingleCatalogProductUpdatedData
    {
        public long RecordId { get; set; }
        public string ProductRecId { get; set; }
        public string SKU { get; set; }
        public string StoreViewCode { get; set; }
        public string RowStatus { get; set; }
    }
}
