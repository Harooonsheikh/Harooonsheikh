using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MongoDB.Bson;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.MongoData.Enums;


namespace VSI.EDGEAXConnector.MongoData.Helpers
{
    public class InventoryMongoHelper
    {
        private MongoDocumentController mongoDocController { get; set; }
        public InventoryMongoHelper(string connection, string dbName)
        {
            this.mongoDocController = new MongoDocumentController(connection, dbName);
        }

        const string headerTag = "header";
        const string recordsTag = "record";        

        public bool SaveInventory(string collectionName, XmlDocument inventoryXmlDocument)
        {
            collectionName = collectionName.Replace("-", "");
            try
            {
                var header = inventoryXmlDocument.GetElementsByTagName(headerTag);
                InsertManyNodes(collectionName, header);

                var records = inventoryXmlDocument.GetElementsByTagName(recordsTag);
                InsertManyNodes(collectionName, records);
            }
            catch
            {
                throw;
            }

            return true;
        }

        private void InsertManyNodes(string collectionName, XmlNodeList nodeList)
        {
            var bsonNodeList = nodeList.Cast<XmlNode>()
                  .Select(node => BsonSerializer.Deserialize<BsonDocument>(node.SerializeToJson()));
            this.mongoDocController.InsertManyDocs(collectionName, bsonNodeList);

        }

        public string ReadNodes(string collectionName, InventoryModel type, int? offSet, int? pageSize, string filter = "")
        {
            collectionName = collectionName.Replace("-", "");
            string result = "";
            switch (type)
            {
                case InventoryModel.Header:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ header : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case InventoryModel.Record:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ 'record' : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                default:
                    {
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
            }
            return result;

        }

        public string ReadRecord(string collectionName, string productId)
        {
            collectionName = collectionName.Replace("-", "");
            var result = this.mongoDocController.FindDoc(collectionName, @"{ 'record' : { $exists : true},'record.@product-id':'" + productId + "'},{'_id':false}", null, null);
            return result;
        }

        public string ReadHeader(string collectionName, string headerId)
        {
            collectionName = collectionName.Replace("-", "");
            var result = this.mongoDocController.FindDoc(collectionName, @"{ header: { $exists : true},'header.@list-id':'" + headerId + "'},{'_id':false}", null, null);
            return result;
        }

        public long ReadModelCount(string collectionName, InventoryModel model)
        {
            collectionName = collectionName.Replace("-", "");
            var type = "header";
            if (model == InventoryModel.Record) {
                type = "record";
            }
            string filter = "{'" + type + "' : { $exists: true }},{'_id':false}";
            return this.mongoDocController.RowsCount(collectionName, filter);

        }
        
        public long SearchRecordCount(string collectionName, string textToFind)
        {
            collectionName = collectionName.Replace("-", "");
            string query = "{ 'record': { $exists : true},'record.@product-id':/.*" + textToFind + ".*/i },{'_id':false}";
            return this.mongoDocController.RowsCount(collectionName, query);
        }

        public string SearchRecord(string collectionName, string textToFind, int? offSet, int? size)
        {
            collectionName = collectionName.Replace("-", "");
            string query = "{ 'record': { $exists : true},'record.@product-id':/.*" + textToFind + ".*/i },{'_id':false}";
            return this.ReadNodes(collectionName,  InventoryModel.Record, offSet, size, query);
        }


    }
}
