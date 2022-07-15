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
    public class DiscountEntity
    {
        public string sku;
        public string special_price;
        public string special_price_from_date;
        public string special_price_to_date;
    }
    public class DiscountMongoHelper
    {
        private MongoDocumentController mongoDocController { get; set; }
        public DiscountMongoHelper(string connection, string dbName)
        {
            this.mongoDocController = new MongoDocumentController(connection, dbName);
        }
        const string headerTag = "header";
        const string priceTableTag = "price-table";

        public bool SaveDiscount(string collectionName, List<DiscountEntity> discountList, int chunkSize, string storeName)
        {
            collectionName = collectionName.Replace("-", "");
            try
            {
                //var header = priceXmlDocument.GetElementsByTagName(headerTag);
                //InsertManyNodes(collectionName, header);

                //var records = priceXmlDocument.GetElementsByTagName(priceTableTag);
                InsertManyNodes(collectionName, discountList, chunkSize);
                InsertTimeStampNodes(collectionName, storeName);
            }
            catch
            {
                throw;
            }

            return true;
        }

        private void InsertManyNodes(string collectionName, List<DiscountEntity> discountList, int chunkSize)
        {
            BsonDocument discountBson = new BsonDocument();

            foreach (var discount in discountList)
            {
                Dictionary<string, object> discountEntity = new Dictionary<string, object>();

                discountBson = new BsonDocument { { "sku", discount.sku } };
                discountBson.Add("special_price", discount.special_price);
                discountBson.Add("special_price_to_date", discount.special_price_to_date);
                discountBson.Add("special_price_from_date", discount.special_price_from_date);

                List<BsonDocument> bsonDocument = new List<BsonDocument>();

                discountEntity.Add("discount-table", discountBson);

                BsonDocument document = new BsonDocument(discountEntity);
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
            this.mongoDocController.InsertManyDocs("Discount", new List<BsonDocument> { document });
        }

        public string ReadNodes(string collectionName, DiscountModel discountModel, int? offSet, int? pageSize, string filter = "")
        {
            collectionName = collectionName.Replace("-", "");
            string result = "";
            switch (discountModel)
            {
                case DiscountModel.Header:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ header : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case DiscountModel.DiscountTable:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ 'discount-table' : { $exists: true }},{'_id':false}";
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

        public long ReadModelCount(string collectionName, DiscountModel model)
        {
            collectionName = collectionName.Replace("-", "");
            var type = "header";
            if (model == DiscountModel.PriceTable)
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
            string query = "{ 'discount-table': { $exists : true},'discount-table.sku':/.*" + textToFind + ".*/i },{'_id':false}";
            return this.ReadNodes(collectionName, DiscountModel.DiscountTable, offSet, size, query);
        }
    }
}
