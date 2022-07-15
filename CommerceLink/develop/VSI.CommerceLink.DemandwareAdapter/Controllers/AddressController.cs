using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.CommerceLink.DemandwareAdapter.DataModels;
using VSI.CommerceLink.MagentoAPI.MageAPI;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
{

    /// <summary>
    /// AddressController Performs Address related activities on DW side.
    /// </summary>
    public class AddressController : BaseController, IAddressController
    {
        #region Data Members
        /// <summary>
        /// CustomerDAL object used to get customer related attributes from database.
        /// </summary>
        CustomerDAL dal = null;
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AddressController(string storyKey)
            : base(true, storyKey)
        {
            CustomerDAL dal = new CustomerDAL(currentStore.StoreKey);

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// SyncAddress sync Addresses in Magento side.
        /// </summary>
        /// <param name="addresses"></param>
        public void SyncAddress(List<EcomcustomerAddressEntityItem> addresses)
        {
            if (addresses != null && addresses.Count > 0)
            {
                CustomLogger.LogTraceInfo("Web Service URL = " + base.Service.Url, currentStore.StoreId, currentStore.CreatedBy);

                foreach (var address in addresses)
                {
                    string strAddress = GetAddressInString(address);

                    try
                    {
                        if (address != null)
                        {
                            int id;
                            if (address.customer_address_id > 0)
                            {
                                try
                                {
                                    id = UpdateAddress(address);
                                    CustomLogger.LogTraceInfo("Address Updated => " + strAddress, currentStore.StoreId, currentStore.CreatedBy);
                                }
                                catch (System.Web.Services.Protocols.SoapHeaderException)
                                {
                                    CustomLogger.LogTraceInfo("Update Address Failed. Address not found in Magento. Address Info here => " + strAddress, currentStore.StoreId, currentStore.CreatedBy);
                                }
                            }
                            else
                            {
                                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

                                id = CreateAddress(address);
                                integrationManager.CreateIntegrationKey(Entities.CustomerAddress, address.edgeaxintegrationkey.ToString(), id.ToString(), address.customer_id.ToString());
                                CustomLogger.LogTraceInfo("Address Created => " + strAddress, currentStore.StoreId, currentStore.CreatedBy);
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        CustomLogger.LogTraceInfo("Address Failed => " + strAddress, currentStore.StoreId, currentStore.CreatedBy);
                        CustomLogger.LogException(exx, currentStore.StoreId, currentStore.CreatedBy);
                    }
                }
            }
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// CreateAddress creates Address in Magento side.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private int CreateAddress(EcomcustomerAddressEntityItem address)
        {
            int id = 0;
            if (address != null)
            {
                int customerId = address.customer_id;
                customerAddressEntityCreate add = ConvertToEcomAddress(address);
                if (add != null)
                {
                    id = base.Service.customerAddressCreate(base.SessionId, customerId, add);
                }
            }
            return id;
        }

        /// <summary>
        /// UpdateAddress Updates Address in Magento side.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private int UpdateAddress(EcomcustomerAddressEntityItem address)
        {
            int id = 0;
            if (address != null)
            {
                int addressId = address.customer_address_id;
                customerAddressEntityCreate add = ConvertToEcomAddress(address);
                if (add != null)
                {
                    bool result = base.Service.customerAddressUpdate(base.SessionId, addressId, add);
                    if (result)
                    {
                        id = address.customer_address_id;
                    }
                }
            }
            return id;

        }

        /// <summary>
        /// ConvertToEcomAddress maps Address to Ecom Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private customerAddressEntityCreate ConvertToEcomAddress(EcomcustomerAddressEntityItem address)
        {
            customerAddressEntityCreate add = new customerAddressEntityCreate();
            add.company = address.company;
            add.firstname = address.firstname;
            add.middlename = address.middlename;
            add.lastname = address.lastname;
            add.connector_updated_at = DateTime.UtcNow.AddHours(configurationHelper.GetSetting(ECOM.TimeZone_Difference_InHours).IntValue()).ToString("yyyy-MM-dd hh:mm:ss");
            add.street = new string[] { address.street };
            add.telephone = address.telephone;
            add.fax = address.fax;
            add.postcode = address.postcode;
            add.city = address.city;
            add.region = address.region;
            DataTable dt = dal.GetState(address.region.ToUpper());

            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                add.region_id = Convert.ToInt32(dt.Rows[0]["Id"]);
                add.region_idSpecified = true;
            }

            add.country_id = address.country_id;

            add.is_default_billing = address.is_default_billing;
            add.is_default_billingSpecified = address.is_default_billing;
            add.is_default_shipping = address.is_default_shipping;
            add.is_default_shippingSpecified = address.is_default_shipping;
            return add;
        }

        /// <summary>
        /// This function returns Customer and Address info in string.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string GetAddressInString(EcomcustomerAddressEntityItem address)
        {
            StringBuilder add = new StringBuilder();
            add.Append(string.Format("Customer Id [{0}] <=> ERP Id : [{1}] <=> EcomKey : [{2}]", address.customer_id, address.edgeaxintegrationkey, address.customer_address_id) + Environment.NewLine);
            add.Append(string.Format("First Name : [{0}] <=> Last Name [{1}]", address.firstname, address.lastname) + Environment.NewLine);
            add.Append(string.Format("Street : [{0}] Zip : [{1}] City : [{2}]  State : [{3}]  Country : [{4}]", address.street, address.postcode, address.city, address.region, address.country_id) + Environment.NewLine);
            return add.ToString();
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
        /// CustomerCSVToEcommerceAddress maps address id and customer id from CSV object to Ecom object. 
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
