using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using VSI.EDGEAXConnector.MongoData.Enums;
using System.Reflection;
using System.ComponentModel;

namespace VSI.EDGEAXConnector.MongoData.Helpers
{
    public class CatalogMongoHelper
    {
        private MongoDocumentController mongoDocController { get; set; }
        public CatalogMongoHelper(string connection, string dbName)
        {
            this.mongoDocController = new MongoDocumentController(connection, dbName);
        }
        const string ProductTag = "product";
        const string CatalogTag = "catalog";
        const string CategoryTag = "category";
        const string CategoryAssignmentTag = "category-assignment";



        public bool SaveCatalog(string collectionName, XmlDocument catalogXmlDoc, int chunkSize, string storeName)
        {
            collectionName = collectionName.Replace("-", "");
            try
            {

                //var categories = catalogXmlDoc.GetElementsByTagName(CategoryTag);
                //InsertManyNodes(collectionName, categories);

                var products = catalogXmlDoc.GetElementsByTagName("item");
                InsertManyNodes(collectionName, products);
                InsertTimeStampNodes(collectionName, storeName);
                //var categoryAssignments = catalogXmlDoc.GetElementsByTagName(CategoryAssignmentTag);
                //InsertManyNodes(collectionName, categoryAssignments);

                return true;
            }

            catch
            {
                throw;
            }
        }

