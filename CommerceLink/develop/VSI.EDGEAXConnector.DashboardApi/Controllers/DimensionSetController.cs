using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class DimensionSetController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {
            DimensionSetDAL dimensionSetDAL = null;
            List<DimensionSet> lstConfigObjects = null;
            List<DimensionSetVM> lstConfigObjectsVM = null;
            try
            {
                dimensionSetDAL = new DimensionSetDAL(this.DbConnStr, this.StoreKey, this.User);
                lstConfigObjects = dimensionSetDAL.GetAllDimensionSets();
                lstConfigObjectsVM = lstConfigObjects.Select(m => MapDimensionSet(m)).ToList();
                return Ok(lstConfigObjectsVM);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Update(DimensionSetVM dimensionSet)
        {
            DimensionSetDAL dimensionSetDAL = null;
            try
            {
                dimensionSetDAL = new DimensionSetDAL(this.DbConnStr, this.StoreKey, this.User);
                bool isUpdated = dimensionSetDAL.UpdateDimensionSetById(MapDimensionSet(dimensionSet));
                if (isUpdated)
                {
                    return Ok("Success");
                }
                return BadRequest("Failed");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        private DimensionSetVM MapDimensionSet(DimensionSet set)
        {
            DimensionSetVM vm = new DimensionSetVM();
            vm.AdditionalErpValue = set.AdditionalErpValue;
            vm.ComValue = set.ComValue;
            vm.ErpValue = set.ErpValue;
            vm.Id = set.DimensionSetId;
            if (set.IsActive != null)
            {
                vm.IsActive = set.IsActive.Value;
            }
            vm.StoreId_FK = set.StoreId;
            return vm;
        }

        private DimensionSet MapDimensionSet(DimensionSetVM vm)
        {
            DimensionSet set = new DimensionSet();
            set.AdditionalErpValue = vm.AdditionalErpValue;
            set.ComValue = vm.ComValue;
            set.ErpValue = vm.ErpValue;
            set.DimensionSetId = vm.Id;
            set.IsActive = vm.IsActive;
            set.StoreId = vm.StoreId_FK;
            return set;
        }
    }
}

