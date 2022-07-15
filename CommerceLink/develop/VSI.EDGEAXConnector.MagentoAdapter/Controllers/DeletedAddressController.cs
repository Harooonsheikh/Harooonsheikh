using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.MagentoAdapter.DataModels;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{
    /// <summary>
    /// This controller don't need to be inherited from base controller as it doesn't require any reference or instance of the Magento APIs.
    /// </summary>
    public class DeletedAddressController : BaseController, IDeletedAddressController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeletedAddressController(string storeKey) : base(false, storeKey)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This funtion returns Ids of deleted addresses,
        /// </summary>
        /// <param name="lFile"></param>
        /// <returns></returns>
        public List<int> GetDeletedAddressIds(string lFile)
        {
            List<int> Ids = new List<int>();
            try
            {
                DataTable dt = null;
                if (FileHelper.CheckFileAvailability(lFile))
                {
                    dt =  DtFromFile(lFile);
                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            try
                            {
                                var c = ConvertToObject(dt.Columns, row);
                                EcomcustomerAddressEntityItem add = CustomerCSVToEcommerceAddress(c);
                                if (!Ids.Contains(add.customer_address_id))
                                {
                                    Ids.Add(add.customer_address_id);
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return Ids;
        }

        #endregion

        #region Private Methods

        private DataTable DtFromFile(string lFile)
        {
            var parser = new CsvParser(new StreamReader(lFile));
            DataTable dt = new DataTable();
            while (true)
            {
                var row = parser.Read();
                if (row == null)
                {

                    break;
                }
                else
                {
                    dt.LoadDataRow(row.ToArray(), false);
                }
            }
            return dt;
        }

        /// <summary>
        /// This function maps DataColumnCollection attributes to CustomerCSV attributes.
        /// </summary>
        /// <param name="p_dcc"></param>
        /// <param name="p_dr"></param>
        /// <returns></returns>
        private CustomerCSV ConvertToObject(DataColumnCollection p_dcc, DataRow p_dr)
        {
            CustomerCSV p_object = new CustomerCSV();
            Type t = p_object.GetType();

            for (Int32 i = 0; i <= p_dcc.Count - 1; i++)
            {
                try
                {
                    t.InvokeMember(p_dcc[i].ColumnName, BindingFlags.SetProperty, null, p_object, new object[] { p_dr[p_dcc[i].ColumnName] });
                }
                catch (Exception ex)
                {
                    if (ex.ToString() != null)
                    { }
                }
            }

            return p_object;
        }

        /// <summary>
        /// CustomerCSVToEcommerceAddress maps Customer CSV object fields to Ecom Address object fields.
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private EcomcustomerAddressEntityItem CustomerCSVToEcommerceAddress(CustomerCSV csv)
        {
            if (string.IsNullOrEmpty(csv._address_id) || string.IsNullOrEmpty(csv.customer_id))
            {
                return null;
            }

            EcomcustomerAddressEntityItem ecomAddres = new EcomcustomerAddressEntityItem();
            ecomAddres.customer_address_id = int.Parse(csv._address_id);
            ecomAddres.customer_id = int.Parse(csv.customer_id);
            return ecomAddres;
        }


        #endregion

    }
}
