namespace AfricanNationsLeague.Domain.Common
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }


    //public class BaseEntity
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    //}
}
