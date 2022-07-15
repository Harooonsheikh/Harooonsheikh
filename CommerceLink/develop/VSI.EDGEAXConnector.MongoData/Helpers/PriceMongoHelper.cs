using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.MongoData.Enums;

namespace VSI.EDGEAXConnector.MongoData.Helpers
{
    public class PriceEntity
    {
        public string sku;
        public string price;
        public string attribute_set_code;
        public string product_type;
    }
    public class PriceMongoHelper
    {
        private MongoDocumentController mongoDocController { get; set; }
        public PriceMongoHelper(string connection, string dbName)
        {
            this.mongoDocController = new MongoDocumentController(connection, dbName);
        }

        const string headerTag = "header";
        const string priceTablesTag = "price-table";

        public bool SavePrice(string collectionName, List<PriceEntity> PriceList, int chunkSize, string storeName)
        {
            collectionName = collectionName.Replace("-", "");
            try
            {
                //var header = priceXmlDocument.GetElementsByTagName(headerTag);
                //InsertManyNodes(collectionName, header, chunkSize);

                //var records = priceXmlDocument.GetElementsByTagName(priceTablesTag);
                InsertManyNodes(collectionName, PriceList, chunkSize);
                InsertTimeStampNodes(collectionName, storeName);
            }
            catch
            {
                throw;
            }

            return true;
        }

        private void InsertManyNodes(string collectionName, List<PriceEntity> priceList, int chunkSize)
        {
            BsonDocument priceBson = new BsonDocument();

            foreach (var price in priceList)
            {
                Dictionary<string, object> priceEntity = new Dictionary<string, object>();

                priceBson = new BsonDocument { { "sku", price.sku } };
                priceBson.Add("price", price.price);
                priceBson.Add("attribute_set_code", price.attribute_set_code);
                priceBson.Add("product_type", price.product_type);

                List<BsonDocument> bsonDocument = new List<BsonDocument>();

                priceEntity.Add("price-table", priceBson);

                BsonDocument document = new BsonDocument(priceEntity);
                bsonDocument.Add(document);

                this.mongoDocController.InsertManyDocs(collectionName, bsonDocument);
            }

        }
        private void InsertTimeStampNodes(string collectionName, string storeName)
        {
            var document = new BsonDocument{
                      {"timestamp",  new BsonDocument
                      {
                          {"collectionname", collectionName },
                          {"datetime",DateTime.Now}
                      }
                },
                      {"storename",new BsonString(storeName)},
                };
            this.mongoDocController.InsertManyDocs("PriceBook", new List<BsonDocument>() { document });
        }

        public string ReadNodes(string collectionName, PriceModel type, int? offSet, int? pageSize, string filter = "")
        {
            collectionName = collectionName.Replace("-", "");
            string result = "";
            switch (type)
            {
                case PriceModel.Header:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ header : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case PriceModel.PriceTable:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ 'price-table' : { $exists: true }},{'_id':false}";
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
            var result = this.mongoDocController.FindDoc(collectionName, @"{ 'price-table' : { $exists : true},'price-table.@product-id':'" + productId + "'},{'_id':false}", null, null);
            return result;
        }

        public string ReadHeader(string collectionName, string headerId)
        {
            collectionName = collectionName.Replace("-", "");
            var result = this.mongoDocController.FindDoc(collectionName, @"{ header: { $exists : true},'header.@list-id':'" + headerId + "'},{'_id':false}", null, null);
            return result;
        }

        public long ReadModelCount(string collectionName, PriceModel model)
        {
            collectionName = collectionName.Replace("-", "");
            var type = "header";
            if (model == PriceModel.PriceTable)
            {
                type = "price-table";
            }
            string filter = "{'" + type + "' : { $exists: true }},{'_id':false}";
            return this.mongoDocController.RowsCount(collectionName, filter);

        }

        public long SearchRecordCount(string collectionName, string textToFind)
        {
            collectionName = collectionName.Replace("-", "");
            string query = "{ 'price-table': { $exists : true},'price-table.@product-id':/.*" + textToFind + ".*/i },{'_id':false}";
            return this.mongoDocController.RowsCount(collectionName, query);
        }

        public string SearchRecord(string collectionName, string textToFind, int? offSet, int? size)
        {
            collectionName = collectionName.Replace("-", "");
            string query = "{ 'price-table': { $exists : true},'price-table.sku':/.*" + textToFind + ".*/i },{'_id':false}";
            return this.ReadNodes(collectionName, PriceModel.PriceTable, offSet, size, query);
        }

    }
}
