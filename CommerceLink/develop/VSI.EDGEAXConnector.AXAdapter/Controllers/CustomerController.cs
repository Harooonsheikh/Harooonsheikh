using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// CustomerController class performs Customer related activities.
    /// </summary>
    public class CustomerController : BaseController, ICustomerController
    {
        //CommerceRuntime runtime;
        //ChannelState currentChannelState;

        #region Properties

        /// <summary>
        /// TransactionLogging object records all transaction in database.
        /// </summary>

        /// <summary>
        /// currentCustomerEmail save current customer email address.
        /// </summary>
        string currentCustomerEmail = string.Empty;

        /// <summary>
        /// CustomerDAL object used to access customer database.
        /// </summary>
        CustomerDAL objDAL = null;

        /// <summary>
        /// DateTime object saves Now date.
        /// </summary>
        DateTime TimeStamp = DateTime.UtcNow;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CustomerController(string storeKey) : base(storeKey)
        {
            objDAL = new CustomerDAL(storeKey);
        }

        #endregion

        #region Public Methods

        /// [MB] - TV - BR 3.0 - 15007 - Start
        /// <summary>
        /// Method used to add default extension properties in ERP customer object if they does not already exist
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private ErpCustomer addDefaultExtensionProperties(ErpCustomer customer)
        {
            customer.ExtensionProperties = customer.ExtensionProperties ?? new List<ErpCommerceProperty>();
            

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "Fax")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("Fax", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "RelationshipType")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("RelationshipType", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "DeliveryTerms")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("DeliveryTerms", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "ModeOfDelivery")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("ModeOfDelivery", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "Warehouse")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("Warehouse", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "IsExternallyMaintained")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("IsExternallyMaintained", new ErpCommercePropertyValue { BooleanValue = false }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "Note")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("Note", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "TermsOfPayment")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("TermsOfPayment", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "Site")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("Site", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "Category")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("Category", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "NumberOfEmployees")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("NumberOfEmployees", new ErpCommercePropertyValue { IntegerValue = 0 }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "Agent")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("Agent", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "CustomerIdOld")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("CustomerIdOld", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "AccountIdGUID")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("AccountIdGUID", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "ReturnMessage")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("ReturnMessage", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }
            
            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "LocalTaxId")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("LocalTaxId", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }
            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key == "TMVAdditionalVAT")))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("TMVAdditionalVAT", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }
            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key.Trim().ToUpper().Equals("TMVSOURCESYSTEM"))))
            {
                // Set default value as WEB as ECOM team will not pass this extension property, CRM system will pass CRM value in this field as per defined Enum
                customer.ExtensionProperties.Add(new ErpCommerceProperty("TMVSourceSystem", new ErpCommercePropertyValue { StringValue = SourceSystem.WEB.ToString() }));
            }
            if (!customer.ExtensionProperties.Contains(customer.ExtensionProperties.FirstOrDefault(p => p.Key.Trim().ToUpper().Equals("TMVCUSTOMERPORTALACCESS"))))
            {
                customer.ExtensionProperties.Add(new ErpCommerceProperty("TMVCustomerPortalAccess", new ErpCommercePropertyValue { StringValue = String.Empty }));
            }

            return customer;
        }
        /// [MB] - TV - BR 3.0 - 15007 - End

        /// <summary>
        /// AssignCustomer creates customer in AX, normally call it from sales order controller.
        /// </summary>
        /// <param name="customer"></param>
        public ErpCustomer AssignCustomer(ErpCustomer customer, string requestId, bool isCreateCustomer = true)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCustomer customers = new ErpCustomer();

            customer = this.addDefaultExtensionProperties(customer);

            string sourceSystem = customer.ExtensionProperties.FirstOrDefault(p => p.Key.ToUpper() == "TMVSOURCESYSTEM")?.Value.StringValue ?? "";

            if (!string.IsNullOrEmpty(customer.EcomCustomerId))
            {
                IntegrationKey customerKey = null;
                var customerController = new CustomerController(currentStore.StoreKey);
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                // Get Customer Key from Integation DB to check if customer already exists or not

                if (sourceSystem.ToUpper() == SourceSystem.EDI.ToString())
                {
                    customerKey = integrationManager.GetKeyByContainsDescription(Entities.Customer, customer.Name);
                }
                else
                {
                    customerKey = integrationManager.GetErpKey(Entities.Customer, customer.EcomCustomerId);
                }

                if (customerKey == null) // New Customer
                {
                    if (isCreateCustomer)
                    { 
                        // Create Customer
                        customers = customerController.CreateNewCustomer(customer, true, requestId);
                        // Get Customer Key from Integration DB
                        customerKey = integrationManager.GetErpKey(Entities.Customer, customer.EcomCustomerId);
                    }
                    else
                    {
                        string customerNotFoundErrorMessage = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40301, currentStore, customer.EcomCustomerId);
                        throw new CommerceLinkError(customerNotFoundErrorMessage);
                    }
                }
                else
                {
                    //commented below lines as recreating customer will duplicate customer address which is not correct
                    //// Update customer key in customer object
                    //customer.AccountNumber = customerKey.Description;

                    //// Create Customer
                    //customers = customerController.CreateNewCustomer(customer, false);

                    if (!string.IsNullOrWhiteSpace(customerKey.Description))
                    {
                        string accountNumber = string.Empty;
                        if (sourceSystem.ToUpper() == SourceSystem.EDI.ToString())
                        {
                            accountNumber = customerKey.Description.Split(':')[0];
                        }
                        else
                        {
                            accountNumber = customerKey.Description;
                        }

                        customers = GetCustomerData(accountNumber, 2);
                    }
                }
                return customers;
            }

            string message = String.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40301), customer.EcomCustomerId);
            CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, message);
            // REMOVE THIS LINE // CustomLogger.LogException(new Exception(message));
            throw new CommerceLinkError(message);
        }

        /// <summary>
        /// CreateCustomer creates customer in AX.
        /// </summary>
        /// <param name="obj"></param>
        public void CreateCustomer(List<ERPDataModels.ErpCustomer> obj)
        {
            CreateCustomer(obj, true);
        }

        /// <summary>
        /// CreateCustomer creates customer in AX.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="CreateIntegrationKey"></param>
        public void CreateCustomer(List<ERPDataModels.ErpCustomer> obj, bool CreateIntegrationKey)
        {
            try
            {
                StringBuilder customerExpceptions = new StringBuilder();
                
                // Set EntityName of each Customer object to "Customer" and EntityName of each Address to "Address"
                obj.ForEach(cus =>
                {
                    cus.EntityName = "Customer";
                    cus.Addresses.ToList().ForEach(ad =>
                    {
                        ad.EntityName = "Address";
                    });
                });

                // Foreach Customer
                foreach (var ec in obj)
                {
                    try
                    {
                        ErpCustomer customer = ConvertToCustomer(ec); //Mapper.Map<ErpCustomer, Customer>(ec);
                        ErpCustomer customer2 = null; // customer2 is used to create a log
                        List<ErpAddress> adds = ec.Addresses.ToList();
                        //NS:
                        var customerCRTManager = new CustomerCRTManager();

                        if (string.IsNullOrEmpty(customer.TaxGroup))
                        {
                            customer.TaxGroup = configurationHelper.GetSetting(APPLICATION.ERP_Customer_Default_TaxGroup);
                        }

                        //NS: if customer group is available user it otherwise get it from integration DB
                        if (string.IsNullOrEmpty(customer.CustomerGroup))
                        {
                            customer.CustomerGroup = configurationHelper.GetSetting(APPLICATION.ERP_Default_Customer_Group);
                        }
                        customer.OrganizationId = Common.CommonUtility.GetMonthName(ec.SLBirthMonth);// ec.SLBirthMonth;
                        if (string.IsNullOrEmpty(customer.CurrencyCode))
                        {
                            customer.CurrencyCode = configurationHelper.GetSetting(CUSTOMER.Default_CurrencyCode);
                        }
                        if (string.IsNullOrEmpty(customer.Language))
                        {
                            customer.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
                        }
                        customer2 = customerCRTManager.CreateCustomer(customer, configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), currentStore.StoreKey, string.Empty);
                        if (CreateIntegrationKey)
                        {
                            try
                            {
                                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                                integrationManager.CreateIntegrationKey(Entities.Customer, customer2.RecordId.ToString(), ec.EcomCustomerId, customer2.AccountNumber);
                            }
                            catch (Exception exp)
                            {
                                // CustomLogger.LogTraceInfo("Integration Key generation failed in Create Customer Mode" + Environment.NewLine + exp.Message);
                                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40302, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                                throw;
                            }
                        }
                        //} // END AQ: End of If

                    }
                    catch (Exception innerException)
                    {
                        string recordsInfo = "Email:" + ec.Email + " ->First Name:" + ec.FirstName + " ->Last Name:" + ec.LastName + Environment.NewLine;
                        string exInfo = Common.CommonUtility.GetExceptionInfo(innerException);
                        customerExpceptions.Append(recordsInfo + exInfo);
                        //throw;
                    }

                    currentCustomerEmail = ec.Email;
                    //if (CreateIntegrationKey) //++TODUsman
                    //{
                    //    Transactionobj.LogTransaction(3, "Customer: " + currentCustomerEmail + " has been created successfully", DateTime.UtcNow, null);
                    //}
                    //else
                    //{
                    //    Transactionobj.LogTransaction(3, "Customer: " + currentCustomerEmail + " has been updated successfully", DateTime.UtcNow, null);
                    //}
                }

                if (customerExpceptions.Length > 10)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, customerExpceptions);
                    // REMOVE THIS LINE // throw new Exception(customerExpceptions.ToString());
                    throw new Exception(message);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// CreateCustomer creates customer in AX.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="CreateIntegrationKey"></param>
        /// <returns></returns>
        public ERPDataModels.ErpCustomer CreateNewCustomer(ERPDataModels.ErpCustomer obj, bool CreateIntegrationKey, string requestId)
        {
            ErpCustomer customerToReturn = new ErpCustomer();
            try
            {
                StringBuilder customerExpceptions = new StringBuilder();
                

                // Set EntityName of each Customer object to "Customer" and EntityName of each Address to "Address"                
                obj.EntityName = "Customer";

                if (obj.Addresses != null)
                {
                    obj.Addresses.ToList().ForEach(ad =>
                    {
                        ad.EntityName = "Address";
                    });
                }

                // Foreach Customer
                ErpCustomer ec;
                ec = obj;
                try
                {
                    ErpCustomer customer = ConvertToCustomer(ec); //Mapper.Map<ErpCustomer, Customer>(ec);
                    ErpCustomer customer2 = null; // customer2 is used to create a log

                    List<ErpAddress> adds = ec.Addresses.ToList();
                    var customerCRTManager = new CustomerCRTManager();
                    if (string.IsNullOrEmpty(customer.TaxGroup))
                    {
                        customer.TaxGroup = configurationHelper.GetSetting(APPLICATION.ERP_Customer_Default_TaxGroup);
                    }

                    //NS: if customer group is available user it otherwise get it from integration DB
                    if (string.IsNullOrWhiteSpace(customer.CustomerGroup))
                    {
                        customer.CustomerGroup = configurationHelper.GetSetting(APPLICATION.ERP_Default_Customer_Group);
                    }
                    customer.OrganizationId = Common.CommonUtility.GetMonthName(ec.SLBirthMonth);
                    if (string.IsNullOrWhiteSpace(customer.CurrencyCode))
                    {
                        customer.CurrencyCode = configurationHelper.GetSetting(CUSTOMER.Default_CurrencyCode);
                    }
                    if (string.IsNullOrWhiteSpace(customer.Language))
                    {
                        customer.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
                    }
                    customer2 = customerCRTManager.CreateCustomer(customer, configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), currentStore.StoreKey, requestId);
                    if (CreateIntegrationKey)
                    {
                        try
                        {
                            var isEcomIdString = configurationHelper.GetSetting(CUSTOMER.Is_Ecom_Id_String);
                            bool isEcomCustomerString = true;
                            if (!string.IsNullOrWhiteSpace(isEcomIdString))
                            {
                                isEcomCustomerString = bool.Parse(isEcomIdString);
                            }
                            IntegrationManager integrationManager = new IntegrationManager(this.currentStore.StoreKey);

                            string desc = customer2.AccountNumber;
                            string sourceSystem = customer.ExtensionProperties.FirstOrDefault(p => p.Key == "TMVSOURCESYSTEM")?.Value.StringValue ?? "";

                            if (sourceSystem.ToUpper() == SourceSystem.EDI.ToString())
                            {
                                desc += ":" + customer2.Name.Trim();
                            }
                            integrationManager.CreateIntegrationKey(Entities.Customer, customer2.RecordId.ToString(), ec.EcomCustomerId, desc, isEcomCustomerString);
                        }
                        catch (Exception exp)
                        {
                            // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Integration Key generation failed in Create Customer Mode" + Environment.NewLine + exp.Message);
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40302, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                            throw;
                        }
                    }
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
                    return customer2;
                }

                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// SaveAddressIntegrationKey updates Intregration Key for Customer Address if exist otherwise creates it.
        /// </summary>
        /// <param name="adds"></param>
        /// <param name="acNumber"></param>
        /// <param name="addrs"></param>
        private void SaveAddressIntegrationKey(List<ErpAddress> adds, string acNumber, ErpAddress addrs)
        {
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            if (addrs.ExpireRecordId > 0)
            {
                var key2 = integrationManager.GetComKey(Entities.CustomerAddress, addrs.ExpireRecordId.ToString());

                if (key2 != null)
                {
                    key2.ErpKey = addrs.RecordId.ToString();
                    integrationManager.UpdateIntegrationKeyAllFields(key2);
                }
                else
                {
                    string ecomKey = GetEcomAddressKey(addrs, adds);
                    if (string.IsNullOrEmpty(ecomKey))
                    {
                        ecomKey = GetEcomAddressKey(addrs.RecordId);
                    }
                    //Ali: warning removed 
                    if (addrs.RecordId > 0)  //  if (addrs.RecordId != null && addrs.RecordId > 0)
                    {
                        if (!string.IsNullOrEmpty(ecomKey))
                            integrationManager.CreateIntegrationKey(Entities.CustomerAddress, addrs.RecordId.ToString(), ecomKey, acNumber);
                        else
                        {
                            //Case: A new address is created in AX but not synched yet. We will not try to create integration key as it will be created automatically when Update from AX to Ecommerce job will execute. Let's wait for it and don't try to create an integration key.
                            // REMOVE THIS LINE // CustomLogger.LogTraceInfo(string.Format("Address [{0}] exists in AX but yet not synched in Ecommerce by AX to Ecomm Update job. So skipping integration key generation for it.", addrs.RecordId));
                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10301, currentStore, addrs.RecordId.ToString());
                        }
                    }

                }
            }
            else
            {
                var key = integrationManager.GetComKey(Entities.CustomerAddress, addrs.RecordId.ToString());

                if (key != null && !string.IsNullOrEmpty(key.ComKey))
                {
                    key.Description = key.Description;
                    key.ModifiedOn = TimeStamp;
                    integrationManager.UpdateIntegrationKey(key);
                }
                else
                {
                    string ecomKey = GetEcomAddressKey(addrs, adds);
                    if (string.IsNullOrEmpty(ecomKey))
                    {
                        ecomKey = GetEcomAddressKey(addrs.RecordId);
                    }
                    if (addrs.RecordId > 0)  // if (addrs.RecordId != null && addrs.RecordId > 0)
                    {
                        if (!string.IsNullOrEmpty(ecomKey))
                            integrationManager.CreateIntegrationKey(Entities.CustomerAddress, addrs.RecordId.ToString(), ecomKey, acNumber);
                        else
                        {
                            //Case: A new address is created in AX but not synched yet. We will not try to create integration key as it will be created automatically when Update from AX to Ecommerce job will execute. Let's wait for it and don't try to create an integration key.
                            // REMOVE THIS LINE // CustomLogger.LogTraceInfo(string.Format("Address [{0}] exists in AX but yet not synched in Ecommerce by AX to Ecomm Update job. So skipping integration key generation for it.", addrs.RecordId));
                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10301, currentStore, addrs.RecordId.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Customer Data
        /// </summary>
        /// <param name="accountNo"></param>
        /// <returns></returns>
        public ErpCustomer GetCustomerData(string accountNo, int searchLocation)
        {
            var customerCRTManager = new CustomerCRTManager();
            ErpCustomer erpCustomer = customerCRTManager.GetCustomerData(accountNo, searchLocation, currentStore.StoreKey);

            if (erpCustomer.CustomerAffiliations.Count == 0)
            {
                erpCustomer.CustomerAffiliations = null;
            }

            return erpCustomer;
        }

        /// <summary>
        /// THis method explains how to use ExtensionProperties attribute of CRT Customer class to get and update custom attributes values of entity.
        /// </summary>
        /// <param name="accountNo"></param>
        public void GetCustomer(string accountNo)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "accountNo: " + accountNo);

            var customerCRTManager = new CustomerCRTManager();
            try
            {
                ErpCustomer customer = customerCRTManager.GetCustomer(accountNo, currentStore.StoreKey);

                customer.FirstName = customer.FirstName + "1";
                customer.FirstName = customer.LastName + "10";
                customerCRTManager.UpdateCustomer(customer, currentStore.StoreKey);
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        public List<ErpCustomer> GetCustomerByLicence(List<string> licenceNumber)
        {
            var customerCRTManager = new CustomerCRTManager();
            List<ErpCustomer> erpCustomers = customerCRTManager.GetCustomerByLicence(licenceNumber, currentStore.StoreKey);

            return erpCustomers;
        }

        public ErpCustomerInfoResponse GetCustomerInfoByInvoiceId(CustomerByInvoiceRequest request)
        {
            var customerCRTManager = new CustomerCRTManager();
            return customerCRTManager.GetCustomerInfoByInvoiceId(request, currentStore.StoreKey);
        }

        public ErpCustomerInoviceDetailResponse GetCustomerInvoiceDetails(CustomerInvoiceDetailRequest request, string requestId)
        {
            var customerCRTManager = new CustomerCRTManager();

            ErpCustomerInoviceDetailResponse customerInvoiceResponse = customerCRTManager.GetCustomerInvoiceDetails(request, currentStore.StoreKey, requestId);

            if (customerInvoiceResponse.CustomerInvoiceDetails != null)
            {
                string retailChannelId = string.Empty;
                if (customerInvoiceResponse.CustomerInvoiceDetails.CustomerInvoiceJour != null)
                {
                    retailChannelId = customerInvoiceResponse.CustomerInvoiceDetails.CustomerInvoiceJour.RetailChannelId;
                }

                string storeKeyByCountryCode = string.IsNullOrWhiteSpace(retailChannelId) ? currentStore.StoreKey : GetStoreKeyByRetailChannelId(retailChannelId);

                foreach (var custJour in customerInvoiceResponse.CustomerInvoiceDetails.CustomerInvoiceTrans)
                {
                    custJour.ItemId = GetEcomItemId(custJour.ItemId, storeKeyByCountryCode);

                    if (custJour.ItemId.Contains(":"))
                    {
                        custJour.ItemId = custJour.ItemId.Replace(':', '_');
                    }
                }

                var erpLanguage = customerInvoiceResponse.CustomerInvoiceDetails.CustomerInvoiceJour.Language;
                var languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);

                var ecomLanguage = languageCodes.GetEcomLanguageCodeForPaymentLink(erpLanguage);
                customerInvoiceResponse.CustomerInvoiceDetails.CustomerInvoiceJour.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;

                var storeCodesDAL = new StoreCodesDAL(storeKeyByCountryCode);
                var ecomSiteCode = storeCodesDAL.GetEcomStoreCodeForPaymentLink(erpLanguage);
                customerInvoiceResponse.CustomerInvoiceDetails.CustomerInvoiceJour.SiteCode = string.IsNullOrWhiteSpace(ecomSiteCode) ? erpLanguage : ecomSiteCode;

                //swap language for customer
                string threeLetterISORegion = string.Empty;

                if (customerInvoiceResponse.CustomerInfo.Addresses.Count > 0)
                {
                    threeLetterISORegion = customerInvoiceResponse.CustomerInfo.Addresses.FirstOrDefault(s => s.IsPrimary)?.ThreeLetterISORegionName;
                }

                storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? currentStore.StoreKey : GetStoreKeyByCountryCode(threeLetterISORegion);
                languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);

                erpLanguage = customerInvoiceResponse.CustomerInfo.Language;
                ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);

                customerInvoiceResponse.CustomerInfo.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;

            }

            return customerInvoiceResponse;
        }

        public ErpCustomerInoviceResponse GetCustomerInvoices(CustomerInvoiceRequest request, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var customerCRTManager = new CustomerCRTManager();

            return customerCRTManager.GetCustomerInvoices(request, currentStore.StoreKey, requestId);

        }

        /// <summary>
        /// GetAllCustomersWithIdwithAccountNumber gets all Customer on basis of key,value pair.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, long> GetAllCustomersWithIdwithAccountNumber()
        {
            try
            {
                Dictionary<string, long> customerDict = new Dictionary<string, long>();

                return customerDict;
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// GetUpdatedCustomers get updated Customers from AX.
        /// </summary>
        public void GetUpdatedCustomers()
        {

        }

        /// <summary>
        /// SaveAddresses save Address in AX.
        /// </summary>
        /// <param name="accountNo"></param>
        /// <param name="adds"></param>
        /// <returns></returns>
        public bool SaveAddresses(string accountNo, List<ErpAddress> adds)
        {
            var customerCRTManager = new CustomerCRTManager();

            bool result = false;
            List<ErpAddress> exitingAddresses = new List<ErpAddress>();// Mapper.Map<List<ErpAddress>, List<Address>>(adds);
            try
            {
                //NS: Remove
                //NS: Comment Start
                //Customer existingCustomer = currentChannelState.CustomerManager.GetCustomer(accountNo);
                //NS: Comment End
                ErpCustomer existingCustomer = customerCRTManager.GetCustomer(accountNo, currentStore.StoreKey);

                if (existingCustomer == null)
                {
                    string message = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40303));
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, message);
                    throw new CommerceLinkError(message);
                }

                if (existingCustomer.Addresses != null && existingCustomer.Addresses.Count > 0)
                {
                    exitingAddresses = existingCustomer.Addresses.ToList();
                }
                StringBuilder newAddresses = new StringBuilder("New Addresses" + Environment.NewLine);
                StringBuilder existAddresses = new StringBuilder("Updated Addresses" + Environment.NewLine);
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                foreach (var erpAdd in adds)
                {
                    if (existingCustomer.Addresses.Count > 0)
                    {
                        IntegrationKey key = null;
                        long recId = erpAdd.RecordId;
                        string ecomAddId = erpAdd.EcomAddressId;

                        DataTable dtIds = objDAL.GetAllAddressIdsByActiveAddressIdVSI(recId);
                        if (dtIds != null && dtIds.Rows != null && dtIds.Rows.Count > 1)
                        {
                            long newRecId = Convert.ToInt64(dtIds.Rows[0]["RECID"]);
                            key = integrationManager.GetComKey(Entities.CustomerAddress, recId.ToString());
                            if (key == null)
                            {
                                if (newRecId != recId)
                                {
                                    key = integrationManager.GetComKey(Entities.CustomerAddress, newRecId.ToString());
                                }

                                if (key == null)
                                {
                                    integrationManager.CreateIntegrationKey(Entities.CustomerAddress, recId.ToString(), ecomAddId, accountNo);
                                }
                            }
                        }
                        if (recId > 0)
                        {
                            key = integrationManager.GetComKey(Entities.CustomerAddress, recId.ToString());
                        }
                        else
                        {

                            key = integrationManager.GetErpKey(Entities.CustomerAddress, ecomAddId);
                        }
                        if (key != null && !string.IsNullOrEmpty(key.ErpKey) && existingCustomer.Addresses != null && existingCustomer.Addresses.Count > 0) // Update Existing Key Timestamp
                        {
                            bool found = false;

                            for (int i = 0; i < exitingAddresses.Count; i++)
                            {
                                if ((exitingAddresses[i].RecordId.ToString() == key.ErpKey.Trim()) || (exitingAddresses[i].ExpireRecordId > 0 && exitingAddresses[i].ExpireRecordId.ToString() == key.ErpKey.Trim()))
                                {
                                    exitingAddresses[i].Name = erpAdd.Name;
                                    exitingAddresses[i].Street = erpAdd.Street;
                                    exitingAddresses[i].City = erpAdd.City;
                                    exitingAddresses[i].State = erpAdd.State;
                                    // DAI-1001

                                    if (Convert.ToBoolean(configurationHelper.GetSetting(APPLICATION.ZipCode_Truncate_Enable)).Equals(true))
                                    {
                                        if (erpAdd.ZipCode.Contains("-"))
                                        {
                                            exitingAddresses[i].ZipCode = erpAdd.ZipCode.Substring(0, erpAdd.ZipCode.IndexOf("-")).Trim();
                                        }
                                        else
                                        {
                                            exitingAddresses[i].ZipCode = erpAdd.ZipCode;
                                        }
                                    }
                                    else
                                    {
                                        exitingAddresses[i].ZipCode = erpAdd.ZipCode;
                                    }
                                    exitingAddresses[i].ThreeLetterISORegionName = erpAdd.ThreeLetterISORegionName;
                                    exitingAddresses[i].Phone = erpAdd.Phone;
                                    exitingAddresses[i].IsPrimary = erpAdd.IsPrimary;
                                    #region DAI-432-866 - Ecomm Default Shipping Address as Primary in AX
                                    exitingAddresses[i].AddressType = (ErpAddressType)configurationHelper.GetSetting(APPLICATION.ERP_Default_Address_Type).IntValue();
                                    exitingAddresses[i].AddressTypeValue = configurationHelper.GetSetting(APPLICATION.ERP_Default_Address_Type).IntValue();

                                    #endregion
                                    found = true;
                                    existAddresses.Append(GetAddressInfo(exitingAddresses[i]));
                                    break;
                                }
                            }
                            if (!found)
                            {
                                ErpAddress add = CreateAddressFromErpAddress(erpAdd);
                                exitingAddresses.Add(add);
                                newAddresses.Append(GetAddressInfo(add));
                            }
                        }
                        else
                        {
                            ErpAddress add = CreateAddressFromErpAddress(erpAdd);
                            exitingAddresses.Add(add);
                            newAddresses.Append(GetAddressInfo(add));
                        }
                    }
                    else
                    {
                        ErpAddress add = CreateAddressFromErpAddress(erpAdd);
                        exitingAddresses.Add(add);
                        newAddresses.Append(GetAddressInfo(add));
                    }
                }
                existingCustomer.Addresses = exitingAddresses;
                CustomLogger.LogTraceInfo(existAddresses.ToString(), currentStore.StoreId, currentStore.CreatedBy);
                CustomLogger.LogTraceInfo(newAddresses.ToString(), currentStore.StoreId, currentStore.CreatedBy);
                var savedCust = customerCRTManager.UpdateCustomer(existingCustomer, currentStore.StoreKey);
                result = true;
                string acNumber = savedCust.AccountNumber;
                foreach (var addrs in savedCust.Addresses)
                {
                    try
                    {
                        SaveAddressIntegrationKey(adds, acNumber, addrs);
                    }
                    catch (Exception exp)
                    {
                        // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Integration Key generation failed in Update/Create Customer Mode" + Environment.NewLine + exp.Message);
                        string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40304);
                        CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, message + JsonConvert.SerializeObject(exp));
                        throw;
                    }
                }
                currentCustomerEmail = existingCustomer.Email;
                //Transactionobj.LogTransaction(3, "Customer: " + currentCustomerEmail + " has been updated successfully", DateTime.UtcNow, null);
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            return result;
        }

        /// <summary>
        /// GetUpdatedCustomersAndAddresses gets updated Customer from ecommerce side.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="CustGroup"></param>
        /// <returns></returns>
        public List<ErpCustomer> GetUpdatedCustomersAndAddresses(DateTime timeStamp, string CustGroup)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            List<ErpCustomer> customerDictionary = new List<ErpCustomer>();
            DataTable dt = objDAL.GetUpdatedCustomers(timeStamp, CustGroup);
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            foreach (DataRow row in dt.Rows)
            {
                string ecomkey = string.Empty;
                string accountNumber = row["ACCOUNTNUMBER"].ToString();
                IntegrationKey key = integrationManager.GetComKey(Entities.Customer, row["CUSTOMERID"].ToString());

                if (key != null) // Update Existing Key Timestamp
                {
                    ecomkey = key.ComKey.ToString();
                    ErpCustomer customer = null;
                    customer = new ErpCustomer();
                    customer.EcomCustomerId = ecomkey;
                    customer.AccountNumber = accountNumber;
                    customer.RecordId = Convert.ToInt64(row["CUSTOMERID"]);
                    customer.Email = row["EMAIL"].ToString();
                    customer.FirstName = row["FIRSTNAME"].ToString();
                    customer.MiddleName = row["MIDDLENAME"].ToString();
                    customer.LastName = row["LASTNAME"].ToString();
                    customer.EntityName = row["Name"].ToString();
                    customer.Phone = row["PHONE"].ToString();
                    customer.PartyNumber = row["PARTYNUMBER"].ToString();
                    customer.CustomerGroup = row["CUSTGROUP"].ToString();
                    customer.TaxGroup = row["TAXGROUP"].ToString();
                    //customer.VatNumber = row["VATNUM"].ToString();
                    customer.SLBirthMonth = row["SLBirthMonth"].ToString();
                    customerDictionary.Add(customer);
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customerDictionary));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return customerDictionary;//.Values.ToList();
        }

        /// <summary>
        /// GetUpdatedAddresses gets updated Address from ecommerce side.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="CustGroup"></param>
        /// <returns></returns>
        public List<ErpAddress> GetUpdatedAddresses(DateTime timeStamp, string CustGroup)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpAddress> AddressList = new List<ErpAddress>();
            DataTable dt = objDAL.GetUpdatedAddress(timeStamp, CustGroup);
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            foreach (DataRow row in dt.Rows)
            {
                string cid = row["CustomerId"].ToString();
                long customerId = 0;
                string EcomCustomerId = string.Empty;
                long.TryParse(cid, out customerId);
                IntegrationKey customerkey = integrationManager.GetComKey(Entities.Customer, cid);

                if (customerkey != null)
                {
                    EcomCustomerId = customerkey.ComKey.ToString();
                    string ecomkey = string.Empty;
                    //var row = row.ItemArray;
                    string EcomAddressId = string.Empty;
                    long erpAddressId = Convert.ToInt64(row["RECID"]);
                    IntegrationKey key = integrationManager.GetComKey(Entities.CustomerAddress, erpAddressId.ToString());

                    if (key != null)
                    {
                        EcomAddressId = key.ComKey.ToString();
                    }
                    else
                    {
                        // Here we will check and try to find if this address is updated on AX side and has some expired record id in our Integration DB
                        //Get All expire Ids of this Address Id from channel DB.
                        DataTable Ids = objDAL.GetAllAddressIdsByActiveAddressIdVSI(erpAddressId);

                        if (Ids != null && Ids.Rows != null && Ids.Rows.Count > 0)
                        {
                            //Get Key from IntegrastionDB if exists for current Id in row.
                            DataTable keys = objDAL.GetAddressKeys(Ids);

                            if (keys != null && keys.Rows != null && keys.Rows.Count > 0)
                            {
                                // Now keys has been found and we will update the crossponding Integration keys with new Address Id.
                                // Ideally one row should come back but still we can expect many rows. So will loop through it
                                foreach (DataRow keyRow in keys.Rows)
                                {
                                    try
                                    {
                                        if (string.IsNullOrEmpty(EcomAddressId))
                                        {
                                            EcomAddressId = Convert.ToString(keyRow["ComKey"]);
                                        }
                                        IntegrationKey iKey = new IntegrationKey()
                                        {
                                            IntegrationKeyId = Convert.ToInt64(keyRow["Id"]),
                                            EntityId = Convert.ToInt16(keyRow["Entity"]),
                                            ErpKey = Convert.ToString(erpAddressId), // Update the ERP Key
                                            ComKey = Convert.ToString(keyRow["ComKey"]),
                                            Description = Convert.ToString(keyRow["Description"]),
                                        };
                                        integrationManager.UpdateIntegrationKeyAllFields(iKey);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }

                            }
                            else
                                EcomAddressId = "0";
                        }
                        else
                            EcomAddressId = "0";
                    }
                    int type = 0;
                    int.TryParse(Convert.ToString(row["ADDRESSTYPE"]), out type);
                    long recId = 0;
                    long.TryParse(row["RECID"].ToString(), out recId);

                    //Check If Integration keys not exists tehn create a new one. Most probably with a valid ERPKey but "0" as EcomKey
                    //Commented
                    /*
                    if(EcomAddressId == "0")
                    {
                        var ik = IntegrationManager.GetComKey(Entities.CustomerAddress, recId.ToString());
                        if(ik == null )
                        {
                            // CReate a New Key with "0" as EcomKe as thsi is a new address in AX and we will updated it's Comkey once we get an update back from Ecom.
                            IntegrationManager.CreateIntegrationKey(Entities.CustomerAddress, recId.ToString(), EcomAddressId, customerkey.Description);
                        }
                        else
                        {
                            EcomAddressId = ik.ComKey;
                        }
                    }*/

                    var address = new ErpAddress()
                    {
                        EcomCustomerId = EcomCustomerId,
                        EcomAddressId = EcomAddressId,
                        RecordId = recId,
                        City = row["CITY"].ToString(),
                        ThreeLetterISORegionName = row["COUNTRYREGIONID"].ToString(),
                        ZipCode = row["ZIPCODE"].ToString(),
                        Street = row["STREET"].ToString(),
                        Name = row["NAME"].ToString(),
                        // AddressType = type > 0 ? (ErpAddressType)type : ErpAddressType.None,
                        StreetNumber = row["STREETNUMBER"].ToString(),
                        Postbox = row["POSTBOX"].ToString(),
                        DistrictName = row["DISTRICTNAME"].ToString(),
                        State = row["STATE"].ToString(),
                        Phone = row["PHONE"].ToString(),
                        Fax = row["FAX"].ToString(),
                        IsPrimary = Convert.ToBoolean(row["ISPRIMARY"]),

                    };
                    AddressList.Add(address);

                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(AddressList));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return AddressList;
        }

        /// <summary>
        /// CreateAddressFromErpAddress creates CRT Address from ERP Address.
        /// </summary>
        /// <param name="erpAddress"></param>
        /// <returns></returns>
        public ErpAddress CreateAddressFromErpAddress(ErpAddress erpAddress)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpAddress add = new ErpAddress();
            add.EntityName = "Address";
            add.RecordId = erpAddress.RecordId;
            add.Name = erpAddress.Name;
            add.Street = erpAddress.Street;
            add.City = erpAddress.City;
            add.State = erpAddress.State;
            // DAI-1001
            if (Convert.ToBoolean(configurationHelper.GetSetting(APPLICATION.ZipCode_Truncate_Enable)).Equals(true))
            {
                if (erpAddress.ZipCode.Contains("-"))
                {
                    add.ZipCode = erpAddress.ZipCode.Substring(0, erpAddress.ZipCode.IndexOf("-")).Trim();
                }
                else
                {
                    add.ZipCode = erpAddress.ZipCode;
                }
            }
            else
            {
                add.ZipCode = erpAddress.ZipCode;
            }
            add.ThreeLetterISORegionName = erpAddress.ThreeLetterISORegionName;
            add.Phone = erpAddress.Phone;
            //add.Phone = erpAddress.Email;
            add.IsPrimary = erpAddress.IsPrimary;
            #region DAI-432-866 - Ecomm Default Shipping Address as Primary in AX
            add.AddressType = (ErpAddressType)configurationHelper.GetSetting(APPLICATION.ERP_Default_Address_Type).IntValue();  //ConfigurationHelper.DefaultAXAddressType;
            add.AddressTypeValue = configurationHelper.GetSetting(APPLICATION.ERP_Default_Address_Type).IntValue();
            #endregion
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(add));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return add;
        }

        /// <summary>
        /// ProcessDeletedAddresses processes deleted Addresses
        /// </summary>
        /// <param name="ecomAddressIds"></param>
        public void ProcessDeletedAddresses(List<int> ecomAddressIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "ecomAddressIds: " + JsonConvert.SerializeObject(ecomAddressIds));

            var customerCRTManager = new CustomerCRTManager();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            if (ecomAddressIds != null && ecomAddressIds.Count > 0)
            {
                try
                {
                    foreach (int ecomId in ecomAddressIds)
                    {
                        // Get Integration Key from Integration DB
                        var key = integrationManager.GetErpKey(Entities.CustomerAddress, ecomId.ToString());

                        if (key != null && !string.IsNullOrEmpty(key.ErpKey) && !string.IsNullOrEmpty(key.Description)) // Description contains Customer Acccount Number
                        {
                            long erpKey = 0;
                            string accountNo = key.Description;

                            if (long.TryParse(key.ErpKey, out erpKey))
                            {
                                //NS: Remove
                                //NS: Comment Start
                                //Customer customer = currentChannelState.CustomerManager.GetCustomer(accountNo);
                                //NS: Comment End
                                ErpCustomer customer = customerCRTManager.GetCustomer(accountNo, currentStore.StoreKey);

                                DataTable dtIds = objDAL.GetAllAddressIdsByActiveAddressIdVSI(erpKey);

                                if (dtIds != null && dtIds.Rows != null && dtIds.Rows.Count > 0 && customer != null)
                                {
                                    erpKey = Convert.ToInt64(dtIds.Rows[0][0]);
                                    if (erpKey > 0 && customer.Addresses.Any(x => x.RecordId == erpKey))
                                    {
                                        List<ErpAddress> adds = customer.Addresses.ToList();
                                        for (int i = 0; i < adds.Count; i++)
                                        {
                                            if (adds[i].RecordId == erpKey)
                                            {
                                                try
                                                {
                                                    adds[i].Deactivate = true;
                                                    customer.Addresses = adds;
                                                    //NS: Remove
                                                    //NS: Comment Start
                                                    //currentChannelState.CustomerManager.UpdateCustomer(customer);
                                                    //NS: Comment End
                                                    customerCRTManager.UpdateCustomer(customer, currentStore.StoreKey);
                                                }
                                                catch (Exception)
                                                {
                                                    // Log Address details and exception details here and donot break the flow.
                                                    throw;
                                                }
                                                break;
                                            }
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

        /// <summary>
        /// Update existing customer
        /// </summary>
        /// <param name="erpCustomer"></param>
        /// <returns></returns>
        public ErpCustomer UpdateCustomer(ErpCustomer erpCustomer)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomer));

            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - Start
            erpCustomer = this.addDefaultExtensionProperties(erpCustomer);
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - End

            ErpCustomer erpCustomerReturn = new ErpCustomer();
            var customerCRTManager = new CustomerCRTManager();
            try
            {
                erpCustomerReturn = customerCRTManager.UpdateCustomer(erpCustomer, currentStore.StoreKey);
            }
            catch (Exception exp)
            {

                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            try
            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                integrationManager.CreateIntegrationKey(Entities.Customer, erpCustomerReturn.RecordId.ToString(), erpCustomerReturn.AccountNumber, erpCustomerReturn.AccountNumber);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40302, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomerReturn));

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCustomerReturn;
        }

        /// <summary>
        /// Update existing customer
        /// </summary>
        /// <param name="erpCustomer"></param>
        /// <returns></returns>
        public ErpUpdateCustomerContactPersonResponse MergeUpdateCustomer(ErpCustomer erpCustomer, string erpCustomerAccountNumber, ErpContactPerson erpContactPerson, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomer));

            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - Start
            erpCustomer = this.addDefaultExtensionProperties(erpCustomer);
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - End

            ErpUpdateCustomerContactPersonResponse erpCustomerReturn = new ErpUpdateCustomerContactPersonResponse(false, "", null, null);
            var customerCRTManager = new CustomerCRTManager();
            try
            {
                erpCustomerReturn = customerCRTManager.UpdateMergeCustomer(erpCustomer, erpCustomerAccountNumber, erpContactPerson, currentStore.StoreKey, requestId);
            }
            catch (Exception exp)
            {

                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            try
            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                if (erpCustomerReturn.Customer != null)
                {
                    //ErpCustomer deserializeCustomer = JsonConvert.DeserializeObject<ErpCustomer>(erpCustomerReturn.Customer.ToString());
                    ErpCustomer deserializeCustomer = (ErpCustomer)(erpCustomerReturn.Customer);
                    integrationManager.CreateIntegrationKey(Entities.Customer, deserializeCustomer.RecordId.ToString(), deserializeCustomer.AccountNumber, deserializeCustomer.AccountNumber);
                }
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40302, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomerReturn));

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCustomerReturn;
        }
        /// <summary>
        /// GetCustomers get list of customers for specified ids.
        /// </summary>
        /// <param name="customerAccounts"></param>
        /// <returns></returns>
        public List<ErpCustomer> GetCustomers(List<string> customerAccounts)
        {
            CommerceLinkLogger.LogDebug(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var customers = new List<ErpCustomer>();
            var customerCRTManager = new CustomerCRTManager();

            foreach (var account in customerAccounts)
            {
                ErpCustomer erpCustomer = customerCRTManager.GetCustomerData(account, 2, currentStore.StoreKey);

                if (erpCustomer.CustomerAffiliations.Count == 0)
                {
                    erpCustomer.CustomerAffiliations = null;
                }

                customers.Add(erpCustomer);
            }

            CommerceLinkLogger.LogDebug(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return customers;
        }

        /// <summary>
        /// Get customer payment methods
        /// </summary>
        /// <param name="erpGetCardRequest"></param>
        /// <returns></returns>
        public ErpCustomerPaymentInfo GetCustomerPaymentMethods(ErpGetCardRequest erpGetCardRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            var customerCRTManager = new CustomerCRTManager();
            ErpCustomerPaymentInfo erpCustomerPaymentInfoResponse = customerCRTManager.GetCustomerPaymentMethods(erpGetCardRequest, currentStore.StoreKey, requestId);

            if (erpCustomerPaymentInfoResponse.CreditCards != null)
            {
                PaymentConnectorDAL paymentConnector = new PaymentConnectorDAL(currentStore.StoreKey);
                foreach (ErpCreditCardCust erpCreditCard in erpCustomerPaymentInfoResponse.CreditCards)
                {
                    PaymentConnector paymentConnectorValue = paymentConnector.GetPaymentConnectorUsingErpPaymentConnector(erpCreditCard.CreditCardProcessors);

                    if (paymentConnectorValue != null)
                    {
                        erpCreditCard.ProcessorId = paymentConnectorValue.EComCreditCardProcessorName;
                    }
                }
            }

            return erpCustomerPaymentInfoResponse;
        }

        /// <summary>
        /// Create customer payment method
        /// </summary>
        /// <param name="erpCreateCardRequest"></param>
        /// <returns></returns>
        public ErpCreditCardResponse CreateCustomerPaymentMethod(ErpCreateCardRequest erpCreateCardRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);
            //Generating Blob for new Credit Card
            if (!string.IsNullOrEmpty(erpCreateCardRequest.TenderLine.MaskedCardNumber))
            {
                SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
                soHelper.SetupPaymentMethod(erpCreateCardRequest.TenderLine, erpCreateCardRequest.SalesOrder, requestId);

                var creditCardCRTManager = new CustomerCRTManager();
                ErpCreditCardResponse erpCreditCardResponse = creditCardCRTManager.CreateCustomerPaymentMethod(erpCreateCardRequest, currentStore.StoreKey);

                if (erpCreditCardResponse.CreditCards != null)
                {
                    PaymentConnectorDAL paymentConnector = new PaymentConnectorDAL(currentStore.StoreKey);

                    foreach (ErpCreditCardCust erpCreditCard in erpCreditCardResponse.CreditCards)
                    {
                        PaymentConnector paymentConnectorValue = paymentConnector.GetPaymentConnectorUsingErpPaymentConnector(erpCreditCard.CreditCardProcessors);

                        if (paymentConnectorValue != null)
                        {
                            erpCreditCard.ProcessorId = paymentConnectorValue.EComCreditCardProcessorName;
                        }
                    }
                }

                return erpCreditCardResponse;
            }
            else
            {
                return new ErpCreditCardResponse(false, "MaskedCardNumber is Missing in tenderLine object", null);
            }
        }

        /// <summary>
        /// Update customer payment method
        /// </summary>
        /// <param name="erpEditCardRequest"></param>
        /// <returns></returns>
        public ErpCreditCardResponse UpdateCustomerPaymentMethod(ErpEditCardRequest erpEditCardRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            //Generating Blob for new Credit Card
            if (!string.IsNullOrEmpty(erpEditCardRequest.TenderLine.MaskedCardNumber))
            {
                SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
                soHelper.SetupPaymentMethod(erpEditCardRequest.TenderLine, erpEditCardRequest.SalesOrder, requestId);

                var creditCardCRTManager = new CustomerCRTManager();
                ErpCreditCardResponse erpCreditCardResponse = creditCardCRTManager.UpdateCustomerPaymentMethod(erpEditCardRequest, currentStore.StoreKey);

                if (erpCreditCardResponse.CreditCards != null)
                {
                    PaymentConnectorDAL paymentConnector = new PaymentConnectorDAL(currentStore.StoreKey);
                    foreach (ErpCreditCardCust erpCreditCard in erpCreditCardResponse.CreditCards)
                    {
                        PaymentConnector paymentConnectorValue = paymentConnector.GetPaymentConnectorUsingErpPaymentConnector(erpCreditCard.CreditCardProcessors);

                        if (paymentConnectorValue != null)
                        {
                            erpCreditCard.ProcessorId = paymentConnectorValue.EComCreditCardProcessorName;
                        }
                    }
                }

                return erpCreditCardResponse;
            }
            else
            {
                return new ErpCreditCardResponse(false, "MaskedCardNumber is Missing in tenderLine object", null);
            }
        }

        /// <summary>
        /// Delete customer payment method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ErpDeleteCustomerPaymentMethodResponse DeleteCustomerPaymentMethod(ErpDeleteCustomerPaymentMethodRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            var customerCRTManager = new CustomerCRTManager();
            return customerCRTManager.DeleteCustomerPaymentMethod(request, currentStore.StoreKey);
        }

        /// <summary>
        /// Create customer bank account method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ErpCustomerBankAccountResponse CreateCustomerBankAccount(ErpCreateCardRequest erpRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);
            var customerCRTManager = new CustomerCRTManager();
            return customerCRTManager.CreateCustomerBankAccount(erpRequest, currentStore.StoreKey);
        }
        /// <summary>
        /// Custom for Team Viewer Third party orders
        /// Method used to create customer from sales order object
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ErpCustomer CreateCustomerThridParty(ErpCustomer customer, string salesOrderCurrencyCode)
        {
            if (customer == null)
                return null;

            try
            {
                customer.VatNumber = string.Empty;
                customer.Address.State = ""; // Ingram sending State Name while AX accept only state code.

                customer.CurrencyCode = !string.IsNullOrEmpty(salesOrderCurrencyCode)
                    ? salesOrderCurrencyCode
                    : configurationHelper.GetSetting(APPLICATION.Default_Currency_Code);

                customer.AccountNumber = new Guid().ToString();

                if (string.IsNullOrEmpty(customer.Name))
                {
                    customer.CustomerType = ErpCustomerType.Person;
                    customer.CustomerTypeValue = (int)ErpCustomerType.Person;
                }
                else
                {
                    customer.CustomerType = ErpCustomerType.Organization;
                    customer.CustomerTypeValue = (int)ErpCustomerType.Organization;

                }

                if (customer.Address.ThreeLetterISORegionName != null && customer.Address.ThreeLetterISORegionName.Length < 3)
                {
                    var countryCodesDAL = new CountryCodeDAL(currentStore.StoreKey);
                    customer.Address.ThreeLetterISORegionName = countryCodesDAL.GetCountryByTwoLetterCode(customer.Address.ThreeLetterISORegionName).ThreeLetterCode;
                }

                if (customer.Address != null)
                {
                    if (customer.Addresses == null)
                        customer.Addresses = new List<ErpAddress>();

                    customer.Addresses.Add(customer.Address);

                    // remove multiple spaces from address
                    customer.Address.BuildingCompliment = Regex.Replace(customer.Address.BuildingCompliment, @"\s+", " ");

                    // Trim length if greater than 60.
                    if (customer.Address.BuildingCompliment.Length > 60)
                    {
                        customer.Address.BuildingCompliment = customer.Address.BuildingCompliment.Substring(0, 60);
                    }

                    // BEGIN - Aqeel, VSTS: 24578 // Create two addresses for customer, 2nd should be invoice and not primary
                    ErpAddress invoiceAddress = JsonConvert.DeserializeObject<ErpAddress>(JsonConvert.SerializeObject(customer.Address));
                    invoiceAddress.AddressType = ErpAddressType.Invoice;
                    invoiceAddress.AddressTypeValue = (int)ErpAddressType.Invoice;
                    invoiceAddress.IsPrimary = false;

                    customer.Addresses.Add(invoiceAddress);
                    // END - Aqeel, VSTS: 24578 // Create two addresses for customer, 2nd should be invoice and not primary
                }

                customer = AssignCustomer(customer, string.Empty, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customer;
        }

        /// <summary>
        /// Trigger Data Sync method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ErpTriggerDataSyncResponse TriggerDataSync(string requestXML)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, currentStore, MethodBase.GetCurrentMethod().Name);

            var customerCRTManager = new CustomerCRTManager();
            return customerCRTManager.TriggerDataSync(requestXML, currentStore.StoreKey);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// GetEcomAddressKey gets Ecom Address Key from DataBase on basis of erpKey.
        /// </summary>
        /// <param name="erpKey"></param>
        /// <returns></returns>
        private string GetEcomAddressKey(long erpKey)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "erpKey: " + erpKey.ToString());
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            string key = string.Empty;
            try
            {
                var k = integrationManager.GetComKey(Entities.CustomerAddress, erpKey.ToString());
                if (k == null || string.IsNullOrEmpty(k.ComKey))
                {
                    DataTable Ids = objDAL.GetAllAddressIdsByActiveAddressIdVSI(erpKey);
                    if (Ids != null && Ids.Rows != null && Ids.Rows.Count > 0)
                    {
                        DataTable keys = objDAL.GetAddressKeys(Ids);
                        if (keys != null && keys.Rows != null && keys.Rows.Count > 0)
                        {
                            // Ideally one row should come back
                            key = Convert.ToString(keys.Rows[0]["ComKey"]);
                        }
                    }
                }
                else
                {
                    key = k.ComKey;
                }
            }
            catch (Exception)
            {
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, key);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return key;
        }

        /// <summary>
        /// GetEcomAddressKey gets Ecom Address Key from lists.
        /// </summary>
        /// <param name="erpAdd"></param>
        /// <param name="ecomAdds"></param>
        /// <returns></returns>
        private string GetEcomAddressKey(ErpAddress erpAdd, List<ERPDataModels.ErpAddress> ecomAdds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            string key = string.Empty;
            try
            {
                string erpKey = erpAdd.Street + erpAdd.ZipCode + erpAdd.City;

                foreach (var ecomAdd in ecomAdds)
                {
                    //string ecomKey = ecomAdd.Street + ecomAdd.ZipCode + ecomAdd.City;
                    string ecomKey = ecomAdd.Street + ecomAdd.ZipCode.Split('-')[0] + ecomAdd.City;
                    if (erpKey == ecomKey)
                    {
                        key = ecomAdd.EcomAddressId;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return key;
        }

        /// <summary>
        /// ConvertToCustomer converts ERP Customer to CRT Customer.
        /// </summary>
        /// <param name="erp"></param>
        /// <returns></returns>
        private ErpCustomer ConvertToCustomer(ErpCustomer erp)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCustomer customer = new ErpCustomer();

            try
            {
                #region Customer Info
                customer.AccountNumber = erp.AccountNumber;
                customer.Name = erp.Name;
                customer.FirstName = erp.FirstName;
                customer.LastName = erp.LastName;
                customer.MiddleName = erp.MiddleName;
                customer.Email = erp.Email;
                customer.Phone = erp.Phone;
                customer.VatNumber = erp.VatNumber;
                customer.CustomerGroup = erp.CustomerGroup;
                customer.CustomerType = erp.CustomerType;
                customer.CustomerTypeValue = erp.CustomerTypeValue;
                customer.Language = erp.Language;
                #endregion
                // As we always clear address after default conversion. so let be empty here.
                #region AddressInfo
                List<ErpAddress> adds = new List<ErpAddress>(erp.Addresses);
                customer.Addresses = adds;
                #endregion

                #region EComInfo
                customer.EcomCustomerId = erp.EcomCustomerId;
                #endregion

                #region AXINFO

                customer.DirectoryPartyRecordId = erp.DirectoryPartyRecordId;
                customer.EmailRecordId = erp.EmailRecordId;
                customer.AccountNumber = erp.AccountNumber;
                customer.EntityName = erp.EntityName;
                customer.PartyNumber = erp.PartyNumber;
                customer.PersonNameId = erp.PersonNameId;
                customer.Phone = erp.Phone;
                customer.PhoneExt = erp.PhoneExt;
                customer.PhoneLogisticsLocationId = erp.PhoneLogisticsLocationId;
                customer.PhoneLogisticsLocationRecordId = erp.PhoneLogisticsLocationRecordId;
                customer.PhonePartyLocationRecId = erp.PhonePartyLocationRecId;
                customer.PhoneRecordId = erp.PhoneRecordId;
                customer.RecordId = erp.RecordId;
                customer.RetailCustomerTableRecordId = erp.RetailCustomerTableRecordId;
                customer.ReceiptEmail = erp.ReceiptEmail;
                customer.SalesTaxGroup = erp.SalesTaxGroup;
                customer.TaxGroup = erp.TaxGroup;
                customer.Attributes = erp.Attributes;

                // [MB] - TV - BR 3.0 - 15007 - Start
                customer.ExtensionProperties = erp.ExtensionProperties;
                customer.CurrencyCode = erp.CurrencyCode;
                customer.Url = erp.Url;
                // [MB] - TV - BR 3.0 - 15007 - Start

                #endregion
            }
            catch (Exception)
            {
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return customer;
        }

        /// <summary>
        /// Check if customer sent from Magento is dirty or not. If dirty, then we will proceed with update else will skip it.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsDirty(ErpCustomer erp, ErpCustomer existing)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            bool dirty = false;

            try
            {
                if (erp != null && existing != null)
                {
                    string intSLBirthMonth = erp.SLBirthMonth;
                    string erpBirthMonth = string.IsNullOrEmpty(intSLBirthMonth) ? string.Empty : Common.CommonUtility.GetMonthName(intSLBirthMonth).ToLower();
                    string existingBirthMonth = string.IsNullOrEmpty(existing.OrganizationId) ? string.Empty : existing.OrganizationId.ToLower();

                    if (erp.FirstName != existing.FirstName)
                    {
                        dirty = true;
                    }
                    else if (erp.LastName != existing.LastName)
                    {
                        dirty = true;
                    }
                    else if (erp.MiddleName != existing.MiddleName)
                    {
                        dirty = true;
                    }
                    else if (erp.Email != existing.Email)
                    {
                        dirty = true;
                    }
                    else if (erp.VatNumber != existing.VatNumber)
                    {
                        dirty = true;
                    }
                    else if (erpBirthMonth != existingBirthMonth)
                    {
                        dirty = true;
                    }
                    if (!dirty && erp.Addresses.Count > 0)
                    {
                        if (existing.Addresses.Count != erp.Addresses.Count)
                        {
                            dirty = true;
                        }
                        else
                        {
                            int existCount = existing.Addresses.Count;
                            int erpCount = erp.Addresses.Count;
                            if (existCount != erpCount)
                            {
                                dirty = true;
                            }
                            else
                            {
                                if (existCount == 1 && erpCount == 1)
                                {
                                    dirty = IsDirtyAddress(erp.Addresses[0], existing.Addresses[0]);
                                }
                                else
                                {
                                    IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                                    foreach (var item in erp.Addresses)
                                    {
                                        IntegrationKey key = integrationManager.GetErpKey(Entities.CustomerAddress, item.EcomAddressId);

                                        if (key != null)
                                        {
                                            ErpAddress add = existing.Addresses.FirstOrDefault(a => a.RecordId.ToString() == key.ErpKey);

                                            if (add != null)
                                            {
                                                dirty = IsDirtyAddress(item, add);
                                            }
                                            else
                                            {
                                                dirty = true;
                                            }
                                        }
                                        else
                                        {
                                            dirty = true;
                                        }
                                        if (dirty)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    dirty = true;
                }

            }
            catch (Exception)
            {
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return dirty;
        }

        /// <summary>
        /// Check if Address sent from Magento is dirty or not.
        /// </summary>
        /// <param name="erpAdd"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        private bool IsDirtyAddress(ErpAddress erpAdd, ErpAddress add)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            bool dirty = false;

            try
            {
                if (erpAdd.Name != add.Name)
                {
                    dirty = true;
                }
                else if (erpAdd.Phone != add.Phone)
                {
                    dirty = true;
                }
                else if (erpAdd.Street != add.Street)
                {
                    dirty = true;
                }
                else if (erpAdd.State != add.State)
                {
                    dirty = true;
                }
                else if (erpAdd.City != add.City)
                {
                    dirty = true;
                }
                else if (erpAdd.ZipCode != add.ZipCode)
                {
                    dirty = true;
                }
                else if ((int)erpAdd.AddressType != (int)add.AddressType)
                {
                    dirty = true;
                }
                else if (erpAdd.ThreeLetterISORegionName != add.ThreeLetterISORegionName)
                {
                    dirty = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return dirty;
        }

        /// <summary>
        /// GetAddressInfo gets Address Information.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        private string GetAddressInfo(ErpAddress add)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            string addInfo = string.Format("Id:[{0}], Name:[{1}], Street:[{2}], City:[{3}], State:[{4}], Zip:[{5}], Country:[{6}], Phone:[{7}], Purpose:[{8}] " + Environment.NewLine
            , add.RecordId,
            add.Name,
            add.Street,
            add.City,
            add.State,
            add.ZipCode,
            add.ThreeLetterISORegionName,
            add.Phone,
            add.AddressType);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return addInfo;
        }

        private string GetEcomItemId(string erpItemId, string storeKey)
        {
            IntegrationManager integrationManager = new IntegrationManager(storeKey);
            var key = integrationManager.GetKeyByDescription(Entities.Product, erpItemId);

            return key == null ? erpItemId : key.ComKey;
        }

        private string GetStoreKeyByCountryCode(string threeLetterISORegionName)
        {
            ApplicationSettingsDAL appSettingsDAL = new ApplicationSettingsDAL(currentStore.StoreKey);

            int storeId = appSettingsDAL.GetStoreId(threeLetterISORegionName);
            var store = StoreService.GetStoreById(storeId);

            if(store == null)
            {
                throw new CommerceLinkError("Store not found with ISO region '" + threeLetterISORegionName + "'");
            }

            return store.StoreKey;
        }

        private string GetStoreKeyByRetailChannelId(string retailChannelId)
        {
            var store = StoreService.GetByRetailChannelId(retailChannelId);

            if (store == null)
                throw new CommerceLinkError("Store not found with Retail Channel Id '" + retailChannelId + "'");

            return store.StoreKey;
        }

        #endregion

    }
}
