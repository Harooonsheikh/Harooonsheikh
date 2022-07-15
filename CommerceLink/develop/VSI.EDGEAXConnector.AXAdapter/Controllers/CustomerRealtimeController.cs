//using Microsoft.Dynamics.Commerce.Runtime;
//using Microsoft.Dynamics.Commerce.Runtime.DataModel;
//using System;
//using System.Collections.Generic;
//using VSI.EDGEAXConnector.AXAdapter.DataModels;
//using VSI.EDGEAXConnector.ErpAdapter.Interface;
//using VSI.EDGEAXConnector.Logging;

//namespace VSI.EDGEAXConnector.AXAdapter.Controllers
//{

//    /// <summary>
//    /// CustomerRealtimeController class is Ax Adapter Base Controller.
//    /// </summary>
//    public class CustomerRealtimeController : BaseController, ICustomerRealtimeController<ERPCustomer>
//    {
//        // initialize the channel: instantiate CRT, create CRT data managers, load channel settings
//        //CommerceRuntime runtime;
//        //ChannelState currentChannelState;

//        #region Constructor

//        /// <summary>
//        /// Initializes a new instance of the class.
//        /// </summary>
//        public CustomerRealtimeController()
//        {
//            //// initialize the channel: instantiate CRT, create CRT data managers, load channel setting
//            //Utility.InitializeCommerceRuntime();
//            //runtime = Utility._CommerceRuntime;
//            //currentChannelState = ChannelState.InitializeChannel(runtime, new Guid());      
//        }

//        #endregion

//        #region Public Methods

//        /// <summary>
//        /// CreateCustomer creates Customer in AX.
//        /// </summary>
//        /// <param name="customer"></param>
//        /// <returns></returns>

//        public ERPCustomer CreateCustomer(ERPCustomer customer)
//        {
//            Customer c;

//            try
//            {
//                c = currentChannelState.CustomerManager.CreateCustomer(customer as Customer);
//                return ERPCustomer.GetDerivedObject(c);
//            }
//            catch (Exception exp)
//            {
//                CustomLogger.LogException(exp);
//            }

//            return null;
//        }

//        /// <summary>
//        /// UpdateCustomer Updates Customer in AX.
//        /// </summary>
//        /// <param name="customer"></param>
//        /// <returns></returns>

//        public ERPCustomer UpdateCustomer(ERPCustomer customer)
//        {
//            Customer c;

//            try
//            {
//                c = currentChannelState.CustomerManager.UpdateCustomer(customer as Customer);
//                return ERPCustomer.GetDerivedObject(c);
//            }
//            catch (Exception exp)
//            {
//                CustomLogger.LogException(exp);
//            }

//            return null;
//        }

//        /// <summary>
//        /// GetCustomer gets Customer from AX.
//        /// </summary>
//        /// <param name="AccountNumber"></param>
//        /// <returns></returns>

//        public ERPCustomer GetCustomer(string AccountNumber)
//        {
//            /*
//            Customer c;

//            try
//            {
//                currentChannelState.ListingQueryCriteria = new QueryResultSettings(PagingInfo.AllRecords);
//                c = currentChannelState.CustomerManager.GetCustomer(AccountNumber);
//                return ERPCustomer.GetDerivedObject(c);
//            }
//            catch (Exception exp)
//            {
//                CustomLogger.LogException(exp);
//            }
//            */
//            return null;
//        }

//        /// <summary>
//        /// GetCustomersList gets Customer list from AX.
//        /// </summary>
//        /// <returns></returns>
//        public List<ERPCustomer> GetCustomersList()
//        {
//            List<ERPCustomer> cList = new List<ERPCustomer>();

//            try
//            {
//                currentChannelState.ListingQueryCriteria = new QueryResultSettings(new PagingInfo());
//                PagedResult<Customer> pagedCustomerResults = currentChannelState.CustomerManager.GetCustomers(currentChannelState.ListingQueryCriteria);

//                foreach (var c in pagedCustomerResults.Results)
//                {
//                    cList.Add(ERPCustomer.GetDerivedObject(c));
//                }

//                return cList;
//            }
//            catch (Exception exp)
//            {
//                CustomLogger.LogException(exp);
//            }
//            return null;
//        }

//        /// <summary>
//        /// FindCustomers searches Customer from AX.
//        /// </summary>
//        /// <param name="Criteria"></param>
//        /// <returns></returns>
//        public List<ERPCustomer> FindCustomers(string Criteria)
//        {
//            List<ERPCustomer> searchResults = new List<ERPCustomer>();

//            currentChannelState.ListingQueryCriteria = new QueryResultSettings(new PagingInfo());
//            CustomerSearchCriteria criteria = new CustomerSearchCriteria();
//            criteria.Keyword = Criteria;

//            PagedResult<GlobalCustomer> pagedCustomerResults = currentChannelState.CustomerManager.SearchCustomers(criteria, currentChannelState.ListingQueryCriteria);
//            ERPCustomer customer;

//            foreach (var globalCustomer in pagedCustomerResults.Results)
//            {
//                customer = new ERPCustomer();
//                customer.AccountNumber = globalCustomer.AccountNumber;
//                customer.CustomerType = globalCustomer.CustomerType;
//                customer.CustomerTypeValue = globalCustomer.CustomerTypeValue;
//                customer.Email = globalCustomer.Email;
//                //customer.ExtensionData = globalCustomer.ExtensionData;
//                customer.ExtensionProperties = globalCustomer.ExtensionProperties;
//                customer.PartyNumber = globalCustomer.PartyNumber;
//                customer.Phone = globalCustomer.Phone;
//                customer.RecordId = globalCustomer.RecordId;
//                searchResults.Add(customer);
//            }

//            return searchResults;
//        }

//        #endregion
//    }
//}
