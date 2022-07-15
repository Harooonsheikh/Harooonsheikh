
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// This controller don't need to be inherited from base controller as it doesn't require any reference or instance of the Magento APIs.
    /// </summary>
    public class CustomerController : BaseController, ICustomerController
    {

        #region Data Members

        /// <summary>
        /// List used to add customer fields.
        /// </summary>
        List<string> CustomerRequiredFields = new List<string>();

        /// <summary>
        /// List used to add Address fields.
        /// </summary>
        List<string> AddressRequiredFields = new List<string>();

 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CustomerController()
            : base(false)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetCustomer reads customer CSV from local directory and add customers in EcomcustomerCustomerEntity List.
        /// </summary>
        /// <returns></returns>
        public List<EcomcustomerCustomerEntity> GetCustomer()
        {
            List<EcomcustomerCustomerEntity> customers = new List<EcomcustomerCustomerEntity>();
            string sourceDir = configurationHelper.GetSetting(CUSTOMER.Local_Input_Path);
            string localDir = configurationHelper.GetSetting(CUSTOMER.Local_Output_Path);
            string[] files = Directory.GetFiles(sourceDir);
            byte[] FileByte = { };
            string LocalFolder;
            foreach (var file in files)
            {
                LocalFolder = FileHelper.MoveFileToLocalFolder(file, string.Empty);
                //Transaction to be logged here.
                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                FileByte = obj.GetCSVFromLocalFolder(LocalFolder);
                obj.LogTransaction(3, "Customer CSV moved to Local Folder Successfully", DateTime.UtcNow, FileByte);
            }

            string[] localFiles = Directory.GetFiles(localDir);

            foreach (var lFile in localFiles)
            {
                List<EcomcustomerCustomerEntity> custs = GetCustomer(lFile);
                if (custs != null && custs.Count > 0)
                {
                    customers.AddRange(custs);
                }
                FileHelper.MoveFileToLocalFolder(lFile, "Processed");
            }

            return customers;
        }

        /// <summary>
        /// GetCustomer gets customers from CSV files and further processed them.
        /// </summary>
        /// <param name="lFile"></param>
        /// <returns></returns>
        public List<EcomcustomerCustomerEntity> GetCustomer(string lFile)
        {
            List<EcomcustomerCustomerEntity> customers = new List<EcomcustomerCustomerEntity>();
            try
            {
                if (FileHelper.CheckFileAvailability(lFile))
                {
                }
            }
            catch (Exception)
            {
                throw;
            }
            return customers;
        }

        private object ConvertToObject(DataColumnCollection columns, DataRow row)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// UpdateCustomer updates customer and also creates CSV for Customer Update.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(List<EcomcustomerCustomerEntity> customer)
        {
            CustomerRequiredFields.Add("customer_id");
            CustomerRequiredFields.Add("email");
            CustomerRequiredFields.Add("dob");
            CustomerRequiredFields.Add("firstname");
            CustomerRequiredFields.Add("gender");
            CustomerRequiredFields.Add("group_id");
            CustomerRequiredFields.Add("lastname");
            CustomerRequiredFields.Add("middlename");
            CustomerRequiredFields.Add("prefix");
            CustomerRequiredFields.Add("suffix");
            CustomerRequiredFields.Add("taxvat");
            CustomerRequiredFields.Add("password");
            CustomerRequiredFields.Add("mob");

            try
            {
                foreach (var p in customer)
                {
                   // ConvertCustomer(p);
                }
                PropertyInfo[] propsheader = customer.First().GetType().GetProperties().ToArray();
                string fileNameCustomer = Path.Combine(configurationHelper.GetSetting(CUSTOMER.Local_Output_Path), "Updated Customer-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");

            }
            catch (Exception exp) //TODO See if we need to throw exception from here
            {
                CustomLogger.LogException(exp);
                throw;
            }
        }

        /// <summary>
        /// UpdateAddress updates address and also creates CSV for address Update.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateAddress(List<EcomcustomerAddressEntityItem> customer)
        {
            AddressRequiredFields.Add("customer_id"); AddressRequiredFields.Add("address_id");
            AddressRequiredFields.Add("city"); AddressRequiredFields.Add("company");
            AddressRequiredFields.Add("country_id"); AddressRequiredFields.Add("fax");
            AddressRequiredFields.Add("firstname"); AddressRequiredFields.Add("lastname");
            AddressRequiredFields.Add("middlename"); AddressRequiredFields.Add("postcode");
            AddressRequiredFields.Add("prefix"); AddressRequiredFields.Add("region");
            AddressRequiredFields.Add("street"); AddressRequiredFields.Add("suffix");
            AddressRequiredFields.Add("telephone"); AddressRequiredFields.Add("is_default_billing");
            AddressRequiredFields.Add("is_default_shipping"); AddressRequiredFields.Add("_edgeaxintegrationkey");

            try
            {
                PropertyInfo[] propsheader = customer.First().GetType().GetProperties().ToArray();
                string fileNameAddress = Path.Combine(configurationHelper.GetSetting(CUSTOMER.Address_Local_Output_Path), "Updated Address-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");
                List<EcomcustomerAddressEntityItem> distinctAddresses = customer.Distinct().ToList();
                foreach (var A in distinctAddresses)
                {
                    // csvAddress.Add(ConvertAddress(A));
                }
            }
            catch (Exception exp)//TODO See if we need to throw exception from here
            {
                CustomLogger.LogException(exp);
                throw;
            }
        }
        public void UpdateCustomerViaXML(List<EcomcustomerCustomerEntity> customer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
