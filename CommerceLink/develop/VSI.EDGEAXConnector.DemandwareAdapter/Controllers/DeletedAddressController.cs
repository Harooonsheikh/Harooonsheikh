
using System;
using System.Collections.Generic;
using System.Data;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{
    /// <summary>
    /// This controller don't need to be inherited from base controller as it doesn't require any reference or instance of the Magento APIs.
    /// </summary>
    public class DeletedAddressController : BaseController, IDeletedAddressController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeletedAddressController()
            : base(false)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This funtion returns Ids of deleted addresses,
        /// </summary>
        /// <param name="lFile"></param>
        /// <returns></returns>
        public List<int> GetDeletedAddressIds(string lFile)
        {
            List<int> Ids = new List<int>();
            try
            {
                if (FileHelper.CheckFileAvailability(lFile))
                {
                    //dt = GenericCSVParser.CSVParserHelper.Parse(lFile);
                    //if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        try
                    //        {
                    //            var c = ConvertToObject(dt.Columns, row);
                    //            EcomcustomerAddressEntityItem add = CustomerCSVToEcommerceAddress(c);
                    //            if (!Ids.Contains(add.customer_address_id))
                    //            {
                    //                Ids.Add(add.customer_address_id);
                    //            }
                    //        }
                    //        catch (Exception)
                    //        {
                    //            throw;
                    //        }
                    //    }
                    //}
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Ids;
        }

        #endregion

    }
}
