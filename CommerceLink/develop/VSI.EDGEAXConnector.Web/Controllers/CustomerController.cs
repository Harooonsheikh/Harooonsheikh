using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Xml;
using VSI.CommerceLink.EcomDataModel;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.CommerceLink.EcomDataModel.Response;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;
using VSI.EDGEAXConnector.Web.ActionFilters;
using VSI.EDGEAXConnector.Web.Controllers;
using static VSI.EDGEAXConnector.Web.Controllers.ContactPersonController;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// CustomerController defines properties and methods for API controller for customer.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class CustomerController : ApiBaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        /// <param name="_eComAdapterFactory"></param>
        public CustomerController()
        {
            ControllerName = "CustomerController";
        }

        #region API Methods

        /// <summary>
        /// CreateCustomer creates customer with provided details.
        /// </summary>
        /// <param name="createCustomerRequest">sakes order transaction request to be created</param>
        /// <returns>SalesOrderTransResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/CreateCustomer")]
        public CustomerResponse CreateCustomer([FromBody] EcomCustomerCreateRequest createCustomerRequest)
        {
            //VSTS: 41099 reset Erp language is not valid.
            LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
            if (!createCustomerRequest.Customer.SwapLanguage && !languageCodes.ValidateErpLanguageCode(createCustomerRequest.Customer.Language))
            {
                createCustomerRequest.Customer.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
            }

            CustomerResponse customerResponse = this.CreateCustomerMethod(createCustomerRequest, MethodBase.GetCurrentMethod().Name);
            return customerResponse;
        }

        public CustomerResponse CreateCustomerMethod(EcomCustomerCreateRequest createCustomerRequest, string methodName)
        {
            CustomerResponse customerResponse;
            CustomerCreateRequest erpCreateCustomerRequest = new CustomerCreateRequest();

            try
            {
                // [MB] - TV - BR 3.0 - 12539 - Start
                for (int i = 0; i < createCustomerRequest.Customer.Addresses.Count; i++)
                {
                    createCustomerRequest.Customer.Addresses[i].BuildingCompliment = createCustomerRequest.Customer.Addresses[i].Street2;
                }
                // [MB] - TV - BR 3.0 - 12539 - End

                erpCreateCustomerRequest = _mapper.Map<EcomCustomerCreateRequest, CustomerCreateRequest>(createCustomerRequest);

                customerResponse = this.ValidateCustomerCreateRequest(erpCreateCustomerRequest, methodName);

                if (customerResponse != null)
                {
                    return customerResponse;
                }
                else
                {
                    customerResponse = CreateNewCustomer(erpCreateCustomerRequest);

                    return customerResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerResponse = new CustomerResponse(false, null, message);
                return customerResponse;
            }
        }

        /// <summary>
        /// Create Customer and Reseller Customer.
        /// </summary>
        /// <param name="mergeCustomerResellerRequest">mergeCustomerResellerRequest</param>
        /// <returns>MergeCustomerResellerResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/MergeCustomerReseller")]
        public MergeCustomerResellerResponse MergeCustomerReseller([FromBody] MergeCustomerResellerRequest mergeCustomerResellerRequest)
        {
            try
            {
                CustomerResponse customerResponse = null;
                CustomerResponse resellerResponse = null;
                if (mergeCustomerResellerRequest.Customer == null && mergeCustomerResellerRequest.Reseller == null)
                {
                    return new MergeCustomerResellerResponse(false, null, null, "Invalid Request.");
                }

                if (mergeCustomerResellerRequest.Customer != null)
                {
                    mergeCustomerResellerRequest.Customer.ExtensionProperties = mergeCustomerResellerRequest.Customer.ExtensionProperties ?? new List<EcomCommerceProperty>();
                    mergeCustomerResellerRequest.Customer.ExtensionProperties.Add(new EcomCommerceProperty()
                    {
                        Key = "RelationshipType",
                        Value = new EcomCommercePropertyValue() { StringValue = "End-User Reseller" }
                    });

                    //VSTS: 41099 if Erp language is not define then empty language, so Default_Culture is applied in AX adapter.
                    LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                    if (!mergeCustomerResellerRequest.Customer.SwapLanguage && !languageCodes.ValidateErpLanguageCode(mergeCustomerResellerRequest.Customer.Language))
                    {
                        mergeCustomerResellerRequest.Customer.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
                    }

                    EcomCustomerCreateRequest customerRequest = new EcomCustomerCreateRequest() { Customer = mergeCustomerResellerRequest.Customer };
                    customerResponse = this.CreateCustomerMethod(customerRequest, MethodBase.GetCurrentMethod().Name);

                    if (!customerResponse.Status)
                    {
                        return new MergeCustomerResellerResponse(false, null, null, customerResponse.Message);
                    }
                }

                if (mergeCustomerResellerRequest.Reseller != null)
                {
                    mergeCustomerResellerRequest.Reseller.ExtensionProperties = mergeCustomerResellerRequest.Reseller.ExtensionProperties ?? new List<EcomCommerceProperty>();
                    mergeCustomerResellerRequest.Reseller.ExtensionProperties.Add(new EcomCommerceProperty()
                    {
                        Key = "RelationshipType",
                        Value = new EcomCommercePropertyValue() { StringValue = "Reseller" }
                    });

                    //VSTS: 41099 if Erp language is not define then empty language, so Default_Culture is applied in AX adapter.
                    LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                    if (!mergeCustomerResellerRequest.Reseller.SwapLanguage && !languageCodes.ValidateErpLanguageCode(mergeCustomerResellerRequest.Reseller.Language))
                    {
                        mergeCustomerResellerRequest.Reseller.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
                    }

                    EcomCustomerCreateRequest resellerRequest = new EcomCustomerCreateRequest() { Customer = mergeCustomerResellerRequest.Reseller };
                    resellerResponse = this.CreateCustomerMethod(resellerRequest, MethodBase.GetCurrentMethod().Name);

                    if (!resellerResponse.Status)
                    {
                        return new MergeCustomerResellerResponse(false, null, null, resellerResponse.Message);
                    }
                }
                return new MergeCustomerResellerResponse(true, customerResponse?.CustomerInfo, resellerResponse?.CustomerInfo, string.Empty);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new MergeCustomerResellerResponse(false, null, null, message.ToString());
            }
        }


        /// <summary>
        /// MergeCreateCustomerContactPerson creates customer and contact person with provided details.
        /// </summary>
        /// <param name="createCustomerContactPersonRequest">Takes Customer Information and Contact Person information</param>
        /// <returns>CustomerContactPersonResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/MergeCreateCustomerContactPerson")]
        public MergeCreateCustomerContactPersonResponse MergeCreateCustomerContactPerson([FromBody] EcomCustomerContactPersonCreateRequest createCustomerContactPersonRequest)
        {
            CLContactPersonResponse contactPersonResponse = new CLContactPersonResponse(false, string.Empty, null);
            ErpCustomer erpCustomerMap = new ErpCustomer();
            ErpContactPerson erpContactPerson = new ErpContactPerson();
            CustomerCreateRequest erpCreateCustomerRequest = new CustomerCreateRequest();
            CustomerAndContactPersonInfo customerAndContactPersonInfo = new CustomerAndContactPersonInfo();
            CustomerContactPersonResponse customerContactPersonResponse = new CustomerContactPersonResponse(true, null, null, "");
            MergeCreateCustomerContactPersonResponse mergeCreateCustomerContactPersonResponse = new MergeCreateCustomerContactPersonResponse(true, null, null, "");

            try
            {
                // [MB] - TV - BR 3.0 - 12539 - Start
                for (int i = 0; i < createCustomerContactPersonRequest.CustomerInfo.Customer.Addresses.Count; i++)
                {
                    createCustomerContactPersonRequest.CustomerInfo.Customer.Addresses[i].BuildingCompliment = createCustomerContactPersonRequest.CustomerInfo.Customer.Addresses[i].Street2;
                }
                // [MB] - TV - BR 3.0 - 12539 - End

                erpCreateCustomerRequest = _mapper.Map<EcomCustomerCreateRequest, CustomerCreateRequest>(createCustomerContactPersonRequest.CustomerInfo);

                erpCustomerMap = _mapper.Map<CommerceLink.EcomDataModel.EcomCustomer, ErpCustomer>(createCustomerContactPersonRequest.CustomerInfo.Customer);
                erpContactPerson = _mapper.Map<CommerceLink.EcomDataModel.EcomContactPerson, ErpContactPerson>(createCustomerContactPersonRequest.ContactPerson);

                if (erpContactPerson != null)
                {
                    erpContactPerson.TMVSourceSystem = string.IsNullOrWhiteSpace(erpContactPerson.TMVSourceSystem) ? SourceSystem.WEB.ToString() : erpContactPerson.TMVSourceSystem;
                }

                CustomerResponse customerResponse = new CustomerResponse(false, null, null);
                customerResponse = this.ValidateCustomerCreateRequest(erpCreateCustomerRequest, string.Empty);
                if (customerResponse != null)
                {
                    // return new CustomerContactPersonResponse(false, null, null, customerResponse.Message);
                    return new MergeCreateCustomerContactPersonResponse(false, null, null, customerResponse.Message);
                }
                //validate Contact Person 
                contactPersonResponse = this.ValidateContactPerson(createCustomerContactPersonRequest.ContactPerson, false);
                if (contactPersonResponse != null)
                {
                    // return new CustomerContactPersonResponse(false, null, null, contactPersonResponse.ErrorMessage);
                    return new MergeCreateCustomerContactPersonResponse(false, null, null, contactPersonResponse.ErrorMessage);
                }
                else
                {
                    //VSTS: 41099 if Erp language is not define then empty language, so Default_Culture is applied in AX adapter.
                    LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                    if (!languageCodes.ValidateErpLanguageCode(erpCreateCustomerRequest.Customer.Language))
                    {
                        erpCreateCustomerRequest.Customer.Language = string.Empty;
                    }

                    customerResponse = CreateNewCustomer(erpCreateCustomerRequest);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customerResponse));
                    if (customerResponse.Status)
                    {
                        if (customerResponse.CustomerInfo != null)
                        {
                            //Apply Cust Account Number 
                            ErpCustomer erpCustomer = (ErpCustomer)customerResponse.CustomerInfo;
                            erpContactPerson.CustAccount = erpCustomer.AccountNumber;
                            erpContactPerson.ContactForParty = erpCustomer.DirectoryPartyRecordId;
                        }
                    }
                    else
                    {
                        CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, customerResponse.Message.ToString());
                        // return new CustomerContactPersonResponse(false, null, null, customerResponse.Message);
                        return new MergeCreateCustomerContactPersonResponse(false, null, null, customerResponse.Message);
                    }

                    // Language code swapping
                    if (createCustomerContactPersonRequest.ContactPerson.Language != null &&
                        !string.IsNullOrEmpty(createCustomerContactPersonRequest.ContactPerson.Language) &&
                        createCustomerContactPersonRequest.ContactPerson.SwapLanguage)
                    {
                        erpContactPerson.Language = languageCodes.GetErpLanguageCode(erpContactPerson.Language);
                    }

                    var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);
                    ERPContactPersonResponse erpContactPersonResponse = erpContactPersonController.CreateContactPerson(erpContactPerson, GetRequestGUID(Request));

                    if (erpContactPersonResponse.Success && erpContactPersonResponse.ContactPerson != null &&
                        !string.IsNullOrWhiteSpace(erpContactPersonResponse.ContactPerson.ContactPersonId))
                    {
                        // Language code swapping for Ecom
                        if (erpContactPersonResponse.ContactPerson != null &&
                            !string.IsNullOrEmpty(erpContactPersonResponse.ContactPerson.Language) &&
                            createCustomerContactPersonRequest.ContactPerson.SwapLanguage)
                        {
                            erpContactPersonResponse.ContactPerson.Language = languageCodes.GetEcomLanguageCode(erpContactPersonResponse.ContactPerson.Language);
                        }

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonResponse.ContactPerson));
                        contactPersonResponse = new CLContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, erpContactPersonResponse.ContactPerson);
                    }
                    else
                    {
                        contactPersonResponse = new CLContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, erpContactPersonResponse.ContactPerson);
                    }
                    //end Contact Person

                    customerAndContactPersonInfo.ErpContactPerson = (ErpContactPerson)contactPersonResponse.ContactPerson;
                    customerAndContactPersonInfo.ErpCustomer = (ErpCustomer)customerResponse.CustomerInfo;
                    customerContactPersonResponse.CustomerInfo = customerAndContactPersonInfo.ErpCustomer;
                    customerContactPersonResponse.ContactPerson = customerAndContactPersonInfo.ErpContactPerson;

                    EcomContactPerson ecomContactPerson = null;
                    if (contactPersonResponse.ContactPerson != null)
                    {
                        ecomContactPerson = convertErpContactPersonToeComContactPerson((ErpContactPerson)contactPersonResponse.ContactPerson, createCustomerContactPersonRequest.ContactPerson.SwapLanguage);
                    }

                    mergeCreateCustomerContactPersonResponse.CustomerInfo = (ErpCustomer)customerResponse.CustomerInfo;

                    mergeCreateCustomerContactPersonResponse.ContactPerson = ecomContactPerson;

                    //return customerContactPersonResponse;
                    return mergeCreateCustomerContactPersonResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                mergeCreateCustomerContactPersonResponse.Message = message;
                // return CustomerContactPersonResponse;
                return mergeCreateCustomerContactPersonResponse;
            }
        }
        /// <summary>
        /// MergeUpdateCustomerContactPerson update customer like create customer provided details and update contact person.
        /// </summary>
        /// <param name="updateCustomerRequest">Merge Update Customer transaction request to be created</param>
        /// <returns>CustomerRespone</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/MergeUpdateCustomerContactPerson")]
        public MergeCreateCustomerContactPersonResponse MergeUpdateCustomerContactPerson([FromBody] EcomCustomerUpdateRequest updateCustomerRequest)
        {
            return MergeUpdateCustomerAndContactPerson(updateCustomerRequest, MethodBase.GetCurrentMethod().Name);
        }
        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/GetCustomer")]
        public CustomerResponse GetCustomer([FromBody] CustomerRequest customerRequest)
        {
            CustomerResponse customerResponse;
            try
            {

                if (customerRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    customerResponse = new CustomerResponse(false, "", message);
                    return customerResponse;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(customerRequest.CustomerId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customerId");
                        customerResponse = new CustomerResponse(false, "", message);
                        return customerResponse;
                    }

                    // It will be 0 by default. Better to use Enumeration.
                    else if (customerRequest.SearchLocation != 1 && customerRequest.SearchLocation != 2)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "Paramter 'searchLocation' can be assigned only '1' or '2'");
                        customerResponse = new CustomerResponse(false, "", message);
                        return customerResponse;
                    }
                }

                string customerId = customerRequest.CustomerId;
                int searchLocation = customerRequest.SearchLocation;
                bool useMapping = customerRequest.UseMapping;

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "customerId: " + customerId.ToString() + ", useMapping: " + useMapping.ToString() + " and searchLocation: " + searchLocation, ToString());

                ErpCustomer erpCustomer = new ErpCustomer();
                object customer;
                var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);

                erpCustomer = erpCustomerController.GetCustomerData(customerId, searchLocation);

                if (erpCustomer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);

                    customerResponse = new CustomerResponse(false, "", message);
                    return customerResponse;
                }
                else
                {
                    // [MB] - TV - BR 3.0 - 15877 - new clause of swapLanguage added in condition - Start
                    if (!string.IsNullOrEmpty(erpCustomer.Language) && customerRequest.SwapLanguage)
                    {
                        LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                        erpCustomer.Language = languageCodes.GetEcomLanguageCode(erpCustomer.Language);
                    }
                    // [MB] - TV - BR 3.0 - 15877 - new clause of swapLanguage added in condition - End
                }

                // [MB] - TV - BR 3.0 - 15877 - swapLanguage return value update as per the request - Start
                erpCustomer.SwapLanguage = customerRequest.SwapLanguage;
                // [MB] - TV - BR 3.0 - 15877 - swapLanguage return value update as per the request - End

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomer));

                // [MB] - TV - BR 3.0 - 12539 - Start
                for (int i = 0; i < erpCustomer.Addresses.Count; i++)
                {
                    erpCustomer.Addresses[i].Street2 = erpCustomer.Addresses[i].BuildingCompliment;
                    erpCustomer.Addresses[i].BuildingCompliment = String.Empty;
                }
                // [MB] - TV - BR 3.0 - 12539 - End

                if (useMapping)
                {
                    using (var ecomProductController = ecomAdapterFactory.CreateCustomerController(currentStore.StoreKey))
                    {
                        erpCustomer.CustomerAddresses = new List<ErpAddress>();
                        foreach (var address in erpCustomer.Addresses)
                        {
                            if (address.IsPrimary == true)
                            {
                                erpCustomer.CustomerAddresses.Add(address);
                            }
                        }
                        XmlDocument xmlDocument = CreateCustomerXmlDocument(erpCustomer);
                        var data = xmlDocument.SerializeToJson();
                        customer = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                    }
                }
                else
                {
                    customer = erpCustomer;
                }
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customer));
                customerResponse = new CustomerResponse(true, customer, "");
                return customerResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerResponse = new CustomerResponse(false, "", message);
                return customerResponse;
            }
        }


        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="customerByLicenceRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/GetCustomerByLicence")]
        public CustomersResponse GetCustomerByLicence([FromBody] CustomerByLicenceRequest customerByLicenceRequest)
        {
            CustomersResponse customerResponse;
            try
            {
                if (customerByLicenceRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    customerResponse = new CustomersResponse(false, null, message);
                    return customerResponse;
                }
                if (customerByLicenceRequest.licenceNumber == null || customerByLicenceRequest.licenceNumber.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "licenceNumber");
                    customerResponse = new CustomersResponse(false, null, message);
                    return customerResponse;
                }
                if (customerByLicenceRequest.licenceNumber.Count > 0)
                {
                    foreach (var licenseID in customerByLicenceRequest.licenceNumber)
                    {
                        if (!(base.ValidateCustomerLicenseIDLength(licenseID)))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400011, currentStore, "");
                            customerResponse = new CustomersResponse(false, null, message);
                            return customerResponse;
                        }
                    }
                }

                List<string> licenceNumbers = customerByLicenceRequest.licenceNumber;
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "licenceNumber: " + string.Join<string>(",", licenceNumbers));
                List<ErpCustomer> erpCustomers = new List<ErpCustomer>();
                List<object> customers;
                var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);

                erpCustomers = erpCustomerController.GetCustomerByLicence(licenceNumbers);

                foreach (var erpCustomer in erpCustomers)
                {
                    //Language code swapping for Ecom
                    if (erpCustomer != null &&
                        !string.IsNullOrEmpty(erpCustomer.Language) &&
                        customerByLicenceRequest.SwapLanguage)
                    {
                        LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                        erpCustomer.Language = languageCodes.GetEcomLanguageCode(erpCustomer.Language);
                    }
                }

                //++TODO update to actual code .
                erpCustomers.ForEach(x => x.LicenceNumber = new List<string>(new string[] { customerByLicenceRequest.licenceNumber[0] }));

                if (erpCustomers == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);

                    customerResponse = new CustomersResponse(false, null, message);
                    return customerResponse;
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomers));
                customers = erpCustomers.Cast<object>().ToList();
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customers));
                customerResponse = new CustomersResponse(true, customers, "");
                return customerResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerResponse = new CustomersResponse(false, null, message);
                return customerResponse;
            }
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customerUpdateRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/UpdateCustomer")]
        public CustomerResponse UpdateCustomer([FromBody] CustomerUpdateRequest customerUpdateRequest)
        {
            CustomerResponse customerResponse;

            // [MB] - TV - BR 3.0 - 12539 - Start
            for (int i = 0; i < customerUpdateRequest.Customer.Addresses.Count; i++)
            {
                customerUpdateRequest.Customer.Addresses[i].BuildingCompliment = customerUpdateRequest.Customer.Addresses[i].Street2;
            }
            // [MB] - TV - BR 3.0 - 12539 - End

            try
            {

                customerResponse = this.ValidateCustomerUpdateRequest(customerUpdateRequest);

                if (customerResponse != null)
                {
                    return customerResponse;
                }
                else
                {
                    LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);

                    // [MB] - TV - BR 3.0 - 15878 - new clause of swapLanguage added in condition - Start
                    //Language code swapping for D365
                    if (customerUpdateRequest.Customer != null && !string.IsNullOrEmpty(customerUpdateRequest.Customer.Language) && customerUpdateRequest.Customer.SwapLanguage)
                    {
                        customerUpdateRequest.Customer.Language = languageCodes.GetErpLanguageCode(customerUpdateRequest.Customer.Language);
                    }
                    // [MB] - TV - BR 3.0 - 15878 - new clause of swapLanguage added in condition - End

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customerUpdateRequest.Customer));

                    var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);

                    ErpCustomer erpCustomer = erpCustomerController.UpdateCustomer(customerUpdateRequest.Customer);

                    // [MB] - TV - BR 3.0 - 15878 - new clause of swapLanguage added in condition - Start
                    //Language code swapping for Ecom
                    if (erpCustomer != null && !string.IsNullOrEmpty(erpCustomer.Language) && customerUpdateRequest.Customer.SwapLanguage)
                    {
                        erpCustomer.Language = languageCodes.GetEcomLanguageCode(erpCustomer.Language);
                    }
                    erpCustomer.SwapLanguage = customerUpdateRequest.Customer.SwapLanguage;
                    // [MB] - TV - BR 3.0 - 15878 - new clause of swapLanguage added in condition - End

                    // [MB] - TV - BR 3.0 - 12539 - Start
                    for (int i = 0; i < erpCustomer.Addresses.Count; i++)
                    {
                        erpCustomer.Addresses[i].Street2 = erpCustomer.Addresses[i].BuildingCompliment;
                        erpCustomer.Addresses[i].BuildingCompliment = String.Empty;
                    }
                    // [MB] - TV - BR 3.0 - 12539 - End

                    //VSTS: 41099: 
                    if (!erpCustomer.SwapLanguage && !languageCodes.ValidateErpLanguageCode(erpCustomer.Language))
                    {
                        erpCustomer.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
                    }

                    object customer = this.GetCustomerDataWithMapping(customerUpdateRequest.UseMapping, erpCustomer);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customer));

                    customerResponse = new CustomerResponse(true, customer, "");

                    //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customerResponse));


                    return customerResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerResponse = new CustomerResponse(false, "", message);
                return customerResponse;
            }
        }

        /// <summary>
        /// GetCreditCards get Credit Cards Customer with provided details and with empty card processor, will retrun all credit cards.
        /// </summary>
        /// <param name="cardRequest">Customer request to be fetch</param>
        /// <returns>CreditCardResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/GetCustomerPaymentMethods")]
        public CreditCardResponse GetCustomerPaymentMethods([FromBody] GetPaymentMethodsRequest cardRequest)
        {
            CreditCardResponse creditCardResponse;
            // Throw error if customerAccount is null
            if (cardRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                creditCardResponse = new CreditCardResponse(false, message, null, null);
                return creditCardResponse;
            }
            creditCardResponse = ValidateGetPaymethodsRequest(cardRequest);
            if (creditCardResponse != null)
            {
                creditCardResponse = new CreditCardResponse(false, creditCardResponse.Message, null, null);
                return creditCardResponse;
            }
            // Extract the data from parameter
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " customerAccountNo: " + cardRequest.customerAccount);
            creditCardResponse = new CreditCardResponse(false, "", null, null);
            ErpCustomerPaymentInfo erpCustomerPaymentInfo = new ErpCustomerPaymentInfo(false, "", null, null);
            try
            {
                ErpGetCardRequest erpGetCardRequest = MapGetEcomRequestToErp(cardRequest);
                var erCreditCardController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);

                erpCustomerPaymentInfo = erCreditCardController.GetCustomerPaymentMethods(erpGetCardRequest, GetRequestGUID(Request));

                if (erpCustomerPaymentInfo.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCustomerPaymentInfo));
                    //erpCreditCardResponse = new ErpCreditCardResponse(erpCreditCardResponse.Success, erpCreditCardResponse.Message, erpCreditCardResponse.CreditCards);
                    creditCardResponse = new CreditCardResponse(erpCustomerPaymentInfo.Success, erpCustomerPaymentInfo.Message, erpCustomerPaymentInfo.CreditCards, erpCustomerPaymentInfo.BankAccounts);
                }
                else
                {
                    //erpCreditCardResponse = new ErpCreditCardResponse(erpCreditCardResponse.Success, erpCreditCardResponse.Message, null);
                    creditCardResponse = new CreditCardResponse(erpCustomerPaymentInfo.Success, erpCustomerPaymentInfo.Message, null, null);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                creditCardResponse = new CreditCardResponse(false, message, null, null);
                return creditCardResponse;
            }
            return creditCardResponse;
        }
        /// <summary>
        /// CreateCreditCard create Credit Cards of Customer with provided details.
        /// </summary>
        /// <param name="creditCardRequest">Credit Card request to be created</param>
        /// <returns>CreditCardResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/CreateCustomerPaymentMethod")]
        public CreditCardResponse CreateCustomerPaymentMethod([FromBody] PaymentMethodRequest creditCardRequest)
        {
            CreditCardResponse creditCardResponse;
            creditCardResponse = new CreditCardResponse(false, "", null, null);

            //Extract the data from parameter
            ErpCreditCardResponse erpCreditCardResponse = null;
            try
            {
                creditCardResponse = ValidateCreditCardRequest(creditCardRequest, "", false);
                if (creditCardResponse != null)
                {
                    return creditCardResponse;
                }
                else
                {
                    ErpCreateCardRequest erpRequest = GetEcomRequestToErpRequest(creditCardRequest);
                    var erCreditCardController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                    if (erpRequest.TenderLine.TenderTypeId.ToUpper() != PaymentCon.SEPA.ToString())
                    {
                        erpCreditCardResponse = erCreditCardController.CreateCustomerPaymentMethod(erpRequest, GetRequestGUID(Request));

                        if (erpCreditCardResponse.Success)
                        {
                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreditCardResponse));
                            creditCardResponse = new CreditCardResponse(true, erpCreditCardResponse.Message, erpCreditCardResponse.CreditCards, null);
                        }
                        else if (!erpCreditCardResponse.Success)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                            creditCardResponse = new CreditCardResponse(false, erpCreditCardResponse.Message, null, null);
                        }

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreditCardResponse));
                    }
                    else
                    {
                        var bankAccountresponse = erCreditCardController.CreateCustomerBankAccount(erpRequest);
                        if (bankAccountresponse.Success)
                        {
                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreditCardResponse));
                            creditCardResponse = new CreditCardResponse(true, bankAccountresponse.Message, null, bankAccountresponse.BankAccount);
                        }
                        else
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                            creditCardResponse = new CreditCardResponse(false, bankAccountresponse.Message, null, bankAccountresponse.BankAccount);
                        }

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(bankAccountresponse));
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                creditCardResponse = new CreditCardResponse(false, message, null, null);
                return creditCardResponse;
            }
            return creditCardResponse;
        }

        /// <summary>
        /// CreateCreditCard create Credit Cards of Customer with provided details.
        /// </summary>
        /// <param name="creditCardRequest">Credit Card request to be created</param>
        /// <returns>CreditCardResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/UpdateCustomerPaymentMethod")]
        public CreditCardResponse UpdateCustomerPaymentMethod([FromBody] PaymentMethodRequest creditCardRequest)
        {
            CreditCardResponse creditCardResponse;
            creditCardResponse = new CreditCardResponse(false, "", null, null);

            // Extract the data from parameter
            ErpCreditCardResponse erpCreditCardResponse = null;
            try
            {
                creditCardResponse = ValidateCreditCardRequest(creditCardRequest, creditCardRequest.CardNumber, true);

                if (creditCardResponse != null)
                {
                    return creditCardResponse;
                }
                else
                {
                    ErpEditCardRequest erpRequest = GetEcomEditRequestToErpRequest(creditCardRequest);
                    var erCreditCardController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);

                    erpCreditCardResponse = erCreditCardController.UpdateCustomerPaymentMethod(erpRequest, GetRequestGUID(Request));

                    if (erpCreditCardResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreditCardResponse));
                        creditCardResponse = new CreditCardResponse(true, erpCreditCardResponse.Message, erpCreditCardResponse.CreditCards, null);
                    }
                    else if (!erpCreditCardResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                        creditCardResponse = new CreditCardResponse(false, erpCreditCardResponse.Message, null, null);
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreditCardResponse));
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                creditCardResponse = new CreditCardResponse(false, message, null, null);
                return creditCardResponse;
            }
            return creditCardResponse;
        }

        /// <summary>
        /// Get customer invoices from D365 by customer id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/GetCustomerInvoices")]
        public CustomerInvoicesResponse GetCustomerInvoices([FromBody] CustomerInvoicesRequest request)
        {
            CustomerInvoicesResponse customerInvoiceResponse = new CustomerInvoicesResponse(false, null, "");
            try
            {
                customerInvoiceResponse = ValidateCustomerInvoiceRequest(request);
                if (customerInvoiceResponse != null)
                {
                    return customerInvoiceResponse;
                }
                else
                {
                    ERPDataModels.Custom.CustomerInvoiceRequest customerInvoiceRequest = new ERPDataModels.Custom.CustomerInvoiceRequest();
                    customerInvoiceRequest.CustomerAccount = request.CustomerAccount;
                    customerInvoiceRequest.LicenseID = request.LicenseID;
                    customerInvoiceRequest.TransactionType = request.TransactionType;

                    var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                    var clResponse = erpCustomerController.GetCustomerInvoices(customerInvoiceRequest, GetRequestGUID(Request));

                    if (clResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        customerInvoiceResponse = new CustomerInvoicesResponse(true, clResponse.CustomerInvoices, clResponse.Message);
                    }
                    else if (!clResponse.Status)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                        customerInvoiceResponse = new CustomerInvoicesResponse(false, null, clResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerInvoiceResponse = new CustomerInvoicesResponse(false, null, message);
                return customerInvoiceResponse;
            }
            return customerInvoiceResponse;
        }

        /// <summary>
        /// Deletes customer credit card in D365 based on provided card recid.
        /// </summary>
        /// <param name="request">Card request to be deleted.</param>
        /// <returns>DeleteCustomerPaymentMethodResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/DeleteCustomerPaymentMethod")]
        public DeleteCustomerPaymentMethodResponse DeleteCustomerPaymentMethod([FromBody] DeleteCustomerPaymentMethodRequest request)
        {
            DeleteCustomerPaymentMethodResponse deleteCustomerPaymentMethodResponse = new DeleteCustomerPaymentMethodResponse(false, "", "");
            try
            {
                deleteCustomerPaymentMethodResponse = ValidateDeleteCustomerPaymentMethodRequest(request);
                if (deleteCustomerPaymentMethodResponse != null)
                {
                    return deleteCustomerPaymentMethodResponse;
                }
                else
                {
                    ERPDataModels.Custom.ErpDeleteCustomerPaymentMethodRequest deletePaymentMethodRequest = new ERPDataModels.Custom.ErpDeleteCustomerPaymentMethodRequest();
                    deletePaymentMethodRequest.CardRecId = request.CardRecId;
                    deletePaymentMethodRequest.BankAccountRecId = request.BankAccountRecId;

                    var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                    var clResponse = erpCustomerController.DeleteCustomerPaymentMethod(deletePaymentMethodRequest);

                    if (clResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        deleteCustomerPaymentMethodResponse = new DeleteCustomerPaymentMethodResponse(true, "", clResponse.Message);
                    }
                    else if (!clResponse.Status)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, clResponse.Message);
                        if (string.Equals(clResponse.Message, "Unable to delete record, transaction exists against payment method"))
                        {
                            deleteCustomerPaymentMethodResponse = new DeleteCustomerPaymentMethodResponse(false, "VSICL400013", clResponse.Message);
                        }
                        else
                        {
                            deleteCustomerPaymentMethodResponse = new DeleteCustomerPaymentMethodResponse(false, "", clResponse.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                deleteCustomerPaymentMethodResponse = new DeleteCustomerPaymentMethodResponse(false, "", message);
                return deleteCustomerPaymentMethodResponse;
            }

            return deleteCustomerPaymentMethodResponse;
        }
        /// <summary>
        /// CreateUpdateCustomer update or create customer 
        /// </summary>
        /// <param name="createUpdateCustomerRequest">Create Update Customer transaction request to be created</param>
        /// <returns>CustomerRespone</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Customer/CreateUpdateCustomer")]
        public ErpCustomerCreateUpdateResponse CreateUpdateCustomer([FromBody] EcomCustomerCreateRequest createUpdateCustomerRequest)
        {
            ErpCustomerCreateUpdateResponse erpCustomerCreateUpdateResponse = new ErpCustomerCreateUpdateResponse(false, string.Empty, null);
            ErpCustomer erpCustomer = new ErpCustomer();
            try
            {
                if (createUpdateCustomerRequest?.Customer != null)
                {
                    createUpdateCustomerRequest.Customer.IsAsyncCustomer = false;

                    //VSTS: 41099 Ecom always use Ecom language in create call.
                    createUpdateCustomerRequest.Customer.SwapLanguage = true;

                    if (string.IsNullOrWhiteSpace(createUpdateCustomerRequest.CustomerAccountNumber))
                    {
                        CustomerResponse response = this.CreateCustomerMethod(createUpdateCustomerRequest, MethodBase.GetCurrentMethod().Name);
                        if (response != null)
                        {
                            if (response.CustomerInfo?.GetType() == typeof(ErpCustomer))
                            {
                                erpCustomer = (ErpCustomer)response.CustomerInfo;
                            }
                            erpCustomerCreateUpdateResponse.Status = response.Status;
                            erpCustomerCreateUpdateResponse.Message = response.Message;
                        }
                    }
                    else
                    {
                        EcomCustomerUpdateRequest customerContactPersonCreateRequest = new EcomCustomerUpdateRequest();
                        customerContactPersonCreateRequest.ContactPerson = null;
                        customerContactPersonCreateRequest.Customer = createUpdateCustomerRequest.Customer;
                        customerContactPersonCreateRequest.UseMapping = createUpdateCustomerRequest.UseMapping;
                        customerContactPersonCreateRequest.CustomerAccountNumber = createUpdateCustomerRequest.CustomerAccountNumber;

                        MergeCreateCustomerContactPersonResponse response = MergeUpdateCustomerAndContactPerson(customerContactPersonCreateRequest, MethodBase.GetCurrentMethod().Name);
                        if (response != null)
                        {
                            if (response.CustomerInfo?.GetType() == typeof(ErpCustomer))
                            {
                                erpCustomer = (ErpCustomer)response.CustomerInfo;
                            }
                            erpCustomerCreateUpdateResponse.Status = response.Status;
                            erpCustomerCreateUpdateResponse.Message = response.Message;
                        }
                    }
                }
                else
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    erpCustomerCreateUpdateResponse = new ErpCustomerCreateUpdateResponse(false, message, null);
                }

                if (erpCustomer != null && erpCustomerCreateUpdateResponse.Status)
                {
                    ERPDataModels.Custom.Responses.CustomerInformation customerInformation = new ERPDataModels.Custom.Responses.CustomerInformation();
                    customerInformation.AccountNumber = erpCustomer.AccountNumber;
                    customerInformation.DirectoryPartyRecordId = erpCustomer.DirectoryPartyRecordId;
                    customerInformation.RecordId = erpCustomer.RecordId;
                    erpCustomerCreateUpdateResponse.CustomerInfo = customerInformation;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                erpCustomerCreateUpdateResponse = new ErpCustomerCreateUpdateResponse(false, message, null);
            }

            return erpCustomerCreateUpdateResponse;
        }
        #endregion

        #region "Private Methods"

        private MergeCreateCustomerContactPersonResponse MergeUpdateCustomerAndContactPerson(EcomCustomerUpdateRequest updateCustomerRequest, string methodName)
        {
            CustomerResponse customerResponse;
            CLContactPersonResponse contactPersonResponse = new CLContactPersonResponse(false, string.Empty, null);
            // ErpUpdateCustomerContactPersonResponse erpUpdateCustomerContactPersonResponse;
            MergeCreateCustomerContactPersonResponse mergeCreateCustomerContactPersonResponse;
            UpdateCustomerRequest erpUpdateCustomerRequest = new UpdateCustomerRequest();
            ErpCustomer erpCustomerMap = new ErpCustomer();
            ErpContactPerson erpContactPerson = new ErpContactPerson();

            try
            {
                // [MB] - TV - BR 3.0 - 12539 - Start
                for (int i = 0; i < updateCustomerRequest.Customer.Addresses.Count; i++)
                {
                    updateCustomerRequest.Customer.Addresses[i].BuildingCompliment = updateCustomerRequest.Customer.Addresses[i].Street2;
                }
                // [MB] - TV - BR 3.0 - 12539 - End

                if (updateCustomerRequest.ContactPerson != null)
                {
                    updateCustomerRequest.ContactPerson.TMVSourceSystem = string.IsNullOrWhiteSpace(updateCustomerRequest.ContactPerson.TMVSourceSystem) ? SourceSystem.WEB.ToString() : updateCustomerRequest.ContactPerson.TMVSourceSystem;
                }

                erpCustomerMap = _mapper.Map<CommerceLink.EcomDataModel.EcomCustomer, ErpCustomer>(updateCustomerRequest.Customer);
                erpContactPerson = _mapper.Map<CommerceLink.EcomDataModel.EcomContactPerson, ErpContactPerson>(updateCustomerRequest.ContactPerson);
                customerResponse = this.ValidateUpdateCustomerRequest(updateCustomerRequest, methodName);
                if (customerResponse != null)
                {
                    // return new ErpUpdateCustomerContactPersonResponse(false, customerResponse.Message, null, null);
                    return new MergeCreateCustomerContactPersonResponse(false, null, null, customerResponse.Message);
                }
                //validate Contact Person 
                contactPersonResponse = this.ValidateContactPerson(updateCustomerRequest.ContactPerson, !updateCustomerRequest.CreateNewContactPerson);
                if (contactPersonResponse != null)
                {
                    // return new ErpUpdateCustomerContactPersonResponse(false, contactPersonResponse.ErrorMessage, null, null);
                    return new MergeCreateCustomerContactPersonResponse(false, null, null, contactPersonResponse.ErrorMessage);
                }
                else
                {
                    LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);

                    if (contactPersonResponse == null && updateCustomerRequest.ContactPerson == null)
                    {
                        erpContactPerson = new ErpContactPerson();
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(updateCustomerRequest.Customer));
                    erpCustomerMap.AccountNumber = updateCustomerRequest.CustomerAccountNumber;

                    if (string.IsNullOrWhiteSpace(updateCustomerRequest.ToString()))
                    {
                        erpCustomerMap.CustomerType = ErpCustomerType.Person;
                        erpCustomerMap.CustomerTypeValue = (int)ErpCustomerType.Person;
                    }
                    else
                    {
                        erpCustomerMap.CustomerTypeValue = (int)updateCustomerRequest.Customer.CustomerType;
                    }

                    if (updateCustomerRequest.Customer.Addresses != null)
                    {
                        foreach (var address in erpCustomerMap.Addresses)
                        {
                            if (!string.IsNullOrEmpty(address.AddressType.ToString()))
                            {
                                address.AddressType = (ErpAddressType)address.AddressType;
                                address.AddressTypeValue = (int)address.AddressType;
                            }
                            else
                            {
                                if (address.IsPrimary)
                                {
                                    address.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                                    address.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                                }
                                else
                                {
                                    address.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                                    address.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                                }
                            }
                            //getting customer phone number if not provided from shipping address
                            address.Phone = string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Phone) ? address.Phone : updateCustomerRequest.Customer.Phone;

                            //NS: TMV FDD-030
                            address.IsPrivate = configurationHelper.GetSetting(SALESORDER.AX_Address_IsPrivate).BoolValue();

                            if (string.IsNullOrEmpty(address.ThreeLetterISORegionName))
                            {
                                //NS: Fix for Magento issue, No this channel will only create addresses of single country confgured in below key
                                address.ThreeLetterISORegionName = configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName).ToString();
                            }
                        }
                    }

                    // [MB] - TV - BR 3.0 - 15878 - new clause of swapLanguage added in condition - Start
                    //Language code swapping for D365
                    if (updateCustomerRequest.Customer != null &&
                        !string.IsNullOrEmpty(updateCustomerRequest.Customer.Language) &&
                        updateCustomerRequest.Customer.SwapLanguage)
                    {
                        erpCustomerMap.Language = languageCodes.GetErpLanguageCode(updateCustomerRequest.Customer.Language);
                    }
                    if (!updateCustomerRequest.CreateNewContactPerson)
                    {
                        if (erpContactPerson != null && !string.IsNullOrEmpty(erpContactPerson.Language) && updateCustomerRequest.ContactPerson.SwapLanguage)
                        {
                            erpContactPerson.Language = languageCodes.GetErpLanguageCode(erpContactPerson.Language);
                        }
                    }
                    // [MB] - TV - BR 3.0 - 15878 - End

                    ContactPersonRequest newContactPerson = new ContactPersonRequest();
                    if (updateCustomerRequest.CreateNewContactPerson)
                    {
                        
                        newContactPerson = _mapper.Map<ErpContactPerson, ContactPersonRequest>(erpContactPerson);
                        erpContactPerson = new ErpContactPerson();
                    }

                    //VSTS: 41099 
                    if (string.IsNullOrEmpty(erpCustomerMap.Language) ||
                        (!erpCustomerMap.SwapLanguage && !languageCodes.ValidateErpLanguageCode(erpCustomerMap.Language))
                       )
                    {
                        erpCustomerMap.Language = configurationHelper.GetSetting(APPLICATION.Default_Culture);
                    }

                    var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                    ErpUpdateCustomerContactPersonResponse erpCustomer = erpCustomerController.MergeUpdateCustomer(erpCustomerMap, updateCustomerRequest.CustomerAccountNumber, erpContactPerson, GetRequestGUID(Request));
                    ContactPersonResponse newContactPersonResponse = new ContactPersonResponse(false, "", null);
                    if (updateCustomerRequest.CreateNewContactPerson)
                    {
                        ContactPersonController contactPersonController = new ContactPersonController();
                        newContactPersonResponse = contactPersonController.CreateContactPerson(newContactPerson);
                        erpCustomer.ContactPerson = _mapper.Map<eComContactPerson, ErpContactPerson>(newContactPersonResponse.ContactPerson);
                    }
                    if (erpCustomer.Success == true)
                    {
                        //ErpCustomer deserializeCustomer = JsonConvert.DeserializeObject<ErpCustomer>(erpCustomer.Customer);
                        ErpCustomer deserializeCustomer = (ErpCustomer)(erpCustomer.Customer);

                        // [MB] - TV - BR 3.0 - 12539 - Start
                        for (int i = 0; i < deserializeCustomer.Addresses.Count; i++)
                        {
                            deserializeCustomer.Addresses[i].Street2 = deserializeCustomer.Addresses[i].BuildingCompliment;
                            deserializeCustomer.Addresses[i].BuildingCompliment = String.Empty;
                        }
                        // [MB] - TV - BR 3.0 - 12539 - End

                        // [MB] - TV - BR 3.0 - 15878 - new clause of swapLanguage added in condition - Start
                        //Language code swapping for Ecom
                        if (erpCustomer != null && !string.IsNullOrEmpty(deserializeCustomer.Language) && updateCustomerRequest.Customer.SwapLanguage)
                        {
                            deserializeCustomer.Language = languageCodes.GetEcomLanguageCode(deserializeCustomer.Language);
                        }
                        deserializeCustomer.SwapLanguage = updateCustomerRequest.Customer.SwapLanguage;

                        ErpContactPerson deserializeContactPerson = (ErpContactPerson)(erpCustomer.ContactPerson);
                        if (deserializeContactPerson != null && !string.IsNullOrEmpty(deserializeContactPerson.Language) && updateCustomerRequest.ContactPerson.SwapLanguage)
                        {
                            deserializeContactPerson.Language = languageCodes.GetEcomLanguageCode(deserializeContactPerson.Language);
                        }
                        erpCustomer.ContactPerson = deserializeContactPerson;
                        // [MB] - TV - BR 3.0 - 15878 - End
                        //Language code swapping of contact person for Ecom
                        if (erpCustomer.ContactPerson != null)
                        {
                            ErpContactPerson deserializedContactPersion = (ErpContactPerson)(erpCustomer.ContactPerson);
                            deserializedContactPersion.Language = languageCodes.GetEcomLanguageCode(deserializedContactPersion.Language);
                        }

                        object customer = this.GetCustomerDataWithMapping(updateCustomerRequest.UseMapping, deserializeCustomer);
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customer));
                        customerResponse = new CustomerResponse(true, customer, "Customer Updated Successfully");
                    }

                    //erpUpdateCustomerContactPersonResponse = new ErpUpdateCustomerContactPersonResponse(erpCustomer.Success,
                    //    erpCustomer.Message, erpCustomer.Customer, erpCustomer.ContactPerson);

                    if (erpCustomer.ContactPerson != null)
                    {
                        mergeCreateCustomerContactPersonResponse = new MergeCreateCustomerContactPersonResponse(erpCustomer.Success,
                            erpCustomer.Customer,
                            convertErpContactPersonToeComContactPerson((ErpContactPerson)erpCustomer.ContactPerson, updateCustomerRequest.ContactPerson.SwapLanguage),
                            erpCustomer.Message);
                    }
                    else
                    {
                        mergeCreateCustomerContactPersonResponse = new MergeCreateCustomerContactPersonResponse(erpCustomer.Success,
                            erpCustomer.Customer,
                            newContactPersonResponse.ErrorMessage,
                            erpCustomer.Message);
                    }



                    // return erpUpdateCustomerContactPersonResponse;
                    return mergeCreateCustomerContactPersonResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                customerResponse = new CustomerResponse(false, "", message);
                // return new ErpUpdateCustomerContactPersonResponse(false, customerResponse.Message, null, null);
                return new MergeCreateCustomerContactPersonResponse(false, null, null, customerResponse.Message);
            }
        }


        private EcomContactPerson convertErpContactPersonToeComContactPerson(ErpContactPerson erpContactPerson, bool swapLanguage)
        {
            EcomContactPerson ecomContactPerson = new EcomContactPerson();
            
            ecomContactPerson = _mapper.Map<ErpContactPerson, EcomContactPerson>(erpContactPerson);
            ecomContactPerson.SwapLanguage = swapLanguage;

            return ecomContactPerson;
        }

        private CustomerResponse CreateNewCustomer(CustomerCreateRequest erpCreateCustomerRequest)
        {
            CustomerResponse customerResponse = new CustomerResponse(false, null, null);
            //Set RecId and DataTypes of customer attributes

            SetCustomerAttributesReqFields(erpCreateCustomerRequest);

            // [MB] - TV - BR 3.0 - 15007 - new clause of swapLanguage added in condition - Start
            //Language code swapping for D365
            if (erpCreateCustomerRequest.Customer != null && !string.IsNullOrEmpty(erpCreateCustomerRequest.Customer.Language) && erpCreateCustomerRequest.Customer.SwapLanguage)
            {
                LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                erpCreateCustomerRequest.Customer.Language = languageCodes.GetErpLanguageCode(erpCreateCustomerRequest.Customer.Language);
            }
            // [MB] - TV - BR 3.0 - 15007 - End

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreateCustomerRequest.Customer));
            erpCreateCustomerRequest.Customer.AccountNumber = new Guid().ToString();

            if (string.IsNullOrWhiteSpace(erpCreateCustomerRequest.Customer.CustomerType.ToString()))
            {
                erpCreateCustomerRequest.Customer.CustomerType = ErpCustomerType.Person;
                erpCreateCustomerRequest.Customer.CustomerTypeValue = (int)ErpCustomerType.Person;
            }
            else
            {
                erpCreateCustomerRequest.Customer.CustomerTypeValue = (int)erpCreateCustomerRequest.Customer.CustomerType;
            }

            if (erpCreateCustomerRequest.Customer.Addresses != null)
            {
                foreach (var address in erpCreateCustomerRequest.Customer.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.AddressType.ToString()))
                    {
                        address.AddressType = (ErpAddressType)address.AddressType;
                        address.AddressTypeValue = (int)address.AddressType;
                    }
                    else
                    {
                        if (address.IsPrimary)
                        {
                            address.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                            address.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                        }
                        else
                        {
                            address.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                            address.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                        }
                    }
                    //getting customer phone number if not provided from shipping address
                    address.Phone = string.IsNullOrWhiteSpace(erpCreateCustomerRequest.Customer.Phone) ? address.Phone : erpCreateCustomerRequest.Customer.Phone;

                    //NS: TMV FDD-030
                    address.IsPrivate = configurationHelper.GetSetting(SALESORDER.AX_Address_IsPrivate).BoolValue();

                    if (string.IsNullOrWhiteSpace(address.ThreeLetterISORegionName))
                    {
                        //NS: Fix for Magento issue, No this channel will only create addresses of single country confgured in below key
                        address.ThreeLetterISORegionName = configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName).ToString();
                    }
                }
            }

            var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);

            ErpCustomer erpCustomer = erpCustomerController.AssignCustomer(erpCreateCustomerRequest.Customer, GetRequestGUID(Request), true);

            // [MB] - TV - BR 3.0 - 15007 - new clause of swapLanguage added in condition - Start
            //Language code swapping for Ecom
            erpCustomer.SwapLanguage = erpCreateCustomerRequest.Customer.SwapLanguage;
            if (erpCustomer != null && !string.IsNullOrEmpty(erpCustomer.Language) && erpCreateCustomerRequest.Customer.SwapLanguage)
            {
                LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                erpCustomer.Language = languageCodes.GetEcomLanguageCode(erpCustomer.Language);
            }
            // [MB] - TV - BR 3.0 - 15007 - End

            // [MB] - TV - BR 3.0 - 12539 - Start
            for (int i = 0; i < erpCustomer.Addresses.Count; i++)
            {
                erpCustomer.Addresses[i].Street2 = erpCustomer.Addresses[i].BuildingCompliment;
                erpCustomer.Addresses[i].BuildingCompliment = String.Empty;
            }
            // [MB] - TV - BR 3.0 - 12539 - End

            object customer = this.GetCustomerDataWithMapping(erpCreateCustomerRequest.UseMapping, erpCustomer);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customer));
            customerResponse = new CustomerResponse(true, customer, "");

            return customerResponse;
        }
        private XmlDocument CreateCustomerXmlDocument(ErpCustomer erpCustomer)
        {
            CustomLogger.LogDebugInfo("Enter in CreateProductFile()", currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                string fileNameProduct = configurationHelper.GetSetting(PRODUCT.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                return xmlHelper.GenerateXmlUsingTemplate(XmlTemplateHelper.XmlSourceDirection.CREATE, erpCustomer);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetCustomerDataWithMapping converts the Customer data into required format.
        /// </summary>
        /// <param name="useMapping"></param>
        /// <param name="erpCustomer"></param>
        /// <returns></returns>
        private object GetCustomerDataWithMapping(bool useMapping, ErpCustomer erpCustomer)
        {
            if (useMapping)
            {
                erpCustomer.CustomerAddresses = new List<ErpAddress>();
                foreach (var address in erpCustomer.Addresses)
                {
                    if (address.IsPrimary == true)
                    {
                        erpCustomer.CustomerAddresses.Add(address);
                    }
                }

                XmlDocument xmlDocument = CreateCustomerXmlDocument(erpCustomer);
                var data = xmlDocument.SerializeToJson();
                return Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            }
            else
            {
                return erpCustomer;
            }
        }

        /// <summary>
        /// ValidateCustomerCreateRequest validate Create Customer Request Object.
        /// </summary>
        /// <param name="createCustomerRequest"></param>
        /// <returns></returns>
        private CustomerResponse ValidateCustomerCreateRequest(CustomerCreateRequest createCustomerRequest, string methodName)
        {
            if (createCustomerRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                //throw new System.ArgumentException("Invalid parameter value for 'createCustomerRequest'.", "createCustomerRequest", new HttpResponseException(HttpStatusCode.BadRequest));
                return new CustomerResponse(false, null, message);
            }
            else
            {
                //Validation for Person type customer
                if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.CustomerType.ToString()) && createCustomerRequest.Customer.CustomerType.ToString() == ErpCustomerType.Person.ToString())
                {
                    if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.FirstName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "FirstName");
                        return new CustomerResponse(false, null, message);
                    }
                    else if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.LastName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LastName");
                        return new CustomerResponse(false, null, message);
                    }
                }

                if (createCustomerRequest.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer");
                    return new CustomerResponse(false, "", message);
                }
                else if (string.IsNullOrEmpty(createCustomerRequest.Customer.EcomCustomerId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "EcomCustomerId");
                    return new CustomerResponse(false, null, message);
                }
                else if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Email))
                {
                    var sourceSystem = createCustomerRequest.Customer.ExtensionProperties.FirstOrDefault(p => p.Key == "TMVSOURCESYSTEM");
                    if (sourceSystem != null && sourceSystem.Value.StringValue.ToUpper() == SourceSystem.EDI.ToString())
                    {
                        var relationshipType = createCustomerRequest.Customer.ExtensionProperties.FirstOrDefault(p => p.Key == "RelationshipType");
                        if (relationshipType == null || (relationshipType != null && relationshipType.Value.StringValue != "Reseller"))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Email");
                            return new CustomerResponse(false, null, message);
                        }
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Email");
                        return new CustomerResponse(false, null, message);
                    }
                }
                //else if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Phone))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Phone");
                //    return new CustomerResponse(false, null, message);
                //}
                //NS: TMV FDD-030 
                //Now will handle CustomerGroup in AX
                //else if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.CustomerGroup))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, MethodBase.GetCurrentMethod().Name, "CustomerGroup");
                //    return new CustomerResponse(false, null, message);
                //}
                else if (createCustomerRequest.Customer.CustomerType.ToString() == ErpCustomerType.Organization.ToString() && string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Name))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Name");
                    return new CustomerResponse(false, null, message);
                }

                //VSTS: 41099 Remove validation so the default value set in AX adapter.
                //else if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Language))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Language");
                //    return new CustomerResponse(false, null, message);
                //}

                //Length issue server side validation
                if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Name) && createCustomerRequest.Customer.Name.Length > 100)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Name", "100");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.FirstName) && createCustomerRequest.Customer.FirstName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.MiddleName) && createCustomerRequest.Customer.MiddleName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "MiddleName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.LastName) && createCustomerRequest.Customer.LastName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Email) && createCustomerRequest.Customer.Email.Length > 255)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Email", "255");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.VatNumber) && createCustomerRequest.Customer.VatNumber.Length > 20)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "VatNumber", "20");
                    return new CustomerResponse(false, null, message);
                }
                //else if (!string.IsNullOrWhiteSpace(createCustomerRequest.Customer.Language) && createCustomerRequest.Customer.Language.Length > 7)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Language", "7");
                //    return new CustomerResponse(false, null, message);
                //}

                if (createCustomerRequest.Customer != null)
                {
                    foreach (var address in createCustomerRequest.Customer.Addresses)
                    {
                        if (!string.IsNullOrWhiteSpace(address.ZipCode) && address.ZipCode.Length > 10)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.ZipCode", "10");
                            return new CustomerResponse(false, null, message);
                        }
                        else if (!string.IsNullOrWhiteSpace(address.Name) && address.Name.Length > 60)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.Name", "60");
                            return new CustomerResponse(false, null, message);
                        }

                        if (configurationHelper.GetSetting(CUSTOMER.StateToValidateForCountries).Contains(address.ThreeLetterISORegionName))
                        {
                            if (string.IsNullOrWhiteSpace(address.State))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Address.State");
                                return new CustomerResponse(false, null, message);
                            }
                        }
                    }
                }

                if (methodName  == "CreateUpdateCustomer")
                {
                    if (configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                    {
                        if (createCustomerRequest.Customer.ExtensionProperties.Count > 0)
                        {
                            var extProp = createCustomerRequest.Customer.ExtensionProperties.FirstOrDefault(p => p.Key == "LocalTaxId");
                            if (extProp == null || string.IsNullOrWhiteSpace(extProp.Value.StringValue))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LocalTaxId");
                                return new CustomerResponse(false, null, message);
                            }
                        }
                        else
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LocalTaxId");
                            return new CustomerResponse(false, null, message);
                        }
                    } 
                }
                return null;
            }
        }
        /// <summary>
        /// ValidateCustomerCreateRequest validate Create Customer Request Object.
        /// </summary>
        /// <param name="createCustomerRequest"></param>
        /// <returns></returns>
        private CustomerResponse ValidateUpdateCustomerRequest(EcomCustomerUpdateRequest updateCustomerRequest, string methodName)
        {
            if (updateCustomerRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                //throw new System.ArgumentException("Invalid parameter value for 'createCustomerRequest'.", "createCustomerRequest", new HttpResponseException(HttpStatusCode.BadRequest));
                return new CustomerResponse(false, null, message);
            }
            else
            {
                //validation for ErpAccountNumber
                if (string.IsNullOrEmpty(updateCustomerRequest.CustomerAccountNumber))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ErpCustomerAccountNumber");
                    return new CustomerResponse(false, null, message);
                }
                //Validation for Person type customer
                if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.CustomerType.ToString()) && updateCustomerRequest.Customer.CustomerType.ToString() == ErpCustomerType.Person.ToString())
                {
                    if (string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.FirstName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "FirstName");
                        return new CustomerResponse(false, null, message);
                    }
                    else if (string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.LastName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LastName");
                        return new CustomerResponse(false, null, message);
                    }
                }

                if (updateCustomerRequest.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer");
                    return new CustomerResponse(false, "", message);
                }
                //else if (string.IsNullOrEmpty(updateCustomerRequest.Customer.EcomCustomerId))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "EcomCustomerId");
                //    return new CustomerResponse(false, null, message);
                //}
                else if (string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Email))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Email");
                    return new CustomerResponse(false, null, message);
                }
                //else if (string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Phone))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Phone");
                //    return new CustomerResponse(false, null, message);
                //}
                //NS: TMV FDD-030 
                //Now will handle CustomerGroup in AX
                //else if (string.IsNullOrWhiteSpace(createCustomerRequest.Customer.CustomerGroup))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, MethodBase.GetCurrentMethod().Name, "CustomerGroup");
                //    return new CustomerResponse(false, null, message);
                //}
                else if (updateCustomerRequest.Customer.CustomerType.ToString() == ErpCustomerType.Organization.ToString() && string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Name))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Name");
                    return new CustomerResponse(false, null, message);
                }
                else if (string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Language))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Language");
                    return new CustomerResponse(false, null, message);
                }

                //Length issue server side validation
                if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Name) && updateCustomerRequest.Customer.Name.Length > 100)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Name", "100");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.FirstName) && updateCustomerRequest.Customer.FirstName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.MiddleName) && updateCustomerRequest.Customer.MiddleName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "MiddleName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.LastName) && updateCustomerRequest.Customer.LastName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Email) && updateCustomerRequest.Customer.Email.Length > 255)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Email", "255");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.VatNumber) && updateCustomerRequest.Customer.VatNumber.Length > 20)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "VatNumber", "20");
                    return new CustomerResponse(false, null, message);
                }
                //else if (!string.IsNullOrWhiteSpace(updateCustomerRequest.Customer.Language) && updateCustomerRequest.Customer.Language.Length > 7)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Language", "7");
                //    return new CustomerResponse(false, null, message);
                //}

                if (updateCustomerRequest.Customer != null)
                {
                    foreach (var address in updateCustomerRequest.Customer.Addresses)
                    {
                        if (!string.IsNullOrWhiteSpace(address.ZipCode) && address.ZipCode.Length > 10)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.ZipCode", "10");
                            return new CustomerResponse(false, null, message);
                        }
                        else if (!string.IsNullOrWhiteSpace(address.Name) && address.Name.Length > 60)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.Name", "60");
                            return new CustomerResponse(false, null, message);
                        }
                        
                        if (configurationHelper.GetSetting(CUSTOMER.StateToValidateForCountries).Contains(address.ThreeLetterISORegionName))
                        {
                            if (string.IsNullOrWhiteSpace(address.State))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Address.State");
                                return new CustomerResponse(false, null, message);
                            }
                        }
                    }
                }
                if (methodName == "CreateUpdateCustomer")
                {
                    if (configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                    {
                        if (updateCustomerRequest.Customer.ExtensionProperties.Count > 0)
                        {
                            var extProp = updateCustomerRequest.Customer.ExtensionProperties.FirstOrDefault(p => p.Key == "LocalTaxId");
                            if (extProp == null || string.IsNullOrWhiteSpace(extProp.Value.StringValue))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LocalTaxId");
                                return new CustomerResponse(false, null, message);
                            }
                        }
                        else
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LocalTaxId");
                            return new CustomerResponse(false, null, message);
                        }
                    } 
                }
                return null;
            }
        }
        /// <summary>
        /// ValidateCustomerUpdateRequest validate Update Customer Request Object.
        /// </summary>
        /// <param name="customerUpdateRequest"></param>
        /// <returns></returns>
        private CustomerResponse ValidateCustomerUpdateRequest(dynamic customerUpdateRequest)
        {
            if (customerUpdateRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CustomerResponse(false, "", message);
            }
            else
            {
                if (customerUpdateRequest.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer");
                    return new CustomerResponse(false, "", message);
                }
                else if (string.IsNullOrEmpty(customerUpdateRequest.Customer.RecordId.ToString())
                    || string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.RecordId.ToString())
                    || customerUpdateRequest.Customer.RecordId.ToString() == "0")
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer.RecordId");
                    return new CustomerResponse(false, "", message);
                }
                else if (customerUpdateRequest.Customer.AccountNumber == null || string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.AccountNumber.ToString()))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer.AccountNumber");
                    return new CustomerResponse(false, "", message);
                }

                else if (customerUpdateRequest.Customer.CustomerTypeValue == 0 || string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.CustomerTypeValue.ToString()))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer.CustomerTypeValue");
                    return new CustomerResponse(false, "", message);
                }

                //Validation for Person type customer
                if (customerUpdateRequest.Customer.CustomerTypeValue == Convert.ToInt32(ErpCustomerType.Person))
                {
                    if (customerUpdateRequest.Customer.FirstName == null || string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.FirstName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer.FirstName");
                        return new CustomerResponse(false, null, message);
                    }
                    else if (customerUpdateRequest.Customer.LastName == null || string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.LastName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer.LastName");
                        return new CustomerResponse(false, null, message);
                    }
                }
                //Validation for Organization type customer
                else if (customerUpdateRequest.Customer.CustomerTypeValue == Convert.ToInt32(ErpCustomerType.Organization))
                {
                    if (customerUpdateRequest.Customer.Name == null || string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.Name.ToString()))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customer.Name");
                        return new CustomerResponse(false, "", message);
                    }
                }

                //Length issue server side validation
                if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.Name) && customerUpdateRequest.Customer.Name.Length > 100)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Name", "100");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.FirstName) && customerUpdateRequest.Customer.FirstName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.MiddleName) && customerUpdateRequest.Customer.MiddleName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "MiddleName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.LastName) && customerUpdateRequest.Customer.LastName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.Email) && customerUpdateRequest.Customer.Email.Length > 255)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Email", "255");
                    return new CustomerResponse(false, null, message);
                }
                else if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.VatNumber) && customerUpdateRequest.Customer.VatNumber.Length > 20)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "VatNumber", "20");
                    return new CustomerResponse(false, null, message);
                }
                //else if (!string.IsNullOrWhiteSpace(customerUpdateRequest.Customer.Language) && customerUpdateRequest.Customer.Language.Length > 7)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Language", "7");
                //    return new CustomerResponse(false, null, message);
                //}

                if (customerUpdateRequest.Customer != null && customerUpdateRequest.Customer.Addresses != null)
                {
                    foreach (var address in customerUpdateRequest.Customer.Addresses)
                    {
                        if (!string.IsNullOrWhiteSpace(address.ZipCode) && address.ZipCode.Length > 10)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.ZipCode", "10");
                            return new CustomerResponse(false, null, message);
                        }
                        else if (!string.IsNullOrWhiteSpace(address.Name) && address.Name.Length > 60)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.Name", "60");
                            return new CustomerResponse(false, null, message);
                        }
                        
                        if (configurationHelper.GetSetting(CUSTOMER.StateToValidateForCountries).Contains(address.ThreeLetterISORegionName))
                        {
                            if (string.IsNullOrWhiteSpace(address.State))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Address.State");
                                return new CustomerResponse(false, null, message);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void SetCustomerAttributesReqFields(CustomerCreateRequest erpCreateCustomerRequest)
        {
            try
            {
                if (erpCreateCustomerRequest.Customer.Attributes.Count > 0)
                {
                    foreach (var attribute in erpCreateCustomerRequest.Customer.Attributes)
                    {
                        if (attribute.Name.ToString().Trim().Equals(CUSTOMER.TMVSanctionFlag.ToString()))
                        {
                            attribute.RecordId = long.Parse(configurationHelper.GetSetting(CUSTOMER.TMVSanctionFlag));
                            attribute.DataTypeValue = int.Parse(configurationHelper.GetSetting(CUSTOMER.TMVSanctionFlagDataType));
                        }
                        else if (attribute.Name.ToString().Trim().Equals(CUSTOMER.TMVSanctionStatus.ToString()))
                        {
                            attribute.RecordId = long.Parse(configurationHelper.GetSetting(CUSTOMER.TMVSanctionStatus));
                            attribute.DataTypeValue = int.Parse(configurationHelper.GetSetting(CUSTOMER.TMVSanctionStatusDataType));
                        }
                        else if (attribute.Name.ToString().Trim().Equals(CUSTOMER.TMVIsDuplicateUser.ToString()))
                        {
                            attribute.RecordId = long.Parse(configurationHelper.GetSetting(CUSTOMER.TMVIsDuplicateUser));
                            attribute.DataTypeValue = int.Parse(configurationHelper.GetSetting(CUSTOMER.TMVIsDuplicateUserDataType));
                        }
                        else if (attribute.Name.ToString().Trim().Equals(CUSTOMER.TMVDuplicateCustomerAccountNumber.ToString()))
                        {
                            attribute.RecordId = long.Parse(configurationHelper.GetSetting(CUSTOMER.TMVDuplicateCustomerAccountNumber));
                            attribute.DataTypeValue = int.Parse(configurationHelper.GetSetting(CUSTOMER.TMVDuplicateCustomerAccountNumberDataType));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ValidateCreateContactPerson validates Create Contact Person Object. 
        /// Message: must change in contact person controller if you add a new validation
        /// </summary>
        /// <param name="contactPersonRequest"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        //private CLContactPersonResponse ValidateContactPerson(EcomCustomerContactPersonCreateRequest contactPersonRequest, bool isUpdate)
        private CLContactPersonResponse ValidateContactPerson(dynamic contactPersonRequest, bool isUpdate)
        {
            if (contactPersonRequest == null && isUpdate == true)
            {
                return null;
            }
            else if (contactPersonRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CLContactPersonResponse(false, message, null);
            }
            else
            {
                //Not Required for Create
                if (isUpdate)
                {
                    if (isUpdate && string.IsNullOrWhiteSpace(contactPersonRequest.ContactPersonId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.ContactPersonId");
                        return new CLContactPersonResponse(false, message, null);
                    }
                    else if (contactPersonRequest.DirPartyRecordId == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.DirPartyRecordId");
                        return new CLContactPersonResponse(false, message, null);
                    }
                }

                //if (contactPersonRequest.ContactPerson.ContactForParty == 0)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.ContactForParty");
                //    return new ContactPersonResponse(false, message, null);
                //}
                //else if (string.IsNullOrWhiteSpace(contactPersonRequest.ContactPerson.CustAccount))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.CustAccount");
                //    return new ContactPersonResponse(false, message, null);
                //}
                if (string.IsNullOrWhiteSpace(contactPersonRequest.FirstName))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.FirstName");
                    return new CLContactPersonResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(contactPersonRequest.LastName))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.LastName");
                    return new CLContactPersonResponse(false, message, null);
                }

                //Length issue server side validation
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.FirstName) && contactPersonRequest.FirstName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                    return new CLContactPersonResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.MiddleName) && contactPersonRequest.MiddleName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "MiddleName", "25");
                    return new CLContactPersonResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.LastName) && contactPersonRequest.LastName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                    return new CLContactPersonResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.Email) && contactPersonRequest.Email.Length > 255)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Email", "255");
                    return new CLContactPersonResponse(false, message, null);
                }
                //else if (!string.IsNullOrWhiteSpace(contactPersonRequest.Language) && contactPersonRequest.Language.Length > 7)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Language", "7");
                //    return new CLContactPersonResponse(false, message, null);
                //}
            }
            return null;
        }
        private CreditCardResponse ValidateGetPaymethodsRequest(GetPaymentMethodsRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new CreditCardResponse(false, message, null, null);
                }
                if (String.IsNullOrWhiteSpace(request.customerAccount) && String.IsNullOrWhiteSpace(request.licenseId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.CustomerAccount and CreditCardRequest.LicenseId not found");
                    return new CreditCardResponse(false, message, null, null);
                }
                if (!(base.ValidateCustomerLicenseIDLength(request.licenseId)))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400011, currentStore, "");
                    return new CreditCardResponse(false, message, null, null);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new CreditCardResponse(false, message, null, null);
            }

            return null;
        }
        private CreditCardResponse ValidateCreditCardRequest(PaymentMethodRequest request, string CardNumber, bool isUpdate)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new CreditCardResponse(false, message, null, null);
                }
                if (isUpdate)
                {
                    if (String.IsNullOrWhiteSpace(CardNumber))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.CardNumber");
                        return new CreditCardResponse(false, message, null, null);
                    }
                }
                if (String.IsNullOrWhiteSpace(request.TransactionDate))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.TransactionDate");
                    return new CreditCardResponse(false, message, null, null);
                }
                if (request.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Customer");
                    return new CreditCardResponse(false, message, null, null);
                }
                else
                {

                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerNo))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Customer.CustomerNo");
                        return new CreditCardResponse(false, message, null, null);
                    }
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Customer.CustomerName");
                        return new CreditCardResponse(false, message, null, null);
                    }
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerEmail))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Customer.CustomerEmail");
                        return new CreditCardResponse(false, message, null, null);
                    }
                }
                if (request.CreditCard == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment");
                    return new CreditCardResponse(false, message, null, null);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.CreditCard.ProcessorId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.ProcessorId");
                        return new CreditCardResponse(false, message, null, null);
                    }
                    else if (request.CreditCard.ProcessorId.ToUpper().Equals(PaymentCon.PAYPAL_EXPRESS.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.CreditCard.PayerId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.PayerId");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.ParentTransactionId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.ParentTransactionId");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.Email))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.Email");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.Note))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.Note");
                            return new CreditCardResponse(false, message, null, null);
                        }
                    }
                    else if (request.CreditCard.ProcessorId.ToUpper().Equals(PaymentCon.ALLPAGO_CC.ToString()))
                    {
                        if (request.CreditCard.NumberOfInstallments < 1)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.NumberOfInstallments");
                            return new CreditCardResponse(false, message, null, null);
                        }

                    }
                    else if (request.CreditCard.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_CC.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.CreditCard.BankIdentificationNumberStart))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.BankIdentificationNumberStart");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        //if (string.IsNullOrWhiteSpace(request.CreditCard.ApprovalCode))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.ApprovalCode");
                        //    return new CreditCardResponse(false, message, null, null);
                        //}
                        if (string.IsNullOrWhiteSpace(request.CreditCard.shopperReference))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.shopperReference");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        //if (string.IsNullOrWhiteSpace(request.CreditCard.IssuerCountry))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.IssuerCountry");
                        //    return new CreditCardResponse(false, message, null, null);
                        //}
                    }
                    else if (request.CreditCard.ProcessorId.ToUpper().Equals(PaymentCon.SEPA.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.CreditCard.IBAN))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.CreditCard.IBAN");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.SwiftCode))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.CreditCard.SwiftCode");
                            return new CreditCardResponse(false, message, null, null);
                        }
                    }
                    if (!request.CreditCard.ProcessorId.ToUpper().Equals(PaymentCon.SEPA.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.CreditCard.CardNumber))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.CardNumber");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.Authorization))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.Authorization");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.CardToken))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.CardToken");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (request.CreditCard.expirationMonth == null || request.CreditCard.expirationMonth < 1 || request.CreditCard.expirationMonth > 12)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.expirationMonth");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (request.CreditCard.expirationYear == null || request.CreditCard.expirationYear < 0)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.expirationYear");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.CardType))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.CardType");
                            return new CreditCardResponse(false, message, null, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.CreditCard.TransactionId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.TransactionId");
                            return new CreditCardResponse(false, message, null, null);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(request.CreditCard.CardHolder))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreditCardRequest.Payment.CardHolder");
                        return new CreditCardResponse(false, message, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new CreditCardResponse(false, message, null, null);
            }

            return null;
        }
        private ErpGetCardRequest MapGetEcomRequestToErp(GetPaymentMethodsRequest creditCardRequest)
        {
            ErpGetCardRequest erpGetCardRequest = new ErpGetCardRequest();
            erpGetCardRequest.customerAccount = creditCardRequest.customerAccount;
            erpGetCardRequest.licenseId = creditCardRequest.licenseId;
            erpGetCardRequest.cardProcessors = creditCardRequest.cardProcessors;
            return erpGetCardRequest;
        }
        private ErpCreateCardRequest GetEcomRequestToErpRequest(PaymentMethodRequest request)
        {
            ErpCreateCardRequest erpRequest = new ErpCreateCardRequest();

            erpRequest.Currency = request.Currency;
            erpRequest.TransactionDate = string.IsNullOrWhiteSpace(request.TransactionDate) == true ? erpRequest.TransactionDate : DateTimeOffset.Parse(request.TransactionDate);
            erpRequest.TransactionDate = new DateTimeOffset(erpRequest.TransactionDate.Year, erpRequest.TransactionDate.Month, erpRequest.TransactionDate.Day, erpRequest.TransactionDate.Hour, erpRequest.TransactionDate.Minute, erpRequest.TransactionDate.Second, TimeSpan.Zero);

            if (request.Customer != null)
            {
                erpRequest.Customer.CustomerNo = request.Customer.CustomerNo;
                erpRequest.Customer.CustomerName = request.Customer.CustomerName;
                erpRequest.Customer.CustomerEmail = request.Customer.CustomerEmail;

                if (request.Customer.BillingAddress != null)
                {
                    erpRequest.Customer.BillingAddress.Name = request.Customer.BillingAddress.Name;
                    erpRequest.Customer.BillingAddress.Street = request.Customer.BillingAddress.Street;
                    erpRequest.Customer.BillingAddress.City = request.Customer.BillingAddress.City;
                    erpRequest.Customer.BillingAddress.ZipCode = request.Customer.BillingAddress.ZipCode;
                    if (request.Customer.BillingAddress.State != null)
                    {
                        erpRequest.Customer.BillingAddress.State = request.Customer.BillingAddress.State;
                    }
                    erpRequest.Customer.BillingAddress.ThreeLetterISORegionName = request.Customer.BillingAddress.ThreeLetterISORegionName;
                    erpRequest.Customer.BillingAddress.Phone = request.Customer.BillingAddress.Phone;
                }

                erpRequest.SalesOrder.CustomerId = request.Customer.CustomerNo;
                erpRequest.SalesOrder.CurrencyCode = request.Currency;
                erpRequest.SalesOrder.BillingAddress = erpRequest.Customer.BillingAddress;
            }

            if (request.CreditCard != null)
            {
                erpRequest.TenderLine.CardTypeId = request.CreditCard.CardType;
                erpRequest.TenderLine.MaskedCardNumber = request.CreditCard.CardNumber;
                erpRequest.TenderLine.CardOrAccount = request.CreditCard.CardHolder;
                erpRequest.TenderLine.Authorization = request.CreditCard.Authorization;
                erpRequest.TenderLine.CardToken = request.CreditCard.CardToken;
                erpRequest.TenderLine.ExpMonth = request.CreditCard.expirationMonth;
                erpRequest.TenderLine.ExpYear = request.CreditCard.expirationYear;
                erpRequest.TenderLine.PayerId = request.CreditCard.PayerId;
                erpRequest.TenderLine.ParentTransactionId = request.CreditCard.ParentTransactionId;
                erpRequest.TenderLine.Email = request.CreditCard.Email;
                erpRequest.TenderLine.Note = request.CreditCard.Note;
                erpRequest.TenderLine.TenderTypeId = request.CreditCard.ProcessorId;
                erpRequest.TenderLine.Amount = request.CreditCard.Amount;
                erpRequest.TenderLine.CustomAttributes = new List<KeyValuePair<string, string>>();
                erpRequest.TenderLine.CustomAttributes.Add(new KeyValuePair<string, string>("transaction-id", request.CreditCard.TransactionId));
                erpRequest.TransactionId = request.CreditCard.TransactionId;
                erpRequest.TenderLine.LineNumber = 1;
                erpRequest.TenderLine.NumberOfInstallments = request.CreditCard.NumberOfInstallments;
                erpRequest.TenderLine.BankIdentificationNumberStart = request.CreditCard.BankIdentificationNumberStart;
                erpRequest.TenderLine.ApprovalCode = request.CreditCard.ApprovalCode;
                erpRequest.TenderLine.shopperReference = request.CreditCard.shopperReference;
                erpRequest.TenderLine.IssuerCountry = request.CreditCard.IssuerCountry;
                erpRequest.TenderLine.IBAN = request.CreditCard.IBAN;
                erpRequest.TenderLine.SwiftCode = request.CreditCard.SwiftCode;
                erpRequest.TenderLine.BankName = request.CreditCard.BankName;
            }

            return erpRequest;
        }
        private ErpEditCardRequest GetEcomEditRequestToErpRequest(PaymentMethodRequest request)
        {
            ErpEditCardRequest erpRequest = new ErpEditCardRequest();

            erpRequest.Currency = request.Currency;
            erpRequest.CardNumber = request.CardNumber;
            erpRequest.TransactionDate = string.IsNullOrWhiteSpace(request.TransactionDate) == true ? erpRequest.TransactionDate : DateTimeOffset.Parse(request.TransactionDate);
            erpRequest.TransactionDate = new DateTimeOffset(erpRequest.TransactionDate.Year, erpRequest.TransactionDate.Month, erpRequest.TransactionDate.Day, erpRequest.TransactionDate.Hour, erpRequest.TransactionDate.Minute, erpRequest.TransactionDate.Second, TimeSpan.Zero);

            if (request.Customer != null)
            {
                erpRequest.Customer.CustomerNo = request.Customer.CustomerNo;
                erpRequest.Customer.CustomerName = request.Customer.CustomerName;
                erpRequest.Customer.CustomerEmail = request.Customer.CustomerEmail;

                if (request.Customer.BillingAddress != null)
                {
                    erpRequest.Customer.BillingAddress.Name = request.Customer.BillingAddress.Name;
                    erpRequest.Customer.BillingAddress.Street = request.Customer.BillingAddress.Street;
                    erpRequest.Customer.BillingAddress.City = request.Customer.BillingAddress.City;
                    erpRequest.Customer.BillingAddress.ZipCode = request.Customer.BillingAddress.ZipCode;
                    if (request.Customer.BillingAddress.State != null)
                    {
                        erpRequest.Customer.BillingAddress.State = request.Customer.BillingAddress.State;
                    }
                    erpRequest.Customer.BillingAddress.ThreeLetterISORegionName = request.Customer.BillingAddress.ThreeLetterISORegionName;
                    erpRequest.Customer.BillingAddress.Phone = request.Customer.BillingAddress.Phone;
                }

                erpRequest.SalesOrder.CustomerId = request.Customer.CustomerNo;
                erpRequest.SalesOrder.CurrencyCode = request.Currency;
                erpRequest.SalesOrder.BillingAddress = erpRequest.Customer.BillingAddress;
            }

            if (request.CreditCard != null)
            {
                erpRequest.TenderLine.CardTypeId = request.CreditCard.CardType;
                erpRequest.TenderLine.MaskedCardNumber = request.CreditCard.CardNumber;
                erpRequest.TenderLine.CardOrAccount = request.CreditCard.CardHolder;
                erpRequest.TenderLine.Authorization = request.CreditCard.Authorization;
                erpRequest.TenderLine.CardToken = request.CreditCard.CardToken;
                erpRequest.TenderLine.ExpMonth = request.CreditCard.expirationMonth;
                erpRequest.TenderLine.ExpYear = request.CreditCard.expirationYear;
                erpRequest.TenderLine.PayerId = request.CreditCard.PayerId;
                erpRequest.TenderLine.ParentTransactionId = request.CreditCard.ParentTransactionId;
                erpRequest.TenderLine.Email = request.CreditCard.Email;
                erpRequest.TenderLine.Note = request.CreditCard.Note;
                erpRequest.TenderLine.TenderTypeId = request.CreditCard.ProcessorId;
                erpRequest.TenderLine.Amount = request.CreditCard.Amount;
                erpRequest.TenderLine.BankIdentificationNumberStart = request.CreditCard.BankIdentificationNumberStart;
                erpRequest.TenderLine.ApprovalCode = request.CreditCard.ApprovalCode;
                erpRequest.TenderLine.shopperReference = request.CreditCard.shopperReference;
                erpRequest.TenderLine.CustomAttributes = new List<KeyValuePair<string, string>>();
                erpRequest.TenderLine.CustomAttributes.Add(new KeyValuePair<string, string>("transaction-id", request.CreditCard.TransactionId));
                erpRequest.TransactionId = request.CreditCard.TransactionId;

                erpRequest.TenderLine.LineNumber = 1;
            }

            return erpRequest;
        }

        private CustomerInvoicesResponse ValidateCustomerInvoiceRequest(CustomerInvoicesRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new CustomerInvoicesResponse(false, null, message);
                }
                else if (String.IsNullOrWhiteSpace(request.CustomerAccount) && String.IsNullOrWhiteSpace(request.LicenseID))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400010, currentStore, "CustomerInvoicesRequest.CustomerAccount", "CustomerInvoicesRequest.LicenseID");
                    return new CustomerInvoicesResponse(false, null, message);
                }
                else if (!String.IsNullOrWhiteSpace(request.LicenseID))
                {
                    if (!(base.ValidateCustomerLicenseIDLength(request.LicenseID)))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400011, currentStore, "");
                        return new CustomerInvoicesResponse(false, null, message);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new CustomerInvoicesResponse(false, null, message);
            }

            return null;
        }

        private DeleteCustomerPaymentMethodResponse ValidateDeleteCustomerPaymentMethodRequest(DeleteCustomerPaymentMethodRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new DeleteCustomerPaymentMethodResponse(false, "", message);
                }
                else if (request.CardRecId <= 0 && request.BankAccountRecId <= 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, "DeleteCustomerPaymentMethod", "CardRecId or BankAccountRecId");
                    return new DeleteCustomerPaymentMethodResponse(false, "", message);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new DeleteCustomerPaymentMethodResponse(false, "", message);
            }

            return null;
        }

        #endregion

        #region Request, Response classes
        /// <summary>
        /// CustomerCreateRequest class is used as input parmeter for Create Customer.
        /// </summary>
        public class CustomerCreateRequest
        {
            public CustomerCreateRequest()
            {
                this.Customer = new ErpCustomer();
            }
            public ErpCustomer Customer { get; set; }
            public bool UseMapping { get; set; }
        }

        /// <summary>
        /// CustomerUpdateRequest class is used as input parmeter for Update Customer.
        /// </summary>
        public class CustomerUpdateRequest : CustomerCreateRequest
        {
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - Start
            public CustomerUpdateRequest()
            {
                this.Customer.ExtensionProperties = new List<ErpCommerceProperty>();
            }
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - End
        }


        /// <summary>
        /// CustomerRequest class is used as output parmeter for Customer calls.
        /// </summary>
        public class CustomerRequest
        {
            [Required]
            public string CustomerId { get; set; }
            public bool UseMapping { get; set; }
            public int SearchLocation { get; set; }

            // [MB] - TV - BR 3.0 - 15877 - new parameter swapLanguage added - Start
            public bool SwapLanguage { get; set; }

            public CustomerRequest()
            {
                this.SwapLanguage = false;
            }
            // [MB] - TV - BR 3.0 - 15877 - new parameter swapLanguage added - End
        }

        /// <summary>
        /// Customer by Licence request class
        /// </summary>
        public class CustomerByLicenceRequest
        {
            /// <summary>
            /// List of lincence numbers to get customers accordinlgy
            /// </summary>
            public List<string> licenceNumber { get; set; }

            /// <summary>
            /// Swap Language
            /// </summary>
            public bool SwapLanguage { get; set; }

            /// <summary>
            /// Instanciate an instance of request class
            /// </summary>

            public CustomerByLicenceRequest()
            {
                this.SwapLanguage = false;
            }
        }

        /// <summary>
        /// CustomerResponse class is used as input parmeter for Get Customer.
        /// </summary>
        public class CustomerResponse
        {
            public CustomerResponse(bool status, object customerInfo, string message)
            {
                this.Status = status;
                this.CustomerInfo = customerInfo;
                this.Message = message;
            }
            public bool Status { get; set; }
            public object CustomerInfo { get; set; }
            public string Message { get; set; }
        }
        public class CustomerContactPersonResponse
        {
            public CustomerContactPersonResponse(bool status, object customerInfo, object contactPerson, string message)
            {
                this.Status = status;
                this.CustomerInfo = customerInfo;
                this.ContactPerson = contactPerson;
                this.Message = message;
            }

            public bool Status { get; set; }
            public object CustomerInfo { get; set; }
            public object ContactPerson { get; set; }
            public string Message { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class CustomersResponse
        {
            public CustomersResponse(bool status, List<object> customerInfo, string message)
            {
                this.Status = status;
                this.CustomerInfo = customerInfo;
                this.Message = message;
            }

            public bool Status { get; set; }
            public List<object> CustomerInfo { get; set; }
            public string Message { get; set; }
        }
        public class CustomerAndContactPersonInfo
        {
            public ErpCustomer ErpCustomer { get; set; }
            public ErpContactPerson ErpContactPerson { get; set; }
        }
        /// <summary>
        /// CustomerCreateRequest class is used as input parmeter for Create Customer.
        /// </summary>
        public class UpdateCustomerRequest
        {
            public UpdateCustomerRequest()
            {
                this.Customer = new ErpCustomer();
            }
            public ErpCustomer Customer { get; set; }
            public bool UseMapping { get; set; }
            public string AccountNumber { get; set; }
            //public ErpContactPerson ContactPerson { get; set; }
        }

        /// <summary>
        /// Represents credit card response
        /// </summary>
        public class CreditCardResponse
        {

            /// <summary>
            /// Initializes a new instance of the CreditCardResponse
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="creditCard">creditCard</param>
            public CreditCardResponse(bool status, string errorMessage, object creditCard, object bankAccount)
            {
                this.Status = status;
                this.Message = errorMessage;
                this.CreditCard = creditCard;
                this.BankAccount = bankAccount;
            }

            /// <summary>
            /// Status of credit card
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of credit card
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Details of credit card
            /// </summary>
            public object CreditCard { get; set; }
            /// <summary>
            /// Details of Bank Account
            /// </summary>
            public object BankAccount { get; set; }

        }
        /// <summary>
        /// Represents Get credit card request
        /// </summary>
        public class GetPaymentMethodsRequest
        {
            /// <summary>
            /// Account Number of customer
            /// </summary>
            [Required]
            public string customerAccount { get; set; }
            /// <summary>
            /// license Number of customer
            /// </summary>
            [Required]
            public string licenseId { get; set; }
            /// <summary>
            /// Card Processors filter
            /// </summary>
            public List<string> cardProcessors { get; set; }
        }
        /// <summary>
        /// Create Credit Card Request
        /// </summary>
        public class CreditCardRequest
        {
            /// <summary>
            /// Currency property
            /// </summary>
            public string Currency { get; set; }
            /// <summary>
            /// TransactionDate property
            /// </summary>
            public string TransactionDate { get; set; }
            /// <summary>
            /// CustomerDetails
            /// </summary>
            public CustomerDetail Customer { get; set; }
            /// <summary>
            /// Payments details
            /// </summary>
            public CreditCard CreditCard { get; set; }
        }
        /// <summary>
        /// CustomerDetails
        /// </summary>
        public class CustomerDetail
        {
            /// <summary>
            /// ERP Customer Id
            /// </summary>
            public string CustomerNo { get; set; }
            /// <summary>
            /// Customer Name
            /// </summary>
            public string CustomerName { get; set; }
            /// <summary>
            /// Customer Email
            /// </summary>
            public string CustomerEmail { get; set; }
            /// <summary>
            /// BillingAddress of PaymentDetails
            /// </summary>
            public ErpAddress BillingAddress { get; set; }
        }
        /// <summary>
        /// CreditCard Details
        /// </summary>
        public class CreditCard
        {
            /// <summary>
            /// Credit Card Type Details
            /// </summary>
            public string CardType { get; set; }
            /// <summary>
            /// CardNumber of Credit Card
            /// </summary>
            public string CardNumber { get; set; }
            /// <summary>
            /// CardHolder of Credit Card
            /// </summary>
            public string CardHolder { get; set; }
            /// <summary>
            /// Authorization of Credit Card
            /// </summary>
            public string Authorization { get; set; }
            /// <summary>
            /// CardToken of Credit Card
            /// </summary>
            public string CardToken { get; set; }
            /// <summary>
            /// expirationMonth of Credit Card
            /// </summary>
            public int? expirationMonth { get; set; }
            /// <summary>
            /// expirationYear of Credit Card
            /// </summary>
            public int? expirationYear { get; set; }
            /// <summary>
            /// PayerId of Credit Card
            /// </summary>
            public string PayerId { get; set; }
            /// <summary>
            /// ParentTransactionId of Credit Card
            /// </summary>
            public string ParentTransactionId { get; set; }
            /// <summary>
            /// Email of Credit Card
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// Note of Credit Card
            /// </summary>
            public string Note { get; set; }
            /// <summary>
            /// Amount of Credit Card
            /// </summary>
            public decimal Amount { get; set; }
            /// <summary>
            /// ProcessorId of Credit Card
            /// </summary>
            public string ProcessorId { get; set; }
            /// <summary>
            /// TransactionId of Credit Card
            /// </summary>
            public string TransactionId { get; set; }
            /// <summary>
            /// NumberOfInstallments
            /// </summary>
            public int NumberOfInstallments { get; set; }

            /// <summary>
            /// BankIdentificationNumberStart
            /// </summary>
            public string BankIdentificationNumberStart { get; set; }
            /// <summary>
            /// ApprovalCode
            /// </summary>
            public string ApprovalCode { get; set; }
            /// <summary>
            /// shopperReference
            /// </summary>
            public string shopperReference { get; set; }
            /// <summary>
            /// IssuerCountry
            /// </summary>
            public string IssuerCountry { get; set; }

            /// <summary>
            /// IBAN
            /// </summary>
            public string IBAN { get; set; }
            /// <summary>
            /// SwiftCode
            /// </summary>
            public string SwiftCode { get; set; }
            /// <summary>
            /// BankName
            /// </summary>
            public string BankName { get; set; }
        }
        /// <summary>
        /// Edit Credit Card Request
        /// </summary>
        public class PaymentMethodRequest
        {
            /// <summary>
            /// Card Number
            /// </summary>
            [Required]
            public string CardNumber { get; set; }
            /// <summary>
            /// Currency property
            /// </summary>
            [Required]
            public string Currency { get; set; }
            /// <summary>
            /// TransactionDate property
            /// </summary>
            [Required]
            public string TransactionDate { get; set; }
            /// <summary>
            /// CustomerDetails
            /// </summary>
            public CustomerDetail Customer { get; set; }
            /// <summary>
            /// Payments details
            /// </summary>
            public CreditCard CreditCard { get; set; }
        }

        /// <summary>
        /// Customer invoices request
        /// </summary>
        public class CustomerInvoicesRequest
        {
            /// <summary>
            /// customer account Id
            /// </summary>
            [Required]
            public string CustomerAccount { get; set; }

            /// <summary>
            /// customer license Id
            /// </summary>
            [Required]
            public string LicenseID { get; set; }

            /// <summary>
            /// transaction type that needs to be fetched from D365
            /// </summary>
            [Required]
            public string TransactionType { get; set; }
        }
        /// <summary>
        /// CustomerInovice Response
        /// </summary>
        public class CustomerInvoicesResponse
        {
            /// <summary>
            /// Initialize a new instance of the CustomerInvoiceDetailResponse 
            /// </summary>
            /// <param name="status">status of the CustomerInvoice request</param>
            /// <param name="customerInvoice">Invoices of customer</param>
            /// <param name="message">message for CustomerInvoice request</param>

            public CustomerInvoicesResponse(bool status, List<ErpCustomerInvoice> customerInvoice, string message)
            {
                this.Status = status;
                this.CustomerInvoices = customerInvoice;
                this.Message = message;
            }

            /// <summary>
            /// status of the CustomerInvoice request
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// Customer Invoice 
            /// </summary>
            public List<ErpCustomerInvoice> CustomerInvoices { get; set; }

            /// <summary>
            /// message of the CustomerInvoice request
            /// </summary>
            public string Message { get; set; }

        }

        public class DeleteCustomerPaymentMethodRequest
        {
            /// <summary>
            /// RecId of card that needs to be deleted from D365
            /// </summary>
            public long CardRecId { get; set; }
            public long BankAccountRecId { get; set; }
        }


        public class DeleteCustomerPaymentMethodResponse
        {
            public DeleteCustomerPaymentMethodResponse(bool status, string errorCode, string message)
            {
                this.Status = status;
                this.ErrorCode = errorCode;
                this.Message = message;
            }
            /// <summary>
            /// Status of delete card request
            /// </summary>
            public bool Status { get; set; }
            /// <summary>
            /// ErrorCode of card request
            /// </summary>
            public string ErrorCode { get; set; }
            /// <summary>
            /// Message against delete card request
            /// </summary>
            public string Message { get; set; }
        }
        public class MergeCreateCustomerContactPersonResponse
        {
            public MergeCreateCustomerContactPersonResponse(bool status, object customer, object contactPerson, string message)
            {
                this.Status = status;
                this.CustomerInfo = customer;
                this.ContactPerson = contactPerson;
                this.Message = message;
            }

            public bool Status { get; set; }
            public object CustomerInfo { get; set; }
            public object ContactPerson { get; set; }
            public string Message { get; set; }
        }

        public class MergeUpdateCustomerContactPersonResponse
        {
            public MergeUpdateCustomerContactPersonResponse(bool status, object customer, object contactPerson, string message)
            {
                this.Status = status;
                this.CustomerInfo = customer;
                this.ContactPerson = contactPerson;
                this.Message = message;
            }

            public bool Status { get; set; }
            public object CustomerInfo { get; set; }
            public object ContactPerson { get; set; }
            public string Message { get; set; }
        }

        #endregion
    }
}
