using MongoDB.Bson.Serialization.Attributes;

namespace AfricanNationsLeague.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Country /*: BaseEntity*/
    {

        public string Code { get; set; } // ISO2 Code e.g. "ZA"

        public string Name { get; set; } // e.g. "South Africa"
        public string FlagUrl { get; set; } // PNG or SVG
    }
}
