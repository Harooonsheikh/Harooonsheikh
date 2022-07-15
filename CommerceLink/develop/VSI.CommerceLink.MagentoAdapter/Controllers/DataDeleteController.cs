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
using VSI.CommerceLink.MagentoAdapter.DataModels;
using VSI.CommerceLink.MagentoAPI.MageAPI;
using System.Configuration;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{
    public class DataDeleteController : BaseController, IDataDeleteController
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataDeleteController(string storeKey) : base(false, storeKey)
        {
        }

        #endregion

        #region Public Methods

        public void DataDeleteSync()
        {
            var dal = new DataDeleteDAL(currentStore.StoreKey);
            var dataDeleteList = dal.GetByStatus(DataDeleteStatus.Created);
            try
            {
                string strMethodNames = ConfigurationManager.AppSettings["GDPRMethodNames"].ToString();
                if (dataDeleteList != null)
                {
                    dal.Delete(dataDeleteList, strMethodNames);
                }
                else
                {
                    CustomLogger.LogTraceInfo("DataDelete List is Empty", currentStore.StoreId, currentStore.CreatedBy);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
