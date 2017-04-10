using MongoDB.Bson;
namespace MongoDataObjects {
    public class MongoEngineer {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
