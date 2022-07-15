using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.Helper
{
    class StoreHelper 
    {
        //Map database table settings specific to newly added store
        //Note: Update below methods in case of database schema change
        public static ApplicationSetting MapAppSettings(ApplicationSetting source, int storeId)
        {
                
                ApplicationSetting appSetting = new ApplicationSetting();
                appSetting.Key = source.Key;
                appSetting.Value = source.Value;
                appSetting.Name = source.Name;
                appSetting.ScreenName = source.ScreenName;
                appSetting.SortOrder = source.SortOrder;
                appSetting.IsActive = source.IsActive;
                appSetting.StoreId = storeId;
                appSetting.FieldTypeId = source.FieldTypeId;
                appSetting.CreatedOn = DateTime.UtcNow;
                appSetting.CreatedBy = "System";
                appSetting.IsUserForDuplicateStore = false;
            
                return appSetting;

        }

        public static PaymentMethod MapPaymentMethod(PaymentMethod source, int storeId)
        {
            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.ParentPaymentMethodId = source.ParentPaymentMethodId;
            paymentMethod.ECommerceValue = source.ECommerceValue;
            paymentMethod.ErpValue = source.ErpValue;
            paymentMethod.HasSubMethod = source.HasSubMethod;
            paymentMethod.ErpCode = source.ErpCode;
            paymentMethod.IsPrepayment = source.IsPrepayment;
            paymentMethod.StoreId = storeId;
            paymentMethod.IsCreditCard = source.IsCreditCard;
            paymentMethod.UsePaymentConnector = source.UsePaymentConnector;
            paymentMethod.ServiceAccountId = source.ServiceAccountId;
            paymentMethod.CreatedOn = DateTime.UtcNow;
            paymentMethod.CreatedBy = "System";
            return paymentMethod;
        }

        public static ConfigurableObject MapConfigObject(ConfigurableObject source, int storeId)
        {
            ConfigurableObject configObject = new ConfigurableObject();
            configObject.ComValue = source.ComValue;
            configObject.ErpValue = source.ErpValue;
            configObject.EntityType = source.EntityType;
            configObject.StoreId = storeId;
            configObject.ConnectorKey = source.ConnectorKey;
            configObject.Description = source.Description;
            configObject.CreatedOn = DateTime.UtcNow;
            configObject.CreatedBy = "System";
            return configObject;
        }

        public static JobSchedule MapJobSchedule(JobSchedule source, int storeId)
        {
            JobSchedule jobSchedule = new JobSchedule();
            jobSchedule.JobId = source.JobId;
            jobSchedule.JobInterval = source.JobInterval;
            jobSchedule.IsRepeatable = source.IsRepeatable;
            jobSchedule.StoreId = storeId;
            jobSchedule.JobStatus = 1;
            jobSchedule.StartTime = source.StartTime;
            jobSchedule.IsActive = source.IsActive;
            jobSchedule.CreatedOn = DateTime.UtcNow;
            jobSchedule.CreatedBy = "System";
            return jobSchedule;
        }

        public static DimensionSet MapDimensionSet(DimensionSet source, int storeId)
        {
            DimensionSet dimensionSet = new DimensionSet();
            dimensionSet.ErpValue = source.ErpValue;
            dimensionSet.ComValue = source.ComValue;
            dimensionSet.IsActive = source.IsActive;
            dimensionSet.StoreId = storeId;
            dimensionSet.AdditionalErpValue = source.AdditionalErpValue;
            dimensionSet.CreatedOn = DateTime.UtcNow;
            dimensionSet.CreatedBy = "System";
            return dimensionSet;
        }

        public static MappingTemplate MapTemplates(MappingTemplate source, int storeId)
        {
            MappingTemplate mappingTemplate = new MappingTemplate();
            mappingTemplate.SourceEntity = source.SourceEntity;
            mappingTemplate.MappingTemplateTypeId = source.MappingTemplateTypeId;
            mappingTemplate.XML = source.XML;
            mappingTemplate.Name = source.Name;
            mappingTemplate.StoreId = storeId;
            mappingTemplate.IsActive = source.IsActive;
            mappingTemplate.CreatedOn = DateTime.UtcNow;
            mappingTemplate.CreatedBy = "System";
            return mappingTemplate;
        }

        public static EmailTemplate MapEmailTemplate(EmailTemplate source, int storeId)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.Name = source.Name;
            emailTemplate.Subject = source.Subject;
            emailTemplate.Body = source.Body;
            emailTemplate.Footer = source.Footer;
            emailTemplate.IsActive = source.IsActive;
            emailTemplate.StoreId = storeId;
            emailTemplate.CreatedOn = DateTime.UtcNow;
            emailTemplate.CreatedBy = "System";
            return emailTemplate;
        }
        
        public static Subscriber MapSubscriber(Subscriber source, int storeId)
        {
            Subscriber subscriber = new Subscriber();
            subscriber.Email = source.Email;
            subscriber.Name = source.Name;
            subscriber.IsActive = source.IsActive;
            subscriber.StoreId = storeId;
            subscriber.CreatedOn = DateTime.UtcNow;
            subscriber.CreatedBy = "System";
            return subscriber;
        }

        public static EmailSubscriber MapEmailSubscriber(EmailSubscriber source, int storeId)
        {
            EmailSubscriber emailSubscriber = new EmailSubscriber();
            emailSubscriber.TemplateId = source.TemplateId;
            emailSubscriber.SubscriberId = source.SubscriberId;
            emailSubscriber.StoreId = storeId;
            emailSubscriber.CreatedOn = DateTime.UtcNow;
            emailSubscriber.CreatedBy = "System";
            return emailSubscriber;
        }

        public static EntityFileNameParameter MapEntityFileNameParameter(EntityFileNameParameter source, int storeId)
        {
            EntityFileNameParameter entityFileNameParameter = new EntityFileNameParameter();
            entityFileNameParameter.EntityId = source.EntityId;
            entityFileNameParameter.Prefix = source.Prefix;
            entityFileNameParameter.StartingParameter = source.StartingParameter;
            entityFileNameParameter.StoreId = storeId;
            entityFileNameParameter.Parameters = source.Parameters;
            entityFileNameParameter.Postfix = source.Postfix;
            return entityFileNameParameter;         
        }

        public static DeliveryMethod MapDeliveryMethod(DeliveryMethod source, int storeId)
        {
            DeliveryMethod deliveryMethod = new DeliveryMethod();
            deliveryMethod.Name = source.Name;
            deliveryMethod.Price = source.Price;
            deliveryMethod.ItemId = source.ItemId;
            deliveryMethod.ErpKey = source.ErpKey;
            deliveryMethod.StoreId = storeId;
            deliveryMethod.Description = source.Description;
            deliveryMethod.CreatedOn = DateTime.UtcNow;
            deliveryMethod.CreatedBy = "System";
            return deliveryMethod;
        }

    }
}
