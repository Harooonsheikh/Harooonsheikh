using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Data
{
    public class IntegrationManager : BaseClass
    {
        public IntegrationManager(string storeKey) : base(storeKey)
        {

        }
        public IntegrationManager(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }
        #region Contsatnts
        private const string AbandonedCartDiscriptionPassed = "Abandoned";
        private const string AbandonedCartDiscriptionFailedToCreate = "Abandoned Failed to Create";
        private const string AbandonedCartDiscriptionFailedCart = "Abandoned Failed due to Cart";
        private const string AbandonedCartDiscriptionFailedContactPerson = "Abandoned Failed due to Contact Person";
        private const string AbandonedCartDiscriptionFailed = "Abandoned Failed";
        #endregion

        public void CreateIntegrationKey(Entities entity, string erpKey, string ecomKey, string extraParam = "", bool isEcomIdString = false)
        {
            bool throwEx = false;
            try
            {
                if (entity == Entities.Customer || entity == Entities.CustomerAddress)
                {
                    if (!isEcomIdString)
                    {
                        long comKey = 0;
                        if (!long.TryParse(ecomKey, out comKey))
                        {
                            throwEx = true;
                            throw new CommerceLinkError(string.Format("Cannot create Integration Key for ERPID [{0}] and COMKEY [{1}] for Entity [{2}] with Description [{3}] at [{4}]", erpKey, ecomKey, entity, extraParam, DateTime.UtcNow));
                        }
                    }
                }
                // Following code was commented to allow GUID data
                //if (entity == Entities.Customer || entity == Entities.CustomerAddress)
                //{
                //    long comKey = 0;
                //    if (!long.TryParse(ecomKey, out comKey))
                //    {
                //        throwEx = true;
                //        throw new Exception(string.Format("Cannot create Integration Key for ERPID [{0}] and COMKEY [{1}] for Entity [{2}] with Description [{3}] at [{4}]", erpKey, ecomKey, entity, extraParam, DateTime.UtcNow));
                //    }
                //}
                short entityId = (short)entity;
                IntegrationKey exitingKey = null;
                if (ecomKey == extraParam)
                    exitingKey = GetKey(entity, extraParam);
                else
                    exitingKey = GetKey(entity, erpKey, ecomKey);


                if (exitingKey == null)
                {
                    IntegrationKey obj = new IntegrationKey()
                    {
                        EntityId = entityId,
                        ErpKey = erpKey,
                        ComKey = ecomKey,
                        Description = extraParam,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = UserId,
                        StoreId = StoreId

                    };
                    using (var db = this.GetConnection())
                    {
                        db.IntegrationKey.Add(obj);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (ecomKey != extraParam)
                    {
                        CustomLogger.LogException(string.Format("Integration Key with ErpKey:{0} and ComKey:{1} already exists", erpKey, ecomKey), StoreId, UserId);
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
                if (throwEx)
                    throw;
            }
        }
        public IntegrationKey GetErpKey(Entities entity, string comKey)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                // VSTS 25336: Query updated to fetch only required data from database
                var integrationKey = db.IntegrationKey
                    .Where(k => k.EntityId == entityId && k.ComKey == comKey && k.StoreId == StoreId)
                    .OrderByDescending(x => x.IntegrationKeyId)
                    .Select(k => new
                    {
                        IntegrationKeyId = k.IntegrationKeyId,
                        EntityId = k.EntityId,
                        ErpKey = k.ErpKey,
                        ComKey = k.ComKey,
                        Description = k.Description,
                        StoreId = k.StoreId
                    })
                    .FirstOrDefault();

                if (integrationKey != null)
                {
                    return new IntegrationKey()
                    {
                        IntegrationKeyId = integrationKey.IntegrationKeyId,
                        EntityId = integrationKey.EntityId,
                        ErpKey = integrationKey.ErpKey,
                        ComKey = integrationKey.ComKey,
                        Description = integrationKey.Description,
                        StoreId = integrationKey.StoreId
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        public IntegrationKey GetIntegrationKey(Entities entity, string comKey)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.OrderByDescending(x => x.CreatedOn).FirstOrDefault(k => k.EntityId == entityId && k.ComKey == comKey);
            }
        }
        public IntegrationKey GetErpKey(Entities entity, string comKey, string description)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.OrderByDescending(x => x.CreatedOn).FirstOrDefault(k => k.EntityId == entityId && k.ComKey == comKey && k.Description == description && k.StoreId == StoreId);
            }
        }
        public IntegrationKey GetComKey(Entities entity, string erpKey)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.OrderByDescending(x => x.CreatedOn).FirstOrDefault(k => k.EntityId == entityId && k.ErpKey == erpKey && k.StoreId == StoreId);
            }
        }
        public IntegrationKey GetKeyByDescription(Entities entity, string desc)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.OrderByDescending(x => x.CreatedOn).FirstOrDefault(k => k.EntityId == entityId && k.Description == desc && k.StoreId == StoreId);
            }
        }
        public IntegrationKey GetKeyByContainsDescription(Entities entity, string desc)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                var customers = db.IntegrationKey.OrderByDescending(x => x.CreatedOn).Where(k => k.EntityId == entityId && k.StoreId == StoreId).ToList();
                foreach (var customer in customers)
                {
                    var descArray = customer.Description.ToUpper().Split(':');
                    if (descArray.Length >= 2 && descArray[1].Trim() == desc.Trim().ToUpper())
                    {
                        return customer;
                    }
                }
                return null;
            }
        }
        public IntegrationKey GetKey(Entities entity, string erpKey, string ecomKey)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.OrderByDescending(x => x.CreatedOn).FirstOrDefault(k => k.EntityId == entityId && k.ErpKey == erpKey && k.ComKey == ecomKey && k.StoreId == StoreId);
            }
        }
        public IntegrationKey GetKey(Entities entity, string description)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.OrderByDescending(x => x.CreatedOn).FirstOrDefault(k => k.EntityId == entityId && k.Description == description && k.StoreId == StoreId);
            }
        }
        public void UpdateIntegrationKey(Entities entity, string erpKey, string ecomKey, string desc = "")
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                var row = db.IntegrationKey.FirstOrDefault(k => k.EntityId == entityId && k.ErpKey == erpKey && k.ComKey == ecomKey && k.StoreId == StoreId);
                if (row != null)
                {
                    row.Description = desc;
                    row.ModifiedOn = DateTime.UtcNow;
                    row.StoreId = StoreId;
                    row.ModifiedBy = UserId;

                    db.SaveChanges();
                }
            }
        }
        public void UpdateIntegrationKey(IntegrationKey key)
        {
            if (key != null)
            {
                using (var context = this.GetConnection())
                {
                    var result = context.UpdateIntegrationKey(key.IntegrationKeyId, key.Description, key.ModifiedOn);
                }
            }
        }
        public void UpdateIntegrationKeyAllFields(IntegrationKey key)
        {
            if (key != null)
            {
                using (var context = this.GetConnection())
                {
                    var result = context.UpdateIntegrationKeyAllFields(key.IntegrationKeyId, key.ErpKey, key.ComKey, null);

                }
            }
        }
        public List<IntegrationKey> GetAllEntityKeys(Entities entity)
        {
            short entityId = (short)entity;
            using (var db = this.GetConnection())
            {
                return db.IntegrationKey.Where(k => k.EntityId == entityId && k.StoreId == StoreId).ToList();
            }
        }
        public List<IntegrationKey> GetMasterProductEcomKey(List<long> erpIds)
        {
            short categoryAssignmentEntity = (short)Entities.CategoryAssignment;
            short prodEntity = (short)Entities.Product;
            List<string> ids = erpIds.ConvertAll<string>(x => x.ToString());
            using (var db = this.GetConnection())
            {
                var comKeys = db.IntegrationKey.Where(k => k.EntityId == prodEntity && ids.Contains((k.ErpKey)) && k.StoreId == StoreId).Select(k => k.ComKey).ToList();
                return db.IntegrationKey.Where(x => x.EntityId == categoryAssignmentEntity && comKeys.Contains(x.ComKey) && x.StoreId == StoreId).ToList();
            }
        }
        /// <summary>
        /// DeleteEntityKeys deletes list of provided items
        /// </summary>
        /// <param name="deletedKeys"></param>
        public void DeleteEntityKeys(List<IntegrationKey> deletedKeys)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    DbEntityEntry entity;

                    foreach (var key in deletedKeys)
                    {
                        entity = db.Entry(key);
                        if (entity.State == EntityState.Detached)
                        {
                            db.IntegrationKey.Attach(key);
                        }
                        db.IntegrationKey.Remove(key);
                    }
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
            }
        }
        /// <summary>
        /// GetAbandonedCartKeys get and returns abandoned cart item keys. 
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public List<IntegrationKey> GetAbandonedCartKeys(int days)
        {
            var cartEntity = (short)Entities.Cart;
            var orderEntity = (short)Entities.SaleOrder;
            var cutoffDate = DateTime.Now.Date.AddDays(days * -1);

            using (var db = new IntegrationDBEntities())
            {
                var oderErpKeys = db.IntegrationKey.Where(k => k.EntityId == orderEntity && k.StoreId == StoreId).Select(k => k.ComKey).ToList();
                //var carts = db.IntegrationKeys.Where(k => !k.Description.Equals(IntegrationManager.AbandonedCartDiscription, StringComparison.CurrentCultureIgnoreCase) && k.Entity == cartEntity && k.CreatedOn <= cutoffDate && !oderErpKeys.Contains(k.ErpKey)).ToList();
                var carts = db.IntegrationKey.Where(k => (k.Description == null || k.Description.Trim() == string.Empty) && k.EntityId == cartEntity && k.CreatedOn <= cutoffDate && !oderErpKeys.Contains(k.ErpKey) && k.StoreId == StoreId).ToList();
                return carts;
            }
        }
        private void UpdateAbandonedCart(string cartId, string description)
        {
            var cartEntity = (short)Entities.Cart;

            using (var db = new IntegrationDBEntities())
            {
                var cartKey = db.IntegrationKey.FirstOrDefault(k => k.EntityId == cartEntity && k.ErpKey == cartId && k.StoreId == StoreId);

                if (cartKey != null)
                {
                    cartKey.Description = description;
                    cartKey.ModifiedOn = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }
        public void MarkAbandonedCartProcessed(string cartId)
        {
            UpdateAbandonedCart(cartId, IntegrationManager.AbandonedCartDiscriptionPassed);
        }
        public void MarkAbandonedCartFailedToCreate(string cartId)
        {
            UpdateAbandonedCart(cartId, IntegrationManager.AbandonedCartDiscriptionFailedToCreate);
        }
        public void MarkAbandonedCartFailedCart(string cartId)
        {
            UpdateAbandonedCart(cartId, IntegrationManager.AbandonedCartDiscriptionFailedCart);
        }
        public void MarkAbandonedCartFailedContactPerson(string cartId)
        {
            UpdateAbandonedCart(cartId, IntegrationManager.AbandonedCartDiscriptionFailedContactPerson);
        }
        public string GetProductVariantId(string erpKey)
        {
            string variantId = string.Empty;
            short entityId = (short)Entities.Product;
            using (var db = new IntegrationDBEntities())
            {
                var integrationKey = db.IntegrationKey.FirstOrDefault(k => k.EntityId == entityId && k.ErpKey == erpKey && k.StoreId == StoreId);

                if (integrationKey != null && !string.IsNullOrWhiteSpace(integrationKey.Description))
                {
                    var descriptionTokens = integrationKey.Description.Split(':');

                    if (descriptionTokens != null && descriptionTokens.Length > 1)
                    {
                        variantId = descriptionTokens[1];
                    }
                }
            }

            return variantId;
        }

    }
}
