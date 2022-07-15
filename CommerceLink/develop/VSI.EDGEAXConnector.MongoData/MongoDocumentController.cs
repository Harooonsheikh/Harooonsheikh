using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;


namespace VSI.EDGEAXConnector.MongoData
{
    /// <summary>
    /// Helper class to find and insert documents in Mongo Document store
    /// </summary>
    public class MongoDocumentController
    {
        protected IMongoClient MongoClient { get; set; }
        protected IMongoDatabase MongoDB { get; set; }
                
        public MongoDocumentController()
        {
            var conString = "mongodb://localhost:27017";//ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClient = new MongoClient(conString);
            //MongoDB = MongoClient.GetDatabase(ConfigurationManager.AppSettings.Get("CLDatabaseName"));
            MongoDB = MongoClient.GetDatabase("EdgeAXCommerceLink");
        }

        public MongoDocumentController(string connection, string dbName)
        {
            var conString = connection;//ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClient = new MongoClient(conString);
            //MongoDB = MongoClient.GetDatabase(ConfigurationManager.AppSettings.Get("CLDatabaseName"));
            MongoDB = MongoClient.GetDatabase(dbName);
        }

        protected IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return MongoDB.GetCollection<BsonDocument>(collectionName);
        }
        protected IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return MongoDB.GetCollection<T>(collectionName);
        }

        public void InsertOneDoc(string collectionName, string documentJson)
        {
            var documnt = BsonSerializer.Deserialize<BsonDocument>(documentJson);
            this.GetCollection(collectionName).InsertOne(documnt);
        }
        public string FindDoc(string collectionName, string filterJson, int? offSet, int? pageSize)
        {
            try
            {
                //TODO:revise method to return in Json
                var filter = BsonSerializer.Deserialize<BsonDocument>(filterJson);
                var resultQuery = this.GetCollection(collectionName).Find(filter);
                if (offSet != null && pageSize != null)
                {
                    resultQuery = resultQuery.Skip(offSet.Value).Limit(pageSize.Value);
                }
                var result = resultQuery.ToList()
                    .Select(m => m.Elements.ElementAt(1)).ToList();
                return result.ToJson();
            }
            catch
            {
                throw;
            }
        }
        public string FindUpdatedDocument(string collectionName, string documentName, string storeName)
        {
            try
            {
                var resultQuery = this.GetCollection(collectionName).Aggregate()
                    .SortByDescending((a) => a[documentName])
                    .Match("{'storename' : '" + storeName + "'}")
                    .Project("{'timestamp':1, '_id':0}")
                    .Project("{collectionname:'$timestamp.collectionname',datetime :{ $dateToString: { format: '%Y-%m-%d %H:%M:%S', date: '$timestamp.datetime' }}}")
                    .First();
                return resultQuery.ToJson();
            }
            catch
            {
                throw;
            }
        }
        public string FindAndUpdateIncrement(string collectionName, int currentCounter, string filterJson)
        {
            //var result = this.GetCollection(collectionName).FindOneAndUpdate(
            //               Builders<BsonDocument>.Filter.Eq("CounterValue", currentCounter),
            //               Builders<BsonDocument>.Update.Set("CounterValue", currentCounter + 1)
            //               );
            var result = this.GetCollection(collectionName).UpdateOne(
                           Builders<BsonDocument>.Filter.Eq("CounterValue", currentCounter),
                           Builders<BsonDocument>.Update.Set("CounterValue", currentCounter + 1)
                           );
            //var builder = Builders<BsonDocument>.Filter;
            ////var filter = builder.Eq("CounterValue", 90002);
            ////var filterSet = builder.Eq("CounterValue", 90005);

            //FieldDefinition<BsonDocument, string> field = "CounterValue";
            //FilterDefinition<BsonDocument> filter = new BsonDocument();
            //var result = this.GetCollection(collectionName).FindOneAndUpdate(
            //   filter,
            //   Builders<BsonDocument>.Update.Set("CounterValue", 90005));
            //var db = this.GetCollection(collectionName);


            //var a = db.DistinctAsync(field, filter).GetAwaiter().GetResult().ToListAsync().GetAwaiter().GetResult();
            return "";
        }
        public string GetCollections()
        {
            try
            {
                List<string> collectionNames = new List<string>();
                foreach (var item in MongoDB.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
                {
                    collectionNames.Add(item.ToString());
                    //Console.WriteLine(item.ToString());
                }
                //var result= collectionNames.ToList().Select(m => m.Elements.ElementAt(1)).ToList();
                var result = collectionNames.ToJson();
                return result;
            }
            catch
            {
                throw;
            }
        }
        public long RowsCount(string collectionName, string filterJson)
        {
            try
            {
                //TODO:revise method to return in Json
                var filter = BsonSerializer.Deserialize<BsonDocument>(filterJson);
                var resultQuery = this.GetCollection(collectionName).Find(filter).Count();
                return resultQuery;
            }
            catch
            {
                throw;
            }
        }

        public void InsertManyDocs(string collectionName, IEnumerable<BsonDocument> documents)
        {
            try
            {
                this.GetCollection(collectionName).InsertMany(documents);
            }
            catch
            {
                throw;
            }
        }
        public void InsertOneDoc(string collectionName, BsonDocument documents)
        {
            try
            {
                this.GetCollection(collectionName).InsertOne(documents);
            }
            catch
            {
                throw;
            }
        }

        public BsonDocument Find(string collectionName, BsonDocument filter)
        {
            return this.GetCollection(collectionName).Find(filter).FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>(string collectionName, FilterDefinition<T> filter, int pageSize, int pageNumber)
        {
            var result = this.GetCollection<T>(collectionName).Find(filter);
            if(pageNumber >  0 && pageSize > 0)
            {
                result = result.Skip(pageNumber * pageSize).Limit(pageSize);
            }

            return result.ToEnumerable();

        }
    }
}
