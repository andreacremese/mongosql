using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace MongoDataObjects {
    public class MongoReport {
        public ObjectId Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Title { get; set; }
        public List<ObjectId> EngineerIds { get; set; } 
        public ObjectId PartnerId { get; set; }
        // as topics are expected not to change too often this de normalization is taken
        // and topic is embedded in the report.
        public MongoTopic Topic { get; set; }
    }
}
