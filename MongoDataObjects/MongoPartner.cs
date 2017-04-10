using MongoDB.Bson;
using System;

namespace MongoDataObjects {
    public class MongoPartner {
        public ObjectId Id { get; set; }
        public String Name { get; set; }
    }
}
