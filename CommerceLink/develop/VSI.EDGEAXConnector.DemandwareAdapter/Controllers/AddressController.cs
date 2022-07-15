using System.Collections.Generic;
using System.Data;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Demandware.Customer;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
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
        CustomerDAL dal = new CustomerDAL(StoreService.StoreLkey);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AddressController()
            : base(true)
        {

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
                //CustomLogger.LogTraceInfo("Web Service URL = " + base.Service.Url);

                //foreach (var address in addresses)
                //{
                //    string strAddress = GetAddressInString(address);

                //    try
                //    {
                //        if (address != null)
                //        {
                //            int id;
                //            if (address.customer_address_id > 0)
                //            {
                //                try
                //                {
                //                    id = UpdateAddress(address);
                //                    CustomLogger.LogTraceInfo("Address Updated => " + strAddress);
                //                }
                //                catch (System.Web.Services.Protocols.SoapHeaderException)
                //                {
                //                    // we will not create new address on Magento in case of exception
                //                    //if (sopex.Message.Contains("Address not exists"))
                //                    //{
                //                    //    id = CreateAddress(address);
                //                    //    IntegrationManager.CreateIntegrationKey(Entities.CustomerAddress, address.edgeaxintegrationkey.ToString(), id.ToString(), address.customer_id.ToString());
                //                    //    CustomLogger.LogTraceInfo("Update Address Failed but Created sucessfully => " + strAddress);
                //                    //}
                //                    //else

                //                    CustomLogger.LogTraceInfo("Update Address Failed. Address not found in Magento. Address Info here => " + strAddress);
                //                }

                //            }
                //            else
                //            {
                //                id = CreateAddress(address);
                //                IntegrationManager.CreateIntegrationKey(Entities.CustomerAddress, address.edgeaxintegrationkey.ToString(), id.ToString(), address.customer_id.ToString());
                //                CustomLogger.LogTraceInfo("Address Created => " + strAddress);
                //            }
                //        }
                //    }
                //    catch (Exception exx)
                //    {
                //        CustomLogger.LogTraceInfo("Address Failed => " + strAddress);
                //        CustomLogger.LogException(exx);
                //    }
                //}
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
           
            return id;

        }

        /// <summary>
        /// ConvertToEcomAddress maps Address to Ecom Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private void ConvertToEcomAddress(EcomcustomerAddressEntityItem address)
        {
            complexTypeAddress add = new complexTypeAddress();
            add.companyname = address.company;
            add.firstname = address.firstname;
            add.secondname = address.middlename;
            add.lastname = address.lastname;
            add.address1 = address.street;
            add.phone = address.telephone;
            add.postalcode = address.postcode;
            add.city = address.city;
            add.statecode = address.region;
            DataTable dt = dal.GetState(address.region.ToUpper());

            add.countrycode = address.country_id;
        }
        #endregion

    }
}
