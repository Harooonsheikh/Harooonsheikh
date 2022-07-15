using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data
{
    /// <summary>
    /// This is a partial class which will let us access data from any mirror data base of the current Data Entity model.
    /// All the mirrors are supposed to be on the same server.
    /// This class will take the name of the data base as arguement.
    /// For example in our case we have 3 similar databases Jo_EdgeAxConnectorDB,Ce_EdgeAxConnectorDB 
    /// and Eq_EdgeAxConnectorDb. The data entity model is mapped from only Jo_EdgeAxConnectorDb, but if we want to access
    /// some data,lets say logs data from Ce_EdgeAxConnectorDB then this class will help us do that by passing the name of the database as
    /// arguement.
    /// </summary>
    public partial class IntegrationDBEntities : DbContext
    {
        /// <summary>
        /// Constructor over loaded.
        /// </summary>
        /// <param name="nameOfDb"></param>

        public IntegrationDBEntities(string nameOfDb = "IntegrationDBEntities")
            : base(nameOfDb)
        {
            //this.Configuration.AutoDetectChangesEnabled = false;
        }

        public virtual int CreateIntegrationKey(Nullable<short> entity, string erpKey, string comKey, string description, Nullable<System.DateTime> createdOn)
        {
            var entityParameter = entity.HasValue ?
                new ObjectParameter("Entity", entity) :
                new ObjectParameter("Entity", typeof(short));

            var erpKeyParameter = erpKey != null ?
                new ObjectParameter("ErpKey", erpKey) :
                new ObjectParameter("ErpKey", typeof(string));

            var comKeyParameter = comKey != null ?
                new ObjectParameter("ComKey", comKey) :
                new ObjectParameter("ComKey", typeof(string));

            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));

            var createdOnParameter = createdOn.HasValue ?
                new ObjectParameter("CreatedOn", createdOn) :
                new ObjectParameter("CreatedOn", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateIntegrationKey", entityParameter, erpKeyParameter, comKeyParameter, descriptionParameter, createdOnParameter);
        }

        public virtual ObjectResult<GetIntegrationKey_Result> GetIntegrationKey(Nullable<short> entity, string key, Nullable<short> keyType)
        {
            var entityParameter = entity.HasValue ?
                new ObjectParameter("Entity", entity) :
                new ObjectParameter("Entity", typeof(short));

            var keyParameter = key != null ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(string));

            var keyTypeParameter = keyType.HasValue ?
                new ObjectParameter("KeyType", keyType) :
                new ObjectParameter("KeyType", typeof(short));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetIntegrationKey_Result>("GetIntegrationKey", entityParameter, keyParameter, keyTypeParameter);
        }

        public virtual int UpdateIntegrationKey(Nullable<long> id, string description, Nullable<System.DateTime> updatedOn)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(long));

            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));

            var updatedOnParameter = updatedOn.HasValue ?
                new ObjectParameter("UpdatedOn", updatedOn) :
                new ObjectParameter("UpdatedOn", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateIntegrationKey", idParameter, descriptionParameter, updatedOnParameter);
        }

        public virtual int UpdateIntegrationKeyAllFields(Nullable<long> id, string erpKey, string comKey, Nullable<System.DateTime> updatedOn)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(long));

            var erpKeyParameter = erpKey != null ?
                new ObjectParameter("ErpKey", erpKey) :
                new ObjectParameter("ErpKey", typeof(string));

            var comKeyParameter = comKey != null ?
                new ObjectParameter("ComKey", comKey) :
                new ObjectParameter("ComKey", typeof(string));

            var updatedOnParameter = updatedOn.HasValue ?
                new ObjectParameter("UpdatedOn", updatedOn) :
                new ObjectParameter("UpdatedOn", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateIntegrationKeyAllFields", idParameter, erpKeyParameter, comKeyParameter, updatedOnParameter);
        }

        public virtual int GetAddressKey()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetAddressKey");
        }

        public virtual int GetAllAddressIdsByActiveAddressIdVSI(Nullable<long> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetAllAddressIdsByActiveAddressIdVSI", idParameter);
        }

        public virtual int GetCustomersByUpdatedAddressVSI(Nullable<System.DateTime> timestamp, string custGroup, string dataAreaId)
        {
            var timestampParameter = timestamp.HasValue ?
                new ObjectParameter("Timestamp", timestamp) :
                new ObjectParameter("Timestamp", typeof(System.DateTime));

            var custGroupParameter = custGroup != null ?
                new ObjectParameter("CustGroup", custGroup) :
                new ObjectParameter("CustGroup", typeof(string));

            var dataAreaIdParameter = dataAreaId != null ?
                new ObjectParameter("DataAreaId", dataAreaId) :
                new ObjectParameter("DataAreaId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetCustomersByUpdatedAddressVSI", timestampParameter, custGroupParameter, dataAreaIdParameter);
        }

        public virtual int GetProductDiscountVSI()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetProductDiscountVSI");
        }

        public virtual ObjectResult<GetState_Result> GetState(string code, string countryCode)
        {
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));

            var countryCodeParameter = countryCode != null ?
                new ObjectParameter("CountryCode", countryCode) :
                new ObjectParameter("CountryCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetState_Result>("GetState", codeParameter, countryCodeParameter);
        }

        public virtual int GetUpdatedCustomersVSI(Nullable<System.DateTime> timestamp, string custGroup, string dataAreaId)
        {
            var timestampParameter = timestamp.HasValue ?
                new ObjectParameter("Timestamp", timestamp) :
                new ObjectParameter("Timestamp", typeof(System.DateTime));

            var custGroupParameter = custGroup != null ?
                new ObjectParameter("CustGroup", custGroup) :
                new ObjectParameter("CustGroup", typeof(string));

            var dataAreaIdParameter = dataAreaId != null ?
                new ObjectParameter("DataAreaId", dataAreaId) :
                new ObjectParameter("DataAreaId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetUpdatedCustomersVSI", timestampParameter, custGroupParameter, dataAreaIdParameter);
        }

        public virtual int InsertCallingUserLog(string userName, string method)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));

            var methodParameter = method != null ?
                new ObjectParameter("Method", method) :
                new ObjectParameter("Method", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertCallingUserLog", userNameParameter, methodParameter);
        }
    }
}
