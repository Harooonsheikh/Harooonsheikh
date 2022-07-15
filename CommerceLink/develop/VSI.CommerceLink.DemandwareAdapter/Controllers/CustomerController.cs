using CsvHelper;
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
using VSI.CommerceLink.DemandwareAdapter.DataModels;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
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

        /// <summary>
        /// List used to add AddressCSV.
        /// </summary>
        List<CustomerCSV2> Addresses = new List<CustomerCSV2>();

        FileHelper fileHelper = null;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CustomerController(string storeKey) : base(false, storeKey)
        {
            fileHelper = new FileHelper(storeKey);
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
            string sourceDir = configurationHelper.GetSetting(CUSTOMER.Remote_File_Path);
            string localDir = configurationHelper.GetSetting(CUSTOMER.Local_Input_Path);
            string[] files = Directory.GetFiles(sourceDir);
            byte[] FileByte = { };
            string LocalFolder;

            foreach (var file in files)
            {
                LocalFolder = fileHelper.MoveFileToLocalFolder(file, string.Empty);
                //Transaction to be logged here.
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
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
                fileHelper.MoveFileToLocalFolder(lFile, "Processed");
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
                DataTable dt = DtFromFile(lFile);

                if (fileHelper.CheckFileAvailability(lFile))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            var c = ConvertToObject(dt.Columns, row);
                            EcomcustomerCustomerEntity customer = customers.Where(k => k.email == c.email).FirstOrDefault();

                            if (customer == null)
                            {
                                EcomcustomerCustomerEntity cust = CustomerCSVToEcommerceCustomer(c);
                                EcomcustomerAddressEntityItem add = CustomerCSVToEcommerceAddress(c);
                                if (add != null)
                                {
                                    cust.addresses.Add(add);
                                }
                                customers.Add(cust);
                            }
                            else
                            {
                                int addressId = 0;
                                if (int.TryParse(c._address_id, out addressId))
                                {
                                    //Uncomment if we want to restrict the creation of 2 addresses in AX 
                                    //while a single address is marked as Default billing and Default Shipping address in Ecom.
                                    EcomcustomerAddressEntityItem add = CustomerCSVToEcommerceAddress(c);
                                    if (add != null)
                                    {
                                        if (!customer.addresses.Any(a => a.customer_address_id == addressId))
                                        {
                                            customer.addresses.Add(add);
                                        }
                                        else // THis is case when same address is marked both as Deault shipping and default billing
                                        {
                                            EcomcustomerAddressEntityItem exiting = customer.addresses.FirstOrDefault(a => a.customer_address_id == addressId);

                                            if (exiting != null && exiting.customer_address_id > 0)
                                            {
                                                if (add.is_default_shipping)
                                                {
                                                    exiting.is_default_shipping = true;
                                                }
                                                if (add.is_default_billing)
                                                {
                                                    exiting.is_default_billing = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            return customers;
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
                List<CustomerCSV2> csvCustomer = new List<CustomerCSV2>();

                foreach (var p in customer)
                {
                    csvCustomer.AddRange(ConvertCustomer(p));
                }

                PropertyInfo[] propsheader = customer.First().GetType().GetProperties().ToArray();
                //string fileNameCustomer = Path.Combine(ConfigurationHelper.CustomerOutputPathLocal, "Updated Customer-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");
                string fileNameCustomer = Path.Combine(configurationHelper.GetSetting(CUSTOMER.Local_Output_Path), "Updated Customer-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");

                //Create CSV for Customer Update
                using (StreamWriter streamWriter = new StreamWriter(fileNameCustomer))
                {
                    using (CsvWriter writer = new CsvWriter(streamWriter))
                    {
                        List<string> headerRow = new List<string>();
                        for (int i = 0; i < CustomerRequiredFields.Count; i++)
                        {
                            headerRow.Add(CustomerRequiredFields[i]);
                        }
                        writer.WriteRecord(headerRow.ToArray());
                        foreach (var p in csvCustomer)
                        {
                            string[] dataRow = GetDataRow(p, CustomerRequiredFields);
                            writer.WriteRecord(dataRow);
                        }
                    } //CSV created for Customer 
                }
                
            }
            catch (Exception exp) //TODO See if we need to throw exception from here
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }
        public void UpdateCustomerViaXML(List<EcomcustomerCustomerEntity> customers)
        {
            try
            {
                foreach (var cus in customers)
                {
                    //TODO: get file name tag from DB
                    string fileNameCustomer = "Updated Customer-" + cus.customer_id + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                    xmlHelper.GenerateXmlUsingTemplate(fileNameCustomer, this.configurationHelper.GetDirectory(configurationHelper.GetSetting(CUSTOMER.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, cus);
                }
            }
            catch (Exception exp) //TODO See if we need to throw exception from here
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
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
                List<CustomerCSV2> csvCustomer = new List<CustomerCSV2>();
                PropertyInfo[] propsheader = customer.First().GetType().GetProperties().ToArray();
                //string fileNameAddress = Path.Combine(ConfigurationHelper.CustomerAddressOutputPathLocal, "Updated Address-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");
                string fileNameAddress = Path.Combine(configurationHelper.GetSetting(CUSTOMER.Address_Local_Output_Path), "Updated Address-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");
                List<CustomerCSV2> csvAddress = new List<CustomerCSV2>();

                List<EcomcustomerAddressEntityItem> distinctAddresses = customer.Distinct().ToList();

                foreach (var A in distinctAddresses)
                {
                    csvAddress.Add(ConvertAddress(A));
                }

                //Create CSV for Address Update
                using (StreamWriter streamWriter = new StreamWriter(fileNameAddress))
                using (CsvWriter writer = new CsvWriter(streamWriter))
                {
                    List<string> headerRow = new List<string>();
                    for (int i = 0; i < AddressRequiredFields.Count; i++)
                    {
                        headerRow.Add(AddressRequiredFields[i]);
                    }
                    writer.WriteRecord(headerRow);
                    foreach (var a in csvAddress)
                    {
                        string[] dataRow = GetDataRowAddress(a, AddressRequiredFields);
                        writer.WriteRecord(dataRow);
                    }
                } //CSV created for Address Update
            }
            catch (Exception exp)//TODO See if we need to throw exception from here
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        #endregion

        #region Private Methods

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
        /// CustomerCSVToEcommerceCustomer maps Customer CSV object fields to Ecom Customer object fields.
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private EcomcustomerCustomerEntity CustomerCSVToEcommerceCustomer(CustomerCSV csv)
        {
            EcomcustomerCustomerEntity ecom = new EcomcustomerCustomerEntity();
            ecom.addresses = new List<EcomcustomerAddressEntityItem>();
            ecom.customer_id = int.Parse(csv.customer_id);
            ecom.email = csv.email;
            ecom.website = csv._website;
            ecom.store = csv._store;
            ecom.confirmation = CommonUtility.StringToBoolean(csv.confirmation);
            ecom.created_at = csv.created_at;
            ecom.created_in = csv.created_in;
            //ecom.disable_auto_group_change = Convert.ToBoolean(csv.confirmation);
            ecom.dob = csv.dob;
            ecom.firstname = csv.firstname;
            ecom.gender = csv.gender;
            ecom.group_id = CommonUtility.StringToInt(csv.group_id);
            ecom.lastname = csv.lastname;
            ecom.middlename = csv.middlename;
            ecom.prefix = csv.prefix;
            ecom.suffix = csv.suffix;
            // ecom.taxvat = csv.taxvat;
            ecom.birthday_month = csv.mob;
            ecom.website_id = CommonUtility.StringToInt(csv.website_id);
            long recId = 0;
            long.TryParse(csv._edgeaxintegrationkey, out recId);
            ecom.edgeaxintegrationkey = recId;
        
            return ecom;
        }

        /// <summary>
        /// CustomerCSVToEcommerceAddress maps Customer CSV object fields to Ecom Address object fields.
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private EcomcustomerAddressEntityItem CustomerCSVToEcommerceAddress(CustomerCSV csv)
        {
            if (string.IsNullOrEmpty(csv._address_id))
            {
                return null;
            }

            EcomcustomerAddressEntityItem ecomAddres = new EcomcustomerAddressEntityItem();
            ecomAddres.customer_address_id = int.Parse(csv._address_id);
            ecomAddres.city = csv._address_city;
            ecomAddres.company = csv._address_company;
            ecomAddres.country_id = csv._address_country_id;
            ecomAddres.fax = csv._address_fax;
            ecomAddres.firstname = csv._address_firstname;
            ecomAddres.lastname = csv._address_lastname;
            ecomAddres.middlename = csv._address_middlename;
            ecomAddres.postcode = csv._address_postcode;
            ecomAddres.prefix = csv._address_prefix;
            ecomAddres.region = csv._address_region;
            ecomAddres.street = csv._address_street;
            ecomAddres.suffix = csv._address_suffix;
            ecomAddres.telephone = csv._address_telephone;
            //ecom.va = csv._address_vat_id;
            ecomAddres.is_default_billing = CommonUtility.StringToBoolean(csv._address_default_billing);
            ecomAddres.is_default_billingSpecified = ecomAddres.is_default_billing;
            ecomAddres.is_default_shipping = CommonUtility.StringToBoolean(csv._address_default_shipping);
            ecomAddres.is_default_shippingSpecified = ecomAddres.is_default_shipping;
            long recId = 0;

            if (long.TryParse(csv._edgeaxintegrationkey, out recId))
            {
                ecomAddres.edgeaxintegrationkey = recId;
            }

            return ecomAddres;
        }

        /// <summary>
        /// ConvertCustomer maps EcomcustomerCustomerEntity object fields to CustomerCSV2 object fields.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private List<CustomerCSV2> ConvertCustomer(EcomcustomerCustomerEntity customer)
        {
            List<CustomerCSV2> customers = new List<CustomerCSV2>();
            var CustomerCSV = new CustomerCSV2();

            CustomerCSV.customer_id = customer.customer_id.ToString();
            CustomerCSV.email = customer.email;
            CustomerCSV.firstname = customer.firstname;
            CustomerCSV.lastname = customer.lastname;
            CustomerCSV.password = customer.password_hash;
            CustomerCSV.group_id = customer.group_id.ToString();
            CustomerCSV.prefix = customer.prefix;
            CustomerCSV.suffix = customer.suffix;
            CustomerCSV.dob = customer.dob;
            // CustomerCSV.taxvat = customer.taxvat;
            CustomerCSV.gender = customer.gender;

            CustomerCSV.middlename = customer.middlename;
            int mob = CommonUtility.GetMonthNo(customer.birthday_month);

            if (mob > 0)
            {
                CustomerCSV.mob = mob.ToString();
            }

            customers.Add(CustomerCSV);
            return customers;
        }

        /// <summary>
        /// ConvertAddress maps EcomcustomerAddressEntityItem object fields to CustomerCSV2 object fields.
        /// </summary>
        /// <param name="CustAddress"></param>
        /// <returns></returns>
        private CustomerCSV2 ConvertAddress(EcomcustomerAddressEntityItem CustAddress)
        {
            try
            {
                string integKey = CustAddress.edgeaxintegrationkey.ToString();
                string addressId = CustAddress.customer_address_id.ToString();
                var AddressCSV = new CustomerCSV2();
                AddressCSV._address_customer_id = CustAddress.customer_id.ToString();
                AddressCSV._address_id = addressId;
                string[] names = CustAddress.firstname.Split(' ');

                if (names.Length > 1)
                {
                    AddressCSV._address_firstname = names[0];
                    if (names.Length > 2)
                    {
                        AddressCSV._address_middlename = names[1];
                        AddressCSV._address_lastname = names[2];
                    }
                    else if (names.Length > 1)
                    {
                        AddressCSV._address_lastname = names[1];
                    }
                    else
                    {
                        AddressCSV._address_lastname = "-";
                    }
                }

                AddressCSV._address_city = CustAddress.city;
                AddressCSV._address_company = CustAddress.company;
                AddressCSV._address_country_id = CustAddress.country_id;
                AddressCSV._address_fax = CustAddress.fax;
                AddressCSV._address_postcode = CustAddress.postcode;
                AddressCSV._address_prefix = CustAddress.prefix;
                AddressCSV._address_region = CustAddress.region;
                AddressCSV._address_street = CustAddress.street.ToString().Replace('\n', ' ');
                AddressCSV._address_suffix = CustAddress.suffix;
                AddressCSV._address_telephone = CustAddress.telephone;

                if (CustAddress.is_default_billing)
                {
                    AddressCSV._address_default_billing_ = "1";
                }
                else
                {
                    AddressCSV._address_default_billing_ = string.Empty;
                }
                if (CustAddress.is_default_shipping)
                {
                    AddressCSV._address_default_shipping_ = "1";
                }
                else
                {
                    AddressCSV._address_default_shipping_ = string.Empty;
                }
                AddressCSV._edgeaxintegrationkey = CustAddress.edgeaxintegrationkey.ToString();
                //Addresses.Add(AddressCSV);
                return AddressCSV;

            }

            catch (Exception)
            {
                throw;
            }

            //return Addresses;
        }

        /// <summary>
        /// GetDataRowAddress returns address in CsvRow.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="AddressRequiredFields"></param>
        /// <returns></returns>
        private string[] GetDataRowAddress(CustomerCSV2 p, List<string> AddressRequiredFields)
        {
            List<string> dataRow = new List<String>();
            string valueString = string.Empty;
            PropertyInfo[] props = p.GetType().GetProperties().ToArray();

            foreach (PropertyInfo prp in props) // Get Attribute Values
            {
                bool has = AddressRequiredFields.Any(cus => cus.Contains(prp.Name));

                if (prp.ToString().Contains("address") || prp.ToString().Contains("edgeaxintegrationkey"))
                {
                    valueString = string.Empty;
                    object value = prp.GetValue(p, new object[] { });

                    if (value != null)
                    {
                        if (value.GetType().Name.Equals("String"))
                        {
                            //object value = prp.GetValue(p, new object[] { });
                            valueString = value.ToString();
                        }
                        else if (value.GetType().Name.Equals("String[]"))
                        {
                            String[] valueArray = (String[])prp.GetValue(p, new object[] { });
                            valueString = string.Join(",", valueArray);
                        }
                        else if (value.GetType().Name.Equals("Int") || GetType().Name.Equals("Decimal"))
                        {
                            valueString = value.ToString();
                        }
                        else if (prp.Name.Equals("CustomAttributes"))
                        {
                            System.Collections.Generic.List<KeyValuePair<string, string>> attributes =
                                (System.Collections.Generic.List<KeyValuePair<string, string>>)value;

                            foreach (var attr in attributes)
                            {
                                dataRow.Add(attr.Value);
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                    dataRow.Add(valueString);
                }
            }
            return dataRow.ToArray();
        }

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
        ///  GetDataRow returns Customer in CsvRow.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="CustomerRequiredFields"></param>
        /// <returns></returns>
        private string[] GetDataRow(CustomerCSV2 p, List<string> CustomerRequiredFields)
        {
            List<string> dataRow = new List<string>();
            string valueString = string.Empty;
            PropertyInfo[] props = p.GetType().GetProperties().ToArray();

            foreach (PropertyInfo prp in props) // Get Attribute Values
            {
                bool has = CustomerRequiredFields.Any(cus => cus.Contains(prp.Name));
                valueString = string.Empty;

                if (has == true)
                {
                    object value = prp.GetValue(p, new object[] { });
                    if (value != null)
                    {
                        if (value.GetType().Name.Equals("String"))
                        {
                            //object value = prp.GetValue(p, new object[] { });
                            valueString = value.ToString();
                        }
                        else if (value.GetType().Name.Equals("String[]"))
                        {
                            String[] valueArray = (String[])prp.GetValue(p, new object[] { });
                            valueString = string.Join(",", valueArray);
                        }
                        else if (value.GetType().Name.Equals("Int") || GetType().Name.Equals("Decimal"))
                        {
                            valueString = value.ToString();
                        }
                        else if (prp.Name.Equals("CustomAttributes"))
                        {
                            System.Collections.Generic.List<KeyValuePair<string, string>> attributes =
                                (System.Collections.Generic.List<KeyValuePair<string, string>>)value;

                            foreach (var attr in attributes)
                            {
                                dataRow.Add(attr.Value);
                            }
                        }
                        else
                        {
                            //valueString = ",";
                        }
                    }
                    else
                    {
                        //valueString = ",";
                    }
                }
                dataRow.Add(valueString);
            }
            return dataRow.ToArray();
        }
        #endregion
    }
}