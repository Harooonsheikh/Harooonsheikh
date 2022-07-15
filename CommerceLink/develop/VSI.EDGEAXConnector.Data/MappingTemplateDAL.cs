using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class MappingTemplateDAL : BaseClass
    {

        public MappingTemplateDAL(string storeKey) : base(storeKey)
        {

        }

        public MappingTemplateDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey,user)
        {

        }

        public List<MappingTemplate> GetAllMappingTemplates()
        {
            // int type = Convert.ToInt32(ConfigObjecsType.DeliveryModes);
            using (IntegrationDBEntities db = this.GetConnection())
            {
                List<MappingTemplate> tempalteList = db.MappingTemplate.Where(o => o.IsActive == true && o.StoreId == StoreId).ToList();
                return tempalteList;
            }
        }
        public MappingTemplate GetMappingTemplateByID(long templateId)
        {
            // int type = Convert.ToInt32(ConfigObjecsType.DeliveryModes);
            using (IntegrationDBEntities db = this.GetConnection())
            {
                MappingTemplate template = db.MappingTemplate.Where(o => o.MappingTemplateId == templateId && o.StoreId == StoreId).FirstOrDefault();
                return template;
            }
        }
        public MappingTemplate GetMappingTemplateByName(string templateName)
        {
            // int type = Convert.ToInt32(ConfigObjecsType.DeliveryModes);
            using (IntegrationDBEntities db = this.GetConnection())
            {
                MappingTemplate template = db.MappingTemplate.FirstOrDefault(o => o.Name == templateName && o.StoreId == StoreId);
                return template;
            }
        }

        public MappingTemplate GetMappingTemplate(string sourceEntity, string fileType)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                int fileTypeId = db.MappingTypeTemplate.FirstOrDefault(m => m.Name.ToLower().Equals(fileType.ToLower())).MappingTypeTemplateId;
                MappingTemplate template = db.MappingTemplate.FirstOrDefault(o => (o.StoreId == StoreId) && (o.SourceEntity == sourceEntity) && (o.MappingTemplateTypeId == fileTypeId));
                return template;
            }
        }

        public MappingTemplate AddMappingTemplate(MappingTemplate dto)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    dto.CreatedBy = UserId;
                    dto.CreatedOn = DateTime.UtcNow;
                    dto.StoreId = this.StoreId;
                    db.MappingTemplate.Add(dto);
                    db.SaveChanges();

                    return dto;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return null;
                }
            }
        }
        public MappingTemplate UpdateMappingTemplate(MappingTemplate dto)
        {
            try
            {
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    var obj = GetMappingTemplateByID(dto.MappingTemplateId);
                    obj.IsActive = dto.IsActive;
                    //   obj.IsDeleted = false;
                    obj.ModifiedOn = DateTime.UtcNow ;
                    obj.Name = dto.Name;
                    obj.SourceEntity = dto.SourceEntity;
                    obj.ReadMode = dto.ReadMode; // Rename from Type 
                    obj.XML = dto.XML;
                    obj.StoreId = StoreId;
                    obj.ModifiedBy = UserId;
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                return null;
            }
        }
        public bool DeleteMappingTemplate(long templateID)
        {
            try
            {
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    var obj = GetMappingTemplateByID(templateID);
                    obj.IsActive = false;
                    obj.ModifiedOn = DateTime.UtcNow;
                    obj.StoreId = StoreId;
                    obj.ModifiedBy = UserId;

                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                return false;
            }
        }

        public bool DeleteMappingTemplate(string name)
        {
            try
            {
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    var obj = GetMappingTemplateByName(name);
                    obj.IsActive = false;
                    obj.ModifiedOn = DateTime.UtcNow;
                    obj.StoreId = StoreId;
                    obj.ModifiedBy = UserId;
                    obj.ModifiedOn = DateTime.UtcNow;

                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                return false;
            }
        }

    }
}
