using System.Web.Http;
using System.Web.Http.Description;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// InventoryController defines properties and methods for API controller for inventory.
    /// </summary>
    [RoutePrefix("api/v1")]
    public class InventoryController : ApiBaseController
    {
        decimal maxQty = 0;

        /// <summary>
        /// Inventory Controller
        /// </summary>
        public InventoryController()
        {
            ControllerName = "InventoryController";
        }


        /// <summary>
        /// ATPInventory provides inentory availability on store info
        /// </summary>
        /// <param name="atpInventoryRequest">Type of inventory.</param>
        /// <returns>ATPInventoryResponse</returns>
        /// 
        [HttpGet]
        [Route("Inventory/GetATPInventory")]
        [ResponseType(typeof(ATPInventoryRequest))]
        public ATPInventoryResponse GetATPInventory([FromBody] ATPInventoryRequest atpInventoryRequest)
        {
            return null;
        }


        #region Private Methods/Types
                
        public class ATPInventoryRequest
        {

        }

        public class ATPInventoryResponse
        {

        }
        
        #endregion

    }
}