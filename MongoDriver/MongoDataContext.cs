using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;
using MongoDataObjects;
using System;

namespace MongoDriver {
    public class MongoDataContext {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        private static String _partnersCollection = "partners";                                                                     
        private static String _engineersCollection = "engineers";
        private static String _reportsCollection = "reports";

        public MongoDataContext() {
            _client = new MongoClient(ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString);
            _database = _client.GetDatabase("dbperformance");
        }

        public List<MongoReport> getReports() {
            var collection = _database.GetCollection<MongoReport>(_reportsCollection);
            return collection.Find(a =>true).ToList();
        }

        public async Task<List<BsonDocument>> GetReportsByPartnerName(String partnerName) {
            var collection = _database.GetCollection<MongoReport>(_reportsCollection);
            var aggregate = collection.Aggregate()
                .Lookup("engineers", "EngineerIds", "_id", "engineer_docs")
                .Lookup("partners", "PartnerId", "_id", "partner_doc")
                .Match(new BsonDocument { { "partner_doc.Name", partnerName } });
            return await aggregate.ToListAsync();
        }

        public async Task<List<BsonDocument>> GetReportsByEngineerName(String partnerName) {
            var collection = _database.GetCollection<MongoReport>(_reportsCollection);
            var aggregate = collection.Aggregate()
                .Lookup("engineers", "EngineerIds", "_id", "engineer_docs")
                .Lookup("partners", "PartnerId", "_id", "partner_doc")
                .Match(new BsonDocument { { "engineer_docs.Name", partnerName } });
            return await aggregate.ToListAsync();
        }

        // this is the pre 3.3.4 implementation, as aggregation requires to unwind the collection
        public List<MongoReport> GetReportsByEngNameNoAggregation(String engName) {
            var collection = _database.GetCollection<MongoReport>(_reportsCollection);
            var filter = Builders<MongoReport>.Filter.Eq("EngineerIds", FindEngineerId(engName));
            return collection.Find(filter).ToList();
        }

        public void InsertEngineer(MongoEngineer engineer) {
            var collection = _database.GetCollection<MongoEngineer>(_engineersCollection);
            collection.InsertOne(engineer);
        }

         public ObjectId FindEngineerId(String name) {
            var collection = _database.GetCollection<MongoEngineer>(_engineersCollection);
            var filter = Builders<MongoEngineer>.Filter.Eq("Name", name);
            var doc = collection.Find(filter).FirstOrDefault();
            return doc.Id;
        }

        public MongoEngineer FindEngineerById(ObjectId _id) {
            var collection = _database.GetCollection<MongoEngineer>(_engineersCollection);
            var filter = Builders<MongoEngineer>.Filter.Eq("_id", _id);
            var doc = collection.Find(filter).FirstOrDefault();
            return doc;
        }

        public List<MongoEngineer> GetEngineers() {
            var collection = _database.GetCollection<MongoEngineer>(_engineersCollection);
            return collection.Find(a => true).ToList();
        }

        public async void RemoveAllEngineers() {
            var collection = _database.GetCollection<MongoEngineer>(_engineersCollection);
            var filter = new BsonDocument();
            var result = await collection.DeleteManyAsync(filter);
        }

        public async void RemoveAllPartners() {
            var collection = _database.GetCollection<MongoPartner>(_partnersCollection);
            var filter = new BsonDocument();
            var result = await collection.DeleteManyAsync(filter);
        }

        public void InsertPartner(MongoPartner partner) {
            var collection = _database.GetCollection<MongoPartner>(_partnersCollection);
            collection.InsertOne(partner);
        }

        public List<MongoPartner> GetAllPartners() {
            var collection = _database.GetCollection<MongoPartner>(_partnersCollection);
            return collection.Find(a => true).ToList();
        }

        public ObjectId FindPartnerId(String name) {
            var collection = _database.GetCollection<MongoPartner>(_partnersCollection);
            var filter = Builders<MongoPartner>.Filter.Eq("Name", name);
            var doc = collection.Find(filter).FirstOrDefault();
            return doc.Id;
        }

        public MongoPartner FindPartnerById(ObjectId _id) {
            var collection = _database.GetCollection<MongoPartner>(_partnersCollection);
            var filter = Builders<MongoPartner>.Filter.Eq("_id", _id);
            var doc = collection.Find(filter).FirstOrDefault();
            return doc;
        }

        public async Task<Boolean> RemoveAllReports() {
            var collection = _database.GetCollection<MongoReport>(_reportsCollection);
            var filter = new BsonDocument();
            var result = await collection.DeleteManyAsync(filter);
            return collection.Count(filter) == 0;
        }

         public void InsertReport(MongoReport report) {
            var collection = _database.GetCollection<MongoReport>(_reportsCollection);
            collection.InsertOne(report);
        }
    }
}
