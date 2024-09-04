﻿using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("schedules")]
        public List<Schedule> Schedules { get; set; }
    }
}
