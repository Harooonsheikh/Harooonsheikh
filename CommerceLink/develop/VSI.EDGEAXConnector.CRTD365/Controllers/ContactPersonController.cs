using EdgeAXCommerceLink.RetailProxy.Extensions;
using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class ContactPersonController : BaseController, IContactPersonController
    {
        public ContactPersonController(string storeKey) : base(storeKey)
        {

        }
        public ERPContactPersonResponse GetContactPerson(string customerAccount, string contactPersonId = "-1")
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ERPContactPersonResponse erpContactPersonResponse = new ERPContactPersonResponse(false, "", null);

            try
            {
                var contactPersonResponse = ECL_GetContactPerson(customerAccount, contactPersonId);

                if ((bool)contactPersonResponse.Status)
                {
                    var erpContactPerson = JsonConvert.DeserializeObject<ErpContactPerson>(contactPersonResponse.ContactPerson);
                    erpContactPersonResponse = new ERPContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, erpContactPerson);
                }
                else
                {
                    erpContactPersonResponse = new ERPContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, null);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpContactPersonResponse = new ERPContactPersonResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpContactPersonResponse;
        }
        public ERPContactPersonResponse UpdateContactPerson(ErpContactPerson contactPerson)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ERPContactPersonResponse erpContactPersonResponse = new ERPContactPersonResponse(false, "", null);

            try
            {
                var conPerson = _mapper.Map<ErpContactPerson, ContactPerson>(contactPerson);

                var contactPersonResponse = ECL_UpdateContactPerson(contactPerson, conPerson);

                if ((bool)contactPersonResponse.Status)
                {
                    if (contactPerson.Addresses.Count > 0)
                        contactPerson.Addresses[0].RecordId = contactPersonResponse.AddressRecId.HasValue ? contactPersonResponse.AddressRecId.Value : 0;

                    erpContactPersonResponse = new ERPContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, contactPerson);
                }
                else
                {
                    erpContactPersonResponse = new ERPContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, null);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpContactPersonResponse = new ERPContactPersonResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpContactPersonResponse;
        }
        /// <summary>
        /// CreateContactPerson creates Contact Person.
        /// </summary>
        /// <param name="contactPerson"></param>
        /// <returns></returns>
        public ERPContactPersonResponse CreateContactPerson(ErpContactPerson contactPerson, string requestId)
        {
            bool isExternalSystemTimeLogged = false;

            ERPContactPersonResponse erpContactPersonResponse = new ERPContactPersonResponse(false, "", null);

            try
            {
                ObservableCollection<Address> addresses = new ObservableCollection<Address>();
                foreach (ErpAddress erpAddress in contactPerson.Addresses)
                {
                    Address address = _mapper.Map<ErpAddress, Address>(erpAddress);
                    addresses.Add(address);
                }

                ContactPerson conPerson = _mapper.Map<ErpContactPerson, ContactPerson>(contactPerson);

                conPerson.Addresses = addresses;

                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_CreateContactPerson", DateTime.UtcNow);
                var contactPersonResponse = ECL_CreateContactPerson(conPerson);

                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateContactPerson", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_CreateContactPerson", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)contactPersonResponse.Status)
                {
                    var erpContactPerson = JsonConvert.DeserializeObject<ErpContactPerson>(contactPersonResponse.Result);
                    erpContactPersonResponse = new ERPContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, erpContactPerson);
                }
                else
                {
                    erpContactPersonResponse = new ERPContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, null);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateContactPerson", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoice", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpContactPersonResponse = new ERPContactPersonResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateContactPerson", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoice", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }

            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpContactPersonResponse;
        }
        /// <summary>
        /// GetContactPersons returns list of Contact Persons for provided list of CustomerIds.
        /// </summary>
        /// <param name="customerAccounts"></param>
        /// <returns></returns>
        public List<ERPContactPersonResponse> GetContactPersons(List<string> customerAccounts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ERPContactPersonResponse> erpContactPersonResponses = new List<ERPContactPersonResponse>();

            foreach (var account in customerAccounts)
            {
                var response = this.GetContactPerson(account);

                if (response != null && response.Success && response.ContactPerson != null)
                {
                    erpContactPersonResponses.Add(response);
                }
            }

            return erpContactPersonResponses;
        }
        public ERPContactAllPersonResponse GetAllContactPersons(string customerAccount)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ERPContactAllPersonResponse erpContactPersonResponse = new ERPContactAllPersonResponse(false, "", null);

            try
            {
                var contactPersonsResponse = ECL_GetAllContactPersons(customerAccount);
                if (contactPersonsResponse != null)
                {
                    if ((bool)contactPersonsResponse.Status)
                    {
                        //List<ErpContactPersonNAL> contactPersonNALList = _mapper.Map<ICollection<ContactPersonNALResponseSet>, ICollection<ErpContactPersonNAL>>(contactPersonsResponse.ListContactPerson).ToList();

                        erpContactPersonResponse = new ERPContactAllPersonResponse((bool)contactPersonsResponse.Status, contactPersonsResponse.Message, null);// contactPersonNALList);
                    }
                    else
                    {
                        erpContactPersonResponse = new ERPContactAllPersonResponse((bool)contactPersonsResponse.Status, contactPersonsResponse.Message, null);
                    }

                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpContactPersonResponse = new ERPContactAllPersonResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpContactPersonResponse;
        }
        public ERPSaveContactPersonResponse SaveContactPerson(ErpContactPersonNAL contactPerson, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            ERPSaveContactPersonResponse erpContactPersonResponse;

            try
            {
                var conPerson = ConvertToContactPerson(contactPerson);

                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_SaveContactPerson", DateTime.UtcNow);
                var contactPersonResponse = ECL_TV_SaveContactPerson(conPerson);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "SaveContactPerson", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_SaveContactPerson", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)contactPersonResponse.Status)
                {
                    var contactPersonResult = JsonConvert.DeserializeObject<ContactPerson>(contactPersonResponse.Result);
                    var erpContactPerson = ConvertToContactPersonNal(contactPersonResult);

                    erpContactPersonResponse = new ERPSaveContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, erpContactPerson);
                }
                else
                {
                    erpContactPersonResponse = new ERPSaveContactPersonResponse((bool)contactPersonResponse.Status, contactPersonResponse.Message, null);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "SaveContactPerson", GetElapsedTime());

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_SaveContactPerson", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpContactPersonResponse = new ERPSaveContactPersonResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "SaveContactPerson", GetElapsedTime());

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_SaveContactPerson", DateTime.UtcNow);
                }

                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);

            }

            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpContactPersonResponse;
        }

        private ContactPerson ConvertToContactPerson(ErpContactPersonNAL contactPerson)
        {
            var conPerson = _mapper.Map<ErpContactPersonNAL, ContactPerson>(contactPerson);
            conPerson.RecId = 0;
            conPerson.FaxRecID = contactPerson.FaxRecordId;
            conPerson.URLRecID = contactPerson.URLRecordId;

            if (!string.IsNullOrEmpty(contactPerson.Street) &&
                !string.IsNullOrEmpty(contactPerson.City) &&
                !string.IsNullOrEmpty(contactPerson.Country)
                )
            {
                conPerson.Addresses = new ObservableCollection<Address>();

                var address = new Address()
                {
                    Name = contactPerson.AddressName,
                    Street = contactPerson.Street,
                    BuildingCompliment = contactPerson.Street2,
                    City = contactPerson.City,
                    State = contactPerson.State,
                    ThreeLetterISORegionName = contactPerson.Country,
                    ZipCode = contactPerson.ZipCode
                };

                conPerson.Addresses.Add(address);
            }

            return conPerson;
        }

        private ErpContactPersonNAL ConvertToContactPersonNal(ContactPerson contactPerson)
        {
            var conPerson = _mapper.Map<ContactPerson, ErpContactPersonNAL>(contactPerson);
            conPerson.FaxRecordId = contactPerson.FaxRecID.HasValue ? contactPerson.FaxRecID.Value : 0;
            conPerson.URLRecordId = contactPerson.URLRecID.HasValue ? contactPerson.URLRecID.Value : 0;

            if (contactPerson.Addresses.Count > 0)
            {
                conPerson.AddressName = contactPerson.Addresses[0].Name;
                conPerson.Street = contactPerson.Addresses[0].Street;
                conPerson.Street2 = contactPerson.Addresses[0].BuildingCompliment;
                conPerson.City = contactPerson.Addresses[0].City;
                conPerson.State = contactPerson.Addresses[0].State;
                conPerson.Country = contactPerson.Addresses[0].ThreeLetterISORegionName;
                conPerson.ZipCode = contactPerson.Addresses[0].ZipCode;
            }

            return conPerson;
        }

        #region RetailServer API Call
        [Trace]
        private GetContactPersonResponse ECL_GetContactPerson(string customerAccount, string contactPersonId)
        {
            var contactPersonManager = RPFactory.GetManager<IContactPersonManager>();
            return Task.Run(async () => await contactPersonManager.ECL_GetContactPerson(0, customerAccount, baseCompany, contactPersonId)).Result;
        }
        [Trace]
        private UpdateContactPersonResponse ECL_UpdateContactPerson(ErpContactPerson contactPerson, ContactPerson conPerson)
        {
            var contactPersonManager = RPFactory.GetManager<IContactPersonManager>();
            return Task.Run(async () => await contactPersonManager.ECL_UpdateContactPerson(conPerson, baseCompany)).Result;
        }
        [Trace]
        private CreateContactPersonResponse ECL_CreateContactPerson(ContactPerson conPerson)
        {
            var contactPersonManager = RPFactory.GetManager<IContactPersonManager>();
            return Task.Run(async () => await contactPersonManager.ECL_CreateContactPerson(conPerson, baseCompany)).Result;
        }
        [Trace]
        private GetContactPersonResponse ECL_GetAllContactPersons(string customerAccount)
        {
            throw new NotImplementedException();
            //var contactPersonManager = RPFactory.GetManager<IContactPersonManager>();
            //return Task.Run(async () => await contactPersonManager.ECL_GetAllContactPersons(0, customerAccount, baseCompany)).Result;
        }
        [Trace]
        private SaveContactPersonResponse ECL_TV_SaveContactPerson(ContactPerson contactPerson)
        {
            var contactPersonManager = RPFactory.GetManager<IContactPersonManager>();
            return Task.Run(async () => await contactPersonManager.ECL_TV_SaveContactPerson(contactPerson, baseCompany)).Result;
        }
        #endregion
    }
}