        private void InsertManyNodes(string collectionName, XmlNodeList nodeList)
        {
            var bsonNodeList = nodeList.Cast<XmlNode>()
                  .Select(node => BsonSerializer.Deserialize<BsonDocument>(node.SerializeToJson()));
            this.mongoDocController.InsertManyDocs(collectionName, bsonNodeList);

        }
        private void InsertTimeStampNodes(string collectionName, string storeName)
        {
            var document = new BsonDocument{
                      {"timestamp",  new BsonDocument
                      {
                          { "collectionname", collectionName },
                          {"datetime",DateTime.Now}
                      }
                },
                      {"storename",new BsonString(storeName)},
                };
            this.mongoDocController.InsertManyDocs("Catalog", new List<BsonDocument>() { document });
        }
        public string ReadNodes(string collectionName, CatalogModel type, int? offSet, int? pageSize, string filter = "")
        {
            collectionName = collectionName.Replace("-", "");
            string result = "";
            switch (type)
            {
                case CatalogModel.Product:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ item : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case CatalogModel.Category:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ category : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case CatalogModel.CategoryAssignment:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ 'category-assignment' : { $exists: true }},{'_id':false}";
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
        public string ReadNodes(CatalogModel type, string storeName)
        {
            string result = "";
            switch (type)
            {
                case CatalogModel.Catalog:
                    {
                        result = this.mongoDocController.FindUpdatedDocument(type.ToString(), CatalogModel.timestamp.ToString(), storeName);
                        break;
                    }
                case CatalogModel.PriceBook:
                    {
                        result = this.mongoDocController.FindUpdatedDocument(type.ToString(), CatalogModel.timestamp.ToString(), storeName);
                        break;
                    }
                case CatalogModel.Discount:
                    {
                        result = this.mongoDocController.FindUpdatedDocument(type.ToString(), CatalogModel.timestamp.ToString(), storeName);
                        break;
                    }
            }
            return result;

        }
        public string ReadTimeStampNodes(string collectionName, CatalogModel type, int? offSet, int? pageSize, string filter = "")
        {
            collectionName = collectionName.Replace("-", "");
            string result = "";
            switch (type)
            {
                case CatalogModel.Product:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ item : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case CatalogModel.Category:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ category : { $exists: true }},{'_id':false}";
                        }
                        result = this.mongoDocController.FindDoc(collectionName, filter, offSet, pageSize);
                        break;
                    }
                case CatalogModel.CategoryAssignment:
                    {
                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = "{ 'category-assignment' : { $exists: true }},{'_id':false}";
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
        public string ReadMasterProducts(string collectionName)
        {
            collectionName = collectionName.Replace("-", "");
            var result = this.mongoDocController.FindDoc(collectionName, @"{ product: { $exists : true},'product.@product-id':/^\d+$/ },{'_id':false}", null, null);
            return result;
        }

        public string ReadProduct(string collectionName, string prodId)
        {
            collectionName = collectionName.Replace("-", "");
            var result = this.mongoDocController.FindDoc(collectionName, @"{ item: { $exists : true},'item.@product-id':'" + prodId + "'},{'_id':false}", null, null);
            return result;
        }

        public long ReadModelCount(string collectionName, CatalogModel model)
        {
            collectionName = collectionName.Replace("-", "");
            var type = EnumExtension.GetDescription(model);
            string filter = "{\"" + type + "\" : { $exists: true }},{'_id':false}";
            return this.mongoDocController.RowsCount(collectionName, filter);

        }


        public long SearchProductCount(string collectionName, string query)
        {
            collectionName = collectionName.Replace("-", "");
            string query1 = "{ product: { $exists : true},'product.custom-attributes.custom-attribute.#text':/.*" + query + ".*/i },{'_id':false}";
            string query2 = "{ product: { $exists : true},'product.@product-id':/.*" + query + ".*/i },{'_id':false}";
            string query3 = "{ product: { $exists : true},'product.display-name.#text':/.*" + query + ".*/i },{'_id':false}";
            string filterQuery = "{$or:[" + query1 + "," + query2 + "," + query3 + "] }";
            return this.mongoDocController.RowsCount(collectionName, filterQuery);
        }

        public string SearchProduct(string collectionName, string query, int? offSet, int? size)
        {
            collectionName = collectionName.Replace("-", "");
            //string query1 = "{ item: { $exists : true},'item.custom-attributes.custom-attribute.#text':/.*" + query + ".*/i },{'_id':false}";
            string query2 = "{ item: { $exists : true},'item.@product-id':/.*" + query + ".*/i },{'_id':false}";
            string query3 = "{ item: { $exists : true},'item.name':/.*" + query + ".*/i },{'_id':false}";
            string sku = "{ item: { $exists : true},'item.sku':/.*" + query + ".*/i },{'_id':false}";
            //string filterQuery = "{$or:[" + query1 + "," + query2 + "," + query3 + "," + sku+"]}";
            string filterQuery = "{$or:[" + "," + query2 + "," + query3 + "," + sku + "]}";
            return this.ReadNodes(collectionName, CatalogModel.Product, offSet, size, filterQuery);
        }

        public long SearchCategoryCount(string collectionName, string query)
        {
            collectionName = collectionName.Replace("-", "");
            string query1 = "{ category: { $exists : true},'category.parent':/.*" + query + ".*/i },{'_id':false}";
            string query2 = "{ category: { $exists : true},'category.@category-id':/.*" + query + ".*/i },{'_id':false}";
            string query3 = "{ category: { $exists : true},'category.display-name.#text':/.*" + query + ".*/i },{'_id':false}";
            string filterQuery = "{$or:[" + query1 + "," + query2 + "," + query3 + "] }";
            return this.mongoDocController.RowsCount(collectionName, filterQuery);
        }

        public string SearchCategory(string collectionName, string query, int? offSet, int? size)
        {
            collectionName = collectionName.Replace("-", "");
            string query1 = "{ category: { $exists : true},'category.parent':/.*" + query + ".*/i },{'_id':false}";
            string query2 = "{ category: { $exists : true},'category.@category-id':/.*" + query + ".*/i },{'_id':false}";
            string query3 = "{ category: { $exists : true},'category.display-name.#text':/.*" + query + ".*/i },{'_id':false}";
            string filterQuery = "{$or:[" + query1 + "," + query2 + "," + query3 + "] }";
            return this.ReadNodes(collectionName, CatalogModel.Category, offSet, size, filterQuery);
        }

        public long SearchCatAssignmentCount(string collectionName, string query)
        {
            collectionName = collectionName.Replace("-", "");
            string query1 = "{ 'category-assignment': { $exists : true},'category-assignment.@category-id':/.*" + query + ".*/i },{'_id':false}";
            string query2 = "{ 'category-assignment': { $exists : true},'category-assignment.@product-id':/.*" + query + ".*/i },{'_id':false}";
            string filterQuery = "{$or:[" + query1 + "," + query2 + "] }";
            return this.mongoDocController.RowsCount(collectionName, filterQuery);
        }

        public string SearchCatAssignment(string collectionName, string query, int? offSet, int? size)
        {
            collectionName = collectionName.Replace("-", "");
            string query1 = "{ 'category-assignment': { $exists : true},'category-assignment.@category-id':/.*" + query + ".*/i },{'_id':false}";
            string query2 = "{ 'category-assignment': { $exists : true},'category-assignment.@product-id':/.*" + query + ".*/i },{'_id':false}";
            string filterQuery = "{$or:[" + query1 + "," + query2 + "] }";
            return this.ReadNodes(collectionName, CatalogModel.CategoryAssignment, offSet, size, filterQuery);
        }




    }
}

