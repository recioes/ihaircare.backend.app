using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Core.Entities
{
    public class Treatment
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("scheduledDate")]
        public DateTime ScheduledDate { get; set; }

        [BsonElement("frequency")]
        public int? FrequencyInDays { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; }

        [BsonElement("completedDate")]
        public DateTime? CompletedDate { get; set; }
    }
}
