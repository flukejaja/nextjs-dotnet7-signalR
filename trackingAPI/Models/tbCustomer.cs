using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace trackingAPI.Models
{
    public class tbCustomer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        public DateTime GetStart { get; set; }

        public DateTime FinishTime  { get; set; }

        public string Time { get; set; } = null!;
    }
}
