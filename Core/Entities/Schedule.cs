using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Core.Entities
{
    public class Schedule
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; }

        [BsonElement("treatments")]
        public List<Treatment> Treatments { get; set; } = new List<Treatment>();
    }
}
