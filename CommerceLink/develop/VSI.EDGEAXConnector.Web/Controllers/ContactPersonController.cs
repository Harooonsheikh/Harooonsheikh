using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// ContactPersonController defines properties and methods for API controller for Contact Person.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class ContactPersonController : ApiBaseController
    {
        /// <summary>
        /// Contact Person Controller 
        /// </summary>
        public ContactPersonController()
        {
            ControllerName = "ContactPersonController";
        }

        #region API Methods

        /// <summary>
        /// GetContactPerson gets contact person with provided details.
        /// </summary>
        /// <param name="customerAccount">Contact person request to be fetch</param>
        /// <param name="swapLanguage">swapLanguage</param>
        /// <param name="contactPersonId">contactPersonId</param>
        /// <returns>GiftCardResponse</returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("ContactPerson/GetContactPerson")]
        [Obsolete("GetContactPerson is deprecated, please use GetContactPerson with POST parameter instead.")]
        public ContactPersonResponse GetContactPerson([FromUri] string customerAccount, [FromUri] bool swapLanguage = true, [FromUri] string contactPersonId = "-1")
        {
            ContactPersonResponse contactPersonResponse;
            // Throw error if customerAccount is null
            if (customerAccount == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                contactPersonResponse = new ContactPersonResponse(false, message, null);
                return contactPersonResponse;
            }
            // Extract the data from parameter
            string customerAccountNo = customerAccount;
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " customerAccountNo: " + customerAccountNo);
            contactPersonResponse = new ContactPersonResponse(false, "", null);
            try
            {                
                var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);

                ERPContactPersonResponse erpContactPersonResponse = erpContactPersonController.GetContactPerson(customerAccountNo, contactPersonId);

                if (erpContactPersonResponse.Success)
                {
                    //Language code swapping for Ecom
                    if (swapLanguage)
                    {
                        if (erpContactPersonResponse.ContactPerson != null && !string.IsNullOrEmpty(erpContactPersonResponse.ContactPerson.Language))
                        {
                            LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                            erpContactPersonResponse.ContactPerson.Language = languageCodes.GetEcomLanguageCode(erpContactPersonResponse.ContactPerson.Language);
                        } 
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonResponse.ContactPerson));

                    eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, false);

                    contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);

                    contactPersonResponse.ContactPerson.SwapLanguage = swapLanguage;
                }
                else
                {
                    eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, false);
                    contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                contactPersonResponse = new ContactPersonResponse(false, message, null);
            }

            return contactPersonResponse;
        }

        /// <summary>
        /// GetContactPerson gets contact person with provided details.
        /// </summary>
        /// <param name="ContactPersonRequest"></param>
        /// <returns>ContactPersonResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("ContactPerson/GetContactPerson")]
        public ContactPersonResponse GetContactPerson([FromBody] GetContactPersonRequest ContactPersonRequest)
        {

            ContactPersonResponse contactPersonResponse;

            // Throw error if customerAccount is null
            if (ContactPersonRequest.customerAccount == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                contactPersonResponse = new ContactPersonResponse(false, message, null);

                return contactPersonResponse;
            }

            // Extract the data from parameter
            string customerAccountNo = ContactPersonRequest.customerAccount;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " customerAccountNo: " + customerAccountNo);

            contactPersonResponse = new ContactPersonResponse(false, "", null);

            try
            {
                var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);

                ERPContactPersonResponse erpContactPersonResponse = erpContactPersonController.GetContactPerson(customerAccountNo, ContactPersonRequest.contactPersonId);

                if (erpContactPersonResponse.Success)
                {
                    //Language code swapping for Ecom
                    if (ContactPersonRequest.swapLanguage)
                    {
                        if (erpContactPersonResponse.ContactPerson != null && !string.IsNullOrEmpty(erpContactPersonResponse.ContactPerson.Language))
                        {
                            LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                            erpContactPersonResponse.ContactPerson.Language = languageCodes.GetEcomLanguageCode(erpContactPersonResponse.ContactPerson.Language);
                        }
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonResponse.ContactPerson));

                    eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, false);

                    contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);

                    contactPersonResponse.ContactPerson.SwapLanguage = ContactPersonRequest.swapLanguage;
                }
                else
                {
                    eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, false);

                    contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                contactPersonResponse = new ContactPersonResponse(false, message, null);
            }

            return contactPersonResponse;
        }

        /// <summary>
        /// UpdateContactPerson updates contact person with provided details.
        /// </summary>
        /// <param name="contactPersonRequest">Contact person request to be update</param>
        /// <returns>GiftCardResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("ContactPerson/UpdateContactPerson")]
        public ContactPersonResponse UpdateContactPerson([FromBody] ContactPersonRequest contactPersonRequest)
        {

            ContactPersonResponse contactPersonResponse = new ContactPersonResponse(false, "", null);

            try
            {
                contactPersonResponse = this.ValidateContactPerson(contactPersonRequest, true);

                if (contactPersonResponse != null)
                {
                    return contactPersonResponse;
                }
                else
                {
                    contactPersonRequest.TMVSourceSystem = string.IsNullOrWhiteSpace(contactPersonRequest.TMVSourceSystem) ? SourceSystem.WEB.ToString() : contactPersonRequest.TMVSourceSystem.ToString().Trim();

                    ErpContactPerson contactPerson = new ErpContactPerson();

                    // Extract the data from parameter
                    contactPerson.DirPartyRecordId = contactPersonRequest.DirPartyRecordId;
                    contactPerson.ContactForParty = contactPersonRequest.ContactForParty;
                    contactPerson.ContactPersonId = contactPersonRequest.ContactPersonId;
                    contactPerson.CustAccount = contactPersonRequest.CustAccount;
                    contactPerson.Email = contactPersonRequest.Email;
                    contactPerson.EmailRecordId = contactPersonRequest.EmailRecordId;
                    contactPerson.InActive = contactPersonRequest.InActive;
                    contactPerson.FirstName = contactPersonRequest.FirstName;
                    contactPerson.MiddleName = contactPersonRequest.MiddleName;
                    contactPerson.LastName = contactPersonRequest.LastName;
                    contactPerson.Phone = contactPersonRequest.Phone;
                    contactPerson.PhoneRecordId = contactPersonRequest.PhoneRecordId;
                    contactPerson.Language = contactPersonRequest.Language;
                    contactPerson.Title = contactPersonRequest.Title;
                    contactPerson.Gender = contactPersonRequest.Gender;
                    contactPerson.Department = contactPersonRequest.Department;
                    contactPerson.DirectMail = contactPersonRequest.DirectMail;
                    contactPerson.IsExternallyMaintained = contactPersonRequest.IsExternallyMaintained;
                    contactPerson.URLRecID = contactPersonRequest.URLRecordId;
                    contactPerson.PrimaryURL = contactPersonRequest.PrimaryURL;
                    contactPerson.Profession = contactPersonRequest.Profession;
                    contactPerson.FaxRecID = contactPersonRequest.FaxRecordId;
                    contactPerson.PrimaryFax = contactPersonRequest.PrimaryFax;
                    contactPerson.TMVSourceSystem = contactPersonRequest.TMVSourceSystem;
                    contactPerson.TMVContactIDGUID = contactPersonRequest.TMVContactIDGUID;
                    contactPerson.IsPrimary = contactPersonRequest.IsPrimary;

                    if (contactPersonRequest.Addresses != null)
                    {
                        foreach (var address in contactPersonRequest.Addresses)
                        {
                            // Map and add addresses
                            ErpAddress erpAddress = new ErpAddress();

                            erpAddress.RecordId = address.RecordId;
                            erpAddress.Street = address.Street;
                            erpAddress.BuildingCompliment = address.Street2;
                            erpAddress.City = address.City;
                            erpAddress.ZipCode = address.ZipCode;
                            erpAddress.State = address.State;
                            erpAddress.ThreeLetterISORegionName = address.ThreeLetterISORegionName;
                            erpAddress.Phone = address.Phone;

                            contactPerson.Addresses.Add(erpAddress);
                        }
                    }

                    //Language code swapping for D365
                    if (!string.IsNullOrEmpty(contactPerson.Language) && contactPersonRequest.SwapLanguage)
                    {
                        LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                        contactPerson.Language = languageCodes.GetErpLanguageCode(contactPerson.Language);
                    }  

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(contactPerson));

                    var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);

                    ERPContactPersonResponse erpContactPersonResponse = erpContactPersonController.UpdateContactPerson(contactPerson);

                    if (erpContactPersonResponse.Success)
                    {
                        //Language code swapping for Ecom
                        if (erpContactPersonResponse.ContactPerson != null && !string.IsNullOrEmpty(erpContactPersonResponse.ContactPerson.Language) &&
                            contactPersonRequest.SwapLanguage)
                        {
                            LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                            erpContactPersonResponse.ContactPerson.Language = languageCodes.GetEcomLanguageCode(erpContactPersonResponse.ContactPerson.Language);
                        } 

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonResponse.ContactPerson));

                        eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, contactPersonRequest.SwapLanguage);

                        contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);
                    }
                    else
                    {
                        eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, contactPersonRequest.SwapLanguage);

                        contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                contactPersonResponse = new ContactPersonResponse(false, message, null);
                return contactPersonResponse;
            }

            return contactPersonResponse;
        }

        /// <summary>
        /// CreateContactPerson creates contact person with provided details.
        /// </summary>
        /// <param name="contactPersonRequest">Contact person request to be created</param>
        /// <returns>GiftCardResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("ContactPerson/CreateContactPerson")]
        public ContactPersonResponse CreateContactPerson([FromBody] ContactPersonRequest contactPersonRequest)
        {
            return this.CreateContactPersonMethod(contactPersonRequest, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Create contact person
        /// </summary>
        /// <param name="contactPersonRequest"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public ContactPersonResponse CreateContactPersonMethod(ContactPersonRequest contactPersonRequest, string methodName)
        {

            ContactPersonResponse contactPersonResponse = new ContactPersonResponse(false, string.Empty, null);

            try
            {
                contactPersonResponse = this.ValidateContactPerson(contactPersonRequest, false);

                if (contactPersonResponse != null)
                {
                    return contactPersonResponse;
                }
                else
                {
                    ErpContactPerson contactPerson = new ErpContactPerson();
                    // Extract the data from parameter
                    contactPerson.DirPartyRecordId = contactPersonRequest.DirPartyRecordId;
                    contactPerson.ContactForParty = contactPersonRequest.ContactForParty;
                    contactPerson.CustAccount = contactPersonRequest.CustAccount;
                    contactPerson.Email = contactPersonRequest.Email;
                    contactPerson.InActive = contactPersonRequest.InActive;
                    contactPerson.IsPrimary = contactPersonRequest.IsPrimary;
                    contactPerson.FirstName = contactPersonRequest.FirstName;
                    contactPerson.MiddleName = contactPersonRequest.MiddleName;
                    contactPerson.LastName = contactPersonRequest.LastName;
                    contactPerson.Phone = contactPersonRequest.Phone;
                    contactPerson.Title = contactPersonRequest.Title;
                    contactPerson.Language = contactPersonRequest.Language;
                    contactPerson.Gender = contactPersonRequest.Gender;
                    contactPerson.Department = contactPersonRequest.Department;
                    contactPerson.DirectMail = contactPersonRequest.DirectMail;
                    contactPerson.IsExternallyMaintained = contactPersonRequest.IsExternallyMaintained;
                    contactPerson.PrimaryURL = contactPersonRequest.PrimaryURL;
                    contactPerson.Profession = contactPersonRequest.Profession;
                    contactPerson.PrimaryFax = contactPersonRequest.PrimaryFax;
                    contactPerson.TMVSourceSystem = string.IsNullOrWhiteSpace(contactPersonRequest.TMVSourceSystem) ? SourceSystem.WEB.ToString() : contactPersonRequest.TMVSourceSystem.ToString().Trim();
                    contactPerson.TMVContactIDGUID = contactPersonRequest.TMVContactIDGUID;
                    contactPerson.Addresses = new List<ErpAddress>();

                    if (contactPersonRequest.Addresses != null)
                    {
                        foreach (var address in contactPersonRequest.Addresses)
                        {
                            // Map and add addresses
                            ErpAddress erpAddress = new ErpAddress();

                            erpAddress.Street = address.Street;
                            erpAddress.BuildingCompliment = address.Street2;
                            erpAddress.City = address.City;
                            erpAddress.ZipCode = address.ZipCode;
                            erpAddress.State = address.State;
                            erpAddress.ThreeLetterISORegionName = address.ThreeLetterISORegionName;
                            erpAddress.Phone = address.Phone;

                            contactPerson.Addresses.Add(erpAddress);
                        }
                    }

                    //Language code swapping for D365
                    if (!string.IsNullOrEmpty(contactPerson.Language) && contactPersonRequest.SwapLanguage)
                    {
                        LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                        contactPerson.Language = languageCodes.GetErpLanguageCode(contactPerson.Language);
                    } 


                    //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(contactPerson));

                    var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);
                    ERPContactPersonResponse erpContactPersonResponse = erpContactPersonController.CreateContactPerson(contactPerson, GetRequestGUID(Request));

                    if (erpContactPersonResponse.Success && erpContactPersonResponse.ContactPerson != null && !string.IsNullOrWhiteSpace(erpContactPersonResponse.ContactPerson.ContactPersonId))
                    {
                        //Language code swapping for Ecom
                        if (erpContactPersonResponse.ContactPerson != null && !string.IsNullOrEmpty(erpContactPersonResponse.ContactPerson.Language) && contactPersonRequest.SwapLanguage)
                        {
                            LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                            erpContactPersonResponse.ContactPerson.Language = languageCodes.GetEcomLanguageCode(erpContactPersonResponse.ContactPerson.Language);
                        }

                        //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonResponse.ContactPerson));

                        eComContactPerson ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, contactPersonRequest.SwapLanguage);

                        contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);
                    }
                    else
                    {
                        eComContactPerson ecomContactPerson = null;

                        if (erpContactPersonResponse.Success)
                        {
                            ecomContactPerson = convertErpContactPersonToeComContactPerson(erpContactPersonResponse.ContactPerson, contactPersonRequest.SwapLanguage);
                        }

                        contactPersonResponse = new ContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, ecomContactPerson);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                contactPersonResponse = new ContactPersonResponse(false, message, null);
                return contactPersonResponse;
            }

            return contactPersonResponse;
        }

        private eComContactPerson convertErpContactPersonToeComContactPerson(ErpContactPerson erpContactPerson, bool swapLanguage)
        {
            eComContactPerson ecomContactPerson = new eComContactPerson();
            
            ecomContactPerson = _mapper.Map<ErpContactPerson, eComContactPerson>(erpContactPerson);

            // Need to revamp below code, mapping should be done automatically
            if (erpContactPerson != null  && erpContactPerson.Addresses != null && erpContactPerson.Addresses.Count > 0)
            {
                for (int addressIndex = 0; addressIndex < erpContactPerson.Addresses.Count; addressIndex++)
                {
                    ecomContactPerson.Addresses[addressIndex].Street2 = erpContactPerson.Addresses[addressIndex].BuildingCompliment;
                }

                ecomContactPerson.SwapLanguage = swapLanguage;
            }
            
            return ecomContactPerson;
        }

        /// <summary>
        /// GetAllContactPerson gets contact persons with provided details.
        /// </summary>
        /// <param name="customerAccount">Contact person request to be fetch</param>
        /// <returns>GiftCardResponse</returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("ContactPerson/GetAllContactPersons")]
        [Obsolete("GetAllContactPersons is deprecated, please use GetAllContactPersons with POST parameter instead.")]
        public GetAllContactPersonResponse GetAllContactPersons([FromUri] string customerAccount, [FromUri] bool swapLanguage = true)
        {

            GetAllContactPersonResponse getAllContactPersonResponse = new GetAllContactPersonResponse(false, string.Empty, null);
            // Throw error if customerAccount is null
            if (customerAccount == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);

                return getAllContactPersonResponse;
            }

            // Extract the data from parameter
            string customerAccountNo = customerAccount;
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " customerAccountNo: " + customerAccountNo);
            try
            {
                var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);

                ERPContactAllPersonResponse erpContactPersonsResponse = erpContactPersonController.GetAllContactPersons(customerAccountNo);

                if (erpContactPersonsResponse.Success)
                {
                    if (swapLanguage)
                    {
                        foreach (var contactPerson in erpContactPersonsResponse.ContactPersonNALList)
                        {
                            //Language code swapping for Ecom
                            if (!string.IsNullOrEmpty(contactPerson.Language))
                            {
                                LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                                contactPerson.Language = languageCodes.GetEcomLanguageCode(contactPerson.Language);
                            }
                        } 
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonsResponse.ContactPersonNALList));

                    var contactPersonList = _mapper.Map<List<ContactPersonCrm>>(erpContactPersonsResponse.ContactPersonNALList);

                    getAllContactPersonResponse = new GetAllContactPersonResponse(erpContactPersonsResponse.Success, erpContactPersonsResponse.Message, contactPersonList);

                    getAllContactPersonResponse.ContactPersonList.ForEach(x => x.SwapLanguage = swapLanguage);
                }
                else
                {
                    getAllContactPersonResponse = new GetAllContactPersonResponse(erpContactPersonsResponse.Success, erpContactPersonsResponse.Message, null);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                getAllContactPersonResponse = new GetAllContactPersonResponse(false, message, null);
            }

            return getAllContactPersonResponse;
        }


        /// <summary>
        /// GetAllContactPerson gets contact persons with provided details.
        /// </summary>
        /// <param name="GetAllContactPersonsRequest">Contact person request to be fetch</param>
        /// <returns>GetAllContactPersonResponse</returns>
        [RequestResponseLog]
        [SanitizeInput]
        [HttpPost]
        [Route("ContactPerson/GetAllContactPersons")]
        public GetAllContactPersonResponse GetAllContactPersons(GetAllContactPersonsRequest allContactPersonsRequest)
        {
            GetAllContactPersonResponse getAllContactPersonResponse = new GetAllContactPersonResponse(false, string.Empty, null);
            // Throw error if customerAccount is null
            if (allContactPersonsRequest.customerAccount == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);

                return getAllContactPersonResponse;
            }

            // Extract the data from parameter
            string customerAccountNo = allContactPersonsRequest.customerAccount;
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " customerAccountNo: " + customerAccountNo);
            try
            {
                var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);

                ERPContactAllPersonResponse erpContactPersonsResponse = erpContactPersonController.GetAllContactPersons(customerAccountNo);

                if (erpContactPersonsResponse.Success)
                {
                    if (allContactPersonsRequest.swapLanguage)
                    {
                        foreach (var contactPerson in erpContactPersonsResponse.ContactPersonNALList)
                        {
                            //Language code swapping for Ecom
                            if (!string.IsNullOrEmpty(contactPerson.Language))
                            {
                                LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                                contactPerson.Language = languageCodes.GetEcomLanguageCode(contactPerson.Language);
                            }
                        }
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonsResponse.ContactPersonNALList));

                    var contactPersonList = _mapper.Map<List<ContactPersonCrm>>(erpContactPersonsResponse.ContactPersonNALList);
                    
                    getAllContactPersonResponse = new GetAllContactPersonResponse(erpContactPersonsResponse.Success, erpContactPersonsResponse.Message, contactPersonList);

                    getAllContactPersonResponse.ContactPersonList.ForEach(x => x.SwapLanguage = allContactPersonsRequest.swapLanguage);
                }
                else
                {
                    getAllContactPersonResponse = new GetAllContactPersonResponse(erpContactPersonsResponse.Success, erpContactPersonsResponse.Message, null);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                getAllContactPersonResponse = new GetAllContactPersonResponse(false, message, null);
            }

            return getAllContactPersonResponse;
        }

        [RequestResponseLog]
        [HttpPost]
        [Route("ContactPerson/SaveContactPerson")]
        public SaveContactPersonResponse SaveContactPerson([FromBody] SaveContactPersonRequest saveContactPersonRequest)
        {
            return SaveContactPerson(saveContactPersonRequest, MethodBase.GetCurrentMethod().Name);
        }
        public SaveContactPersonResponse SaveContactPerson(SaveContactPersonRequest saveContactPersonRequest,string methodName)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10006, currentStore, MethodBase.GetCurrentMethod().Name, methodName.ToString());
            SaveContactPersonResponse saveContactPersonResponse = new SaveContactPersonResponse(false, string.Empty, null);

            try
            {
                saveContactPersonResponse = this.ValidateSaveContactPerson(saveContactPersonRequest, false);

                if (saveContactPersonResponse != null)
                {
                    return saveContactPersonResponse;
                }
                else
                {
                    saveContactPersonRequest.ContactPerson.TMVSourceSystem = string.IsNullOrWhiteSpace(saveContactPersonRequest.ContactPerson.TMVSourceSystem) ? SourceSystem.WEB.ToString() : saveContactPersonRequest.ContactPerson.TMVSourceSystem.ToString().Trim();
                    //Language code swapping for D365
                    if (saveContactPersonRequest.ContactPerson.SwapLanguage)
                    {
                        if (!string.IsNullOrEmpty(saveContactPersonRequest.ContactPerson.Language))
                        {
                            LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                            saveContactPersonRequest.ContactPerson.Language = languageCodes.GetErpLanguageCode(saveContactPersonRequest.ContactPerson.Language);
                        } 
                    }

                    var erpContactPersonController = erpAdapterFactory.CreateContactPersonController(currentStore.StoreKey);
                    ERPSaveContactPersonResponse erpContactPersonResponse = erpContactPersonController.SaveContactPerson(saveContactPersonRequest.ContactPerson, GetRequestGUID(Request));


                    if (erpContactPersonResponse.Success && erpContactPersonResponse.ContactPerson != null && !string.IsNullOrWhiteSpace(erpContactPersonResponse.ContactPerson.ContactPersonId))
                    {
                        //Language code swapping for Ecom
                        if (saveContactPersonRequest.ContactPerson.SwapLanguage)
                        {
                            if (erpContactPersonResponse.ContactPerson != null && !string.IsNullOrEmpty(erpContactPersonResponse.ContactPerson.Language))
                            {
                                LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);
                                erpContactPersonResponse.ContactPerson.Language = languageCodes.GetEcomLanguageCode(erpContactPersonResponse.ContactPerson.Language);
                            } 
                        }

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpContactPersonResponse.ContactPerson));
                        saveContactPersonResponse = new SaveContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, erpContactPersonResponse.ContactPerson);

                        saveContactPersonResponse.ContactPerson.SwapLanguage = saveContactPersonRequest.ContactPerson.SwapLanguage;
                    }
                    else
                    {
                        saveContactPersonResponse = new SaveContactPersonResponse(erpContactPersonResponse.Success, erpContactPersonResponse.Message, erpContactPersonResponse.ContactPerson);
                    }
                }
               
                return saveContactPersonResponse;

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                saveContactPersonResponse = new SaveContactPersonResponse(false, message, null);
                return saveContactPersonResponse;
            }
        }
        [RequestResponseLog]
        [HttpPost]
        [Route("ContactPerson/CreateUpdateContactPerson")]
        public CreateUpdateContactPersonResponse CreateUpdateContactPerson([FromBody] CreateUpdateContactPersonRequest createUpdateContactPersonRequest)
        {
            CreateUpdateContactPersonResponse createUpdateContactPersonResponse = new CreateUpdateContactPersonResponse(false, string.Empty, null);
            try
            {
                var contactPersonReq = _mapper.Map<SaveContactPersonRequest>(createUpdateContactPersonRequest);

                var responseContactPerson = SaveContactPerson(contactPersonReq, MethodBase.GetCurrentMethod().Name);

                if (responseContactPerson.Status)
                {
                    
                    var contactPersonInfo = _mapper.Map<ErpContactPersonNAL, ContactPersonInfo>(responseContactPerson.ContactPerson);
                    createUpdateContactPersonResponse = new CreateUpdateContactPersonResponse(responseContactPerson.Status, responseContactPerson.ErrorMessage, contactPersonInfo);
                    return createUpdateContactPersonResponse;
                }
                else
                {
                    createUpdateContactPersonResponse = new CreateUpdateContactPersonResponse(responseContactPerson.Status, responseContactPerson.ErrorMessage, null);
                    return createUpdateContactPersonResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                createUpdateContactPersonResponse = new CreateUpdateContactPersonResponse(false, message, null);
                return createUpdateContactPersonResponse;
            }
        }
        #endregion

        #region Private 
        /// <summary>
        /// ValidateCreateContactPerson validates Create Contact Person Object
        /// Message: must change in customer controller if you add a new validation
        /// </summary>
        /// <param name="contactPersonRequest"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private ContactPersonResponse ValidateContactPerson(ContactPersonRequest contactPersonRequest, bool isUpdate)
        {
            if (contactPersonRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new ContactPersonResponse(false, message, null);
            }
            else
            {
                //Not Required for Create
                if (isUpdate)
                {
                    if (isUpdate && string.IsNullOrWhiteSpace(contactPersonRequest.ContactPersonId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.ContactPersonId");
                        return new ContactPersonResponse(false, message, null);
                    }
                    else if (contactPersonRequest.DirPartyRecordId == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.DirPartyRecordId");
                        return new ContactPersonResponse(false, message, null);
                    }
                }

                if (contactPersonRequest.ContactForParty == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.ContactForParty");
                    return new ContactPersonResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(contactPersonRequest.CustAccount))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.CustAccount");
                    return new ContactPersonResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(contactPersonRequest.FirstName))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.FirstName");
                    return new ContactPersonResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(contactPersonRequest.LastName))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "contactPersonRequest.LastName");
                    return new ContactPersonResponse(false, message, null);
                }

                //Length issue server side validation
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.FirstName) && contactPersonRequest.FirstName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                    return new ContactPersonResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.MiddleName) && contactPersonRequest.MiddleName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "MiddleName", "25");
                    return new ContactPersonResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.LastName) && contactPersonRequest.LastName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                    return new ContactPersonResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(contactPersonRequest.Email) && contactPersonRequest.Email.Length > 255)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Email", "255");
                    return new ContactPersonResponse(false, message, null);
                }
                //else if (!string.IsNullOrWhiteSpace(contactPersonRequest.Language) && contactPersonRequest.Language.Length > 7)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Language", "7");
                //    return new ContactPersonResponse(false, message, null);
                //}

                foreach (var address in contactPersonRequest.Addresses)
                {
                    if (!string.IsNullOrWhiteSpace(address.ZipCode) && address.ZipCode.Length > 10)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.ZipCode", "10");
                        return new ContactPersonResponse(false, message, null);
                    }
                }
            }
            return null;
        }
        
        /// <summary>
        /// ValidateCreateContactPerson validates Create Contact Person Object
        /// Message: must change in customer controller if you add a new validation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private SaveContactPersonResponse ValidateSaveContactPerson(SaveContactPersonRequest request, bool isUpdate)
        {
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new SaveContactPersonResponse(false, message, null);
            }
            else
            {
                //Not Required for Create
                if (isUpdate)
                {
                    if (isUpdate && string.IsNullOrWhiteSpace(request.ContactPerson.ContactPersonId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPerson.ContactPersonId");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (request.ContactPerson.DirPartyRecordId == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPerson.DirPartyRecordId");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                }

                if (string.IsNullOrWhiteSpace(request.ContactPerson.ContactPersonId))
                {

                    if (request.ContactPerson.ContactForParty == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPerson.ContactForParty");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (string.IsNullOrWhiteSpace(request.ContactPerson.CustAccount))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPerson.CustAccount");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (string.IsNullOrWhiteSpace(request.ContactPerson.FirstName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPerson.FirstName");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (string.IsNullOrWhiteSpace(request.ContactPerson.LastName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPerson.LastName");
                        return new SaveContactPersonResponse(false, message, null);
                    }

                    //Length issue server side validation
                    else if (!string.IsNullOrWhiteSpace(request.ContactPerson.FirstName) && request.ContactPerson.FirstName.Length > 25)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (!string.IsNullOrWhiteSpace(request.ContactPerson.MiddleName) && request.ContactPerson.MiddleName.Length > 25)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "MiddleName", "25");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (!string.IsNullOrWhiteSpace(request.ContactPerson.LastName) && request.ContactPerson.LastName.Length > 25)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (!string.IsNullOrWhiteSpace(request.ContactPerson.Email) && request.ContactPerson.Email.Length > 255)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Email", "255");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    //else if (!string.IsNullOrWhiteSpace(request.ContactPerson.Language) && request.ContactPerson.Language.Length > 7)
                    //{
                    //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Language", "7");
                    //    return new SaveContactPersonResponse(false, message, null);
                    //}
                    else if (!string.IsNullOrWhiteSpace(request.ContactPerson.ZipCode) && request.ContactPerson.ZipCode.Length > 10)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "ZipCode", "10");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                    else if (!string.IsNullOrWhiteSpace(request.ContactPerson.AddressName) && request.ContactPerson.AddressName.Length > 60)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "AddressName", "60");
                        return new SaveContactPersonResponse(false, message, null);
                    }
                }
            }
            return null;
        }
        #endregion

        #region ContactPerson Request, Response classes
        public class CreateUpdateContactPersonResponse
        {

            /// <summary>
            /// Initializes a new instance of the ContactPersonResponse
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="contactPerson">contact person</param>
            public CreateUpdateContactPersonResponse(bool status, string errorMessage, ContactPersonInfo contactPersonInfo)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.ContactPersonInfo = contactPersonInfo;
            }

            /// <summary>
            /// Status of contact person
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of contact person
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// ContactPerson of contact person
            /// </summary>
            public ContactPersonInfo ContactPersonInfo { get; set; }

        }
        public class ContactPersonInfo
        {
            public string ContactPersonId { get; set; }
            public long DirPartyRecordId { get; set; }
            public string CustAccount { get; set; }
            public long ContactForParty { get; set; }
            public string Email { get; set; }
        }
        /// <summary>
        /// Represents contact person request
        /// </summary>
        public class ContactPersonRequest
        {
            public ContactPersonRequest()
            {
                this.SwapLanguage = true;
                this.Addresses = new List<Address>();
                this.TMVContactIDGUID = string.Empty;
            }

            /// <summary>
            /// Party for contact person
            /// </summary>
            public long DirPartyRecordId { get; set; }
            /// <summary>
            /// ContactPersonId for contact person
            /// </summary>
            [Required]
            public string ContactPersonId { get; set; }
            /// <summary>
            /// ContactForParty for contact person
            /// </summary>
            public long ContactForParty { get; set; }
            /// <summary>
            /// CustAccount for contact person
            /// </summary>
            [Required]
            public string CustAccount { get; set; }
            /// <summary>
            /// Inactive for contact person
            /// </summary>
            public bool InActive { get; set; }
            /// <summary>
            /// Inactive for contact person
            /// </summary>
            [Required]
            public string IsPrimary { get; set; }
            /// <summary>
            /// Title for contact person
            /// </summary>
            [Required]
            public string Title { get; set; }
            /// <summary>
            /// FirstName for contact person
            /// </summary>
            [Required]
            public string FirstName { get; set; }
            /// <summary>
            /// MiddleName for contact person
            /// </summary>
            [Required]
            public string MiddleName { get; set; }
            /// <summary>
            /// LastName for contact person
            /// </summary>
            [Required]
            public string LastName { get; set; }
            /// <summary>
            /// EmailRecordId for contact person
            /// </summary>
            [Required]
            public long EmailRecordId { get; set; }
            /// <summary>
            /// Email for contact person
            /// </summary>
            [Required]
            public string Email { get; set; }
            /// <summary>
            /// PhoneRecordId for contact person
            /// </summary>
            public long PhoneRecordId { get; set; }
            /// <summary>
            /// Phone for contact person
            /// </summary>
            [Required]
            public string Phone { get; set; }
            /// <summary>
            /// Language for contact person
            /// </summary>
            [Required]
            public string Language { get; set; }
            /// <summary>
            /// Gender of contact person
            /// </summary>
            [Required]
            public string Gender { get; set; }
            /// <summary>
            /// Department of contact person
            /// </summary>
            [Required]
            public string Department { get; set; }
            /// <summary>
            /// Direct mail setting of contact person
            /// </summary>
            public bool DirectMail { get; set; }
            /// <summary>
            /// Is externally maintained setting of contact person
            /// </summary>
            public bool IsExternallyMaintained { get; set; }
            /// <summary>
            /// Record ID of URL
            /// </summary>
            public long URLRecordId { get; set; }
            /// <summary>
            /// Primary URL of contact person
            /// </summary>
            [Required]
            public string PrimaryURL { get; set; }
            /// <summary>
            /// Profession of contact person
            /// </summary>
            [Required]
            public string Profession { get; set; }
            /// <summary>
            /// Record ID of Fax
            /// </summary>
            public long FaxRecordId { get; set; }
            /// <summary>
            /// Primary fax of contact person
            /// </summary>
            [Required]
            public string PrimaryFax { get; set; }
            /// <summary>
            /// Swap language
            /// </summary>
            public bool SwapLanguage { get; set; }
            /// <summary>
            /// Addresses of contact person
            /// </summary>
            public List<Address> Addresses { get; set; }
            /// <summary>
            /// TMVSourceSystem
            /// </summary>
            [Required]
            public string TMVSourceSystem { get; set; }
            /// <summary>
            /// TMVContactIDGUID
            /// </summary>
            [Required]
            public string TMVContactIDGUID { get; set; }
        }

        /// <summary>
        /// Represents contact person's address
        /// </summary>
        public class Address
        {
            /// <summary>
            /// Record ID of Address
            /// </summary>
            public long RecordId { get; set; }
            /// <summary>
            /// First name of the contact person address
            /// </summary>
            public string Street { get; set; }
            /// <summary>
            /// Street2
            /// </summary>
            public string Street2 { get; set; }
            /// <summary>
            /// City of the contact person address
            /// </summary>
            public string City { get; set; }
            /// <summary>
            /// Zip code of the contact person address
            /// </summary>
            public string ZipCode { get; set; }
            /// <summary>
            /// State of the contact person address
            /// </summary>
            public string State { get; set; }
            /// <summary>
            /// Country of the contact person address
            /// </summary>
            public string ThreeLetterISORegionName { get; set; }
            /// <summary>
            /// Phone of the contact person address
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// Address type of the contact person address
            /// </summary>
            public int AddressTypeValue { get; set; }
        }

        /// <summary>
        /// Represents contact person response
        /// </summary>
        public class ContactPersonResponse
        {

            /// <summary>
            /// Initializes a new instance of the ContactPersonResponse
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="contactPerson">contact person</param>
            public ContactPersonResponse(bool status, string errorMessage, eComContactPerson contactPerson)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.ContactPerson = contactPerson;
            }

            /// <summary>
            /// Status of contact person
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of contact person
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// ContactPerson of contact person
            /// </summary>
            public eComContactPerson ContactPerson { get; set; }
        }

        public class eComContactPerson {
            public long DirPartyRecordId { get; set; }
            public string ContactPersonId { get; set; }
            public long ContactForParty { get; set; }
            public string CustAccount { get; set; }
            public bool InActive { get; set; }
            public string IsPrimary { get; set; }
            public string Title { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public long EmailRecordId { get; set; }
            public string Email { get; set; }
            public long PhoneRecordId { get; set; }
            public string Phone { get; set; }
            public string Language { get; set; }
            public bool SwapLanguage { get; set; }
            public string Gender { get; set; }
            public string Department { get; set; }
            public bool DirectMail { get; set; }
            public bool IsExternallyMaintained { get; set; }
            public string PrimaryURL { get; set; }
            public string Profession { get; set; }
            public string PrimaryFax { get; set; }
            public long FaxRecID { get; set; }
            public long URLRecID { get; set; }
            public List<eComAddress> Addresses { get; set; }
        }

        public class eComAddress {
            public long RecordId { get; set; }
            public string Street { get; set;}
            public string Street2 { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string State { get; set; }
            public string ThreeLetterISORegionName { get; set; }
            public string Phone { get; set; }
        }

        /// <summary>
        /// Represents contact person request.
        /// </summary>
        public class SaveContactPersonRequest
        {
            public SaveContactPersonRequest ()
            {
                ContactPerson = new ErpContactPersonNAL();
            }
            public ErpContactPersonNAL ContactPerson { get; set; }
        }

        /// <summary>
        /// Represents contact person request.
        /// </summary>
        public class CreateUpdateContactPersonRequest
        {
            public CreateUpdateContactPersonRequest()
            {
                ContactPerson = new ContactPersonCrm();
            }
            public ContactPersonCrm ContactPerson { get; set; }
        }

        /// <summary>
        /// Represents contact person response
        /// </summary>
        public class SaveContactPersonResponse
        {

            /// <summary>
            /// Initializes a new instance of the ContactPersonResponse
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="contactPerson">contact person</param>
            public SaveContactPersonResponse(bool status, string errorMessage, ErpContactPersonNAL contactPerson)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.ContactPerson = contactPerson;
            }

            /// <summary>
            /// Status of contact person
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of contact person
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// ContactPerson of contact person
            /// </summary>
            public ErpContactPersonNAL ContactPerson { get; set; }

        }

        /// <summary>
        /// Represents contact person response
        /// </summary>
        public class GetAllContactPersonResponse
        {

            /// <summary>
            /// Initializes a new instance of the ContactPersonResponse
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="contactPerson">contact person</param>
            public GetAllContactPersonResponse(bool status, string errorMessage, List<ContactPersonCrm> erpContactPersonNALList)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.ContactPersonList = erpContactPersonNALList;
            }

            /// <summary>
            /// Status of contact person
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of contact person
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// ContactPerson of contact person
            /// </summary>
            public List<ContactPersonCrm> ContactPersonList { get; set; }

        }

        public class GetContactPersonRequest
        {
            [Required]
            public string customerAccount { get; set; }
            [Required]
            public bool swapLanguage { get; set; } = true;
            [Required]
            public string contactPersonId { get; set; } = "-1";
        }
        public class GetAllContactPersonsRequest
        {
            [Required]
            public string customerAccount { get; set; }
            public bool swapLanguage { get; set; } = true;

        }

        #endregion

    }
}