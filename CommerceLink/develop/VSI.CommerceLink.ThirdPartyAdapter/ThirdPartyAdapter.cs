using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.CommerceLink.ThirdPartyAdapter.Controllers;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;

namespace VSI.CommerceLink.ThirdPartyAdapter
{
    public class ThirdPartyAdapter 
    {
        #region Protected Methods

        /// <summary>
        /// This function binds controllers to interfaces.
        /// </summary>
        /// <param name="builder"></param>
        //protected override void Load(ContainerBuilder builder)
        //{
        //    builder.Register((c, p) => new SaleOrderController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<ISaleOrderController>();
        //    builder.Register((c, p) => new SalesOrderStatusController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<ISalesOrderStatusController>();

        //}

        #endregion
    }
}
