using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class CustomerController : BaseController, ICustomerController
    {
        public ErpCustomer CreateCustomer(ErpCustomer objCustomer, long channelId)
        {
            Customer customer = new Customer();
            Customer objCustomerReturn;
            ErpCustomer objErpCustomerReturn;
            var objCustomerManager = RPFactory.GetManager<ICustomerManager>();

            try {
                // Map the ErpCustomer object (passed in parmeter) to Customer object
                customer = Mapper.Map<ErpCustomer, Customer>(objCustomer);

                /*
                Microsoft.Web.Services3.Security.Tokens.UsernameToken unt = new UsernameToken("aali@vitaminworld.com", "D0killjust1", PasswordOption.SendPlainText);
                EdgeAXCommerceLink.Commerce.RetailProxy.Authentication.UserToken a;

                EdgeAXCommerceLink.Commerce.RetailProxy.RetailServerContext rsc = new RetailServerContext(new Uri(""));
                rsc.SetUserToken(unt);
                */

                string externalIdentityId = objCustomer.EcomCustomerId;
                //string externalIdentityIssuer = "VW";
                string externalIdentityIssuer = string.Empty;

                // Create new customer
                /*US
               // objCustomerReturn = objCustomerManager.CreateNewCustomer(customer, externalIdentityId, externalIdentityIssuer, baseChannelId).Result; // New Method

                // Map returned Customer object to ErpCustomer object
               // objErpCustomerReturn = Mapper.Map<Customer, ErpCustomer>(objCustomerReturn); */

                return null;
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }

            // return the returned ErpCustomer object
            return objErpCustomerReturn;
        }
        public ErpCustomer UpdateCustomer(ErpCustomer customer)
        {
            //TODO: this method is not in VW scope, 
            //If wants to enable SyncCustomer Job then need to implement this function
            return customer;
        }
        public ErpCustomer GetCustomer(string accountNuber)
        {
            ErpCustomer customer = new ErpCustomer();
            var objCustomerManager = RPFactory.GetManager<ICustomerManager>();

            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();

            // Setup QueryResultSettings
            QueryResultSettings objQueryResultSettings = new QueryResultSettings();
            objQueryResultSettings.Paging = Paging_0_1000;

            PagedResult<GlobalCustomer> globalCustomer;

            try
            {
                //TODO: this method is not in VW scope, 
                //If wants to enable SyncCustomer Job then need to implement this function
                // fetch customer
                globalCustomer = objCustomerManager.Search(customerSearchCriteria, objQueryResultSettings).Result;

                // Map returned Customer object to ErpCustomer object
                //customer = Mapper.Map<GlobalCustomer, ErpCustomer>(globalCustomer);
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
            return customer;
        }

        public ErpCustomer GetCustomerData(string AccountNuber, int searchLocation)
        {
            throw new NotImplementedException();
        }
    }
}
