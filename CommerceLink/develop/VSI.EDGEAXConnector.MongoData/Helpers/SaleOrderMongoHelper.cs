using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.MongoData.Enums;

namespace VSI.EDGEAXConnector.MongoData.Helpers
{
    public class SaleOrderMongoHelper
    {
        const string ordersTag = "orders";
        const string orderTag = "order";
        const string xmlOfSalesOrder = "XmlOfSalesOrder";

        private MongoDocumentController mongoDocController { get; set; }
        public SaleOrderMongoHelper(string connection, string dbName)
        {
            this.mongoDocController = new MongoDocumentController(connection, dbName);
        }


        public bool SaveSaleOrder(string collectionName, XmlDocument saleOrderXmlDocument)
        {
            collectionName = collectionName.Replace("-", "");
            try
            {
                XmlNodeList ordersNodeList = saleOrderXmlDocument.GetElementsByTagName(ordersTag);
                InsertManyNodes(collectionName, ordersNodeList);

                XmlNodeList orderNodeList = saleOrderXmlDocument.GetElementsByTagName(orderTag);
                InsertManyNodes(collectionName, orderNodeList);

                XmlNodeList xmlOfSalesOderNodeList = saleOrderXmlDocument.GetElementsByTagName(xmlOfSalesOrder);
                InsertManyNodes(collectionName, xmlOfSalesOderNodeList);
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

        public string ReadNodes(string collectionName, SaleOrdersModel salesOrdersModel, int? offSet, int? pageSize, string filter = "")
        {
            collectionName = collectionName.Replace("-", "");
            string result = "";
            switch (salesOrdersModel)
            {
                case SaleOrdersModel.Orders:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ orders : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict }; // key part 
                        result = result.ToJson(jsonWriterSettings);
                        break;
                    }
                case SaleOrdersModel.Order:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ order : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict }; // key part 
                        result = result.ToJson(jsonWriterSettings);
                        break;
                    }
                case SaleOrdersModel.XmlOfSalesOrder:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ XmlOfSalesOrder : { $exists: true }},{'_id':false}";
                        }

                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict }; // key part 
                        var json = result.ToJson(jsonWriterSettings);
                        result = Regex.Unescape(json);
                        
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

        public string IncrementalData(string collectionName)
        {
           string filter = "{ incrementeddocument : { $exists: true }},{'_id':false}";
            string collectionExist = this.mongoDocController.FindDoc(collectionName, filter, null, null);
            //If collection Not found create new collection
            int count = collectionExist.Count();
            if (count == 2)
            {
            string update = CreateCollection(collectionName);
            }
            var serilizeCollection = new JavaScriptSerializer().Deserialize<List<RootObject>>(collectionExist);
            int currrentCounter = serilizeCollection[0].Value.CounterValue;
            this.mongoDocController.FindAndUpdateIncrement(collectionName, currrentCounter, filter);
            return "";
        }

        public bool SaveOrder(OrderModel model)
        {
            model.CreatedOn = DateTime.Now;

            var saleOrder = model.ToBsonDocument();

            this.mongoDocController.InsertOneDoc("SalesOrder", saleOrder);

            return true;
        }

        public OrderModel GetById(string id)
        {
            var filter = new BsonDocument("_id", id);

            var order = this.mongoDocController.Find("SalesOrder", filter);
            if(order != null)
            {
                return BsonSerializer.Deserialize<OrderModel>(order);
            }
            return null;
        }

        public IEnumerable<OrderModel> GetAll(string userId, int type, int pageSize, int pageNumber)
        {
            var filter = Builders<OrderModel>.Filter.And(new BsonDocument("UserId", userId));

            var order = this.mongoDocController.GetAll<OrderModel>("SalesOrder", filter, pageSize, pageNumber);
            if(order != null)
            {
               order = order.OrderByDescending(m => m.CreatedOn);
            }
            return order;
        }

        private string CreateCollection(string collectionName)
        {
            BsonDocument documentValue = new BsonDocument();


            Dictionary<string, object> documentName = new Dictionary<string, object>();

            documentValue = new BsonDocument { { "collectionname", collectionName } };
            documentValue.Add("CounterName", "ChannelReferenceId");
            documentValue.Add("CounterValue", 90001);
            documentValue.Add("CounterSeed", 90000);
            documentValue.Add("CounterPrefix", "00");
            
            List<BsonDocument> incrementedDocument = new List<BsonDocument>();

            documentName.Add("incrementeddocument", documentValue);

            BsonDocument document = new BsonDocument(documentName);
            incrementedDocument.Add(document);

            this.mongoDocController.InsertManyDocs("SalesOrder", incrementedDocument);

            return "";
        }
    }

    [BsonIgnoreExtraElements]
    public class OrderModel
    {
        [BsonId]
        public string Id { get; set; }

        public string Content { get; set; }
        public string CartContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int Type { get; set; }
    }
}
